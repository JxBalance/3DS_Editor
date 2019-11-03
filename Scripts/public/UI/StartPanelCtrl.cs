using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Global;
using UnityEngine;
using UnityEngine.UI;

public class StartPanelCtrl : BasePanel_T1
{
    public Text titleText;
    public InputField ipInput;
    public Dropdown operateDropDown;
    public Dropdown clientDropdown;
    public Dropdown examDropdown;
    public Toggle gmsToggle;

    public GameObject OperateGameObject;
    public GameObject CrewGameObject;
    public GameObject IPGameObject;
    public GameObject ExamModeGameObject;
    public GameObject DesktopCanvasGameObject;

    public GameObject StartButtonGameObject;
    public GameObject NextButtonGameObject;


    void Start()
    {
        Init();
    }

    private void Init()
    {
        List<Dropdown.OptionData> optionDataList = new List<Dropdown.OptionData>();
        foreach (var c in GamaManagerGlobal._instance.Setting.ClientArray)
        {
            Dropdown.OptionData optionData = new Dropdown.OptionData();
            optionData.text = c.ToString();
            optionDataList.Add(optionData);
        }
        clientDropdown.options = optionDataList;
    }

    public void OnGMSToggleChanged(bool isOn)
    {
        GamaManagerGlobal._instance.isMultiplayerCollaboration = isOn;
        if (isOn)
        {
            StartCoroutine(ShowIPInput());
        }
        else
        {
            StartCoroutine(HideIPInput());
        }
    }

    /// <summary>
    /// 联机运行
    /// </summary>
    /// <returns></returns>
    private IEnumerator ShowIPInput()
    {
        OperateGameObject.transform.DOLocalMoveY(123f, 0.25f);
        ExamModeGameObject.transform.DOLocalMoveY(-123f, 0.25f);
        yield return new WaitForSeconds(0.15f);
        IPGameObject.SetActive(true);
        CrewGameObject.SetActive(true);
        StartButtonGameObject.SetActive(false);
        NextButtonGameObject.SetActive(true);
    }

    /// <summary>
    /// 单机运行
    /// </summary>
    /// <returns></returns>
    private IEnumerator HideIPInput()
    {
        IPGameObject.SetActive(false);
        CrewGameObject.SetActive(false);
        OperateGameObject.transform.DOLocalMoveY(60f, 0.25f);
        ExamModeGameObject.transform.DOLocalMoveY(-60f, 0.25f);
        yield return null;
        StartButtonGameObject.SetActive(true);
        NextButtonGameObject.SetActive(false);
    }

    /// <summary>
    /// 开始
    /// </summary>
    public void StartButtonClick()
    {
        //相机切换
        GamaManagerGlobal._instance.StartGame();
    }

    public void OnOperateModeDropDownChanged(int value)
    {
        GamaManagerGlobal._instance.operateMode = (OperateMode) value;
    }

    public void OnClientModeDropDownChanged(int value)
    {
        GMSManagerGlobal._instance.ID = value;
    }

    public void OnExamModeDropDownChanged(int value)
    {
        GamaManagerGlobal._instance.examMode = (ExamMode)value;
    }

    /// <summary>
    /// 下一步切换
    /// </summary>
    public void NextButtonClick()
    {
        if(!IPCheck(ipInput.text)) return;
        //GMSManagerGlobal._instance.ConnectServer("127.0.0.1", 10010);
        GMSManagerGlobal._instance.ConnectServer(ipInput.text, 10010);
        UIManagerGlobal._instance.SwitchToWaitingConnectPanel();
    }

    private bool IPCheck(string IPString)
    {
        string[] IPStringArray = IPString.Split('.');
        int[] IPIntArray = {0,0,0,0};
        bool result = true;
        for (int i = 0; i < 4; i++)
        {
            try
            {
                Int32.TryParse(IPStringArray[i], out IPIntArray[i]);
                if (IPIntArray[i] > 255 || IPIntArray[i] < 0)
                {
                    ShowMainMessage("IP地址应在0到255区间内");
                    result = false;
                }
            }
            catch (Exception e)
            {
                ShowMainMessage("IP地址格式错误：XXX.XXX.XXX.XXX");
                result = false;
                throw;
            }
        }
        return result;
    }
}
