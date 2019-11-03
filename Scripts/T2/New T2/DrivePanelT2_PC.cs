using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrivePanelT2_PC : DrivePanelT2
{

	// Use this for initialization
	void Start ()
	{
        SetPanel(GetComponent<RectTransform>());
	    ButtonsParent = transform.Find("List").Find("Panel").GetComponent<RectTransform>();
	}


}
