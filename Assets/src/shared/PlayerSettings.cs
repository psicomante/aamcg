using UnityEngine;
using System.Collections;

namespace Amucuga
{

    /// <summary>
    /// Static class in order to allow saving player settings (Using the Unity PlayerPref class)
    /// </summary>
    public static class PlayerSettings
    {
        /// <summary>
        /// Sets or gets the Host
        /// </summary>
        public static string Host
        {
            set
            {
                PlayerPrefs.SetString(AmApplication.PLAYERPREF_HOST_KEY, value);
                PlayerPrefs.Save();
            }

            get
            {
                return PlayerPrefs.GetString(AmApplication.PLAYERPREF_HOST_KEY, AmApplication.DEFAULT_SERVER_HOST);
            }
        }

        /// <summary>
        /// Sets or gets the Player Name
        /// </summary>
        public static string PlayerName
        {
            get
            {
                return PlayerPrefs.GetString(AmApplication.PLAYERPREF_PLAYER_NAME_KEY, AmApplication.DEFAULT_PLAYER_NAME + "_" + (int)(Random.value * 1000)); 
            }

            set
            {
                PlayerPrefs.SetString(AmApplication.PLAYERPREF_PLAYER_NAME_KEY, value);
                PlayerPrefs.Save();
            }
        }

        /// <summary>
        /// Sets or get the port
        /// </summary>
        public static int Port
        {
            get
            {
                return PlayerPrefs.GetInt(AmApplication.PLAYERPREF_PORT_KEY, AmApplication.DEFAULT_SERVER_PORT);
            }

            set
            {
                PlayerPrefs.SetInt(AmApplication.PLAYERPREF_PORT_KEY, value);
                PlayerPrefs.Save();
            }
        }

        /// <summary>
        /// Sets or get wether the server is dedicated or not
        /// </summary>
        public static bool DedicatedServer {
			get {
				return PlayerPrefs.GetInt (AmApplication.PLAYERPREF_DEDICATED_SERVER_KEY, AmApplication.DEFAULT_DEDICATED_SERVER ? 1 : 0) == 0 ? false : true;
			}

			set {
				PlayerPrefs.SetInt (AmApplication.PLAYERPREF_DEDICATED_SERVER_KEY, value == true ? 1 : 0);
				PlayerPrefs.Save ();
			}
		}
		
		public static float MatchDuration {
			get {
				return PlayerPrefs.GetFloat (AmApplication.PLAYERPREF_MATCH_DURATION_KEY, AmApplication.DEFAULT_MATCH_DURATION);
			}
			
			set {
				PlayerPrefs.SetFloat (AmApplication.PLAYERPREF_MATCH_DURATION_KEY, value);
				PlayerPrefs.Save ();				
			}
		}
		
    }
}