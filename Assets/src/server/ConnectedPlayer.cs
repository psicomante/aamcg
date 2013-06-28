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
        public List<PowerUp> PowerUps {get; private set;}

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
        /// A counter for updating the client status.
        /// </summary>
        private float _clientUpdateCounter;

        /// <summary>
        /// The main networkView
        /// </summary>
        private NetworkView _networkView;

        /// <summary>
        /// The player cube
        /// </summary>
        public GameObject Cube { get { return gameObject; } }
	
        /// <summary>
        /// The NetworkPlayer associated to the player
        /// </summary>
		public NetworkPlayer NPlayer { get; set; }

        /// <summary>
        /// Indicates wether or not this ConnectedPlayer instance can send RPCs
        /// </summary>
        public bool CanSendRPC
        {
            get
            {
                return Network.isServer && (Network.player.guid != NPlayer.guid);
            }
        }

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

            set
            {
                _score = value;
                CreateScoreText(value - _score);
                RPC("UpdateScore", _score);
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
        public ConnectedPlayer LastTouched { get; set; }

        /// <summary>
        /// Destroys all the player objects
        /// </summary>
        public void Destroy()
        {
            TerminatePowerUps();
            GameObject.DestroyImmediate(Cube);
            Network.RemoveRPCs(NPlayer);
            Network.DestroyPlayerObjects(NPlayer);
        }

        /// <summary>
        /// Initialize the player
        /// </summary>
        public void Start ()
		{
			PowerUps = new List<PowerUp> ();
			ForceMultiplier = AmApplication.DEFAULT_FORCE_MULTIPLIER;
			CanFly = false;
            LastTouched = null;
            _touchCounter = 0;
            _comboScoreTemp = 0;
            _comboScoreMultiplier = 0;
            _killComboCounter = KILL_COMBO_DURATION;
            _networkView = GameObject.Find(AmApplication.GAMEOBJECT_MAP_GENERATOR_NAME).GetComponent<NetworkView>();
            _clientUpdateCounter = AmApplication.CLIENT_UPDATE_TIME;
            Debug.Log("Network view imported: " + _networkView);
            // resets score
            Score = 0;
		}

        /// <summary>
        /// Updates the player
        /// </summary>
        void Update()
        {
            // blocks update if not in match state
            if (AmApplication.CurrentMatchState != MatchState.MATCH)
                return;
            
            // The powerups are updated also by the client
            UpdatePowerUps();

            // blocks wrong execution
            if (!Network.isServer)
                return;

            UpdateCounters();
        }

        /// <summary>
        /// Updates the powerups
        /// </summary>
        private void UpdatePowerUps()
        {
            List<PowerUp> mustDie = new List<PowerUp>();

            // Updates all the powerups in the player
            foreach (PowerUp p in PowerUps)
            {
                p.Update(Time.deltaTime);
                if (p.State == PowerUpState.DEAD)
                    mustDie.Add(p);
            }

            // Deletes all the powerups that must die
            foreach (PowerUp p in mustDie)
            {
                PowerUps.Remove(p);
            }
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

            // Updates the client status
            _clientUpdateCounter -= Time.deltaTime;
            if (_clientUpdateCounter <= 0)
            {
                _clientUpdateCounter = AmApplication.CLIENT_UPDATE_TIME;
                UpdateClientPowerUps();
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
            RPC("NewPowerUpCollected", powerUp.GetType().ToString());
        }

        /// <summary>
        /// Adds a new powerup to the player
        /// </summary>
        /// <param name="powerUp"></param>
        public void AddPowerUp(PowerUp powerUp)
        {
            if (powerUp.IsCumulative)
                PowerUps.Add(powerUp);
            else
                ResetOrAddPowerUp(powerUp);

            // Only the server can update the score, because the client receives score updates via RPC
            if(Network.isServer)
                Score += 1;
        }

        /// <summary>
        /// Adds a new powerup to the player, or resets a powerup if exists in the list.
        /// </summary>
        /// <param name="powerUp"></param>
        private void ResetOrAddPowerUp (PowerUp powerUp)
		{
			bool contains = false;
			foreach (PowerUp p in PowerUps) {
				if (p == powerUp) {
					p.Reset ();
					contains = true;
					break;
				}
			}
			if (!contains) {
				PowerUps.Add (powerUp);
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
            //blocks wrong execution
            if (!Network.isServer)
                return;

            Camera.main.GetComponent<PlayerManager>().OnGenerateExplosion(NPlayer.guid);
        }

        /// <summary>
        /// Resets all the powerups
        /// </summary>
        public void TerminatePowerUps()
        {
            foreach (PowerUp p in PowerUps)
            {
                p.TerminateImmediate();
            }
        }

        /// <summary>
        /// Resets and instantiate a new list of powerups
        /// </summary>
        public void EmptyPowerUps()
        {
            TerminatePowerUps();
            PowerUps.RemoveRange(0, PowerUps.Count);
        }

        /// <summary>
        /// Updates the status of the client
        /// </summary>
        private void UpdateClientPowerUps()
        {
            string serializedPowerUpTypes = "";
            string serializedPowerUpCountDowns = "";
            for (int i = 0; i < PowerUps.Count; i++)
            {
                if (i > 0)
                {
                    serializedPowerUpTypes += ",";
                    serializedPowerUpCountDowns += ",";
                }
                serializedPowerUpTypes += PowerUps[i].GetType().ToString();
                serializedPowerUpCountDowns += PowerUps[i].CountDown.ToString();
            }
            RPC("UpdatePowerUps", serializedPowerUpTypes, serializedPowerUpCountDowns);
        }

        /// <summary>
        /// Sends an RPC call to the client associated to this instance of ConnectedPlayer
        /// </summary>
        /// <param name="type">The type of the powerup</param>
        private void RPC(string methodName, params object[] args)
        {
            if(CanSendRPC)
                _networkView.RPC(methodName, NPlayer, args);
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