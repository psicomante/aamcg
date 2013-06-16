using UnityEngine;
using System.Collections;
using Amucuga;

/// <summary>
/// GUI for server in the Game Scene
/// </summary>
public class GameGui : MonoBehaviour {
	
	private string _serverIPAddress;

	/// <summary>
	/// Start the Game GUI
	/// </summary>
	void Start ()
	{
		//Blocks client execution
		if (Network.isClient)
			return;
		
		_serverIPAddress = NetworkManager.GetLocalIPAddress();
		
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
	}
}
