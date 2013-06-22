using System;
using System.Collections.Generic;
using UnityEngine;

namespace Amucuga
{
    /// <summary>
    /// A player connected to the server
    /// </summary>
	public class ConnectedPlayer : MonoBehaviour
	{
        private List<PowerUp> _powerUps;

        /// <summary>
        /// The player cube
        /// </summary>
        public GameObject Cube { get { return gameObject; } }
	
        /// <summary>
        /// The NetworkPlayer associated to the player
        /// </summary>
		public NetworkPlayer NPlayer { get; set; }
		
		/// <summary>
		/// The player name. Displayed on the player cube (on the server)
		/// </summary>
		public string Name { get; set; }
		
		/// <summary>
		/// The player score. Displayed in the server (on the player list), in the client (on the hud)
		/// </summary>
		public int Score {get; private set;}

        /// <summary>
        /// Indicates if the player can fly
        /// </summary>
        public bool CanFly { get; set; }

        /// <summary>
        /// Multiplies the unary force applied to the rigidbody
        /// </summary>
        public float ForceMultiplier { get; set; }

        /// <summary>
        /// Limits the speed of the player
        /// </summary>
        public float MaxVelocityMagnitude { get; set; }

        /// <summary>
        /// Destroys all the player objects
        /// </summary>
        public void Destroy()
        {
            GameObject.DestroyImmediate(Cube);
            Network.RemoveRPCs(NPlayer);
            Network.DestroyPlayerObjects(NPlayer);
        }

        /// <summary>
        /// Initialize the player
        /// </summary>
        public void Start()
        {
            _powerUps = new List<PowerUp>();
            ForceMultiplier = AmApplication.DEFAULT_FORCE_MULTIPLIER;
            CanFly = false;
            MaxVelocityMagnitude = AmApplication.DEFAULT_MAX_VELOCITY_MAGNITUDE;
        }

        /// <summary>
        /// Updates the player
        /// </summary>
        void Update()
        {
            List<PowerUp> mustDie = new List<PowerUp>();

            // Updates all the powerups in the player
            foreach (PowerUp p in _powerUps)
            {
                p.Update(Time.deltaTime);
                if (p.State == PowerUpState.DEAD)
                    mustDie.Add(p);
            }

            // Deletes all the powerups that must die
            foreach (PowerUp p in mustDie)
            {
                _powerUps.Remove(p);
            }
        }

        /// <summary>
        /// Collision detection
        /// </summary>
        /// <param name="collider"></param>
        void OnTriggerEnter(Collider powerUpCollider)
        {
            PowerUp powerUp = powerUpCollider.gameObject.GetComponent<PowerUpManager>().powerUp;
            AddPowerUp(powerUp);
            powerUp.CollectedByPlayer(this);
        }

        /// <summary>
        /// Adds a new powerup to the player
        /// </summary>
        /// <param name="powerUp"></param>
        public void AddPowerUp(PowerUp powerUp)
        {
            if (powerUp.IsCumulative)
                _powerUps.Add(powerUp);
            else
                ResetOrAddPowerUp(powerUp);
        }

        /// <summary>
        /// Adds a new powerup to the player, or resets a powerup if exists in the list.
        /// </summary>
        /// <param name="powerUp"></param>
        private void ResetOrAddPowerUp(PowerUp powerUp)
        {
            bool contains = false;
            foreach (PowerUp p in _powerUps)
            {
                if (p == powerUp)
                {
                    p.Reset();
                    contains = true;
                    break;
                }
            }
            if (!contains)
            {
                _powerUps.Add(powerUp);
            }
        }
	}
}