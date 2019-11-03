using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using VRTK;

public class GameManagerT1 : GameManager
{
    //单例模式
    public static GameManagerT1 _instance;

    public List<GameObject> models = new List<GameObject>();
    public List<IntroduceBoardT1> introBoard = new List<IntroduceBoardT1>();
    public List<UnitGroupT1> unitGroups = new List<UnitGroupT1>();

    public UnitGroupT1 currentUnitGroup;

    public GameObject MainCameraGameObject_PC;
    public GameObject VRTK_SDKManagerGameObject_VR;
    public GameObject VRTK_ScriptsGameObject_VR;

    public AudioListener AudioListener_VR;
    public AudioListener AudioListener_PC;

    private List<GameObject> showModels = new List<GameObject>();

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
	void Update () 
    {
	
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

    public void InitCameraGameObject(GameObject mainCameraGameObject_PC, GameObject vrtk_SDKManagerGameObject_VR,
        GameObject vrtk_ScriptsGameObject_VR)
    {
        MainCameraGameObject_PC = mainCameraGameObject_PC;
        VRTK_SDKManagerGameObject_VR = vrtk_SDKManagerGameObject_VR;
        VRTK_ScriptsGameObject_VR = vrtk_ScriptsGameObject_VR;
        AudioListener_VR = VRTK_SDKManagerGameObject_VR.transform.GetChild(0).GetChild(3).GetChild(0).GetChild(3).GetChild(1).GetComponent<AudioListener>();
        AudioListener_PC = MainCameraGameObject_PC.GetComponent<AudioListener>();
    }

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

}
