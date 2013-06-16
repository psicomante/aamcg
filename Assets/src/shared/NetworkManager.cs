using UnityEngine;
using System.Collections;
using System.Net;
using Amucuga;

public class NetworkManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		//TODO: maybe move this code to OnDisconnect event.
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
		string HostName = Dns.GetHostName ();
		print (HostName);
		
		IPAddress[] ipAddresses = System.Net.Dns.GetHostAddresses (HostName);
		return ipAddresses[0].ToString();
		/*
		IPHostEntry ipEntry = System.Net.Dns.GetHostEntry (HostName);
		
		IPAddress[] ipAddressesEntry = ipEntry.AddressList;
		for (int i = 0; i < ipAddressesEntry.Length; i++)
			print (ipAddressesEntry [i].ToString ());			
		return HostName;*/		
	}
	
	
}
