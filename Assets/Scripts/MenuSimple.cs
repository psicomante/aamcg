using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Amucuga;

/**
 * 
 * @author Roberto Pesando <https://github.com/psicomante>
 * @author Ruben Caliandro <https://github.com/Chosko>
 * 
 */ 
public class MenuSimple : MonoBehaviour
{
	private int maxConnections = 10;
	private string serverPassword = "Psicomante";
	private int serverPort = 25001;	
	
	public const string DEFAULT_SERVER_IP = "127.0.0.1";
	public string serverIP = DEFAULT_SERVER_IP;
	public GameObject playerPrefab;
	public ConnectedPlayer[] cubePlayers = new ConnectedPlayer[10];
	private int playerCount = 0;
	private int playerNumber = -1;
	private float forceMultiplier = 20;
	//public string username = "";
	//bool RegisterUI = false;
	//bool LoginUI = false;	
	
	// GUI Variables
	private bool _loginUI = false;	//save the click on the login menu
	private string _username = "";				//save the login name
	
	void createPlayer (string name, NetworkPlayer np)
	{
		//GameObject cube = GameObject.CreatePrimitive (PrimitiveType.Cube);
		//cube.AddComponent<Rigidbody> ();
		GameObject playerInstance = (GameObject)GameObject.Instantiate(playerPrefab, new Vector3 (0, playerCount, 0), Quaternion.identity);
		cubePlayers [playerCount - 1] = new ConnectedPlayer (playerInstance, name, np);
	}
	
	/**
	 * Returns the connected player index
	 */
	int findPlayer (string guid)
	{
		for (int i = 0; i < playerCount; i++) {
			if (guid == cubePlayers[i].NPlayer.guid)
				return i; 
		}
		return -1;	
	}
	
	int destroyPlayer (NetworkPlayer np)
	{
		int playerIndex = findPlayer (np.guid);
		if (playerIndex >= 0) {
			Network.RemoveRPCs(np);
			Network.DestroyPlayerObjects(np);
			GameObject.DestroyImmediate (cubePlayers [playerIndex].Cube);
			playerCount--;
			cubePlayers[playerIndex] = null;
		}
		return playerIndex;
			
	}	
	/**
	 * Unity Event - launches on Player Connection
	 * 
	 * @see createPlayer
	 */
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
	void ConnectToServer (string serverIP = DEFAULT_SERVER_IP)
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
		_username = GUI.TextArea (new Rect (100, 125, 110, 25), _username = "Player");
		serverIP = GUI.TextArea (new Rect (100, 150, 110, 25), serverIP);
		if (GUILayout.Button ("Login")) {
			ConnectToServer (serverIP);
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
					networkView.RPC ("ChangeColor", RPCMode.Server, Network.player.guid);
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
	/**
	 * Unity Event
	 */
	void FixedUpdate ()
	{
		if(Network.peerType == NetworkPeerType.Disconnected)
			return;
		float keyboardGravity = -9.81f;
		
		if (Input.GetKey("space"))
		{
			keyboardGravity*=-1;
		}
		
		if (Input.GetKey("s"))
		{
			networkView.RPC("ChangeForce", RPCMode.Server, Network.player.guid, Vector3.back * forceMultiplier);
			//rigidbody.AddTorque(Vector3.back * rollSpeed);
		}
		 
		if (Input.GetKey("w"))
		{
			networkView.RPC("ChangeForce", RPCMode.Server, Network.player.guid, Vector3.forward * forceMultiplier);
			//rigidbody.AddTorque(Vector3.forward * rollSpeed);
		}
		 
		if (Input.GetKey("d"))
		{
			networkView.RPC("ChangeForce", RPCMode.Server, Network.player.guid, Vector3.right * forceMultiplier);
			//rigidbody.AddTorque(Vector3.right * rollSpeed);
		}
		 
		if (Input.GetKey("a"))
		{
			networkView.RPC("ChangeForce", RPCMode.Server, Network.player.guid, Vector3.left * forceMultiplier);
			//rigidbody.AddTorque(Vector3.left * rollSpeed);
		}//rigidbody.AddTorque(Vector3.left * rollSpeed);
		
		if(Input.acceleration.magnitude != 0)
		{
			networkView.RPC("ChangeForce", RPCMode.Server, Network.player.guid, Input.acceleration);
		}
		else
		{
			networkView.RPC("ChangeForce", RPCMode.Server, Network.player.guid, new Vector3(0,keyboardGravity,0));
		}
	}


	[RPC]
	void ChangeColor (string guid)
	{
		
		cubePlayers[findPlayer(guid)].Cube.renderer.material.color = new Color (Random.value, Random.value, Random.value);
	}
	
	[RPC]
	void ChangeForce (string guid, Vector3 force)
	{
		findPlayer(guid);
		cubePlayers[findPlayer(guid)].Cube.rigidbody.AddForce(force);
		Debug.Log(force.ToString());
	}
}
