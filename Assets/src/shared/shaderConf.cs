using UnityEngine;
using System.Collections;

public class shaderConf : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 cameraDirection = Camera.main.transform.forward;
        Vector3 lightDirection = GameObject.Find("DirLight").transform.forward;
        gameObject.renderer.material.SetVector("_CameraDirection", new Vector4(cameraDirection.x, cameraDirection.y, cameraDirection.z, 0));
        gameObject.renderer.material.SetVector("_LightDirection", new Vector4(lightDirection.x, lightDirection.y, lightDirection.z, 0));
	}
}
