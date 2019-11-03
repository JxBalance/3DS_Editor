using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAttrT3 : MonoBehaviour {

    public int index = -1;
    public int isClick = 0;         //0:未点击 1:现在点击 2;上次点击

    public void OnClick()
    {
        isClick = 1;
        int count = transform.parent.childCount;
        for (int i = 0; i < count; i++)
        {
            if(transform.parent.GetChild(i).gameObject.name != "StateText" && 
                transform.parent.GetChild(i).gameObject.name != "DetailButton" &&
                 transform.parent.GetChild(i).gameObject.name != gameObject.name)
            {
                ButtonAttrT3 butAtt = transform.parent.GetChild(i).gameObject.GetComponent<ButtonAttrT3>();
                //butAtt.isClick = false;
            }
        }

    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
