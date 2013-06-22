using UnityEngine;
using System.Collections;

public class GUIMatchCountDown : MonoBehaviour {
	
	private float _lastUpdate;

	// Use this for initialization
	void Start ()
	{
		_lastUpdate = Time.time;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Time.time - Time.deltaTime > 1) {
			_lastUpdate = Time.time;
			gameObject.guiText.text = ((int)Amucuga.AmApplication.MatchCountDown).ToString ();
		}
	}
}
