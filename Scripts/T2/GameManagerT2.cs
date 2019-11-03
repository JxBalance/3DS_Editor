using UnityEngine;

using System.Collections.Generic;


/// <summary>
/// 定义路线类，起始点终止点
/// </summary>
public class DisassemblyRoute
{
    private Vector3 startPos;
    private Vector3 endPos;

    public Vector3 StartPos
    {
        get { return startPos; }
        set { startPos = value; }
    }

    public Vector3 EndPos
    {
        get { return endPos; }
        set { endPos = value; }
    }
}


public class DisassemblyStep
{
    private string partName;
    private int toolIndex;
    private List<DisassemblyRoute> disassemblyRouteList;

    public string PartName
    {
        get { return partName; }
        set { partName = value; }
    }

    public int ToolIndex
    {
        get { return toolIndex; }
        set { toolIndex = value; }
    }

    public List<DisassemblyRoute> DisassemblyRouteList
    {
        get { return disassemblyRouteList; }
        set { disassemblyRouteList = value; }
    }
}

public class GameManagerT2 : GameManager
{
    public static GameManagerT2 _instance;
    public List<GameObject> mechanismModels = new List<GameObject>();
    public List<GameObject> environmentModels = new List<GameObject>();
    public List<bool> controlSliderStatus = new List<bool>();
    public List<DisassemblyStep> disassemblySteps = new List<DisassemblyStep>();
    public bool toolButtonStatus = false;

    public List<int> toolIndexList = new List<int>();
    public List<Vector3> startPosList = new List<Vector3>();
    public List<Vector3> endPosList = new List<Vector3>();
    public List<GameObject> partList = new List<GameObject>();

    //start界面
    public GameObject MainCameraGameObject_PC;
    public GameObject VRTK_SDKManagerGameObject_VR;
    public GameObject VRTK_ScriptsGameObject_VR;
    public AudioListener AudioListener_VR;
    public AudioListener AudioListener_PC;

    public  bool PCstate;
    // public List<UnitGroupT2> unitGroups = new List<UnitGroupT2>();
    //public UnitGroupT2 currentUnitGroup;


    public int currentCrewIndex = -1;
    public string nameStr;
    void Awake()
    {
        _instance = this;
    }

    // Use this for initialization
    void Start ()
    {
        OnWaitStartState();

    }
	
	// Update is called once per frame
	void Update () {

	}
    public void OnWaitStartState()
    {
        MainCameraGameObject_PC.SetActive(true);
        VRTK_SDKManagerGameObject_VR.SetActive(true);
        VRTK_ScriptsGameObject_VR.SetActive(true);
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
            MainCameraGameObject_PC.transform.position = new Vector3(-15, 0,10);
            MainCameraGameObject_PC.transform.rotation = Quaternion.Euler(0, 180, 0);
            AudioListener_PC.enabled = true;
            PCstate = true;
        }
        else if (operateType == OperateType.头盔式)
        {
            MainCameraGameObject_PC.SetActive(false);
            VRTK_SDKManagerGameObject_VR.SetActive(true);
            VRTK_ScriptsGameObject_VR.SetActive(true);
            AudioListener_VR.enabled = true;
            PCstate = false;
        }
    }

    public void InitCameraGameObject(GameObject mainCameraGameObject_PC, GameObject vrtk_SDKManagerGameObject_VR,
        GameObject vrtk_ScriptsGameObject_VR)
    {
        MainCameraGameObject_PC = mainCameraGameObject_PC;
        VRTK_SDKManagerGameObject_VR = vrtk_SDKManagerGameObject_VR;
        VRTK_ScriptsGameObject_VR = vrtk_ScriptsGameObject_VR;
        AudioListener_VR = VRTK_SDKManagerGameObject_VR.transform.GetChild(0).GetChild(3).GetChild(0).GetChild(3).GetChild(1).GetComponent<AudioListener>();
        AudioListener_PC = MainCameraGameObject_PC.GetComponent<AudioListener>();
    }

}
