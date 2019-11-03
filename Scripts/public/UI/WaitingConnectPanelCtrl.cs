using System.Collections;
using System.Collections.Generic;
using Global;
using UnityEngine;

public class WaitingConnectPanelCtrl : BasePanel_T1
{

    public GameObject ClientItemGameObject;
    public GameObject ClientListParent;

    public List<ClientItem> ClientItemList = new List<ClientItem>();

    public GameObject StartButtonGameObject;
    public GameObject WaitingTextGameObject;

    /// <summary>
    /// 界面初始化
    /// </summary>
    public void InitPanel()
    {
        for (int i = 0; i < ClientListParent.transform.childCount; i++)
        {
            Destroy(ClientListParent.transform.GetChild(i).gameObject);
        }
        ClientItemList.Clear();
        for (int i = 0; i < GamaManagerGlobal._instance.Setting.ClientArray.Length; i++)
        {
            GameObject go = Instantiate(ClientItemGameObject, ClientListParent.transform);
            ClientItem item = go.GetComponent<ClientItem>();
            item.InitItem(GMSManagerGlobal._instance.clientInfoList[i]);
            ClientItemList.Add(item);
        }


        if (GMSManagerGlobal._instance.currentClientMode == ClientMode.教员端)
        {
            StartButtonGameObject.SetActive(true);
            WaitingTextGameObject.SetActive(false);
        }
        else
        {
            StartButtonGameObject.SetActive(false);
            WaitingTextGameObject.SetActive(true);
        }

    }

    /// <summary>
    /// 返回按钮点击响应
    /// </summary>
    public void BackButtonClick()
    {
        GMSManagerGlobal._instance.DisconnectServer();
        UIManagerGlobal._instance.SwitchToStartPanel();
    }

    /// <summary>
    /// 客户端连接检测
    /// </summary>
    /// <param name="id"></param>
    public void ClientConnectionCheck(int id)
    {
        ClientItemList[id].OnConnect();
    }

    /// <summary>
    /// 客户端断开检测
    /// </summary>
    /// <param name="id"></param>
    public void ClientDisconnectionCheck(int id)
    {
        ClientItemList[id].OnDisconnect();
    }
    /// <summary>
    /// 开始按钮点击响应
    /// </summary>
    public void StartButtonClick()
    {
        GamaManagerGlobal._instance.StartGame();
        return;

        bool TeacherConnect = false;
        bool StudentConnect = false;
        foreach (var c in GMSManagerGlobal._instance.clientInfoList)
        {
            if (c.clientMode == ClientMode.教员端)
            {
                if (c.isConnect)
                {
                    TeacherConnect = true;
                }
            }
            if (c.clientMode == ClientMode.学员端)
            {
                if (c.isConnect)
                {
                    StudentConnect = true;
                }
            }
        }

        if (TeacherConnect && StudentConnect)
        {
            GamaManagerGlobal._instance.StartGame();
        }
        else
        {
            string clientMsg = "";
            if (!TeacherConnect)
            {
                clientMsg += "教员端 ";
            }
            if (!StudentConnect)
            {
                clientMsg += "学员端 ";
            }
            ShowMainMessage("相关客户端未连接：" + clientMsg);
        }
        
    }
}
