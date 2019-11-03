using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitDrivePanelCtrl_T2 : MonoBehaviour {

    public Button DriveButton;
    public Button DisassemblyButton;
    public Button StepButton;
    public Button AutoButton;
    public Text NameTitleTex;
    public GameObject map;

    protected bool isSteState = false;
    protected bool isAutonState = false;

    protected bool updateState = false;


    /// <summary>
    /// 更新按钮状态
    /// </summary>
    protected virtual void UpdateState()
    {
        DriveButton.gameObject.SetActive(isSteState);
        DisassemblyButton.gameObject.SetActive(isSteState);
        StepButton.gameObject.SetActive(isSteState);
        AutoButton.gameObject.SetActive(isAutonState);
    }

    //protected virtual void SetText(Text text, string str)
    //{
    //    text.text = str;
    //}
}
