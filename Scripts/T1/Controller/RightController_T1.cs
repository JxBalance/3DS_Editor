using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;


public class RightController_T1 : MonoBehaviour
{
    [SerializeField]
    private Transform currentTargetTransform;
    [SerializeField]
    private UnitMemberT1 currentPointMember;

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
        currentTargetTransform = e.target;
        try
        {
            currentPointMember = currentTargetTransform.GetComponent<UnitMemberT1>();
        }
        catch (Exception)
        {
            throw;
        }
        if (currentPointMember)
        {
            currentPointMember.DoChangeGroupColor();
        }
    }

    /// <summary>
    /// 射线移出
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void DoPointerOut(object sender, DestinationMarkerEventArgs e)
    {
        if (currentPointMember)
        {
            currentPointMember.DoReturnGroupColor();
        }
        currentPointMember = null;
        currentTargetTransform = null;
    }

    /// <summary>
    /// 按钮2按下
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void DoButtonTwoPressed(object sender, ControllerInteractionEventArgs e)
    {
        UIControllerT1._instance.ShowVRGameCanvasPanelByIndex(1);
    }

    /// <summary>
    /// 按下扳机
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void DoTriggerClicked(object sender, ControllerInteractionEventArgs e)
    {
        if (currentTargetTransform)
        {
            if (currentPointMember)
            {
                currentPointMember.DoShowGroup();
            }
        }
    }


}
