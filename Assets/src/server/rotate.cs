using UnityEngine;
using System.Collections;

public class rotate : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
        gameObject.transform.rotation *= new Quaternion(0, Mathf.PI / 3 * Time.deltaTime, 0, 1);
	}
}
