using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiddleBoardCtrlT3 : MonoBehaviour {
    
    public GameObject fixedViewButton;
    public GameObject defaultViewButton;

    private Vector3 cameraPosition;
    private Vector3 cameraRotation;

    private GameObject mainCamera;

    void FixedView()
    {
        if (mainCamera.GetComponent<FirstView_T3>().enabled == false)
        {
            mainCamera.GetComponent<FirstView_T3>().enabled = true;
        }
        else
        {
            mainCamera.GetComponent<FirstView_T3>().enabled = false;
        }
    }

    void DefaultView()
    {
        mainCamera.transform.localPosition = cameraPosition;
        mainCamera.transform.localEulerAngles = cameraRotation;
    }

    void Awake()
    {
        fixedViewButton.GetComponent<Button>().onClick.AddListener(FixedView);
        defaultViewButton.GetComponent<Button>().onClick.AddListener(DefaultView);
    }
    // Use this for initialization
    void Start () {
        cameraPosition = new Vector3(151.9f, 716.9f, -1003.1f);
        cameraRotation = new Vector3(24.20016f, 52.66833f, -2.479156f);
        mainCamera = GameObject.Find("Main Camera T3");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
