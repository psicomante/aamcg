using System;
using UnityEngine;

namespace Amucuga
{
    /// <summary>
    /// A player connected to the server
    /// </summary>
	public class ConnectedPlayer
	{
        /// <summary>
        /// The player cube
        /// </summary>
        public GameObject Cube { get; set; }
	
        /// <summary>
        /// The NetworkPlayer associated to the player
        /// </summary>
		public NetworkPlayer NPlayer { get; set; }
		
		/// <summary>
		/// The player name. Displayed on the player cube (on the server)
		/// </summary>
		public string Name { get; private set; }
		
		/// <summary>
		/// The player score. Displayed in the server (on the player list), in the client (on the hud)
		/// </summary>
		public int Score {get; private set;}
		
        /// <summary>
        /// Initializes a new instance of the ConnectedPlayer Class
        /// </summary>
        /// <param name="playerCube">The unity cube object associated to the player</param>
        /// <param name="networkPlayer">The NetworkPlayer associated to the player</param>
        public ConnectedPlayer (GameObject playerCube, NetworkPlayer networkPlayer)
		{
			Cube = playerCube;
			NPlayer = networkPlayer;
		}

        /// <summary>
        /// Destroys all the player objects
        /// </summary>
        public void Destroy()
        {
            GameObject.DestroyImmediate(Cube);
            Network.RemoveRPCs(NPlayer);
            Network.DestroyPlayerObjects(NPlayer);
        }
	}
}