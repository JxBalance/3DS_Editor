using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Global;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class TestObserverCtrl : BaseObserverCtrl
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
                    DataQueue.Enqueue(new TestData(Int32.Parse(jd["ID"].ToString()), Int32.Parse(jd["x"].ToString()),
                        Int32.Parse(jd["y"].ToString()), Int32.Parse(jd["z"].ToString())));
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
            TestData data = dataQueue.Dequeue() as TestData;
            if (data == null) return;
            //进行全局数据处理
            GMSManagerGlobal._instance.JIGOU_X = data.x;
            GMSManagerGlobal._instance.JIGOU_X = data.y;
            GMSManagerGlobal._instance.JIGOU_X = data.z;

        }

    }

    protected override string PackageData()
    {
        base.PackageData();
        string str = base.PackageData();
        //创建JSON数据包
        JObject jd = new JObject();
        jd["PGS"] = GamaManagerGlobal._instance.Setting.SceneName;//程序识别码
        jd["IDT"] = "JIGOUYUNDONG";//数据识别码
        jd["ID"] = GMSManagerGlobal._instance.ID.ToString();//ID

        jd["X"] = GMSManagerGlobal._instance.JIGOU_X.ToString();
        jd["Y"] = GMSManagerGlobal._instance.JIGOU_X.ToString();
        jd["Z"] = GMSManagerGlobal._instance.JIGOU_X.ToString();
        //jd["PPF"] = GMSManagerGlobal._instance.PersonalPositionFollowID.ToString();
        //jd["POF"] = GMSManagerGlobal._instance.PersonalOperateFollowID.ToString();
        return Regex.Unescape(jd.ToString());//序列化JSON对象
    }
}
public class TestData : BaseData
{
    public int ID;
    
    public float x;
    public float y;
    public float z;

    public TestData(int ID, int x, int y, int z)
    {
        this.ID = ID;
        this.x = x;
        this.y = y;
        this.z = z;
        //this.PPF = PPF;
        //this.POF = POF;
    }
}