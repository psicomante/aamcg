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
	private ConnectedPlayer _player;

    /// <summary>
    /// Start the Game GUI
    /// </summary>
    void Start ()
	{
		Debug.Log ("GameGui.cs Started");
		
		//Blocks client execution
		if (!Network.isServer)
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
		// pause menu toggle
		if (Input.GetKeyUp (KeyCode.Escape) || Input.GetKey (KeyCode.Menu)) {
			Debug.Log("EscKey");
			_escPressed = !_escPressed;
		}
		
		// reference to the current ConnectedPlayer
		if (Network.isClient)
			_player = GameObject.Find (AmApplication.GAMEOBJECT_MAP_GENERATOR_NAME).GetComponent<C_PlayerManager> ().Player;
		
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
				// Player Scores
				windowRect = new Rect (Screen.width - SCOREBOX_WIDTH - SCOREBOX_RIGHTMARGIN, SCOREBOX_TOPMARGIN, SCOREBOX_WIDTH, SCORE_HEIGHT_LABEL * (Network.connections.Length) + 50);
				windowRect = GUI.Window (2, windowRect, DrawScorePanel, "Scores");		
				
				// show IP Address and Connections (not in waiting room)
				windowRect = new Rect (10, 10, 200, 80);
				windowRect = GUI.Window (1, windowRect, DrawServerStats, "Server Stats");			
			}
			// Waiting Room
			if (AmApplication.CurrentMatchState == MatchState.WAITING_ROOM) {
				// Match Final Score
				windowRect = new Rect (_scoreWindowLeftMargin, _scoreWindowTopMargin, Screen.width - 100, Screen.height - 40);
				windowRect = GUI.Window (2, windowRect, DrawWaitingRoomWindow, "End Game : Scores");
			}
		}
		
		// CLIENT GUI
		if (Network.isClient) {
			// player stats
			windowRect = new Rect (SCOREBOX_RIGHTMARGIN + SCOREBOX_WIDTH, SCOREBOX_TOPMARGIN + 30, SCOREBOX_WIDTH, SCOREBOX_HEIGHT * 5);
			windowRect = GUI.Window (4, windowRect, DrawPlayerStatistics, "Player Stats");		
			
			// active powerups panel
			if (_player.PowerUps.Count > 0) {
				windowRect = new Rect (SCOREBOX_RIGHTMARGIN, SCOREBOX_TOPMARGIN + 30, SCOREBOX_WIDTH, SCORE_HEIGHT_LABEL * (_player.PowerUps.Count) + 20);
				windowRect = GUI.Window (3, windowRect, DrawPlayerPowerUps, "PowerUps");	
			}			
		}
		
		// Match CountDown
		windowRect = new Rect (Screen.width - 120, 10, 100, 50);
		windowRect = GUI.Window (0, windowRect, DrawMatchCountDown, "CountDown");	

		// show Logout
		if (_escPressed && GUI.Button (new Rect (Screen.width / 2 - 150, Screen.height / 2 - 150, 300, 300), "Logout")) {
			NetworkManager.Shutdown ();
			AmApplication.LoadMainMenu ();
		}
	}
	
	void DrawPlayerStatistics (int windowID)
	{
		GUILayout.Label ("Weight: " + _player.Cube.GetComponent<Rigidbody>().mass);
		GUILayout.Label ("Size: " + _player.Cube.transform.localScale.x + "x;" + _player.Cube.transform.localScale.y + "y;" + _player.Cube.transform.localScale.z + "z;");
		GUILayout.Label ("CanFly: " + _player.CanFly);
		GUILayout.Label ("Strength: " + _player.ForceMultiplier);
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
		// player name + value
		foreach (PowerUp pup in _player.PowerUps) {
			GUILayout.Label (pup.Name + ":" + pup.CountDown);
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
    void DrawScorePanel(int windowID)
    {
        Score(GameObject.Find(
                    AmApplication.GAMEOBJECT_MAP_GENERATOR_NAME)
                    .GetComponent<PlayerManager>()
                    .GetPlayersList(),
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