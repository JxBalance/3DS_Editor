using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using UnityEngine;

/// <summary>
/// 客户端连接观察者
/// </summary>
public class ConnectObserverCtrl : BaseObserverCtrl
{

    // Use this for initialization
    protected override void Start ()
    {
        base.Start();
        time = 2f;
        useTimer = true;
    }

    protected override void Update()
    {
        base.Update();
    }

    /// <summary>
    /// 解析数据 存入DataList 并等待Update处理
    /// </summary>
    /// <param name="jd"></param>
    public override void AnalysisData(JObject jd)
    {
        base.AnalysisData(jd);
        try
        {
            DataQueue.Enqueue(new ConnectData(Int32.Parse(jd["ID"].ToString())));
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
            throw;
        }
    }
    /// <summary>
    /// Update调用 在unity主线程中处理数据 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="dataList"></param>
    protected override void ProcessingData<T>(Queue<T> dataQueue)
    {
        base.ProcessingData(dataQueue);
        while (dataQueue.Count > 0)
        {
            ConnectData data = dataQueue.Dequeue() as ConnectData;
            if(data == null) return;
            //逐条处理
            GMSManagerGlobal._instance.ClientConnectionCheck(data.ID);
        }
    }
    
    /// <summary>
    /// 数据打包 
    /// </summary>
    /// <returns></returns>
    protected override string PackageData()
    {
        base.PackageData();
        //创建JSON数据包
        JObject jd = new JObject();
        jd["PGS"] = GamaManagerGlobal._instance.Setting.SceneName;//程序识别码
        jd["IDT"] = "CON";//数据识别码
        jd["ID"] = GMSManagerGlobal._instance.ID.ToString();//ID
        return Regex.Unescape(jd.ToString());//序列化JSON对象
    }
}

/// <summary>
/// 客户端连接数据格式类  便于列表管理
/// </summary>
public class ConnectData : BaseData
{
    public int ID;

    public ConnectData(int ID)
    {
        this.ID = ID;
    }
}