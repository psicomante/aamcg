using UnityEngine;
using System.Collections;

namespace Amucuga{
	
	/// <summary>
	/// Static class in order to allow saving player settings (Using the Unity PlayerPref class)
	/// </summary>
	public static class PlayerSettings {
	
	/// <summary>
	/// Saves the host set in the MainMenuGui
	/// </summary>
	/// <param name='serverHost'>
	/// Server host written in the GUI
	/// </param>
	
		public static void SetHost (string serverHost)
		{
			PlayerPrefs.SetString (AmApplication.PLAYERPREF_HOST_KEY, serverHost);
			PlayerPrefs.Save ();
		}
	
		/// <summary>
		/// Gets the host from PlayerPrefs if exists. Otherwise return the default value for Host.
		/// </summary>
		/// <returns>
		/// The host saved in the players prefs.
		/// </returns>
		public static string GetHost(){
			return PlayerPrefs.GetString (AmApplication.PLAYERPREF_HOST_KEY, AmApplication.DEFAULT_SERVER_HOST);
		}
	
		/// <summary>
		/// Sets the name of the player.
		/// </summary>
		/// <param name='playerName'>
		/// Player name.
		/// </param>
		public static void SetPlayerName (string playerName)
		{
			PlayerPrefs.SetString (AmApplication.PLAYERPREF_PLAYER_NAME_KEY, playerName);
			PlayerPrefs.Save ();			
		}
		
		/// <summary>
		/// Gets the name of the player.
		/// </summary>
		/// <returns>
		/// The player name.
		/// </returns>
		public static string GetPlayerName ()
		{
			return PlayerPrefs.GetString (AmApplication.PLAYERPREF_PLAYER_NAME_KEY, AmApplication.DEFAULT_PLAYER_NAME + "_" + (int)(Random.value * 1000));
		}
		
		public static void SetPort (int portNumber)
		{
			PlayerPrefs.SetInt (AmApplication.PLAYERPREF_PORT_KEY, portNumber);
			PlayerPrefs.Save ();	
		}
		
		public static int GetPort () {
			return PlayerPrefs.GetInt (AmApplication.PLAYERPREF_PORT_KEY, AmApplication.DEFAULT_SERVER_PORT);			
		}
		
	

	}
}
