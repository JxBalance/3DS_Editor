using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RightBoardCtrlT3 : MonoBehaviour {

    public GameObject stateButtonPerfab;

    //public List<StateSetT3> state = new List<StateSetT3>();

    private GameManagerT3 gm;

    private void OnLastButtonClick() 
    {

        GameManagerT3 gm = GameObject.Find("GameManagerT3(Clone)").GetComponent<GameManagerT3>();

        //飞机设为透明
        gm.SetAllChild(gm.airplaneModel.transform, "Transparent/Diffuse", (gm.airplaneTransparent / 255f));

        int count = transform.childCount;
        for(int i = 0; i < count; i++)
        {
            if (transform.GetChild(i).GetComponent<ButtonAttrT3>())
            {
                int index = transform.GetChild(i).GetComponent<ButtonAttrT3>().index;
                if (transform.GetChild(i).GetComponent<ButtonAttrT3>().isClick == 1)
                {
                    //显示管道

                    //管道设为透明
                    int pipeCount = gm.uiSets.uiStates[index].statePipegroups[0].pipeModels.Count;                      //这里应该读取多个管道组而不是第一个
                    for (int j = 0; j < pipeCount; j++)
                    {
                        gm.SetAllChild(gm.uiSets.uiStates[index].statePipegroups[0].pipeModels[j].transform, "Transparent/Diffuse", (60 / 255f));
                    }
                    //显示流水
                    gm.uiSets.uiStates[index].transform.GetChild(1).gameObject.SetActive(true);
                    //按钮设为上次点击状态
                    transform.GetChild(i).GetComponent<ButtonAttrT3>().isClick = 2;
                    //修改界面标题
                    GameObject stateName = GameObject.Find("StateName");
                    stateName.GetComponent<Text>().text = gm.uiSets.uiStates[index].stateTitle;
                }
                else if(transform.GetChild(i).GetComponent<ButtonAttrT3>().isClick == 2)
                {
                    //不显示管道

                    //管道设为不透明
                    int pipeCount = gm.uiSets.uiStates[index].statePipegroups[0].pipeModels.Count;                      //这里应该读取多个管道组而不是第一个
                    for (int j = 0; j < pipeCount; j++)
                    {
                        gm.SetAllChild(gm.uiSets.uiStates[index].statePipegroups[0].pipeModels[j].transform, "Standard", 1f);
                    }
                    //隐藏流水
                    gm.uiSets.uiStates[index].transform.GetChild(1).gameObject.SetActive(false);
                    //按钮设为未点击状态
                    transform.GetChild(i).GetComponent<ButtonAttrT3>().isClick = 0;
                }
            }
        }

        
    }

    // Use this for initialization
    void Start () {
        
        //LastButton2.onClick.AddListener(OnLastButtonClick);        
        int i, j = 0;

        GameManagerT3 gm = GameObject.Find("GameManagerT3(Clone)").GetComponent<GameManagerT3>();
        int count = 0;
        if (gm.uiSets)
        {
            if (gm.uiSets.uiStates.Count > 0)
            {
                count = gm.uiSets.uiStates.Count;
            }
        }

        GameObject detailButton = GameObject.Find("DatailButton");
        detailButton.transform.parent = gameObject.transform.parent;
        GameObject rightBackground = GameObject.Find("RightBoard");

        for (i = 0; i < count; i++)
        {
            if (i > 0)              //向下排列按钮
            {
                int num = rightBackground.transform.childCount;
                GameObject[] child = new GameObject[num];
                for (j = 0; j < num; j++)
                {
                    child[j] = rightBackground.transform.GetChild(j).gameObject;
                }
        ;
                for (j = 0; j < num; j++)
                {
                    if (child[j].name == "StateText")
                    {
                        continue;
                    }
                    else
                    {
                        child[j].transform.parent = rightBackground.transform.parent;
                    }
                }

                transform.localPosition = new Vector3(transform.localPosition.x, (transform.localPosition.y - 30f), transform.localPosition.z);
                float newY = GetComponent<Image>().rectTransform.sizeDelta.y + 60f;
                GetComponent<Image>().rectTransform.sizeDelta
                    = new Vector2(GetComponent<Image>().rectTransform.sizeDelta.x, newY);

                for (j = 0; j < num; j++)
                {
                    if (child[j].name == "StateText")
                    {
                        continue;
                    }
                    else
                    {
                        child[j].transform.parent = rightBackground.transform;
                    }
                }
            }
            GameObject createButton = GameObject.Instantiate(stateButtonPerfab);
            createButton.name = "stateButton0" + i;
            createButton.transform.parent = transform.parent;

            createButton.transform.localScale = new Vector3(1f, 1f, 1f);
            //createButton.transform.localPosition = new Vector3(360f, (200f - (60f * i)), 0f);
            createButton.transform.localPosition = new Vector3(detailButton.transform.localPosition.x, detailButton.transform.localPosition.y + 80f - 60f * i, 0f);

            createButton.transform.parent = transform;
            createButton.transform.localScale = new Vector3(1f, 1f, 1f);
            createButton.GetComponent<ButtonAttrT3>().index = i;
            createButton.transform.GetChild(0).GetComponent<Text>().text = gm.uiSets.uiStates[i].stateTitle;

            createButton.GetComponent<Button>().onClick.AddListener(OnLastButtonClick);

        }
        detailButton.transform.localPosition = new Vector3(detailButton.transform.localPosition.x,
                                                       (detailButton.transform.localPosition.y - (60f * (i - 1))),
                                                       detailButton.transform.localPosition.z);
        detailButton.transform.parent = transform;
        detailButton.transform.localScale = new Vector3(1f, 1f, 1f);

    }
	
	// Update is called once per frame
	void Update () {

    }
}
