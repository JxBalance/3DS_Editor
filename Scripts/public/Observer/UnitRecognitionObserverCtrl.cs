using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Global;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class UnitRecognitionObserverCtrl : BaseObserverCtrl
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
                if (id == GMSManagerGlobal._instance.CurrentOperateFollowID)
                {
                    DataQueue.Enqueue(new UnitRecognitionData(Int32.Parse(jd["ID"].ToString()), Int32.Parse(jd["GRP"].ToString()),
                        Boolean.Parse(jd["PAN"].ToString()), Int32.Parse(jd["SHW"].ToString())));
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
            UnitRecognitionData data = dataQueue.Dequeue() as UnitRecognitionData;
            if (data == null) return;
            GamaManagerGlobal._instance.currentUnitGroupIndex = data.GRP;
            UIManagerGlobal._instance.isShowUnitRecognitionPanel = data.PAN;
            UIManagerGlobal._instance.currentShowModelStateIndex = data.SHW;
        }
    }

    protected override string PackageData()
    {
        base.PackageData();
        string str = base.PackageData();
        //创建JSON数据包
        JObject jd = new JObject();
        jd["PGS"] = GamaManagerGlobal._instance.Setting.SceneName;//程序识别码
        jd["IDT"] = "REC";//数据识别码
        jd["ID"] = GMSManagerGlobal._instance.ID.ToString();//ID
        jd["GRP"] = GamaManagerGlobal._instance.currentUnitGroupIndex.ToString();
        jd["PAN"] = UIManagerGlobal._instance.isShowUnitRecognitionPanel.ToString();
        jd["SHW"] = UIManagerGlobal._instance.currentShowModelStateIndex.ToString();
        return Regex.Unescape(jd.ToString());//序列化JSON对象
    }

}

public class UnitRecognitionData : BaseData
{
    public int ID;
    public int GRP;
    public bool PAN;
    public int SHW;

    public UnitRecognitionData(int ID, int GRP, bool PAN, int SHW)
    {
        this.ID = ID;
        this.GRP = GRP;
        this.PAN = PAN;
        this.SHW = SHW;
    }
}