using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class CubePlayer
{
	private GameObject _cubeAvatar;
	private string _name;
	public string Name {
		get {
			return _name;
		}
		private set {
			_name = value;
		}
	}
	private NetworkPlayer _networkPlayer;
	
	public CubePlayer (GameObject ca, string n, NetworkPlayer np)
	{
		_cubeAvatar = ca;
		Name = n;
		_networkPlayer = np;
	}
}

public class MenuSimple : MonoBehaviour
{
	private int maxConnections = 10;
	private string serverPassword = "Psicomante";
	private int serverPort = 25001;	
	
	public const string defaultServerIP = "127.0.0.1";
	public GameObject target;
	public GameObject playerPrefab;
	public List<CubePlayer> cubePlayers = new List<CubePlayer>();
	private int playerCount = 0;
	//public string username = "";
	//bool RegisterUI = false;
	//bool LoginUI = false;	
	
	void createPlayer (string name, NetworkPlayer np)
	{
		GameObject cube = GameObject.CreatePrimitive (PrimitiveType.Cube);
		cube.AddComponent<Rigidbody> ();
		cube.transform.position = new Vector3 (0, playerCount, 0);
		Debug.Log (cube.transform.position.y);
		
		cubePlayers.Add (new CubePlayer (cube, name, np));
	}

	void OnPlayerConnected (NetworkPlayer player)
	{
		string name = "Player-" + playerCount++;
		createPlayer (name, player);
		Debug.Log (name + " connected from " + player.ipAddress + ":" + player.port);
	}

	
	/**
	 * Launches server
	 **/
	void LaunchServer ()
	{
		Network.incomingPassword = serverPassword;
		bool useNat = !Network.HavePublicAddress ();
		Network.InitializeServer (maxConnections, serverPort, useNat);
	}
	
	/**
	 * Used by the client to connect to the server
	 * 
	 * @param serverIP the server IP, discover
	 **/
	void ConnectToServer (string serverIP = defaultServerIP)
	{
		Network.Connect (serverIP, serverPort, serverPassword);
	}
	
	void OnGUI ()
	{
		if (Network.peerType == NetworkPeerType.Disconnected) {
			if (GUI.Button (new Rect (100, 100, 100, 25), "Start Client")) {
				string username = "Player";
				//username = GUI.TextArea (new Rect (100, 125, 110, 25), username);
					
				//if (GUI.Button (new Rect (100, 150, 110, 25), "Login")) {
					ConnectToServer ();
				//}				
			}
			if (GUI.Button (new Rect (100, 125, 100, 25), "Start Server")) {
				LaunchServer ();
			}
		} else {
			if (Network.peerType == NetworkPeerType.Client) {
				
				GUI.Label (new Rect (100, 100, 100, 25), "Client");
					
				if (GUI.Button (new Rect (100, 125, 110, 25), "Change Colour")) {
					networkView.RPC ("ChangeColor", RPCMode.Others);
				}
					
				if (GUI.Button (new Rect (100, 175, 110, 25), "Logout")) {
					Network.Disconnect (250);	
				}
			}
				
			if (Network.peerType == NetworkPeerType.Server) {
				GUI.Label (new Rect (100, 100, 100, 25), "Server");
				GUI.Label (new Rect (100, 125, 100, 25), "Connections: " + Network.connections.Length);
				
				if (GUI.Button (new Rect (100, 150, 100, 25), "Logout")) {
					Network.Disconnect (250);	
				}
			}
		}
		
		GUI.Label (new Rect (Screen.width - 100, Screen.height - 25, 100, 25), "v1.0");
	}

	[RPC]
	void ChangeColor ()
	{
		
		target.renderer.material.color = new Color (Random.value, Random.value, Random.value);
	}
	
}
