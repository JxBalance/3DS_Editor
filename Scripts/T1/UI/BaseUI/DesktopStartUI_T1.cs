using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class DesktopStartUI_T1 : MonoBehaviour
{

    public static DesktopStartUI_T1 _instance;
    public Text titleText;
    public InputField ipInput;
    public Dropdown operateDropDown;
    public Dropdown crewDropdown;
    public Dropdown examDropdown;
    public Toggle gmsToggle;


    public GameObject OperateGameObject;
    public GameObject CrewGameObject;
    public GameObject IPGameObject;
    public GameObject ExamModeGameObject;
    public GameObject DesktopCanvasGameObject;

    public GameObject StartButtonGameObject;
    public GameObject NextButtonGameObject;
    

    void Awake()
    {
        _instance = this;
    }

	// Use this for initialization
	void Start ()
	{
        ////测试用默认IP
        //if (GameManagerT1._instance != null)
        //{
        //    if (GameManagerT1._instance.nameStr != "")
        //    {
        //        titleText.text = GameManagerT1._instance.nameStr;
        //    }
        //    ipInput.text = "127.0.0.1";
        //    List<string> crewNames = new List<string>();
        //    foreach (var c in GameManagerT1._instance.crew)
        //    {
        //        crewNames.Add(c.ToString());
        //    }
        //    crewDropdown.AddOptions(crewNames);

        //}

    }

    void Update()
    {
        //Debug.Log(OperateGameObject.transform.localPosition);

    }

    public void OnGMSToggleChanged(bool isOn)
    {
        if(isOn)
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
        OperateGameObject.transform.DOLocalMoveY(220f, 0.25f);
        CrewGameObject.transform.DOLocalMoveY(132f, 0.25f);
        ExamModeGameObject.transform.DOLocalMoveY(-44f, 0.25f);
        yield return new WaitForSeconds(0.15f);
        IPGameObject.SetActive(true);
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
        OperateGameObject.transform.DOLocalMoveY(220f, 0.25f);
        CrewGameObject.transform.DOLocalMoveY(132f, 0.25f);
        yield return null;
        StartButtonGameObject.SetActive(true);
        NextButtonGameObject.SetActive(false);
    }

    /// <summary>
    /// 开始
    /// </summary>
    public void StartButtonClick()
    {
        //开始进行
        //GameManagerT1._instance.currentCrewIndex = crewDropdown.value;
        //是否启动多人协同
        if (gmsToggle.isOn)
        {
            
        }
        else
        {
            
        }
        //UI切换
        UIManagerGlobal._instance.ActivePanel(GamaManagerGlobal._instance.operateMode);
        //相机切换
        GamaManagerGlobal._instance.ActiveCamera(GamaManagerGlobal._instance.operateMode);
    }
}
