using UnityEngine;
using System.Collections;

namespace Amucuga{
/// <summary>
/// Static class in order to allow saving player settings (Using the Unity PlayerPref class)
/// </summary>
/// 
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
	
}
}