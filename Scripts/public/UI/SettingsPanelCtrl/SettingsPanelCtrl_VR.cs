using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPanelCtrl_VR : SettingsPanelCtrl
{

	// Use this for initialization
	void Start () {

	    SetPanel(GetComponent<RectTransform>());
	    OnClosePanel();

	    ClosePanelButton = panelRectTransform.Find("CloseButton").GetComponent<Button>();
	    ClosePanelButton.onClick.AddListener(ClosePanel);

	    LeftTransform = panelRectTransform.Find("LeftSettings").GetComponent<RectTransform>();
	    McRectTransform = panelRectTransform.Find("MCSettings").GetComponent<RectTransform>();

	    OperateSettingsPanelRectTransform = LeftTransform.Find("OperateSettingsPanel").GetComponent<RectTransform>();
	    WindowSettingsPanelRectTransform = LeftTransform.Find("WindowSettingsPanel").GetComponent<RectTransform>();

	    GlobalSettingsPanelRectTransform = McRectTransform.Find("GlobalSettingsPanel").GetComponent<RectTransform>();
	    PersonalSettingsPanelRectTransform = McRectTransform.Find("PersonalSettingsPanel").GetComponent<RectTransform>();
	    ConnectStateShowPanelRectTransform = McRectTransform.Find("ConnectStateShowPanel").GetComponent<RectTransform>();

	    ViewMoveSpeedSlider = OperateSettingsPanelRectTransform.Find("ViewPanel").Find("MoveSpeed").Find("Slider")
	        .GetComponent<Slider>();
	    ViewRotateSpeedSlider = OperateSettingsPanelRectTransform.Find("ViewPanel").Find("RotateSpeed").Find("Slider")
	        .GetComponent<Slider>();

	    ViewMoveSpeedSlider.onValueChanged.AddListener(OnViewMoveSpeedSliderValueChanged);
	    ViewRotateSpeedSlider.onValueChanged.AddListener(OnViewRotateSpeedSliderValueChanged);

	    GlobalPositionFollowDropdown = GlobalSettingsPanelRectTransform.Find("PositionFollow").Find("Dropdown")
	        .GetComponent<Dropdown>();
	    GlobalOperationFollowDropdown = GlobalSettingsPanelRectTransform.Find("OperationFollow").Find("Dropdown")
	        .GetComponent<Dropdown>();
	    GlobalStudentFollowCtrlToggle = GlobalSettingsPanelRectTransform.Find("FollowCtrl").Find("Toggle")
	        .GetComponent<Toggle>();

	    GlobalPositionFollowDropdown.onValueChanged.AddListener(OnGlobalPositionFollowDropdownValueChanged);
	    GlobalOperationFollowDropdown.onValueChanged.AddListener(OnGlobalOperationFollowDropdownValueChanged);
	    GlobalStudentFollowCtrlToggle.onValueChanged.AddListener(OnGlobalStudentFollowCtrlToggleValueChanged);

	    PersonalPositionFollowDropdown = PersonalSettingsPanelRectTransform.Find("PositionFollow").Find("Dropdown")
	        .GetComponent<Dropdown>();
	    PersonalOperationFollowDropdown = PersonalSettingsPanelRectTransform.Find("OperationFollow").Find("Dropdown")
	        .GetComponent<Dropdown>();

	    PersonalPositionFollowDropdown.onValueChanged.AddListener(OnPersonalPositionFollowDropdownValueChanged);
	    PersonalOperationFollowDropdown.onValueChanged.AddListener(OnPersonalOperationFollowDropdownValueChanged);

	    ClientItemSettingParentRectTransform = McRectTransform.Find("ConnectStateShowPanel").Find("List").Find("Panel")
	        .GetComponent<RectTransform>();

    }

    public override void OnOpenPanel()
    {
        base.OnOpenPanel();
        if (GamaManagerGlobal._instance.isMultiplayerCollaboration)
        {
            McRectTransform.gameObject.SetActive(true);
            panelRectTransform.sizeDelta = new Vector2(960, 600);
            InitClientItem();
        }
        else
        {
            McRectTransform.gameObject.SetActive(false);
            panelRectTransform.sizeDelta = new Vector2(480, 600);
        }
    }
}
