using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetailShow : MonoBehaviour {

    bool isShowing = false;
    public GameObject detail1;
    public GameObject detail2;

    public void OnDetailShowClick()
    {
        switch (GamaManagerGlobal._instance.currentUnitGroup.ToString().Split('(')[0])
        {
            case "顶部版区域控制板 ":
                if (isShowing)
                {
                    detail1.SetActive(false);
                    isShowing = false;
                }
                else
                {
                    detail1.SetActive(true);
                    isShowing = true;
                }
                break;
            case "中央操纵台 ":
                if (isShowing)
                {
                    detail2.SetActive(false);
                    isShowing = false;
                }
                else
                {
                    detail2.SetActive(true);
                    isShowing = true;
                }
                break;
            default:
                break;
        }
    }
}
