using System;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class IntroBoardObserverCtrl : BaseObserverCtrl
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
                    DataQueue.Enqueue(new IntroBoardData(Int32.Parse(jd["ID"].ToString()),
                        Boolean.Parse(jd["PAN"].ToString()), Int32.Parse(jd["NUM"].ToString())));
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
            IntroBoardData data = dataQueue.Dequeue() as IntroBoardData;
            if (data == null) return;
            UIManagerGlobal._instance.isShowIntroBoardPanel = data.PAN;
            UIManagerGlobal._instance.currentShowIntroBoardIndex = data.NUM;
        }
    }

    protected override string PackageData()
    {
        base.PackageData();
        string str = base.PackageData();
        //创建JSON数据包
        JObject jd = new JObject();
        jd["PGS"] = GamaManagerGlobal._instance.Setting.SceneName;//程序识别码
        jd["IDT"] = "INT";//数据识别码
        jd["ID"] = GMSManagerGlobal._instance.ID.ToString();//ID
        jd["PAN"] = UIManagerGlobal._instance.isShowIntroBoardPanel.ToString();
        jd["NUM"] = UIManagerGlobal._instance.currentShowIntroBoardIndex.ToString();
        return Regex.Unescape(jd.ToString());//序列化JSON对象
    }

}
public class IntroBoardData : BaseData
{
    public int ID;
    public bool PAN;
    public int NUM;

    public IntroBoardData(int ID, bool PAN, int NUM)
    {
        this.ID = ID;
        this.PAN = PAN;
        this.NUM = NUM;
    }
}