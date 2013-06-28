using UnityEngine;
using System.Collections;
using Amucuga;

public class rotate : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
        gameObject.transform.rotation *= new Quaternion(0, Mathf.PI / 4 * Time.deltaTime, 0, 1);
	}
}
