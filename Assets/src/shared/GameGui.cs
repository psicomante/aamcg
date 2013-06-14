using UnityEngine;
using System.Collections;
using Amucuga;

/// <summary>
/// GUI for server in the Game Scene
/// </summary>
public class GameGui : MonoBehaviour {

	/// <summary>
	/// Start the Game GUI
	/// </summary>
	void Start () {
	    //Blocks client execution
        if (Network.isClient)
            return;
	}

    /// <summary>
    /// Renders the GUI
    /// </summary>
    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 60, 180, 25), "Logout"))
        {
            Network.Disconnect(500);
            Application.LoadLevel(AmApplication.MAIN_MENU);
        }
    }
}
