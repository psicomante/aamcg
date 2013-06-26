using UnityEngine;
using System.Collections;
using System.Net;
using Amucuga;

/// <summary>
/// Network manager.
/// </summary>
/// <description>
/// Remember that Every object that sends or receives network messages 
/// requires a NetworkView component; You could just use one 
/// networkview for your entire game by referencing it from script, but that wouldn't make sense; in 
/// general just add a networkview per object that you want networked.
/// </description>
public class NetworkManager : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
		// blocks the sleep timeout on smartphones
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
		
		// if in game_scene and disconnected load main menu
		if (Application.loadedLevelName == AmApplication.GAME_SCENE && Network.peerType == NetworkPeerType.Disconnected)
			Application.LoadLevel(AmApplication.MAIN_MENU);
	}
	
	// Update is called once per frame
	void Update () {}
	
	//////////////////////////////////////////////////////////////////
	// UNITY BUILT-IN NETWORK EVENTS: CLIENT
	//////////////////////////////////////////////////////////////////
	
	/// <summary>
	/// Raises the failed to connect event.
	/// * CLIENT event
	/// </summary>
	/// <param name='error'>
	/// Error.
	/// </param>
	void OnFailedToConnect (NetworkConnectionError error)
	{
		Debug.Log ("Could not connect to server: " + error);
	}
	
	/// <summary>
	/// Raises the disconnected from server event.
	/// * CLIENT event (if we use MasterServer, SERVER too)
	/// </summary>
	/// <param name='info'>
	/// NetworkDisconnection Info
	/// </param>
	void OnDisconnectedFromServer (NetworkDisconnection info)
	{
		Debug.Log ("This CLIENT has disconnected from a server");	
		AmApplication.LoadMainMenu ();
	}
	
	/// <summary>
	/// Raises the connected to server event.
	/// * CLIENT event
	/// </summary>
	void OnConnectedToServer ()
	{
		Debug.Log ("Client connected to server - loading " + AmApplication.GAME_SCENE);
		Application.LoadLevel (AmApplication.GAME_SCENE);
		Debug.Log ("Scene loaded");
	}
	
	//////////////////////////////////////////////////////////////////
	// UNITY BUILT-IN NETWORK EVENTS: SERVER
	//////////////////////////////////////////////////////////////////
		
	/// <summary>
	/// Raises the server initialized event.
	/// * SERVER event
	/// </summary>
	void OnServerInitialized ()
	{
		Debug.Log ("Server initialized and ready - loading " + AmApplication.GAME_SCENE);
		Application.LoadLevel (AmApplication.GAME_SCENE);
		Debug.Log ("Connections: " + Network.connections.Length);
	}
	
	/// <summary>
	/// Raises the player connected event.
	/// * SERVER event
	/// </summary>
	/// <param name='player'>
	/// The Player connected
	/// </param>
	void OnPlayerConnected (NetworkPlayer player)
	{
		Debug.Log ("Player connected from: " + player.ipAddress + ":" + player.port + ":" + player.guid);
	}
	
	/// <summary>
	/// Raises the player disconnected event.
	/// </summary>
	/// <param name='player'>
	/// The player disconnected
	/// </param>
	void OnPlayerDisconnected(NetworkPlayer player) {
		Debug.Log("Player disconnected from: " + player.ipAddress+":" + player.port);
	}

	//////////////////////////////////////////////////////////////////
	// UNITY BUILT-IN NETWORK EVENTS: OTHERS
	//////////////////////////////////////////////////////////////////
	
	// at this time these are not used
	
	/// <summary>
	/// Raises the failed to connect to master server event.
	/// </summary>
	/// <param name='info'>
	/// Info.
	/// </param>
	void OnFailedToConnectToMasterServer (NetworkConnectionError info)
	{
		Debug.Log ("Could not connect to master server: " + info);
	}
	
	/// <summary>
	/// Raises the network instantiate event.
	/// </summary>
	/// <param name='info'>
	/// Info.
	/// </param>
	void OnNetworkInstantiate (NetworkMessageInfo info)
	{
		Debug.Log ("New object instantiated by " + info.sender);
	}
	
	/// <summary>
	/// Raises the serialize network view event.
	/// </summary>
	/// <param name='stream'>
	/// Stream.
	/// </param>
	/// <param name='info'>
	/// Info.
	/// </param>
	void OnSerializeNetworkView (BitStream stream, NetworkMessageInfo info)
	{
		Debug.Log ("Stream " + stream.ToString ());
		Debug.Log ("NetworkMessageInfo" + info);
	}	
	
	//////////////////////////////////////////////////////////////////
	// NETWORK MANAGER SPECIFIC FUNCTIONS
	//////////////////////////////////////////////////////////////////	
	
	/// <summary>
	/// Shutdown the entire Network Interface. 
	/// 
	/// Close all open connections and shuts down the network interface with a timout of 500ms
	/// http://docs.unity3d.com/Documentation/ScriptReference/Network.Disconnect.html
	/// </summary>
	public static void Shutdown ()
	{
		Network.Disconnect (500);
		
		// if we use MasterServer
		// MasterServer.UnregisterHost ();
	}
	
	/// <summary>
	/// Logout from server (client code?)
	/// </summary>
	public static void Logout ()
	{
		
	
	}
	
	/// <summary>
	/// Gets the server local IP Address.
	/// TODO: to get the external ip address you must query an external service (maybe the MasterServer)
	/// </summary>
	/// <returns>
	/// The local IP Address
	/// </returns>
	public static string GetLocalIPAddress ()
	{
		// gets local machine HostName
		string HostName = Dns.GetHostName ();		
		
		// gets ipAddresses list. I take the first one, maybe it would need a bit of refactoring.
		IPAddress[] ipAddresses = System.Net.Dns.GetHostAddresses (HostName);
		
		// return the first address on ip address list
		return ipAddresses [0].ToString ();	
	}
	
	
}
