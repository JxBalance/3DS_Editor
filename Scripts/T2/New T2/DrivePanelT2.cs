using System.Collections;
using System.Collections.Generic;
using Global;
using UnityEngine;
using UnityEngine.UI;

public class DrivePanelT2 : BasePanel_T1
{

    [SerializeField]protected RectTransform ButtonsParent;

    protected Drive currentDrive;

    public Drive CurrentDrive
    {
        set { currentDrive = value; }
    }


    public override void OnOpenPanel()
    {
        base.OnOpenPanel();
        UpdateList();
        Debug.Log(GamaManagerGlobal._instance.currentUnitGroup);
    }

    public override void OnClosePanel()
    {
        base.OnClosePanel();
        //面板被关闭 采取一些操作

    }

    public void UpdateList()
    {
        //删除已有的按钮
        for (int i = 0; i < ButtonsParent.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
        //根据现在的currentDrive生成新的按钮
        for (int i = 0; i < currentDrive.ActuatorModelName.Count; i++)
        {
            GameObject ButtonGameObject =
                Instantiate(Resources.Load(ResourcesPath.DRUnitButtonT2GameObject) as GameObject, ButtonsParent);
            ButtonGameObject.transform.GetChild(0).GetComponent<Text>().text = currentDrive.ActuatorModelName[i];
            ButtonGameObject.GetComponent<DRUnitButtonT2>().setButtonId(i);
        }
        //Debug.Log("根据现在的currentDrive生成新的按钮");
    }
}
