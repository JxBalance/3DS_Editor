using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRotate : MonoBehaviour {

    public GameObject camera1;
    public GameObject camera2;

	// Use this for initialization
	void Start () {
        camera1 = GameObject.Find("Main Camera");
        camera2 = GameObject.Find("Camera (eye)");
	}
	
	// Update is called once per frame
	void Update () {
        if (camera1.activeInHierarchy == true)
        {
            transform.LookAt(camera1.transform);
            transform.Rotate(Vector3.right * 90);
        }
        else
        {
            transform.LookAt(camera2.transform);
            transform.Rotate(Vector3.right * 90);
        }
	}
}
