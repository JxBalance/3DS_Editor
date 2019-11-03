using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using VRTK;

public class GameManagerT3 : GameManager
{
    //单例模式
    public static GameManagerT3 _instance;

    public GameObject airplaneModel;
    public int airplaneTransparent = 127;

    public List<PipeGroupT3> pipegroups = new List<PipeGroupT3>();
    public List<StateSetT3> stateSets = new List<StateSetT3>();

    public UISetT3 uiSets;

    public GameObject MainCameraGameObject_PC;
    public GameObject VRTK_SDKManagerGameObject_VR;
    public GameObject VRTK_ScriptsGameObject_VR;
    public GameObject StartUICanvasGameObject;
    public GameObject DesktopGameCanvasGameObject;
    public AudioListener AudioListener_VR;
    public AudioListener AudioListener_PC;

    public int currentCrewIndex = -1;
    public string nameStr;


    void Awake()
    {
        _instance = this;
    }

    // Use this for initialization
    void Start()
    {
        HideVirtices();                 //开始后隐藏流水点
        PlaneTransparentSetting();      //将飞机设为正常材质
        OnWaitStartState();
        //uiSets = new UISetT3();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void HideVirtices()
    {
        GameObject stateSetsParent = GameObject.Find("StateSetsParentT3(Clone)");
        int i = 0, j = 0, k = 0;
        for (i = 0; i < stateSetsParent.transform.childCount; i++)
        {
            Transform stateParent = stateSetsParent.transform.GetChild(i);
            stateParent.GetChild(0).gameObject.SetActive(false);                    //隐藏流水点
            stateParent.GetChild(1).gameObject.SetActive(false);                    //隐藏管道
        }
    }
    
    private void PlaneTransparentSetting()
    {
        if (airplaneModel)
        {
            SetAllChild(airplaneModel.transform, "Standard", 1f);
        }
    }

    //设置飞机模型所有节点和子节点的材质透明度
    public void SetAllChild(Transform parent, string shaderType, float trans)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            SetAllChild(parent.GetChild(i), shaderType, trans);
        }
        if (parent.GetComponent<Renderer>())
        {
            for (int i = 0; i < parent.GetComponent<Renderer>().materials.Length; i++)
            {
                parent.GetComponent<Renderer>().materials[i].shader = Shader.Find(shaderType);
                Color colorChange = new Color(parent.GetComponent<Renderer>().materials[i].color.r,
                                               parent.GetComponent<Renderer>().materials[i].color.g, 
                                                parent.GetComponent<Renderer>().materials[i].color.b, trans);
                parent.GetComponent<Renderer>().materials[i].color = colorChange;
            }
        }
    }

    public void OnWaitStartState()
    {
        MainCameraGameObject_PC.SetActive(true);
        VRTK_SDKManagerGameObject_VR.SetActive(true);
        VRTK_ScriptsGameObject_VR.SetActive(true);
        StartUICanvasGameObject.SetActive(true);
        //DesktopGameCanvasGameObject.SetActive(false);
        AudioListener_VR.enabled = false;
        AudioListener_PC.enabled = false;
    }

    public void ActiveCamera(OperateType operateType)
    {
        if (operateType == OperateType.桌面式)
        {
            MainCameraGameObject_PC.SetActive(true);
            VRTK_SDKManagerGameObject_VR.SetActive(false);
            VRTK_ScriptsGameObject_VR.SetActive(false);
            AudioListener_PC.enabled = true;
        }
        else if (operateType == OperateType.头盔式)
        {
            MainCameraGameObject_PC.SetActive(false);
            VRTK_SDKManagerGameObject_VR.SetActive(true);
            VRTK_ScriptsGameObject_VR.SetActive(true);
            AudioListener_VR.enabled = true;
        }
    }

    public void InitCameraGameObject(GameObject mainCameraGameObject_PC,
                                      GameObject startUICanvasGameObject, GameObject desktopGameCanvasGameObject,
                                       GameObject vrtk_SDKManagerGameObject_VR, GameObject vrtk_ScriptsGameObject_VR)
    {
        MainCameraGameObject_PC = mainCameraGameObject_PC;
        VRTK_SDKManagerGameObject_VR = vrtk_SDKManagerGameObject_VR;
        VRTK_ScriptsGameObject_VR = vrtk_ScriptsGameObject_VR;
        StartUICanvasGameObject = startUICanvasGameObject;
        DesktopGameCanvasGameObject = desktopGameCanvasGameObject;

        AudioListener_VR = VRTK_SDKManagerGameObject_VR.transform.GetChild(0).GetChild(3).GetChild(0).GetChild(3).GetChild(1).GetComponent<AudioListener>();
        AudioListener_PC = MainCameraGameObject_PC.GetComponent<AudioListener>();
    }
    /*
    public void ShowUnitGroup(UnitGroupT1 group)
    {
        currentUnitGroup = group;
        group.ChangeGroupColor();
        UIControllerT1._instance.ShowUnitGroupInfo(group);
        UIControllerT1._instance.CloseIntroPanel();
    }

    public void StopShowUnitGroup()
    {
        if (currentUnitGroup)
        {
            UIControllerT1._instance.StopShowUnitGroupInfo();
            currentUnitGroup.ReturnGroupColor();
            currentUnitGroup = null;
        }
    }

    public void ShowCurrentModelOperation()
    {
        currentUnitGroup.ShowOperation();
    }

    public void StopShowCurrentModelOperation()
    {
        currentUnitGroup.StopShowOperation();
    }
    */


}
