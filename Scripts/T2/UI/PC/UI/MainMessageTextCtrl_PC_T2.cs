using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 界面显示信息更新 仿PUBG
/// </summary>
public class MainMessageTextCtrl_PC_T2 : MainMessageTextCtrl_T2
{
    public GameObject textPrefab;
    private GameObject currentTextGO;
    private GameObject lastTextGO;

    private void AddMessage(string message,float showTime)
    {
        if (currentTextGO)
        {
            StartCoroutine(currentTextGO.GetComponent<MainMessageText_PC_T2>().ShowAsLastText());
            if (lastTextGO)
            {
                lastTextGO.GetComponent<MainMessageText_PC_T2>().DestroyThis();
            }
            lastTextGO = currentTextGO;
        }
        currentTextGO = Instantiate(textPrefab, textPrefab.transform.position, Quaternion.identity, transform);
        currentTextGO.GetComponent<MainMessageText_PC_T2>().Message = message;
        currentTextGO.GetComponent<MainMessageText_PC_T2>().ShowTime = showTime;
        currentTextGO.SetActive(true);
    }

    public override void ShowMainMessage(string message, float showTime = 2)
    {
        base.ShowMainMessage(message, showTime);
        AddMessage(message, showTime);    
    }
}
