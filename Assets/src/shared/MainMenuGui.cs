using UnityEngine;
using System.Collections;
using Amucuga;

/// <summary>
/// Main menu GUI.
/// </summary>
public class MainMenuGui : MonoBehaviour {
	
	/// <summary>
	/// Enum to save the current opened panel
	/// </summary>
	/// TODO: move this code to AmApplication
	public enum GUIState
	{
		MAIN_PANEL,
		CLIENT_PANEL,
		SERVER_PANEL
	}
	
	public GUIState currentGUIState = GUIState.MAIN_PANEL;

	// TODO:
	// move this code to AmApplication Class (maybe creating a new AmApplication objects)
	private int GUIPort = AmApplication.DEFAULT_SERVER_PORT;
	private string GUIServerHost = AmApplication.DEFAULT_SERVER_HOST;
	private string GUIServerName = AmApplication.DEFAULT_SERVER_NAME;
	private int GUIServerPort = AmApplication.DEFAULT_SERVER_PORT;
	private int GUIMaxPlayers = AmApplication.DEFAULT_MAX_PLAYERS;
	
	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start ()
	{
		print ("Starting Main Menu GUI");
	}
	
	/// <summary>
	/// Update this instance.
	/// </summary>
	void Update () {}
	
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
	
	/// <summary>
	/// Raises the failed to connect event.
	/// </summary>
	/// <param name='error'>
	/// Error.
	/// </param>
	void OnFailedToConnect (NetworkConnectionError error)
	{
		Debug.Log ("Could not connect to server: " + error);
	}
	
	/// <summary>
	/// Raises the server initialized event.
	/// </summary>
	void OnServerInitialized() {
		Debug.Log("Server initialized and ready - loading " + AmApplication.SERVER_SCENE);
		Application.LoadLevel(AmApplication.SERVER_SCENE);
		Debug.Log("Connections: " + Network.connections.Length);
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
		GUIPort = int.Parse(GUI.TextField (new Rect (0, 20, 50, 21), GUIPort.ToString()));
		GUILayout.EndArea ();
		
		//Max Players
		GUILayout.BeginArea (new Rect (65, 10, 100, 50));
		GUILayout.Label ("Max. Players: ");
		GUIMaxPlayers = int.Parse(GUI.TextField (new Rect (0, 20, 50, 21), GUIMaxPlayers.ToString()));
		GUILayout.EndArea ();
		
		//Game Name
		GUILayout.BeginArea (new Rect (155, 10, 200, 50));
		GUILayout.Label ("Game Name: ");
		GUIServerName = GUI.TextField (new Rect (0, 20, 150, 21), GUIServerName);
		GUILayout.EndArea ();
	
		if (GUI.Button (new Rect (10, 60, 180, 25), "Start a Dedicated Server")) {
			Network.InitializeServer (GUIMaxPlayers, GUIPort, !Network.HavePublicAddress ());
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
		GUIServerPort = int.Parse(GUI.TextField (new Rect (0, 20, 80, 21), GUIServerPort.ToString()));
		GUILayout.EndArea ();
		if (GUI.Button (new Rect (10, 80, 100, 24), "Connect")) {
			Debug.Log ("Connecting to " + GUIServerHost + ":" + GUIServerPort);
			Network.Connect (GUIServerHost, GUIServerPort);
		}
		
		//return to the main menu
		if (GUI.Button (new Rect (10, 110, 100, 24), "Main Menu")) {
			currentGUIState = GUIState.MAIN_PANEL;
		}
	}
	
	/// <summary>
	/// Displaies the main panel.
	/// </summary>
	private void displayMainPanel() {
		GUILayout.BeginArea(new Rect(10, 10, 200, 80));
			GUILayout.Label(AmApplication.GAME_NAME);
		GUILayout.EndArea();
		
		if (GUI.Button(new Rect(210, 10, 100, 25), "Start a Server")) {
			currentGUIState = GUIState.SERVER_PANEL;
		}
		
		if (GUI.Button(new Rect(210, 50, 150, 25), "Connect to a Server ...")) {
			currentGUIState = GUIState.CLIENT_PANEL;
		}
	}
}
