using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DAUnitButtonT2 : MonoBehaviour {

    public Transform ass;
    public GameObject disassemblyPanel;
    public bool change = false;
    public int disassemblyIndex;

    void Start()
    {
        ass = GameManagerT2._instance.mechanismModels[0].transform;
    }

    //申请一个零件的类

    public void OnButtonClick()
    {
        if (GameManagerT2._instance.PCstate)
        {
            Transform parentPC = GameObject.FindGameObjectWithTag("PC_UI_T2").transform.GetChild(1);
            ToolPanel(parentPC);
        }
       else
        {
            Transform parentVR = GameObject.FindGameObjectWithTag("VR_UI_T2").transform.GetChild(2);
            ToolPanel(parentVR);
        }
        GameManagerT2._instance.toolButtonStatus = true;
    }


    public void ToolPanel(Transform parent)
    {
        if (GameManagerT2._instance.toolButtonStatus ==true)
        {
            Destroy(parent.GetChild(1).GetChild(0).gameObject);
        }
        GameObject newdisassemblyPanel = Instantiate(disassemblyPanel);

        newdisassemblyPanel.transform.SetParent(parent,false);
        //获取拆装工具
        int actToolIndex = GameManagerT2._instance.toolIndexList[disassemblyIndex - 1];
        //Debug.Log(actToolIndex);
        for (int i = 0; i < 3; i++)
        {
            Transform toolButton = newdisassemblyPanel.transform.GetChild(1).GetChild(0).GetChild(i);
            toolButton.transform.GetComponent<ToolButtonT2>().SetStepIndex(disassemblyIndex);
            if (i==actToolIndex)
            {
              toolButton.transform.GetComponent<ToolButtonT2>().SetAct();
            }
        }
    }


    public void setDisassemblyId(int did)
    {
        disassemblyIndex = did;
    }
}
