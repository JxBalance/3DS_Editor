using UnityEngine;
using System.Collections;

public class SwitchCtrl : MonoBehaviour {

    //开关数量
    static int amounts = 4;
    //bool[] switchOn = { true, true, true, true };

    //开关状态变量
    bool[] switchOn = new bool[amounts];

    //预制管道物体
    public GameObject pipeEnter;
    public GameObject pipeA;
    public GameObject pipeB;
    public GameObject pipeExit;

    //开关变量
    public GameObject[] switches = new GameObject[amounts];

    //设置管道状态，变量a代表开关数字
    public void setPipe(int a)  
    {
        a -= 1;
        //改变开关状态，开至关 或 关至开
        if (switchOn[a] == true)
        {
            switchOn[a] = false;
        }
        else
        {
            switchOn[a] = true;
        }

        //获取各开关状态，判断各管道状态
        if (switchOn[0] == false || switchOn[2] == false)
        {
            pipeA.SetActive(true);
            if (switchOn[1] == false || switchOn[3] == false)
            {
                pipeExit.SetActive(true);
            }
        }
        else
        {
            pipeA.SetActive(false);
            pipeExit.SetActive(false);
        }
        if (switchOn[1] == false || switchOn[3] == false)
        {
            pipeB.SetActive(true);
        }
        else
        {
            pipeB.SetActive(false);
            pipeExit.SetActive(false);
        }
        
    }

    // Use this for initialization
    void Start () {
        //openAllSwitch();

        //获取初始开关状态
        switchStatus();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    //获取初始开关状态
    public void switchStatus()
    {
        GameObject test = GameObject.Find("Cylinder011");
        if ( test.transform.eulerAngles.y == 0)
        {
            switchOn[0] = false;
            pipeA.SetActive(true);
        }
        else
        {
            switchOn[0] = true;
        }
        test = GameObject.Find("Cylinder010");
        if (test.transform.eulerAngles.y == 0)
        {
            switchOn[1] = false;
            pipeB.SetActive(true);
        }
        else
        {
            switchOn[1] = true;
        }
        test = GameObject.Find("Cylinder007");
        if (test.transform.eulerAngles.y == 0)
        {
            switchOn[2] = false;
            pipeA.SetActive(true);
        }
        else
        {
            switchOn[2] = true;
        }
        test = GameObject.Find("Cylinder006");
        if (test.transform.eulerAngles.y == 0)
        {
            switchOn[3] = false;
            pipeB.SetActive(true);
        }
        else
        {
            switchOn[3] = true;
        }

    }

    //打开所有开关
    public void openAllSwitch()
    {
        switchOn[0] = true;
        switchOn[1] = true;
        switchOn[2] = true;
        switchOn[3] = true;
        pipeA.SetActive(false);
        pipeB.SetActive(false);
        pipeExit.SetActive(false);
    }
    
    //关闭所有开关
    public void closeAllSwitch()
    {
        switchOn[0] = false;
        switchOn[1] = false;
        switchOn[2] = false;
        switchOn[3] = false;
        pipeA.SetActive(true);
        pipeB.SetActive(true);
        pipeExit.SetActive(true);
    }
}
