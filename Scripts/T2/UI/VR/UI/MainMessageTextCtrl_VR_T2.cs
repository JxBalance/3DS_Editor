using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMessageTextCtrl_VR_T2 : MainMessageTextCtrl_T2
{
    private Text mainText;
    private int messageCount = 0;

    /// <summary>
    /// 显示主界面信息协程
    /// </summary>
    /// <param name="message"></param>
    /// <param name="showTime"></param>
    /// <returns></returns>
    private IEnumerator DoShowMainMessage(string message, float showTime)
    {
        mainText.text = message;
        messageCount++;
        yield return new WaitForSeconds(showTime);
        messageCount--;
        if (messageCount == 0)
        {
            mainText.text = "";
        }
    }

    /// <summary>
    /// 显示主界面文字信息
    /// </summary>
    /// <param name="message"></param>
    /// <param name="showTime"></param>
    public override void ShowMainMessage(string message, float showTime = 2)
    {
        base.ShowMainMessage(message, showTime);
        StartCoroutine(DoShowMainMessage(message, showTime));
    }




}
