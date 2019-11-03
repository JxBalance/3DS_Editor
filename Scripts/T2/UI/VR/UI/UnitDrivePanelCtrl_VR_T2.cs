using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitDrivePanelCtrl_VR_T2 : UnitDrivePanelCtrl_T2
{
    public GameObject MainCamera_VR;
    public GameObject CabinCamera;
    RectTransform VRGamePanelRT;
    RectTransform WorkingUnitPanelRT;
    RectTransform DriveModePanelRT;
    // Use this for initialization
    void Start ()
    {
        MainCamera_VR = GameObject.Find("[CameraRig]");
        VRGamePanelRT = GetComponent<RectTransform>();
        map = GameObject.FindGameObjectWithTag("map");
        WorkingUnitPanelRT =VRGamePanelRT.Find("WorkingModePanel").GetComponent<RectTransform>();
        DriveButton = WorkingUnitPanelRT.Find("DriveButton").GetComponent<Button>();
        DisassemblyButton = WorkingUnitPanelRT.Find("DisassemblyButton").GetComponent<Button>();

        DriveModePanelRT = VRGamePanelRT.Find("DriveModePanel").GetComponent<RectTransform>();
        AutoButton = DriveModePanelRT.Find("AutoButton").GetComponent<Button>();
        StepButton = DriveModePanelRT.Find("StepButton").GetComponent<Button>();

        DriveButton.onClick.AddListener(OnToDriveButtonClick);
        DisassemblyButton.onClick.AddListener(OnToDisassemblyClick);
        AutoButton.onClick.AddListener(OnToAutoButtonClick);
        StepButton.onClick.AddListener(OnToStepButtonClick);

    }

    // Update is called once per frame
    void Update()
    {
        if (updateState)
        {
            UpdateState();
            updateState = false;
        }
    }

    /// <summary>
    /// 作动模式--PC
    /// </summary>
    public void OnToDriveButtonClick()
    {
        map.SetActive(true);
        GameObject rootObj = GameObject.Find("VRGameCanvasT2");
        GameObject DriveModePanelObj = rootObj.transform.Find("DriveModePanel").gameObject;
        DriveModePanelObj.SetActive(true);
        GameObject DisassemblyPanelObj = rootObj.transform.Find("DisassemblyPanel").gameObject;
        DisassemblyPanelObj.SetActive(false);
        GameObject DrivePanelObj = rootObj.transform.Find("DrivePanel").gameObject;
        DrivePanelObj.SetActive(false);

    }
    /// <summary>
    /// 拆装模式--PC
    /// </summary>
    public void OnToDisassemblyClick()
    {
        map.SetActive(true);
        GameObject rootObj = GameObject.Find("VRGameCanvasT2");
        GameObject DisassemblyPanelObj = rootObj.transform.Find("DisassemblyPanel").gameObject;
        DisassemblyPanelObj.SetActive(true);
        GameObject DriveModePanelObj = rootObj.transform.Find("DriveModePanel").gameObject;
        DriveModePanelObj.SetActive(false);
        GameObject DrivePanelObj = rootObj.transform.Find("DrivePanel").gameObject;
        DrivePanelObj.SetActive(false);
        MainCamera_VR.transform.position = new Vector3(5, -2.5f, -25);
        MainCamera_VR.transform.rotation = Quaternion.Euler(0, 200, 0);
    }


    /// <summary>
    /// 作动演示模式--PC
    /// </summary>
    public void OnToAutoButtonClick()
    {
        map.SetActive(true);
        GameObject rootObj = GameObject.Find("VRGameCanvasT2");
        GameObject DrivePanelObj = rootObj.transform.Find("DrivePanel").gameObject;
        DrivePanelObj.SetActive(false);
        GameObject DriveModePanelObj = rootObj.transform.Find("DriveModePanel").gameObject;
        DriveModePanelObj.SetActive(false);
        MainCamera_VR.transform.position = new Vector3(-0.5f, -0.3f, -10);
        MainCamera_VR.transform.rotation = Quaternion.Euler(0, 0, 0);

    }
    /// <summary>
    /// 分步作动模式--PC
    /// </summary>
    public void OnToStepButtonClick()
    {
        map.SetActive(false);
        GameObject rootObj = GameObject.Find("VRGameCanvasT2");
        GameObject DrivePanelObj = rootObj.transform.Find("DrivePanel").gameObject;
        DrivePanelObj.SetActive(true);
        GameObject DriveModePanelObj = rootObj.transform.Find("DriveModePanel").gameObject;
        DriveModePanelObj.SetActive(false);
        MainCamera_VR.transform.position = new Vector3(5, -2.5f, -25);
        MainCamera_VR.transform.rotation = Quaternion.Euler(0, 200, 0);
    }
}
