using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlobalView {

    private string[] globalUnits = new string[7] { "顶部板", "天花板", "遮光罩", "主仪表板", "左操纵台", "右操纵台", "中央操纵台" };
    private GameObject globalViewButton;
    private Vector3[] viewPoints;
    private UIManagerGlobal instance;
    private AutoPlayCtrl_T1 autoPlayPanel;
    private Text text;

    public GlobalView(UIManagerGlobal _instance, AutoPlayCtrl_T1 _autoPlayPanel)
    {
        instance = _instance;
        autoPlayPanel = _autoPlayPanel;
        globalViewButton = GameObject.Find("GlobalViewButton");
        viewPoints = new Vector3[7]
        {
            new Vector3(0f, 180f, 0f),
            new Vector3(0f, 120f, 0f),
            new Vector3(14f, 180f, 0f),
            new Vector3(26f, 180f, 0f),
            new Vector3(18f, 130f, 0f),
            new Vector3(18f, -130f, 0f),
            new Vector3(40f, 180f, 0f)
        };
        text = GameObject.Find("ThisText").GetComponent<Text>();
    }

    private IEnumerator ShowOneByOne(string[] units)
    {
        Camera.main.gameObject.transform.position = new Vector3(0f, 1.5f, 1f);
        Camera.main.gameObject.transform.eulerAngles = new Vector3(15f, 180f, 0f);
        text.text = "";

        for (int i = 0; i < globalUnits.Length; i++)
        {
            instance.PushPanel(autoPlayPanel);
            yield return new WaitForSeconds(2f);
            text.text = units[i];
            Camera.main.gameObject.transform.eulerAngles = viewPoints[i];
        }
    } 

    public void AutoPlay()
    {
        IeTools.Instance.StartCoroutine(ShowOneByOne(globalUnits));
    }
}
