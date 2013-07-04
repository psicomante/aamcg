using UnityEngine;
using System.Collections;

public class shaderConf : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        gameObject.renderer.material.SetVector("_CameraPosition", Camera.mainCamera.transform.position);
        //gameObject.renderer.material.SetVector("_LightDirection", GameObject.Find("DirLight").transform.forward);
	}
}
