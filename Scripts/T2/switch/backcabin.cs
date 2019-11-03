using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class backcabin : MonoBehaviour {

    public GameObject vrcamera;
    Camera m_MainCamera;

    void Start()
    {
        m_MainCamera = Camera.main;

    }

    void OnMouseDown()
    {
        Debug.Log("back");
        m_MainCamera.transform.position = new Vector3(0, 1 -10);
        m_MainCamera.transform.rotation = Quaternion.Euler(0, 0, 0);
    }
    public void OnRayPointInSphere()
    {
        Debug.Log("back");
        vrcamera.transform.position = new Vector3(0, -0.3f, -10);
        vrcamera.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

}
	

