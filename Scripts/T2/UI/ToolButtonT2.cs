using UnityEngine;
using System.Collections;

public class ToolButtonT2 : MonoBehaviour {

    private int stepIndex;
    private bool act = false;
    public  Transform  ass;

    void Start()
    {
       // ass = GameManagerT2._instance.mechanismModels[0].transform;
    }

    public void OnButtonClick()
    {
        if (act)
        {
            Vector3 startPos = GameManagerT2._instance.startPosList[stepIndex - 1];
            //Debug.Log(startPos);
            Vector3 endPos = GameManagerT2._instance.endPosList[stepIndex - 1];
            //Debug.Log(endPos);
            
            GameObject part = GameManagerT2._instance.partList[stepIndex - 1];

            GameManagerT2._instance.mechanismModels[0].GetComponent<DisassemblyBase>().Part = part;
            GameManagerT2._instance.mechanismModels[0].GetComponent<DisassemblyBase>().V = (endPos - startPos) / 2;
            GameManagerT2._instance.mechanismModels[0].GetComponent<DisassemblyBase>().ActSi = true;
        }

    }

    public void SetAct()
    {
        act = true;
    }

    public void SetStepIndex(int sid)
    {
        stepIndex = sid;
    }

}
