using System;
using UnityEngine;

namespace Amucuga
{
	public class ConnectedPlayer
	{
		public string Name {get; set;}				// displayed player name
		
		public int Score {get; set;}				// player score
		
		public GameObject Cube { get; set; }	// the cube unity object
	
		public NetworkPlayer NPlayer { get; set; }
		
		public ConnectedPlayer (GameObject playerCube, string playerName, NetworkPlayer networkPlayer)
		{
			Score = 0;			//reset the score
			Cube = playerCube;	//
			Name = playerName;
			NPlayer = networkPlayer;
		}
	}
}