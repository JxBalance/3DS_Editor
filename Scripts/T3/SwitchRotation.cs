using UnityEngine;
using System.Collections;

public class SwitchRotation : MonoBehaviour
{
    //鼠标信息变量
    bool isShowInfo;

    public GUIStyle _GUIStyle;
    public float Offset = 15;

    //开关信息变量
    public string switchNum = "";

    //开关集合变量
    GameObject switchGrp;

    // Use this for initialization
    void Start()
    {
        isShowInfo = false;
        switchGrp = GameObject.Find("switch");
    }

    // Update is called once per frame
    void Update()
    {

    }

    //鼠标触碰开关，鼠标旁显示开关信息
    void OnMouseEnter()
    {
        string name = gameObject.name;

        switch (name)
        {
            case "Tube031":
                switchNum = "开关1";
                break;
            case "Tube032":
                switchNum = "开关2";
                break;
            case "Tube025":
                switchNum = "开关3";
                break;
            case "Tube026":
                switchNum = "开关4";
                break;

        }
        isShowInfo = true;

        //gameObject.GetComponent<Renderer>().material.color = Color.gray;

    }

    void OnMouseExit()
    {
        isShowInfo = false;
    }


    //鼠标点击开关，调整开关状态
    void OnMouseDown()
    {
        string name = gameObject.name;
        string switchName = "";

        switch (name)
        {
            case "Tube031":
                switchName = "Cylinder011";
                switchGrp.GetComponent<SwitchCtrl>().setPipe(1);    //setPipe()设置管道状态
                break;
            case "Tube032":
                switchName = "Cylinder010";
                switchGrp.GetComponent<SwitchCtrl>().setPipe(2);
                break;
            case "Tube025":
                switchName = "Cylinder007";
                switchGrp.GetComponent<SwitchCtrl>().setPipe(3);
                break;
            case "Tube026":
                switchName = "Cylinder006";
                switchGrp.GetComponent<SwitchCtrl>().setPipe(4);
                break;

        }

        GameObject switches = GameObject.Find(switchName);

        //旋转开关部件
        if (switches.transform.rotation.y == 0)
        {
            switches.transform.eulerAngles = new Vector3(0f, -90f, 0f);
        }
        else
        {
            switches.transform.eulerAngles = new Vector3(0f, 0f, 0f);
        }

    }

    void OnGUI()
    {

        if (isShowInfo)
        {
            //鼠标旁显示开关信息
            GUI.Label(new Rect(Input.mousePosition.x + Offset, Screen.height - Input.mousePosition.y, 100, 100), 
                      switchNum);
        }

    }


}
