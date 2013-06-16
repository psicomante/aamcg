using UnityEngine;
using System.Collections;
using Amucuga;

public class NetworkManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Network.peerType == NetworkPeerType.Disconnected) 
			AmApplication.LoadMainMenu ();
	
	}
	
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
	public static void Logout (){
		
	}
	
}
