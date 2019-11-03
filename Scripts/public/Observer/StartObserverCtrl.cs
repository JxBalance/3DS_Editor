using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Global;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class StartObserverCtrl : BaseObserverCtrl
{
    protected override void Start()
    {
        base.Start();
        useTimer = false;
    }

    public override void AnalysisData(JObject jd)
    {
        base.AnalysisData(jd);
        try
        {
            int id = Int32.Parse(jd["ID"].ToString());
            if (id != GMSManagerGlobal._instance.ID)
            {
                if (GMSManagerGlobal._instance.clientInfoList[id].clientMode == ClientMode.教员端)
                {
                    DataQueue.Enqueue(new StartData(Int32.Parse(jd["ID"].ToString())));
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
            throw;
        }
    }

    protected override void ProcessingData<T>(Queue<T> dataQueue)
    {
        base.ProcessingData(dataQueue);
        while (dataQueue.Count > 0)
        {
            StartData data = dataQueue.Dequeue() as StartData;
            if (data == null) return;
            GamaManagerGlobal._instance.StartGame();
        }
    }

    protected override string PackageData()
    {
        base.PackageData();
        //创建JSON数据包
        JObject jd = new JObject();
        jd["PGS"] = GamaManagerGlobal._instance.Setting.SceneName;//程序识别码
        jd["IDT"] = "STT";//数据识别码
        jd["ID"] = GMSManagerGlobal._instance.ID.ToString();//ID
        return Regex.Unescape(jd.ToString());//序列化JSON对象
    }
}

public class StartData : BaseData
{
    public int ID;

    public StartData(int ID)
    {
        this.ID = ID;
    }
}
