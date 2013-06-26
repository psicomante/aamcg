using UnityEngine;
using System.Collections.Generic;
using Amucuga;

/// <summary>
/// GUI for server in the Game Scene
/// </summary>
public class GameGui : MonoBehaviour
{

    private string _serverIPAddress;
    private bool _displayScore = true;
	private bool _escPressed = false;

    public const int SCOREBOX_WIDTH = 150;
    public const int SCOREBOX_HEIGHT = 50;
    public const int SCOREBOX_TOPMARGIN = 10;
    public const int SCOREBOX_RIGHTMARGIN = 10;
	
	// Score
	public const int SCORE_HEIGHT_LABEL = 30;
	
	public GUIStyle GameGuiStyle;
	public GUISkin GameGuiSkin;

    /// <summary>
    /// Start the Game GUI
    /// </summary>
    void Start ()
	{
		Debug.Log ("GameGui.cs Started");
		
		//Blocks client execution
		if (Network.isClient)
			return;

		_serverIPAddress = NetworkManager.GetLocalIPAddress ();
	}
	
	void Update () {
		if (Input.GetKeyUp (KeyCode.Escape)) {
			_escPressed = !_escPressed;	
		}
	}

    /// <summary>
    /// Renders the GUI
    /// </summary>
    void OnGUI ()
	{
		// Setting Skin (every ONGUI call, what a shame!)
		GUI.skin = GameGuiSkin;
		
		// show IP Address and Connections
		if (Network.isServer) {
			GUILayout.Label ("IP Address: " + _serverIPAddress);
			GUILayout.Label ("Connections: " + Network.connections.Length);
		}

		// show Logout
		if (_escPressed && GUI.Button (new Rect (Screen.width / 2 - 150, Screen.height / 2 - 150, 300, 300), "Logout")) {
			NetworkManager.Shutdown ();
			AmApplication.LoadMainMenu ();
		}
		
		// score GUI function

		
		if (AmApplication.CurrentMatchState == MatchState.WAITING_ROOM) {
			Rect windowRect = new Rect (10, 10, Screen.width - 100, Screen.height - 40);
			windowRect = GUI.Window (0, windowRect, DrawWaitingRoomWindow, "End Game");
		} else {
			Score (GameObject.Find (
					AmApplication.GAMEOBJECT_MAP_GENERATOR_NAME)
					.GetComponent<PlayerManager> ()
					.GetPlayersList (), 
					false
			);
		}

	}
	
	void DrawWaitingRoomWindow (int windowID)
	{		
		Score (GameObject.Find (
					AmApplication.GAMEOBJECT_MAP_GENERATOR_NAME)
					.GetComponent<PlayerManager> ()
					.GetPlayersList (), 
					true
			);
	}
	
	public void Score (SortedList<string, ConnectedPlayer> playersList, bool isWaitingRoom)
	{
		// players to render score
		int numPlayers = playersList.Count + 1; // "score" label included
		Rect areaRect;
		
		if (!isWaitingRoom)
			areaRect = new Rect (Screen.width - SCOREBOX_WIDTH, SCOREBOX_TOPMARGIN, SCOREBOX_WIDTH, SCORE_HEIGHT_LABEL * numPlayers);
		else 
			areaRect = new Rect (100, 50, Screen.width, Screen.height);

		GUILayout.BeginArea (areaRect);
		GUILayout.Label ("Score");
		foreach (KeyValuePair<string, ConnectedPlayer> p in playersList) {
			GUILayout.Label (p.Value.Name + "\t" + p.Value.Score);
		}		
		GUILayout.EndArea ();

		// &lt;- Push the Slider to the end of the Label
		//screenRect.x += screenRect.width; 
	}
}