using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using AAMCG;


public class MenuSimple : MonoBehaviour
{
	private int maxConnections = 10;
	private string serverPassword = "Psicomante";
	private int serverPort = 25001;	
	
	public const string defaultServerIP = "127.0.0.1";
	public GameObject target;
	public GameObject playerPrefab;
	public ConnectedPlayer[] cubePlayers = new ConnectedPlayer[10];
	private int playerCount = 0;
	//public string username = "";
	//bool RegisterUI = false;
	//bool LoginUI = false;	
	
	// GUI Variables
	private bool _loginUI = false;	//save the click on the login menu
	private string _username = "";				//save the login name
	
	void createPlayer (string name, NetworkPlayer np)
	{
		GameObject cube = GameObject.CreatePrimitive (PrimitiveType.Cube);
		cube.AddComponent<Rigidbody> ();
		cube.transform.position = new Vector3 (0, playerCount, 0);
		Debug.Log (cube.transform.position.y);
		
		cubePlayers [playerCount - 1] = new ConnectedPlayer (cube, name, np);
	}
	
	/**
	 * Returns the connected player index
	 */
	int findPlayer (NetworkPlayer np)
	{
		for (int i = 0; i < playerCount; i++) {
			if (np.guid == cubePlayers[i].Np.guid)
				return i; 
		}
		return -1;	
	}
	
	int destroyPlayer (NetworkPlayer np)
	{
		int playerIndex = findPlayer (np);
		if (playerIndex >= 0) {
			GameObject.DestroyImmediate (cubePlayers [playerIndex].CubePrefab);
		}
		return playerIndex;
			
	}	

	void OnPlayerConnected (NetworkPlayer player)
	{
		string name = "Player-" + playerCount++;
		createPlayer (name, player);
		Debug.Log (name + " connected from " + player.ipAddress + ":" + player.port);
	}
	
	void OnPlayerDisconnected (NetworkPlayer player)
	{
		int i = destroyPlayer (player);
		Debug.Log(cubePlayers[i].Name + " has disconnected");
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
	
	void createGameModeMenu ()
	{
		// Wrap everything in the designated GUI Area
		GUILayout.BeginArea (new Rect (0, 0, 200, 200));

		// Begin the singular Horizontal Group
		//GUILayout.BeginHorizontal();

		// Place a Button normally
		//if (GUILayout.RepeatButton ("Increase max\nSlider Value"))
		//{
		//	maxSliderValue += 3.0f * Time.deltaTime;
		//}

		// Arrange two more Controls vertically beside the Button
		GUILayout.BeginVertical ();
		GUILayout.Box ("Select a Mode");
		if (GUILayout.Button ("Start Client"))
			_loginUI = true;
		if (GUILayout.Button ("Start Server"))
			LaunchServer ();
		
		// End the Groups and Area
		GUILayout.EndVertical ();
		//GUILayout.EndHorizontal();
		GUILayout.EndArea ();
	}
	
	public void createLoginGUI ()
	{
		_username = GUI.TextArea (new Rect (100, 125, 110, 25), _username);
					
		if (GUILayout.Button ("Login")) {
			ConnectToServer ();
			_loginUI = false;
		}
	}
	
	void OnGUI ()
	{		
		if (Network.peerType == NetworkPeerType.Disconnected && !_loginUI) {
			createGameModeMenu ();
		} else if (Network.peerType == NetworkPeerType.Disconnected && _loginUI) {
			createLoginGUI ();
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
