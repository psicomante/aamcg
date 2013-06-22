using UnityEngine;
using System.Collections;
using Amucuga;

/// <summary>
/// Main menu GUI.
/// </summary>
public class MainMenuGui : MonoBehaviour
{
    /// <summary>
    /// The current GUI State
    /// </summary>
    public GUIState currentGUIState = GUIState.MAIN_PANEL;

    //Server configuration vars
	//TODO: a better naming?
	//TODO: a better code class position?
    private int GUIPort, GUIMaxPlayers;
    private string GUIServerHost, GUIServerName, GUIPlayerName;

    /// <summary>
    /// Start this instance.
    /// </summary>
    void Start ()
	{
		Debug.Log ("Starting Main Menu GUI");
		
		// update the version number (test code)
		
		// getting the gameobject named "Version"
		GameObject go = GameObject.Find ("GameVersion");
		// getting the component script named "VariableTest(.cs)" and setting 
		// the public variable to AmApplication.VERSION_NUMBER;
		go.GetComponent<GameVersionController> ().versionNumber = AmApplication.VERSION_NUMBER;
		go.guiText.text = go.GetComponent<GameVersionController> ().versionNumber;
		
		// GUI variables initialization 
		GUIServerHost = PlayerSettings.GetHost ();
		GUIPlayerName = PlayerSettings.GetPlayerName ();
		GUIPort = PlayerSettings.GetPort ();
		GUIServerName = AmApplication.DEFAULT_SERVER_NAME;
		GUIMaxPlayers = AmApplication.DEFAULT_MAX_PLAYERS;
		
	}

    /// <summary>
    /// Update this instance.
    /// </summary>
    void Update() { }

    /// <summary>
    /// Raises the GU event.
    /// </summary>
    void OnGUI ()
	{
		// calls the correct GUI Panel base on state
		switch (currentGUIState) {
		case GUIState.MAIN_PANEL:
			displayMainPanel ();
			break;
		case GUIState.CLIENT_PANEL:
			displayClientSettings ();
			break;
		case GUIState.SERVER_PANEL:
			displayServerSettings ();
			break;
		}
	}

    //////////////////////////////////////////////////////////////////
    // DISPLAY MENU FUNCTIONS
    //////////////////////////////////////////////////////////////////

    /// <summary>
    /// Displaies the server settings.
    /// Render the menu controls for starting a dedicated server
    /// </summary>
    private void displayServerSettings ()
	{

		//Port Area
		GUILayout.BeginArea (new Rect (10, 10, 100, 50));
		GUILayout.Label ("Port: ");
		GUIPort = int.Parse (GUI.TextField (new Rect (0, 20, 50, 21), GUIPort.ToString ()));
		GUILayout.EndArea ();

		//Max Players
		GUILayout.BeginArea (new Rect (65, 10, 100, 50));
		GUILayout.Label ("Max. Players: ");
		GUIMaxPlayers = int.Parse (GUI.TextField (new Rect (0, 20, 50, 21), GUIMaxPlayers.ToString ()));
		GUILayout.EndArea ();

		//Game Name
		GUILayout.BeginArea (new Rect (155, 10, 200, 50));
		GUILayout.Label ("Game Name: ");
		GUIServerName = GUI.TextField (new Rect (0, 20, 150, 21), GUIServerName);
		GUILayout.EndArea ();

		//start a server
		if (GUI.Button (new Rect (10, 60, 180, 25), "Start a Dedicated Server")) {
			PlayerSettings.SetPort (GUIPort);
			
			Network.InitializeServer (GUIMaxPlayers, GUIPort, false); //!Network.HavePublicAddress());
		}

		//return to the main menu
		if (GUI.Button (new Rect (10, 90, 100, 25), "Main Menu")) {
			currentGUIState = GUIState.MAIN_PANEL;
		}
	}

    /// <summary>
    /// Displaies the client settings.
    /// </summary>
    private void displayClientSettings ()
	{
		GUILayout.BeginArea (new Rect (10, 10, 100, 50));
		GUILayout.Label ("Direct connect");
		GUILayout.EndArea ();

		GUILayout.BeginArea (new Rect (10, 30, 100, 50));
		GUILayout.Label ("IP or Host name: ");
		GUIServerHost = GUI.TextField (new Rect (0, 20, 80, 21), GUIServerHost);
		GUILayout.EndArea ();

		GUILayout.BeginArea (new Rect (120, 30, 100, 50));
		GUILayout.Label ("Game Port: ");
		GUIPort = int.Parse (GUI.TextField (new Rect (0, 20, 80, 21), GUIPort.ToString ()));
		GUILayout.EndArea ();

		GUILayout.BeginArea (new Rect (230, 30, 100, 50));
		GUILayout.Label ("Player Name: ");
		GUIPlayerName = GUI.TextField (new Rect (0, 20, 80, 21), GUIPlayerName);
		GUILayout.EndArea ();

		// connection to server
		if (GUI.Button (new Rect (10, 80, 100, 24), "Connect")) {
			PlayerSettings.SetHost (GUIServerHost);
			PlayerSettings.SetPlayerName (GUIPlayerName);
			PlayerSettings.SetPort(GUIPort);
			
			Debug.Log ("Connecting to " + GUIServerHost + ":" + GUIPort);
			Network.Connect (GUIServerHost, GUIPort);
		}

		//return to the main menu
		if (GUI.Button (new Rect (10, 110, 100, 24), "Main Menu")) {
			currentGUIState = GUIState.MAIN_PANEL;
		}
	}

    /// <summary>
    /// Displaies the main panel.
    /// </summary>
    private void displayMainPanel()
    {
        GUILayout.BeginArea(new Rect(10, 10, 200, 80));
        GUILayout.Label(AmApplication.GAME_NAME);
        GUILayout.EndArea();

        if (GUI.Button(new Rect(210, 10, 100, 25), "Start a Server"))
        {
            currentGUIState = GUIState.SERVER_PANEL;
        }

        if (GUI.Button(new Rect(210, 50, 150, 25), "Connect to a Server ..."))
        {
            currentGUIState = GUIState.CLIENT_PANEL;
        }

        if (GUI.Button(new Rect(210, 90, 100, 25), "Exit"))
        {
            Application.Quit();
        }
    }
}
