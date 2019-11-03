using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIControllerT2 : MonoBehaviour 
{

    public static UIControllerT2 _instance;//单例模式
    [SerializeField]
    public GameObject StartPanelCanvas;//起始界面
    [SerializeField]
    public GameObject GamePanelCanvas_PC;//桌面式界面
    [SerializeField]
    public GameObject GamePanelCanvas_VR;//头盔式界面
    [SerializeField]
    public GameObject GamePanelCanvas_VR_Follow;//头盔式跟随界面
    [SerializeField]
    public GameObject CameraRigGameObject;//VR相机物体

    [SerializeField]
    public Transform VRCameraEye;//VR 眼

    [SerializeField]
    private UnitDrivePanelCtrl_T2 unitDrivePanelCtrl; //根据操作方式的不同 初始化不同的Ctrl  TODO
    [SerializeField]
    private MainMessageTextCtrl_T2 mainMessageTextCtrl_T2; //根据操作方式的不同 初始化不同的Ctrl TODO
    private List<GameObject> VRGameCanvasPanelList = new List<GameObject>(); //TODO

    void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        OnWaitStartState();

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

        VRGameCanvasPanelList.Add(GamePanelCanvas_VR.transform.Find("DrivePanel").gameObject);//0
        VRGameCanvasPanelList.Add(GamePanelCanvas_VR.transform.Find("DisassemblyPanel").gameObject);//1
        VRGameCanvasPanelList.Add(GamePanelCanvas_VR.transform.Find("DriveModePanel").gameObject);//2
        VRGameCanvasPanelList.Add(GamePanelCanvas_VR.transform.Find("MainMessageTextPanel").gameObject);//3
        VRGameCanvasPanelList.Add(GamePanelCanvas_VR.transform.Find("WorkingModePanel").gameObject);//4
    }

    /// <summary>
    /// 初始化部件信息
    /// </summary>
    /// <param name="controller"></param>
    public void InitController(UnitDrivePanelCtrl_T2 unitController, MainMessageTextCtrl_T2 messageController)
    {
        unitDrivePanelCtrl = unitController;
        mainMessageTextCtrl_T2 = messageController;
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
    /// 根据交互方式激活某一个界面
    /// </summary>
    /// <param name="operateType"></param>
    public void ActivePanel(OperateType operateType)
    {
        StartPanelCanvas.SetActive(false);
        if (operateType == OperateType.桌面式)
        {
            CanvasGroup cg = GamePanelCanvas_PC.GetComponent<CanvasGroup>();
            cg.alpha = 1;
            cg.blocksRaycasts = true;
            GamePanelCanvas_PC.SetActive(true);
            UnitDrivePanelCtrl_PC_T2 uCtrl = GamePanelCanvas_PC.transform.Find("DriveModePanel").GetComponentInChildren<UnitDrivePanelCtrl_PC_T2>();
            MainMessageTextCtrl_PC_T2 mCtrl = GamePanelCanvas_PC.transform.Find("MainMessageTextPanel").GetComponentInChildren<MainMessageTextCtrl_PC_T2>();
            InitController(uCtrl, mCtrl);
            GameObject.Find("DrivePanel").SetActive(false);
            GameObject.Find("DisassemblyPanel").SetActive(false);
            GameObject.Find("DriveModePanel").SetActive(false);

        }
        else if (operateType == OperateType.头盔式)
        {
            CanvasGroup cg = GamePanelCanvas_VR.GetComponent<CanvasGroup>();
            cg.alpha = 1;
            cg.blocksRaycasts = true;
            GamePanelCanvas_VR.SetActive(true);
            GamePanelCanvas_VR_Follow.SetActive(true);
            UnitDrivePanelCtrl_VR_T2 uCtrl =  GamePanelCanvas_VR.transform.Find("DriveModePanel").GetComponentInChildren<UnitDrivePanelCtrl_VR_T2>();
            MainMessageTextCtrl_VR_T2 mCtrl = GamePanelCanvas_VR_Follow.transform.Find("MainMessageText").GetComponent<MainMessageTextCtrl_VR_T2>();
            InitController(uCtrl, mCtrl);
            GameObject.Find("DrivePanel").SetActive(false);
            GameObject.Find("DisassemblyPanel").SetActive(false);
            GameObject.Find("DriveModePanel").SetActive(false);
        }
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
    /// 显示一个VR界面----做界面切换用
    /// </summary>
    /// <param name="index"></param>
    public void ShowVRGameCanvasPanelByIndex(int index)
    {
        foreach (var panel in VRGameCanvasPanelList)
        {
            panel.SetActive(false);
        }
        if (index <= 0)
        {
            return;
        }
        VRGameCanvasPanelList[index].SetActive(true);
    }


}
