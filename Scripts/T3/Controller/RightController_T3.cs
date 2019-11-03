using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class RightController_T3 : MonoBehaviour {

	// Use this for initialization
	void Start () {

	    GetComponent<VRTK_DestinationMarker>().DestinationMarkerEnter += new DestinationMarkerEventHandler(DoPointerIn);
	    GetComponent<VRTK_DestinationMarker>().DestinationMarkerExit += new DestinationMarkerEventHandler(DoPointerOut);

	    GetComponent<VRTK_ControllerEvents>().TriggerClicked += new ControllerInteractionEventHandler(DoTriggerClicked);
	    GetComponent<VRTK_ControllerEvents>().ButtonTwoPressed += new ControllerInteractionEventHandler(DoButtonTwoPressed);


    }

    // Update is called once per frame
    void Update () {
		
	}

    /// <summary>
    /// 射线进入
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void DoPointerIn(object sender, DestinationMarkerEventArgs e)
    {


    }

    /// <summary>
    /// 射线移出
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void DoPointerOut(object sender, DestinationMarkerEventArgs e)
    {

    }

    /// <summary>
    /// 按钮2按下
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void DoButtonTwoPressed(object sender, ControllerInteractionEventArgs e)
    {

    }

    /// <summary>
    /// 按下扳机
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void DoTriggerClicked(object sender, ControllerInteractionEventArgs e)
    {

    }
}
