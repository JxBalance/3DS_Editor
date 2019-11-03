using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;



public class UnitDrivePanelCtrl_PC_T2 : UnitDrivePanelCtrl_T2
{
    GameObject MainCamera_PC;
    // Use this for initialization
    void Start ()
	{
        map = GameObject.FindGameObjectWithTag("map");
        DriveButton.onClick.AddListener(OnToDriveButtonClick);
        DisassemblyButton.onClick.AddListener(OnToDisassemblyClick);
        AutoButton.onClick.AddListener(OnToAutoButtonClick);
        StepButton.onClick.AddListener(OnToStepButtonClick);
        MainCamera_PC = GameObject.Find("Main Camera");
    }
	
	// Update is called once per frame
	void Update () 
    {
        if (updateState)
	    {
	        UpdateState();
            updateState = false;
	    }
	}

    protected override void UpdateState()
    {
        base.UpdateState();
        updateState = false;
    }

    /// <summary>
    /// 作动模式--PC
    /// </summary>
    public void OnToDriveButtonClick()
    {
        map.SetActive(true);
        GameObject rootObj = GameObject.Find("CanvasT2(Clone)");
        GameObject DriveModePanelObj = rootObj.transform.Find("DriveModePanel").gameObject;
        DriveModePanelObj.SetActive(true);
        GameObject DisassemblyPanelObj = rootObj.transform.Find("DisassemblyPanel").gameObject;
        DisassemblyPanelObj.SetActive(false);

    }
    /// <summary>
    /// 拆装模式--PC
    /// </summary>
    public void OnToDisassemblyClick()
    {
        map.SetActive(true);
        GameObject rootObj = GameObject.Find("CanvasT2(Clone)");
        GameObject DisassemblyPanelObj = rootObj.transform.Find("DisassemblyPanel").gameObject;
        DisassemblyPanelObj.SetActive(true);
        GameObject DriveModePanelObj = rootObj.transform.Find("DriveModePanel").gameObject;
        DriveModePanelObj.SetActive(false);
        GameObject DrivePanelObj = rootObj.transform.Find("DrivePanel").gameObject;
        DrivePanelObj.SetActive(false);
        MainCamera_PC.transform.position = new Vector3(5, -2.5f, -25);
        MainCamera_PC.transform.rotation = Quaternion.Euler(0, 200, 0);
    }


    /// <summary>
    /// 作动演示模式--PC
    /// </summary>
    public void OnToAutoButtonClick()
    {
        map.SetActive(true);
        GameObject rootObj = GameObject.Find("CanvasT2(Clone)");
        GameObject DrivePanelObj = rootObj.transform.Find("DrivePanel").gameObject;
        DrivePanelObj.SetActive(false);
        MainCamera_PC.transform.position = new Vector3(0, 1, -10);
        MainCamera_PC.transform.rotation = Quaternion.Euler(0, 0, 0);

    }
    /// <summary>
    /// 分步作动模式--PC
    /// </summary>
    public void OnToStepButtonClick()
    {
        map.SetActive(false);
        GameObject rootObj = GameObject.Find("CanvasT2(Clone)");
        GameObject DrivePanelObj = rootObj.transform.Find("DrivePanel").gameObject;
        DrivePanelObj.SetActive(true);
        MainCamera_PC.transform.position = new Vector3(5, -2.5f, -25);
        MainCamera_PC.transform.rotation = Quaternion.Euler(0, 200, 0);
    }
}
