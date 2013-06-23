using UnityEngine;
using System.Collections;
using Amucuga;

public class rotate : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
        // Blocks wrong execution
        if (!Network.isServer || AmApplication.CurrentMatchState != MatchState.MATCH)
            return;

        gameObject.transform.rotation *= new Quaternion(0, Mathf.PI / 3 * Time.deltaTime, 0, 1);
	}
}
