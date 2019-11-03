using System.Collections;
using System.Collections.Generic;
using Global;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPanelCtrl : BasePanel_T1
{
    protected Button ClosePanelButton;

    protected RectTransform LeftTransform;
    protected RectTransform McRectTransform;

    protected RectTransform OperateSettingsPanelRectTransform;
    protected RectTransform WindowSettingsPanelRectTransform;

    protected RectTransform GlobalSettingsPanelRectTransform;
    protected RectTransform PersonalSettingsPanelRectTransform;
    protected RectTransform ConnectStateShowPanelRectTransform;

    protected Slider ViewMoveSpeedSlider;
    protected Slider ViewRotateSpeedSlider;

    protected Dropdown GlobalPositionFollowDropdown;
    protected Dropdown GlobalOperationFollowDropdown;
    protected Toggle GlobalStudentFollowCtrlToggle;


    protected Dropdown PersonalPositionFollowDropdown;
    protected Dropdown PersonalOperationFollowDropdown;

    public GameObject ClientItemSettingGameObject;
    public RectTransform ClientItemSettingParentRectTransform;

    protected List<ClientItem> ClientItemList = new List<ClientItem>();


    public override void OnOpenPanel()
    {
        base.OnOpenPanel();
        InitClientItem();
    }

    public virtual void ClosePanel()
    {
        UIManagerGlobal._instance.OnCloseSettingsPanel();
    }

    protected virtual void OnViewMoveSpeedSliderValueChanged(float value)
    {
        
    }

    protected virtual void OnViewRotateSpeedSliderValueChanged(float value)
    {

    }

    protected virtual void OnGlobalPositionFollowDropdownValueChanged(int value)
    {
        // 0== 无限制 1== Conn_0  2== Conn_1
        List<ClientInfo> connectClientInfoList = GMSManagerGlobal._instance.GetConnectClientInfoList();
        if (value == 0)
        {
            GMSManagerGlobal._instance.GlobalPositionFollowID = -1;
            SetActivePersonalPositionFollowDropdown(true);
        }
        else
        {
            GMSManagerGlobal._instance.GlobalPositionFollowID = connectClientInfoList[value - 1].ID;
            SetActivePersonalPositionFollowDropdown(false);
        }
        GMSManagerGlobal._instance.SendSettingsObserverData();
    }

    protected virtual void OnGlobalOperationFollowDropdownValueChanged(int value)
    {
        // 0== 无限制 1== Conn_0  2== Conn_1
        List<ClientInfo> connectClientInfoList = GMSManagerGlobal._instance.GetConnectClientInfoList();
        if (value == 0)
        {
            GMSManagerGlobal._instance.GlobalOperateFollowID = -1;
            SetActivePersonalOperationFollowDropdown(true);
        }
        else
        {
            GMSManagerGlobal._instance.GlobalOperateFollowID = connectClientInfoList[value - 1].ID;
            SetActivePersonalOperationFollowDropdown(false);
        }
        GMSManagerGlobal._instance.SendSettingsObserverData();
    }

    protected virtual void OnGlobalStudentFollowCtrlToggleValueChanged(bool value)
    {
        GMSManagerGlobal._instance.StudentsFollowSettings = value;
        GMSManagerGlobal._instance.SendSettingsObserverData();
    }

    protected virtual void OnPersonalPositionFollowDropdownValueChanged(int value)
    {
        List<ClientInfo> connectClientInfoList = GMSManagerGlobal._instance.GetConnectClientInfoList();
        if (value == 0)
        {
            GMSManagerGlobal._instance.PersonalPositionFollowID = -1;
        }
        else
        {
            GMSManagerGlobal._instance.PersonalPositionFollowID = connectClientInfoList[value - 1].ID;
        }
        GMSManagerGlobal._instance.SendSettingsObserverData();
    }

    protected virtual void OnPersonalOperationFollowDropdownValueChanged(int value)
    {
        List<ClientInfo> connectClientInfoList = GMSManagerGlobal._instance.GetConnectClientInfoList();
        if (value == 0)
        {
            GMSManagerGlobal._instance.PersonalOperateFollowID = -1;
        }
        else
        {
            GMSManagerGlobal._instance.PersonalOperateFollowID = connectClientInfoList[value - 1].ID;
        }
        GMSManagerGlobal._instance.SendSettingsObserverData();
    }

    protected virtual void InitClientItem()
    {

        for (int i = 0; i < ClientItemSettingParentRectTransform.childCount; i++)
        {
            Destroy(ClientItemSettingParentRectTransform.GetChild(i).gameObject);
        }
        ClientItemList.Clear();
        for (int i = 0; i < GamaManagerGlobal._instance.Setting.ClientArray.Length; i++)
        {
            GameObject go = Instantiate(ClientItemSettingGameObject, ClientItemSettingParentRectTransform);
            ClientItem item = go.GetComponent<ClientItem>();
            item.InitItem(GMSManagerGlobal._instance.clientInfoList[i]);
            ClientItemList.Add(item);
        }

        List<Dropdown.OptionData> optionDataList = new List<Dropdown.OptionData>();

        Dropdown.OptionData optionData = new Dropdown.OptionData();
        optionData.text = "无限制";
        optionDataList.Add(optionData);

        foreach (var c in GMSManagerGlobal._instance.GetConnectClientInfoList())
        {
            Dropdown.OptionData optionData1 = new Dropdown.OptionData();
            optionData1.text = c.clientMode.ToString();
            optionDataList.Add(optionData1);
        }

        GlobalPositionFollowDropdown.options = optionDataList;
        GlobalOperationFollowDropdown.options = optionDataList;
        PersonalPositionFollowDropdown.options = optionDataList;
        PersonalOperationFollowDropdown.options = optionDataList;

        if (GMSManagerGlobal._instance.currentClientMode == ClientMode.教员端)
        {
            SetActiveGlobalSettingsPanel(true);
        }
        else
        {
            SetActiveGlobalSettingsPanel(false);
        }
    }

    public void SetActiveGlobalSettingsPanel(bool active)
    {
        GlobalSettingsPanelRectTransform.gameObject.SetActive(active);
    }

    public void SetActivePersonalSettingsPanel(bool active)
    {
        PersonalSettingsPanelRectTransform.gameObject.SetActive(active);
    }

    public void SetActivePersonalPositionFollowDropdown(bool active)
    {
        PersonalPositionFollowDropdown.transform.parent.gameObject.SetActive(active);
    }

    public void SetActivePersonalOperationFollowDropdown(bool active)
    {
        PersonalOperationFollowDropdown.transform.parent.gameObject.SetActive(active);
    }


    /// <summary>
    /// 客户端连接检测
    /// </summary>
    /// <param name="id"></param>
    public virtual void ClientConnectionCheck(int id)
    {
        if(state == PanelState.Show) ClientItemList[id].OnConnect();
    }

    /// <summary>
    /// 客户端断开检测
    /// </summary>
    /// <param name="id"></param>
    public virtual void ClientDisconnectionCheck(int id)
    {
        if(state == PanelState.Show) ClientItemList[id].OnDisconnect();
    }

}
