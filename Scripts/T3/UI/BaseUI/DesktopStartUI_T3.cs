using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class DesktopStartUI_T3 : MonoBehaviour
{

    public static DesktopStartUI_T3 _instance;
    public Text titleText;
    public InputField ipInput;
    public Dropdown operateDropDown;
    public Dropdown crewDropdown;
    public Dropdown examDropdown;
    public Toggle gmsToggle;
    public GameObject operateGameObject;
    public GameObject crewGameObject;
    public GameObject ipGameObject;
    public GameObject DesktopCanvasGameObject;
    public GameObject VRLeftUI;

    void Awake()
    {
        _instance = this;
    }

	// Use this for initialization
	void Start ()
	{
        //测试用默认IP
        if (GameManagerT1._instance != null)
        {
            if (GameManagerT1._instance.nameStr != "")
            {
                titleText.text = GameManagerT1._instance.nameStr;
            }
            ipInput.text = "127.0.0.1";
            List<string> crewNames = new List<string>();
            foreach (var c in GameManagerT1._instance.crew)
            {
                crewNames.Add(c.ToString());
            }
            crewDropdown.AddOptions(crewNames);

        }
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
    /// 显示IP输入
    /// </summary>
    /// <returns></returns>
    private IEnumerator ShowIPInput()
    {
        operateGameObject.transform.DOLocalMoveY(90f, 0.25f);
        crewGameObject.transform.DOLocalMoveY(40f, 0.25f);        
        yield return new WaitForSeconds(0.15f);
        ipGameObject.SetActive(true);
    }
    /// <summary>
    /// 隐藏IP输入
    /// </summary>
    /// <returns></returns>
    private IEnumerator HideIPInput()
    {
        ipGameObject.SetActive(false);
        operateGameObject.transform.DOLocalMoveY(55f, 0.25f);
        crewGameObject.transform.DOLocalMoveY(5f, 0.25f);
        yield return null;        
    }

    /// <summary>
    /// 开始
    /// </summary>
    public void StartButtonClick()
    {
        //开始进行
        GameManagerT3._instance.currentCrewIndex = crewDropdown.value;
        //是否启动多人协同
        if (gmsToggle.isOn)
        {
            
        }
        else
        {
            
        }
        //UI切换
        UIControllerT3._instance.ActivePanel((OperateType)operateDropDown.value);
        //相机切换
        GameManagerT3._instance.ActiveCamera((OperateType)operateDropDown.value);
    }
}
