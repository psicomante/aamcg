using System;
using UnityEngine;

namespace Amocogo
{
	public class ConnectedPlayer
	{
		public string Name {get; set;}				// displayed player name
		
		public int Score {get; set;}				// player score
		
		public GameObject CubePrefab { get; set; }	// the cube unity object
	
		public NetworkPlayer Np { get; set; }
		
		public ConnectedPlayer (GameObject ca, string n, NetworkPlayer np)
		{
			Score = 0;			//reset the score
			CubePrefab = ca;	//
			Name = n;
			Np = np;
		}
	}
}

/*		public string Name {
			get {
				return _name;
			}
			private set {
				_name = value;
			}
		}
			 */