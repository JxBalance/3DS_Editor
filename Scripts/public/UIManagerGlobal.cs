using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Global;
using UnityEngine;

public class UIManagerGlobal : MonoBehaviour
{

    public static UIManagerGlobal _instance;//单例模式

    [SerializeField] private GameObject StartPanelCanvas;//起始界面
    [SerializeField] private GameObject GamePanelCanvas_PC;//桌面式界面
    [SerializeField] private GameObject GamePanelCanvas_VR;//头盔式界面
    [SerializeField] private GameObject GamePanelCanvas_VR_Follow;//头盔式跟随界面
    [SerializeField] private GameObject CameraRigGameObject;//VR相机物体

    [SerializeField] private Transform VRCameraEye;//VR 眼

     //各子界面控制器
    [SerializeField] private TitlePanelCtrl_T1 titlePanelCtrl;
    [SerializeField] private UnitRecognitionPanelCtrl_T1 unitRecognitionPanelCtrl;
    [SerializeField] private MainMessageTextCtrl_T1 mainMessageTextCtrl;
    [SerializeField] private IntroPanelCtrl_T1 introPanelCtrl;
    [SerializeField] private AutoPlayCtrl_T1 autoPlayCtrl;
    [SerializeField] private ObservePointPanelCtrl_T1 observePointPanelCtrl;
    [SerializeField] private DoHandlePanelCtrl doHandlePanelCtrl;

    [SerializeField] private StartPanelCtrl startPanelCtrl;
    [SerializeField] private WaitingConnectPanelCtrl waitingConnectPanelCtrl;
    [SerializeField] private SettingsPanelCtrl settingsPanelCtrl;


    [SerializeField] private DrivePanelT2 drivePanelT2;



    //主界面栈 通常只存一个Panel
    private Stack<BasePanel_T1> panelStack = new Stack<BasePanel_T1>(); 


    /// <summary>
    /// 把某个页面入栈，  把某个页面显示在界面上
    /// </summary>
    public void PushPanel(BasePanel_T1 panel)
    {
        if (panelStack == null)
            panelStack = new Stack<BasePanel_T1>();

        //判断一下栈里面是否有页面
        if (panelStack.Count > 0)
        {
            BasePanel_T1 topPanel = panelStack.Pop();
            topPanel.OnClosePanel();
        }
        
        panel.OnOpenPanel();
        panelStack.Push(panel);
    }
    /// <summary>
    /// 出栈 ，把页面从界面上移除
    /// </summary>
    public void PopPanel()
    {
        if (panelStack == null)
            panelStack = new Stack<BasePanel_T1>();

        if (panelStack.Count <= 0) return;

        //关闭栈顶页面的显示
        BasePanel_T1 topPanel = panelStack.Pop();
        topPanel.OnClosePanel();

        if (panelStack.Count <= 0) return;
        BasePanel_T1 topPanel2 = panelStack.Peek();
        topPanel2.OnOpenPanel();
    }

    public Transform CameraRigTransform
    {
        get { return CameraRigGameObject.transform; }
    }

    public Transform VrCameraEyeTransform
    {
        get { return VRCameraEye; }
    }

    public int currentShowModelStateIndex
    {
        get { return (int) unitRecognitionPanelCtrl.currentShowModelState; }
        set
        {
            unitRecognitionPanelCtrl.SetShowModelState(GamaManagerGlobal._instance.currentUnitGroup,
                (UnitRecognitionPanelCtrl_T1.ShowModelState) value);
        }
    }

    public bool isShowUnitRecognitionPanel
    {
        get { return unitRecognitionPanelCtrl.state == BasePanel_T1.PanelState.Show; }
        set
        {
            if (value)
            {
                ShowUnitRecognitionPanel(GamaManagerGlobal._instance.currentUnitGroup);
            }
            else
            {
                CloseUnitRecognitionPanel();
            }
        }
    }

    public bool isShowIntroBoardPanel
    {
        get { return introPanelCtrl.state == BasePanel_T1.PanelState.Show; }
        set
        {
            if (value && introPanelCtrl.state != BasePanel_T1.PanelState.Show)
            {
                ShowIntroPanel();
            }
            else if(!value)
            {
                CloseIntroPanel();
            }
        }
    }

    public int currentShowIntroBoardIndex
    {
        get { return introPanelCtrl.CurrentBoardIndex; }
        set { introPanelCtrl.CurrentBoardIndex = value; }
    }


    void Awake()
    {
        _instance = this;
    }

	// Use this for initialization
	void Start () {

	    LoadBasicSetting();
        OnWaitStartState();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SwitchToWaitingConnectPanel()
    {
        startPanelCtrl.transform.DOLocalMoveX(-490f, 0.25f);
        waitingConnectPanelCtrl.transform.DOLocalMoveX(0f, 0.25f);
        waitingConnectPanelCtrl.InitPanel();
    }

    public void SwitchToStartPanel()
    {
        startPanelCtrl.transform.DOLocalMoveX(0f, 0.25f);
        waitingConnectPanelCtrl.transform.DOLocalMoveX(490f, 0.25f);
    }

    /// <summary>
    /// 编辑器使用，生成场景实例化物体时调用
    /// </summary>
    /// <param name="startPanelCanvas"></param>
    /// <param name="gamePanelCanvas_PC"></param>
    /// <param name="gamePanelCanvas_VR"></param>
    /// <param name="gamePanelCanvas_VR_Follow"></param>
    public void InitUICanvas(GameObject startPanelCanvas, GameObject gamePanelCanvas_PC, GameObject gamePanelCanvas_VR,
        GameObject gamePanelCanvas_VR_Follow)
    {
        StartPanelCanvas = startPanelCanvas;
        GamePanelCanvas_PC = gamePanelCanvas_PC;
        GamePanelCanvas_VR = gamePanelCanvas_VR;
        GamePanelCanvas_VR_Follow = gamePanelCanvas_VR_Follow;
        VRCameraEye = GamePanelCanvas_VR_Follow.transform.parent;
        CameraRigGameObject = VRCameraEye.transform.parent.parent.gameObject;

        startPanelCtrl = StartPanelCanvas.transform.Find("Mask").Find("StartPanel").GetComponent<StartPanelCtrl>();
        waitingConnectPanelCtrl = StartPanelCanvas.transform.Find("Mask").Find("WaitingConnectPanel")
            .GetComponent<WaitingConnectPanelCtrl>();
    }

    /// <summary>
    /// 初始化子界面控制器
    /// </summary>
    /// <param name="controller"></param>
    public void InitController(TitlePanelCtrl_T1 _titlePanelCtrl, UnitRecognitionPanelCtrl_T1 _unitRecognitionPanelCtrl,
        MainMessageTextCtrl_T1 _mainMessageTextCtrl,IntroPanelCtrl_T1 _introPanelCtrl, AutoPlayCtrl_T1 _autoPlayPanelCtrl, ObservePointPanelCtrl_T1 _observePointPanelCtrl,
        DoHandlePanelCtrl _doHandlePanelCtrl, SettingsPanelCtrl _settingsPanelCtrl, DrivePanelT2 _drivePanelT2)
    {
        this.titlePanelCtrl = _titlePanelCtrl;
        this.unitRecognitionPanelCtrl = _unitRecognitionPanelCtrl;
        this.mainMessageTextCtrl = _mainMessageTextCtrl;
        this.introPanelCtrl = _introPanelCtrl;
        this.autoPlayCtrl = _autoPlayPanelCtrl;
        this.observePointPanelCtrl = _observePointPanelCtrl;
        this.doHandlePanelCtrl = _doHandlePanelCtrl;
        this.settingsPanelCtrl = _settingsPanelCtrl;
        this.drivePanelT2 = _drivePanelT2;
    }

    /// <summary>
    /// 根据交互方式激活某一个界面
    /// </summary>
    /// <param name="operateType"></param>
    public void ActivePanel(OperateMode operateMode)
    {
        StartPanelCanvas.SetActive(false);
        if (operateMode == OperateMode.桌面式)
        {
            GamePanelCanvas_PC.SetActive(true);

            TitlePanelCtrl_T1 tCtrl = GamePanelCanvas_PC.transform.Find("Title").GetComponent<TitlePanelCtrl_T1>();
            UnitRecognitionPanelCtrl_T1 uCtrl = GamePanelCanvas_PC.transform.Find("UnitRecognitionPanel").GetComponentInChildren<UnitRecognitionPanelCtrl_T1>();
            MainMessageTextCtrl_T1 mCtrl = GamePanelCanvas_PC.transform.Find("MainMessageTextPanel").GetComponentInChildren<MainMessageTextCtrl_T1>();
            IntroPanelCtrl_T1 iCtrl = GamePanelCanvas_PC.transform.Find("IntroPanel").GetComponentInChildren<IntroPanelCtrl_T1>();
            AutoPlayCtrl_T1 aCtrl = GamePanelCanvas_PC.transform.Find("AutoPlayPanel").GetComponentInChildren<AutoPlayCtrl_T1>();
            ObservePointPanelCtrl_T1 oCtrl = GamePanelCanvas_PC.transform.Find("ObservePointInfoSimple").GetComponentInChildren<ObservePointPanelCtrl_T1>();
            DoHandlePanelCtrl dCtrl = GamePanelCanvas_PC.transform.Find("HandlePanel").GetComponentInChildren<DoHandlePanelCtrl>();
            SettingsPanelCtrl sCtrl = GamePanelCanvas_PC.transform.Find("SettingsPanel").GetComponentInChildren<SettingsPanelCtrl>();
            DrivePanelT2 drivePanelCtrl = GamePanelCanvas_PC.transform.Find("DrivePanelT2").GetComponentInChildren<DrivePanelT2>();

            InitController(tCtrl, uCtrl, mCtrl, iCtrl, aCtrl, oCtrl, dCtrl, sCtrl, drivePanelCtrl);
        }
        else if (operateMode == OperateMode.头盔式)
        {
            GamePanelCanvas_VR.SetActive(true);
            GamePanelCanvas_VR_Follow.SetActive(true);

            UnitRecognitionPanelCtrl_T1 uCtrl = GamePanelCanvas_VR.transform.Find("UnitRecognitionPanel").GetComponentInChildren<UnitRecognitionPanelCtrl_T1>();
            MainMessageTextCtrl_T1 mCtrl = GamePanelCanvas_VR_Follow.transform.Find("MainMessageText").GetComponentInChildren<MainMessageTextCtrl_T1>();
            IntroPanelCtrl_T1 iCtrl = GamePanelCanvas_VR.transform.Find("IntroPanel").GetComponentInChildren<IntroPanelCtrl_T1>();
            ObservePointPanelCtrl_T1 oCtrl = GamePanelCanvas_VR.transform.Find("ObservePointInfoPanel").GetComponentInChildren<ObservePointPanelCtrl_T1>();
            DoHandlePanelCtrl dCtrl = GamePanelCanvas_VR.transform.Find("HandlePanel").GetComponentInChildren<DoHandlePanelCtrl>();
            SettingsPanelCtrl sCtrl = GamePanelCanvas_VR.transform.Find("SettingsPanel").GetComponentInChildren<SettingsPanelCtrl>();
            InitController(null, uCtrl, mCtrl, iCtrl, null, oCtrl, dCtrl, sCtrl, null);
        }
    }

    /// <summary>
    /// 等待开始状态界面
    /// </summary>
    public void OnWaitStartState()
    {
        GamePanelCanvas_PC.SetActive(false);
        GamePanelCanvas_VR.SetActive(false);
        GamePanelCanvas_VR_Follow.SetActive(false);
    }

    /// <summary>
    /// 加载基本设置
    /// </summary>
    public void LoadBasicSetting()
    {
        startPanelCtrl.titleText.text = GamaManagerGlobal._instance.Setting.SceneName;
    }

    /// <summary>
    /// 显示部件信息
    /// </summary>
    /// <param name="group"></param>
    public void ShowUnitGroupInfo(UnitGroupT1 group)
    {
        ShowDoHandlePanel(group);
        ShowObservePointPanel(group);
    }

    /// <summary>
    /// 停止显示部件信息
    /// </summary>
    public void StopShowUnitGroupInfo(UnitGroupT1 group)
    {
        //停止一切展示
        CloseUnitRecognitionPanel();
        CloseShowModel(group);
        CloseDoHandlePanel();
        CloseObservePointPanel();
    }

    /// <summary>
    /// 设置VR界面位置
    /// </summary> 
    public void SetVRGameCanvasTrans()
    {
        //计算平面空间夹角
        float C = Mathf.Sqrt(VRCameraEye.forward.x * VRCameraEye.forward.x + VRCameraEye.forward.z * VRCameraEye.forward.z);
        float cos = VRCameraEye.forward.x / C;
        float sin = VRCameraEye.forward.z / C;

        GamePanelCanvas_VR.transform.position = CameraRigGameObject.transform.position + new Vector3(3 * cos, 1f, 3 * sin);
        //计算VRGameCanvasGameObject在空间中的旋转
        GamePanelCanvas_VR.transform.eulerAngles = new Vector3(0, VRCameraEye.eulerAngles.y, 0);
    }

    /// <summary>
    /// 显示VR界面
    /// </summary>
    public void ShowVRGameCanvas()
    {
        if (GamePanelCanvas_VR)
        {
            SetVRGameCanvasTrans();
            GamePanelCanvas_VR.SetActive(true);
        }
    }

    /// <summary>
    /// 关闭VR界面
    /// </summary>
    public void CloseVRGameCanvas()
    {
        if (GamePanelCanvas_VR)
        {
            GamePanelCanvas_VR.SetActive(false);
        }
    }

    /// <summary>
    /// 主信息显示
    /// </summary>
    /// <param name="message"></param>
    /// <param name="showTime"></param>
    public void ShowMainMessage(string message, float showTime = 2f)
    {
        mainMessageTextCtrl.ShowMainMessage(message, showTime);
    }

    public void OnShowIntroButtonClick()
    {
        if (introPanelCtrl.state == BasePanel_T1.PanelState.Show)
        {
            PopPanel();
        }
        else
        {
            if (introPanelCtrl.IntroBoardCount <= 0) return;
            PushPanel(introPanelCtrl);
            introPanelCtrl.ShowIntroBoardByCurrent();
        }
    }

    public void OnGlobalViewButtonClick()
    {
        if (autoPlayCtrl.state == BasePanel_T1.PanelState.Show)
        {
            PopPanel();
        }
        else
        {
            if (introPanelCtrl.IntroBoardCount <= 0) return;
            new GlobalView(_instance, autoPlayCtrl).AutoPlay();
        }
    }

    /// <summary>
    /// 打开介绍面板
    /// </summary>
    public void ShowIntroPanel()
    {
        if(introPanelCtrl.IntroBoardCount <= 0) return;
        PushPanel(introPanelCtrl);
        introPanelCtrl.ShowIntroBoardByCurrent();
    }

    /// <summary>
    /// 关闭介绍面板
    /// </summary>
    public void CloseIntroPanel()
    {
        if (introPanelCtrl.state == BasePanel_T1.PanelState.Show)
        {
            PopPanel();
        }
    }

    /// <summary>
    /// 打开部件识别面板
    /// </summary>
    /// <param name="group"></param>
    public void ShowUnitRecognitionPanel(UnitGroupT1 group)
    {
        PushPanel(unitRecognitionPanelCtrl);
        unitRecognitionPanelCtrl.ShowUnitRecognitionPanel(group);
    }

    /// <summary>
    /// 关闭部件识别面板
    /// </summary>
    public void CloseUnitRecognitionPanel()
    {
        if (unitRecognitionPanelCtrl.state == BasePanel_T1.PanelState.Show)
        {
            PopPanel();
            unitRecognitionPanelCtrl.CloseUnitRecognitionPanel();
        }
    }

    /// <summary>
    /// 打开观察点面板
    /// </summary>
    /// <param name="group"></param>
    public void ShowObservePointPanel(UnitGroupT1 group)
    {
        observePointPanelCtrl.InitPanel(group);
        observePointPanelCtrl.OnOpenPanel();
    }

    /// <summary>
    /// 关闭观察点面板
    /// </summary>
    public void CloseObservePointPanel()
    {
        if (observePointPanelCtrl.state == BasePanel_T1.PanelState.Show)
        {
            observePointPanelCtrl.OnClosePanel();
        }
    }

    /// <summary>
    /// 打开操作面板
    /// </summary>
    /// <param name="group"></param>
    public void ShowDoHandlePanel(UnitGroupT1 group)
    {
        doHandlePanelCtrl.InitPanel(group);
        doHandlePanelCtrl.OnOpenPanel();
    }

    /// <summary>
    /// 关闭操作面板
    /// </summary>
    public void CloseDoHandlePanel()
    {
        if (doHandlePanelCtrl.state == BasePanel_T1.PanelState.Show)
        {
            doHandlePanelCtrl.OnClosePanel();
        }
    }

    /// <summary>
    /// 开始模型展示
    /// </summary>
    /// <param name="group"></param>
    public void ShowModel(UnitGroupT1 group)
    {
        unitRecognitionPanelCtrl.SetShowModelState(group, UnitRecognitionPanelCtrl_T1.ShowModelState.color);
    }

    /// <summary>
    /// 关闭模型展示
    /// </summary>
    /// <param name="group"></param>
    public void CloseShowModel(UnitGroupT1 group)
    {
        unitRecognitionPanelCtrl.SetShowModelState(group,UnitRecognitionPanelCtrl_T1.ShowModelState.normal);
    }

    /// <summary>
    /// 点击操作面板的关闭按钮响应
    /// </summary>
    public void OnCloseDoHandlePanelButtonClick()
    {
        GamaManagerGlobal._instance.StopShowUnitGroup();
    }
    
    /// <summary>
    /// 点击部件识别按钮响应
    /// </summary>
    public void OnUnitRecognitionButtonClick()
    {
        if (unitRecognitionPanelCtrl.state == BasePanel_T1.PanelState.Show)
        {
            unitRecognitionPanelCtrl.CloseUnitRecognitionPanel();
        }
        else
        {
            ShowUnitRecognitionPanel(GamaManagerGlobal._instance.currentUnitGroup);
        }
    }

    /// <summary>
    /// 点击模型展示按钮响应
    /// </summary>
    public void OnShowModelButtonClick()
    {
        if (GamaManagerGlobal._instance.currentUnitGroup)
        {
            unitRecognitionPanelCtrl.SetNextShowModelState(GamaManagerGlobal._instance.currentUnitGroup);
        }
    }


    public void ClientConnectionCheck(int id)
    {
        waitingConnectPanelCtrl.ClientConnectionCheck(id);
        if(settingsPanelCtrl) settingsPanelCtrl.ClientConnectionCheck(id);
    }

    public void ClientDisconnectionCheck(int id)
    {
        waitingConnectPanelCtrl.ClientDisconnectionCheck(id);
        if(settingsPanelCtrl) settingsPanelCtrl.ClientDisconnectionCheck(id);
    }

    public void ShowSettingsPanel()
    {
        PushPanel(settingsPanelCtrl);
    }

    public void OnCloseSettingsPanel()
    {
        if (settingsPanelCtrl.state == BasePanel_T1.PanelState.Show)
        {
            PopPanel();
        }
    }

    public void SetActivePersonalPositionFollowDropdown(bool active)
    {
        settingsPanelCtrl.SetActivePersonalPositionFollowDropdown(active);
    }

    public void SetActivePersonalOperationFollowDropdown(bool active)
    {
        settingsPanelCtrl.SetActivePersonalOperationFollowDropdown(active);
    }

    public void SetActivePersonalSettingsPanel(bool active)
    {
        settingsPanelCtrl.SetActivePersonalSettingsPanel(active);
    }


    #region T2
    /// <summary>
    /// 点击机构作动按钮响应
    /// </summary>
    public void OnDriveButtonClick()
    {
        if (drivePanelT2.state == BasePanel_T1.PanelState.Show)
        {
            drivePanelT2.OnClosePanel();
        }
        else
        {
            drivePanelT2.OnOpenPanel();
        }

    }

    /// <summary>
    /// drivePanelT2
    /// </summary>
    public void OnOpenDrivePanel()
    {
        drivePanelT2.OnOpenPanel();
    }

    public void UpdateDrivePanel(Drive CurrentDrive)
    {
        drivePanelT2.CurrentDrive = CurrentDrive;
    }


    #endregion

}
