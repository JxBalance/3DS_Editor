using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class  DRUnitButtonT2 : MonoBehaviour {

    public Transform ass;
    public GameObject controlPanel;
    public bool change = false;
    public int id;

    void Start()
    {
        //ass = GameManagerT2._instance.mechanismModels[0].transform;
    }

    //申请一个零件的类
    public void OnButtonClick()
    {
        Debug.Log("处理按钮点击");

        //if (GameManagerT2._instance.controlSliderStatus[id] == false)
        //{
        //    if (GameManagerT2._instance.PCstate)
        //    {
        //        Transform parentPC = GameObject.FindGameObjectWithTag("PC_UI_T2").transform.GetChild(0);
        //        SetSolider(parentPC);
        //    }
        //    else
        //    {
        //        Transform parentVR = GameObject.FindGameObjectWithTag("VR_UI_T2").transform.GetChild(1);
        //        SetSolider(parentVR);
        //    }

        //    GameManagerT2._instance.controlSliderStatus[id] = true;
        //}
    }

    public void SetSolider(Transform parent)
   {
        GameObject newcontrolPanel = Instantiate(controlPanel);
        newcontrolPanel.transform.SetParent(parent, false);
        //具体间距需要调试
        Vector3 pos = newcontrolPanel.transform.position;
            pos.y = pos.y + 60 * id;
        newcontrolPanel.transform.position = pos;
        newcontrolPanel.transform.GetChild(1).GetComponent<Text>().text = "作动筒";
        newcontrolPanel.transform.GetChild(0).GetComponent<ControlSliderT2>().setControlSliderID(id);    
    }
    public void setButtonId(int bid)
    {
        id = bid;
    }
}
