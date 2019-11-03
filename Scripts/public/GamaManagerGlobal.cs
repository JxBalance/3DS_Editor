using System.Collections;
using System.Collections.Generic;
using Global;
using UnityEngine;
using UnityEngine.EventSystems;
using VRTK;

public class GamaManagerGlobal : MonoBehaviour
{
    public static GamaManagerGlobal _instance;

    public GameObject GameObserverGameObject;//观察者空物体    
    public GameObject StartUICanvasGameObject;//开始界面画布
    public GameObject UnitParentGameObject;//部件组父物体
    public GameObject EventSystemGameObject;//UI事件物体
    public GameObject DesktopGameCanvasGameObject;//PC端画布
    public GameObject VRTK_SDKManagerGameObject;//VR端SDK物体
    public GameObject VRTK_ScriptsGameObject;//VR端控制器脚本物体

    public GameObject MainCameraGameObject_PC;//PC端主相机

    public AudioListener AudioListener_VR;//声音获取组件VR
    public AudioListener AudioListener_PC;//声音获取组件PC

    public Material TransparentMaterial;
    public Material StandardMaterial;


    /// <summary>
    /// 初始化被管理物体
    /// </summary>
    /// <param name="startUICanvasGameObject"></param>
    /// <param name="unitParentGameObject"></param>
    /// <param name="eventSystemGameObject"></param>
    /// <param name="desktopGameCanvasGameObject"></param>
    /// <param name="vrtk_SDKManagerGameObject"></param>
    /// <param name="vrtk_ScriptsGameObject"></param>
    public void InitGameObjects(GameObject gameObserverGameObject,GameObject startUICanvasGameObject,GameObject unitParentGameObject,
        GameObject eventSystemGameObject, GameObject desktopGameCanvasGameObject,
        GameObject vrtk_SDKManagerGameObject, GameObject vrtk_ScriptsGameObject)
    {
        GameObserverGameObject = gameObserverGameObject;
        StartUICanvasGameObject = startUICanvasGameObject;
        UnitParentGameObject = unitParentGameObject;
        EventSystemGameObject = eventSystemGameObject;
        DesktopGameCanvasGameObject = desktopGameCanvasGameObject;
        VRTK_SDKManagerGameObject = vrtk_SDKManagerGameObject;
        VRTK_ScriptsGameObject = vrtk_ScriptsGameObject;

        VRTK_SDKManagerGameObject.GetComponent<VRTK_SDKManager>().scriptAliasLeftController =
            VRTK_ScriptsGameObject.transform.Find("LeftController").gameObject;
        VRTK_SDKManagerGameObject.GetComponent<VRTK_SDKManager>().scriptAliasRightController =
            VRTK_ScriptsGameObject.transform.Find("RightController").gameObject;

        MainCameraGameObject_PC = GameObject.Find("Main Camera");
        MainCameraGameObject_PC.AddComponent<PhysicsRaycaster>();
        MainCameraGameObject_PC.AddComponent<FreeLookT1>();
        AudioListener_VR = VRTK_SDKManagerGameObject.transform.GetChild(0).GetChild(3).GetChild(0).GetChild(3).GetChild(1).GetComponent<AudioListener>();
        AudioListener_PC = MainCameraGameObject_PC.GetComponent<AudioListener>();
    }

    private bool isStartGame = false;

    public BasicSetting Setting = new BasicSetting();// 基本设置项
    public Model[] ModelList;//模型列表
    public IntroBoard[] IntroBoardList;//介绍面板列表
    public UnitGroup[] UnitGroupList;//部件识别组列表
    public Drive[] DriveList;//机构作动列表

    public OperateMode operateMode = OperateMode.桌面式;
    public ExamMode examMode = ExamMode.教学模式;
    public bool isMultiplayerCollaboration = true;


    public UnitGroupT1 currentUnitGroup;//当前组
    public int currentUnitGroupIndex
    {
        get
        {
            if (currentUnitGroup)
            {
                for (int i = 0; i < UnitGroupList.Length; i++)
                {
                    if (UnitGroupList[i].unitGroup == currentUnitGroup)
                    {
                        return i;
                    }
                }
            }
            return -1;
        }
        set
        {
            if (currentUnitGroupIndex != value)
            {
                if (value == -1)
                {
                    StopShowUnitGroup();
                }
                else
                {
                    ShowUnitGroup(UnitGroupList[value].unitGroup);
                }
            }
        }
    }

    void Awake()
    {
        _instance = this;
    }

    // Use this for initialization
    void Start ()
    {
        InitUnitGroup();
        OnWaitStartState();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// 部件组信息初始化
    /// </summary>
    void InitUnitGroup()
    {
        foreach (var unitGroup in UnitGroupList)
        {
            unitGroup.LoadUnitGroupData();
        }
    }

    /// <summary>
    /// 根据启动模式，判断场景物体的使用
    /// </summary>
    /// <param name="operateType"></param>
    public void ActiveCamera(OperateMode operateMode)
    {
        if (operateMode == OperateMode.桌面式)
        {
            MainCameraGameObject_PC.SetActive(true);
            VRTK_SDKManagerGameObject.SetActive(false);
            VRTK_ScriptsGameObject.SetActive(false);
            AudioListener_PC.enabled = true;
        }
        else if (operateMode == OperateMode.头盔式)
        {
            MainCameraGameObject_PC.SetActive(false);
            VRTK_SDKManagerGameObject.SetActive(true);
            VRTK_ScriptsGameObject.SetActive(true);
            AudioListener_VR.enabled = true;
        }
    }

    /// <summary>
    /// 等待开始，场景物体的使用
    /// </summary>
    public void OnWaitStartState()
    {
        MainCameraGameObject_PC.SetActive(true);
        VRTK_SDKManagerGameObject.SetActive(true);
        VRTK_ScriptsGameObject.SetActive(true);
        AudioListener_VR.enabled = false;
        AudioListener_PC.enabled = false;
    }

    /// <summary>
    /// 展示一个部件组
    /// </summary>
    /// <param name="group"></param>
    public void ShowUnitGroup(UnitGroupT1 group)
    {
        currentUnitGroup = group;
        UIManagerGlobal._instance.ShowModel(group);
        UIManagerGlobal._instance.ShowUnitGroupInfo(group);
        GMSManagerGlobal._instance.SendUnitRecognitionObserver();
    }

    /// <summary>
    /// 停止展示当前部件组
    /// </summary>
    public void StopShowUnitGroup()
    {
        if (currentUnitGroup)
        {
            UIManagerGlobal._instance.StopShowUnitGroupInfo(currentUnitGroup);
            currentUnitGroup.ReturnGroupColor();
            currentUnitGroup = null;
            GMSManagerGlobal._instance.SendUnitRecognitionObserver();
        }
    }

    /// <summary>
    /// 展示当前部件的操作信息 （弃用）
    /// </summary>
    public void ShowCurrentModelOperation()
    {
        currentUnitGroup.ShowOperation();
    }

    /// <summary>
    /// 停止展示当前部件的操作信息 （弃用）
    /// </summary>
    public void StopShowCurrentModelOperation()
    {
        currentUnitGroup.StopShowOperation();
    }

    public void StartGame()
    {
        Debug.Log("开始");
        if(isStartGame) return;
        isStartGame = true;
        //UI切换
        UIManagerGlobal._instance.ActivePanel(operateMode);
        //相机切换
        ActiveCamera(operateMode);
        //数据发送
        if (isMultiplayerCollaboration)
        {
            GMSManagerGlobal._instance.SendStartObserverData();
            GMSManagerGlobal._instance.RegisterUnitRecognitionObserver();
            GMSManagerGlobal._instance.RegisterSettingsObserver();
            GMSManagerGlobal._instance.RegisterCameraTransformObserver();
            GMSManagerGlobal._instance.RegisterIntroBoardCtrl();
        }
    }

    public void SetMainCameraTransform(Vector3 position, Vector3 eulerAngles)
    {
        MainCameraGameObject_PC.transform.position = position;
        MainCameraGameObject_PC.transform.eulerAngles = eulerAngles;
    }

    public void SetCameraRigTransform(Vector3 position, Vector3 eulerAngles)
    {
        UIManagerGlobal._instance.CameraRigTransform.position = position;
        UIManagerGlobal._instance.CameraRigTransform.eulerAngles = eulerAngles;
    }

}

