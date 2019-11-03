using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class LeftController_T3 : MonoBehaviour {

    private VRTK_SlideObjectControlAction[] controlActions;


    // Use this for initialization
    void Start()
    {

        GetComponent<VRTK_ControllerEvents>().ButtonTwoPressed += new ControllerInteractionEventHandler(DoButtonTwoPressed);
        GetComponent<VRTK_ControllerEvents>().TriggerClicked += new ControllerInteractionEventHandler(DoTriggerClicked);

        controlActions = GetComponents<VRTK_SlideObjectControlAction>();
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
        bool state = false;
        foreach (var action in controlActions)
        {
            state = action.isMoveIn3D = !action.isMoveIn3D;
        }

        if (state)
        {
            UIControllerT3._instance.ShowMainMessage("切换为空间移动");
        }
        else
        {
            UIControllerT3._instance.ShowMainMessage("切换为平面移动");
        }
    }
}
