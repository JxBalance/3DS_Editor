using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Global;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class SettingsObserverCtrl : BaseObserverCtrl
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
                    DataQueue.Enqueue(new SettingsData(Int32.Parse(jd["ID"].ToString()), Int32.Parse(jd["GPF"].ToString()),
                        Int32.Parse(jd["GOF"].ToString()), Boolean.Parse(jd["SPS"].ToString())));
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
            SettingsData data = dataQueue.Dequeue() as SettingsData;
            if (data == null) return;
            //进行全局数据处理
            GMSManagerGlobal._instance.GlobalPositionFollowID = data.GPF;
            GMSManagerGlobal._instance.GlobalOperateFollowID = data.GOF;
            GMSManagerGlobal._instance.StudentsFollowSettings = data.SPS;
            
        }

    }

    protected override string PackageData()
    {
        base.PackageData();
        string str = base.PackageData();
        //创建JSON数据包
        JObject jd = new JObject();
        jd["PGS"] = GamaManagerGlobal._instance.Setting.SceneName;//程序识别码
        jd["IDT"] = "SET";//数据识别码
        jd["ID"] = GMSManagerGlobal._instance.ID.ToString();//ID
        jd["GPF"] = GMSManagerGlobal._instance.GlobalPositionFollowID.ToString();
        jd["GOF"] = GMSManagerGlobal._instance.GlobalOperateFollowID.ToString();
        jd["SPS"] = GMSManagerGlobal._instance.StudentsFollowSettings.ToString();
        //jd["PPF"] = GMSManagerGlobal._instance.PersonalPositionFollowID.ToString();
        //jd["POF"] = GMSManagerGlobal._instance.PersonalOperateFollowID.ToString();
        return Regex.Unescape(jd.ToString());//序列化JSON对象
    }
}

public class SettingsData : BaseData
{
    public int ID;
    public int GPF;
    public int GOF;
    public bool SPS;
    //public int PPF;
    //public int POF;

    public SettingsData(int ID, int GPF, int GOF, bool SPS)
    {
        this.ID = ID;
        this.GPF = GPF;
        this.GOF = GOF;
        this.SPS = SPS;
        //this.PPF = PPF;
        //this.POF = POF;
    }
}