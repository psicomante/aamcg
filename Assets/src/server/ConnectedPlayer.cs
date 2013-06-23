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
        private const float LAST_TOUCHED_DURATION = 5;
        private const float KILL_COMBO_DURATION = 5;
        
        /// <summary>
        /// The current player score (modified by getter and setter)
        /// </summary>
        private int _score;

        /// <summary>
        /// The list of active powerups collected by player
        /// </summary>
        private List<PowerUp> _powerUps;

        /// <summary>
        /// A counter for last touched player
        /// </summary>
        private float _touchCounter;

        /// <summary>
        /// A counter for kill combo (multikill)
        /// </summary>
        private float _killComboCounter;

        /// <summary>
        /// A storage for the combo score
        /// </summary>
        private int _comboScoreTemp;

        /// <summary>
        /// The current multiplier for the combo score
        /// </summary>
        private int _comboScoreMultiplier;

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
		public int Score 
        {
            get
            {
                return _score;
            }

            private set
            {
                CreateScoreText(value - _score);
                _score = value;
            }
        }

        /// <summary>
        /// Indicates if the player can fly
        /// </summary>
        public bool CanFly { get; set; }

        /// <summary>
        /// Multiplies the unary force applied to the rigidbody
        /// </summary>
        public float ForceMultiplier { get; set; }

        /// <summary>
        /// The last player that touched this player
        /// </summary>
        public ConnectedPlayer LastTouched { get; private set; }

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
        public void Start ()
		{
			// resets score
			Score = 0;
			
			_powerUps = new List<PowerUp> ();
			ForceMultiplier = AmApplication.DEFAULT_FORCE_MULTIPLIER;
			CanFly = false;
            LastTouched = null;
            _touchCounter = 0;
            _comboScoreTemp = 0;
            _comboScoreMultiplier = 0;
            _killComboCounter = KILL_COMBO_DURATION;
		}

        /// <summary>
        /// Updates the player
        /// </summary>
        void Update()
        {
            // blocks wrong execution
            if (!Network.isServer || AmApplication.CurrentMatchState != MatchState.MATCH)
                return;

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

            UpdateCounters();
        }

        /// <summary>
        /// Updates all the counters of the player logic
        /// </summary>
        private void UpdateCounters()
        {
            // Updates the last touched counter
            _touchCounter -= Time.deltaTime;
            if (_touchCounter <= 0)
            {
                LastTouched = null;
                _touchCounter = LAST_TOUCHED_DURATION;
            }

            // Updates the score
            _killComboCounter -= Time.deltaTime;
            if (_killComboCounter <= 0)
            {
                UpdateScore();
            }
        }

        /// <summary>
        /// Update the score and empty the score temp
        /// </summary>
        private void UpdateScore()
        {
            Score += _comboScoreTemp * _comboScoreMultiplier;
            _comboScoreTemp = 0;
            _comboScoreMultiplier = 0;
            _killComboCounter = KILL_COMBO_DURATION;
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
            Score += 1;
        }

        /// <summary>
        /// Adds a new powerup to the player, or resets a powerup if exists in the list.
        /// </summary>
        /// <param name="powerUp"></param>
        private void ResetOrAddPowerUp (PowerUp powerUp)
		{
			bool contains = false;
			foreach (PowerUp p in _powerUps) {
				if (p == powerUp) {
					p.Reset ();
					contains = true;
					break;
				}
			}
			if (!contains) {
				_powerUps.Add (powerUp);
			}
		}
		
		/// <summary>
		/// Resets the score at the end of the match
		/// </summary>
		public void ResetScore ()
		{
			Score = 0;
		}

        /// <summary>
        /// The player respawns.
        /// </summary>
        public void OnRespawn()
        {   
            // Killed by another player
            if (LastTouched != null)
            {
                LastTouched.OnKill(this);
                UpdateScore();
                Score -= 20;
            }

            // Suicided
            else
            {
                UpdateScore();
                Score -= 30;
            }
        }

        /// <summary>
        /// This player just killed another player
        /// </summary>
        /// <param name="connectedPlayer">The killed player</param>
        private void OnKill(ConnectedPlayer connectedPlayer)
        {
            // The points for the kill are storage (for potential multiKill)
            _comboScoreTemp += 100;

            // Multikill multiplier increased by 1
            _comboScoreMultiplier += 1;
        }

        /// <summary>
        /// Creates a score text over the player
        /// </summary>
        private void CreateScoreText(int score)
        {
            if (score != 0)
            {
                Debug.Log(Name + " scored: " + score);
                Debug.LogError("TODO: Implement CreateScoreText");
            }
        }

        /// <summary>
        /// Updates collision
        /// </summary>
        /// <param name="collision"></param>
        void OnCollisionEnter(Collision collision)
        {
            if (gameObject.transform.position.y > 0)
            {
                ConnectedPlayer touchedPlayer = collision.gameObject.GetComponent<ConnectedPlayer>();
                if (touchedPlayer != null)
                {
                    LastTouched = touchedPlayer;
                    _touchCounter = LAST_TOUCHED_DURATION;
                }
            }
        }

        /// <summary>
        /// Generates an explosion
        /// </summary>
        public void OnGenerateExplosion()
        {
            Camera.main.GetComponent<PlayerManager>().OnGenerateExplosion(NPlayer.guid);
        }

        /// <summary>
        /// Resets all the powerups
        /// </summary>
        public void ResetPowerUps()
        {
            foreach (PowerUp p in _powerUps)
            {
                p.TerminateImmediate();
            }
        }
		
		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="Amucuga.ConnectedPlayer"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents the current <see cref="Amucuga.ConnectedPlayer"/>.
		/// </returns>
		public override string ToString ()
		{
			return (Name + " \t " + Score);
		}
    }
}