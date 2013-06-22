using UnityEngine;
using System.Collections;
using Amucuga;

public class MatchManager : MonoBehaviour {
	
	public GameObject GUIMatchCountDownPrefab;

	// Use this for initialization
	void Start ()
	{
		// initializes the matchcountdown
		AmApplication.MatchCountDown = PlayerSettings.MatchDuration;
		
		//
		GameObject.Instantiate(GUIMatchCountDownPrefab);
	}
	
	// Update is called once per frame
	void Update ()
	{
		//TODO: porcage to ease my life
		AmApplication.MatchCountDown -= Time.deltaTime;
		if (AmApplication.MatchCountDown <= 0 && Network.isServer)
			EndMatch ();
	}
	
	/// <summary>
	/// Ends the match. Should display the match riepilogue, reloads the level
	/// </summary>
	public void EndMatch () {
		AmApplication.MatchCountDown = PlayerSettings.MatchDuration;		
		GameObject.Find(AmApplication.GAMEOBJECT_MAP_GENERATOR_NAME).SendMessage("Reset");
	}
}
