using UnityEngine;
using System.Collections.Generic;
using Amucuga;

/// <summary>
/// GUI for server in the Game Scene
/// </summary>
public class GameGui : MonoBehaviour {
	
	public GameObject GUIScorePrefab;	
	private GameObject _GUIScore;
	
	private string _serverIPAddress;
	private bool _displayScore = true;
	
	public const int SCOREBOX_WIDTH = 150;
	public const int SCOREBOX_HEIGHT = 50;
	public const int SCOREBOX_TOPMARGIN = 20;
	public const int SCOREBOX_RIGHTMARGIN = 10;

	/// <summary>
	/// Start the Game GUI
	/// </summary>
	void Start ()
	{
		//Blocks client execution
		if (Network.isClient)
			return;
		
		_GUIScore = (GameObject)GameObject.Instantiate (GUIScorePrefab);
		_serverIPAddress = NetworkManager.GetLocalIPAddress ();
		
	}

    /// <summary>
    /// Renders the GUI
    /// </summary>
    void OnGUI ()
	{
		// show IP Address and Connections
		if (Network.isServer) {
			GUILayout.Label ("IP Address: " + _serverIPAddress);
			GUILayout.Label ("Connections: " + Network.connections.Length);			
		}

		// show Logout
		if (GUI.Button (new Rect (10, 60, 180, 25), "Logout")) {
			NetworkManager.Shutdown ();
			AmApplication.LoadMainMenu ();
		}

		if (_displayScore && Network.isServer) {
			GUI.Box (new Rect (Screen.width - SCOREBOX_WIDTH - SCOREBOX_RIGHTMARGIN, SCOREBOX_TOPMARGIN, SCOREBOX_WIDTH, SCOREBOX_HEIGHT + Network.connections.Length * 20), "Score");
			PrintScore();
		}
			
	}
	
	void PrintScore ()
	{
		SortedList<string, ConnectedPlayer> playersList = GameObject.Find (AmApplication.GAMEOBJECT_MAP_GENERATOR_NAME).GetComponent<PlayerManager> ().GetPlayersList ();
		_GUIScore.guiText.text = "";
		foreach (KeyValuePair<string, ConnectedPlayer> p in playersList) {
			_GUIScore.guiText.text += p.Value.ToString () + "\n";
		}
	}

}