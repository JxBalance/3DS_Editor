using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIControllerT3 : MonoBehaviour
{

    public static UIControllerT3 _instance;//单例模式
    [SerializeField] private GameObject StartPanelCanvas;//起始界面
    [SerializeField] private GameObject GamePanelCanvas_PC;//桌面式界面
    [SerializeField] private GameObject GamePanelCanvas_VR;//头盔式界面
    [SerializeField] private GameObject GamePanelCanvas_VR_Follow;//头盔式跟随界面
    [SerializeField] private GameObject CameraRigGameObject;//VR相机物体

    [SerializeField] private Transform VRCameraEye;//VR 眼


    [SerializeField] private MainMessageTextCtrl_T3 mainMessageTextCtrl; //根据操作方式的不同 初始化不同的Ctrl


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
    }

    /// <summary>
    /// 初始化部件信息
    /// </summary>
    /// <param name="controller"></param>
    public void InitController(MainMessageTextCtrl_T3 messageController)
    {
        mainMessageTextCtrl = messageController;
        
    }


    /// <summary>
    /// 等待开始状态界面
    /// </summary>
    public void OnWaitStartState()
    {
        StartPanelCanvas.SetActive(true);
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
            GamePanelCanvas_PC.SetActive(true);

            //MainMessageTextCtrl_T3 mCtrl = GamePanelCanvas_PC.transform.Find("MainMessageTextPanel").GetComponentInChildren<MainMessageTextCtrl_T3>();
            //InitController(mCtrl);

        }
        else if (operateType == OperateType.头盔式)
        {
            GamePanelCanvas_VR.SetActive(true);
            GamePanelCanvas_VR_Follow.SetActive(true);


            //LX修改，初始化主页面的文本组件
            MainMessageTextCtrl_T3 mCtrl = GamePanelCanvas_VR_Follow.transform.Find("MainMessageText").GetComponent<MainMessageTextCtrl_T3>();
            InitController(mCtrl);
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
    /// 主信息显示
    /// </summary>
    /// <param name="message"></param>
    /// <param name="showTime"></param>
    public void ShowMainMessage(string message, float showTime = 2f)
    {
        mainMessageTextCtrl.ShowMainMessage(message, showTime);
    }

}
