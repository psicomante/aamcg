using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Amucuga;

/// <summary>
/// GUI for server in the Game Scene
/// </summary>
public class GameGui : MonoBehaviour
{

    private string _serverIPAddress;
    //private bool _displayScore = true;
	private bool _escPressed = false;
	
	// Score Box Constansts
    public const int SCOREBOX_WIDTH = 150;
    public const int SCOREBOX_HEIGHT = 50;
    public const int SCOREBOX_TOPMARGIN = 80;
    public const int SCOREBOX_RIGHTMARGIN = 20;
	public const int SCORE_HEIGHT_LABEL = 30;
	
	/// <summary>
	/// The _score window top margin.
	/// </summary>
	private int _scoreWindowTopMargin;
	/// <summary>
	/// The _score window left margin.
	/// </summary>
	private int _scoreWindowLeftMargin;	
	
	/// <summary>
	/// Used in score windows "waiting room".
	/// </summary>
	public const string WAITING_ROOM_TABS = "\t\t\t\t\t\t\t\t\t\t\t";
	
	/// <summary>
	/// The game GUI skin asset.
	/// </summary>
	public GUISkin GameGuiSkin;
	
	/// <summary>
	/// Players List
	/// </summary>
	private SortedList<string, ConnectedPlayer> _playersList;

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
		
		// score window dinamic size
		_scoreWindowTopMargin = Screen.height / 30;
		_scoreWindowLeftMargin = Screen.width / 20;	
	}
	
	
	/// <summary>
	/// Update this Monobehaviour instance
	/// </summary>
	void Update ()
	{
		// update player list
		_playersList = GameObject.Find (AmApplication.GAMEOBJECT_MAP_GENERATOR_NAME).GetComponent<PlayerManager> ().GetPlayersList ();
		
		// pause menu toggle
		if (Input.GetKeyUp (KeyCode.Escape) || Input.GetKey (KeyCode.Menu)) {
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
		
		// Window rectangle manager
		Rect windowRect;
		
		// SERVER GUI
		if (Network.isServer) {
			if (AmApplication.CurrentMatchState == MatchState.MATCH) {
				// Match CountDown
				windowRect = new Rect (Screen.width - 120, 10, 100, 50);
				windowRect = GUI.Window (0, windowRect, DrawMatchCountDown, "CountDown");				
				
				// show IP Address and Connections (not in waiting room)
				windowRect = new Rect (10, 10, 200, 80);
				windowRect = GUI.Window (1, windowRect, DrawServerStats, "Server Stats");	
				
				// Player Scores
				windowRect = new Rect (Screen.width - SCOREBOX_WIDTH - SCOREBOX_RIGHTMARGIN, SCOREBOX_TOPMARGIN, SCOREBOX_WIDTH, SCORE_HEIGHT_LABEL * (Network.connections.Length) + 50);
				windowRect = GUI.Window (2, windowRect, DrawScorePanel, "Scores");				
			}
			// Waiting Room
			if (AmApplication.CurrentMatchState == MatchState.WAITING_ROOM) {
				// Match Final Score
				windowRect = new Rect (_scoreWindowLeftMargin, _scoreWindowTopMargin, Screen.width - 100, Screen.height - 40);
				windowRect = GUI.Window (2, windowRect, DrawWaitingRoomWindow, "End Game : Scores");
			}
		}
		
		if ((Network.isServer || !PlayerSettings.DedicatedServer) /*&& _playersList [Network.player.guid].PowerUps.Count > 0*/) {
			// Player Scores
			windowRect = new Rect (SCOREBOX_RIGHTMARGIN, SCOREBOX_TOPMARGIN + 30, SCOREBOX_WIDTH, SCORE_HEIGHT_LABEL * (_playersList [Network.player.guid].PowerUps.Count) + 20);
			windowRect = GUI.Window (3, windowRect, DrawPlayerPowerUps, "PowerUps");
			
			windowRect = new Rect (SCOREBOX_RIGHTMARGIN + SCOREBOX_WIDTH, SCOREBOX_TOPMARGIN + 30, SCOREBOX_WIDTH, SCOREBOX_HEIGHT * 5);
			windowRect = GUI.Window (4, windowRect, DrawPlayerStatistics, "Player Stats");				
		}

		// show Logout
		if (_escPressed && GUI.Button (new Rect (Screen.width / 2 - 150, Screen.height / 2 - 150, 300, 300), "Logout")) {
			NetworkManager.Shutdown ();
			AmApplication.LoadMainMenu ();
		}
	}
	
	void DrawPlayerStatistics (int windowID)
	{
		ConnectedPlayer cp = _playersList [Network.player.guid];
		GUILayout.Label ("Weight: " + cp.Cube.rigidbody.mass);
		GUILayout.Label ("Size: " + cp.Cube.transform.localScale.x + "x;" + cp.Cube.transform.localScale.y + "y;" + cp.Cube.transform.localScale.z + "z;");
		GUILayout.Label ("CanFly: " + cp.CanFly);
		GUILayout.Label ("Strength: " + cp.ForceMultiplier);
		GUILayout.Label ("Tile: " + AmApplication.MapTileDepth + "d; " + AmApplication.MapTileWidth + "w;");
		
	}
	
	/// <summary>
	/// Draws the match count down.
	/// </summary>
	/// <param name='windowID'>
	/// Window I.
	/// </param>
	void DrawMatchCountDown (int windowID)
	{
		GUILayout.Label (AmApplication.MatchCountDown.ToString ());
	}
	
	/// <summary>
	/// Draws the player power ups.
	/// </summary>
	/// <param name='windowID'>
	/// Window I.
	/// </param>
	void DrawPlayerPowerUps (int windowID)
	{
		ConnectedPlayer p = _playersList [Network.player.guid];
		// player name + value
		foreach (PowerUp pup in p.PowerUps) {
			GUILayout.Label (pup.Name + ":" + pup.RemainingTime);
		}
	}
	
	/// <summary>
	/// Draws the waiting room window.
	/// </summary>
	/// <param name='windowID'>
	/// Window I.
	/// </param>
	void DrawWaitingRoomWindow (int windowID)
	{		
		Score (GameObject.Find (
					AmApplication.GAMEOBJECT_MAP_GENERATOR_NAME)
					.GetComponent<PlayerManager> ()
					.GetPlayersList (), 
					true
			);
	}
	
	/// <summary>
	/// Draws the score panel.
	/// </summary>
	/// <param name='windowID'>
	/// Window I.
	/// </param>
	void DrawScorePanel (int windowID)
	{		
		Score (GameObject.Find (
					AmApplication.GAMEOBJECT_MAP_GENERATOR_NAME)
					.GetComponent<PlayerManager> ()
					.GetPlayersList (), 
					false
			);
	}	
	
	/// <summary>
	/// Draws the server stats.
	/// </summary>
	void DrawServerStats (int windowID)
	{
		GUILayout.Label ("IP Address: " + _serverIPAddress);
		GUILayout.Label ("Connections: " + Network.connections.Length);		
	}
	
	/// <summary>
	/// Score the specified playersList and isWaitingRoom.
	/// </summary>
	/// <param name='playersList'>
	/// Players list.
	/// </param>
	/// <param name='isWaitingRoom'>
	/// Is waiting room.
	/// </param>
	public void Score (SortedList<string, ConnectedPlayer> playersList, bool isWaitingRoom)
	{
		string tabs = isWaitingRoom ? WAITING_ROOM_TABS : "\t";
		
		// player name + value
		foreach (KeyValuePair<string, ConnectedPlayer> p in playersList) {
			GUILayout.Label (p.Value.Name + tabs + p.Value.Score);
		}		
	}
}