using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Global;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class CameraTransformObserverCtrl : BaseObserverCtrl
{
    protected override void Start()
    {
        base.Start();
        useTimer = true;
        time = 0.01f;
    }

    public override void AnalysisData(JObject jd)
    {
        base.AnalysisData(jd);
        try
        {
            int id = Int32.Parse(jd["ID"].ToString());
            if (id != GMSManagerGlobal._instance.ID)
            {
                if (id == GMSManagerGlobal._instance.CurrentPositionFollowID)
                {
                    DataQueue.Enqueue(new CameraTransformData(id,(OperateMode) Int32.Parse(jd["OM"].ToString()),
                        float.Parse(jd["CPX"].ToString()), float.Parse(jd["CPY"].ToString()),
                        float.Parse(jd["CPZ"].ToString()), float.Parse(jd["CRX"].ToString()),
                        float.Parse(jd["CRY"].ToString()), float.Parse(jd["CRZ"].ToString()),
                        float.Parse(jd["EPX"].ToString()), float.Parse(jd["EPY"].ToString()),
                        float.Parse(jd["EPZ"].ToString()), float.Parse(jd["ERX"].ToString()),
                        float.Parse(jd["ERY"].ToString()), float.Parse(jd["ERZ"].ToString()),
                        float.Parse(jd["RPX"].ToString()), float.Parse(jd["RPY"].ToString()),
                        float.Parse(jd["RPZ"].ToString()), float.Parse(jd["RRX"].ToString()),
                        float.Parse(jd["RRY"].ToString()), float.Parse(jd["RRZ"].ToString())));
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
        
        while (DataQueue.Count > 0)
        {
            CameraTransformData data = DataQueue.Dequeue() as CameraTransformData;
            if (data == null) return;
            Vector3 position, eulerAngles;
            switch (data.operateMode)
            {
                case OperateMode.桌面式:
                    //来自于桌面式数据
                    switch (GamaManagerGlobal._instance.operateMode)
                    {
                        case OperateMode.桌面式:
                            //用于桌面式
                            position = new Vector3(data.MainCameraPositionX, data.MainCameraPositionY,
                                data.MainCameraPositionZ);
                            eulerAngles = new Vector3(data.MainCameraEulerAnglesX, data.MainCameraEulerAnglesY,
                                data.MainCameraEulerAnglesZ);
                            GamaManagerGlobal._instance.SetMainCameraTransform(position, eulerAngles);
                            break;
                        case OperateMode.头盔式:
                            //用于头盔式
                            position = new Vector3(data.MainCameraPositionX, data.MainCameraPositionY,
                                data.MainCameraPositionZ);
                            eulerAngles = new Vector3(0, data.MainCameraEulerAnglesY, 0);
                            GamaManagerGlobal._instance.SetCameraRigTransform(position, eulerAngles);
                            break;
                    }
                    break;
                case OperateMode.头盔式:
                    //来自于桌面式数据
                    switch (GamaManagerGlobal._instance.operateMode)
                    {
                        case OperateMode.桌面式:
                            //用于桌面式
                            position = new Vector3(data.EyePositionX, data.EyePositionY, data.EyePositionZ);
                            eulerAngles = new Vector3(data.EyeEulerAnglesX, data.EyeEulerAnglesY, data.EyeEulerAnglesZ);
                            GamaManagerGlobal._instance.SetMainCameraTransform(position, eulerAngles);
                            break;
                        case OperateMode.头盔式:
                            //用于头盔式
                            position = new Vector3(data.CameraRigPositionX, data.CameraRigPositionY,
                                data.CameraRigPositionZ);
                            eulerAngles = new Vector3(0, data.EyeEulerAnglesY, 0);
                            GamaManagerGlobal._instance.SetCameraRigTransform(position, eulerAngles);
                            break;
                    }
                    break;
            }

        }
    }

    protected override string PackageData()
    {
        base.PackageData();
        //创建JSON数据包
        JObject jd = new JObject();
        jd["PGS"] = GamaManagerGlobal._instance.Setting.SceneName;//程序识别码
        jd["IDT"] = "CMT";//数据识别码
        jd["ID"] = GMSManagerGlobal._instance.ID.ToString();//ID
        jd["OM"] = ((int) GamaManagerGlobal._instance.operateMode).ToString();

        if (GamaManagerGlobal._instance.operateMode == OperateMode.桌面式)
        {
            jd["CPX"] = GamaManagerGlobal._instance.MainCameraGameObject_PC.transform.position.x.ToString();
            jd["CPY"] = GamaManagerGlobal._instance.MainCameraGameObject_PC.transform.position.y.ToString();
            jd["CPZ"] = GamaManagerGlobal._instance.MainCameraGameObject_PC.transform.position.z.ToString();
            jd["CRX"] = GamaManagerGlobal._instance.MainCameraGameObject_PC.transform.eulerAngles.x.ToString();
            jd["CRY"] = GamaManagerGlobal._instance.MainCameraGameObject_PC.transform.eulerAngles.y.ToString();
            jd["CRZ"] = GamaManagerGlobal._instance.MainCameraGameObject_PC.transform.eulerAngles.z.ToString();
            jd["EPX"] = "0.00";
            jd["EPY"] = "0.00";
            jd["EPZ"] = "0.00";
            jd["ERX"] = "0.00";
            jd["ERY"] = "0.00";
            jd["ERZ"] = "0.00";
            jd["RPX"] = "0.00";
            jd["RPY"] = "0.00";
            jd["RPZ"] = "0.00";
            jd["RRX"] = "0.00";
            jd["RRY"] = "0.00";
            jd["RRZ"] = "0.00";
        }
        else if (GamaManagerGlobal._instance.operateMode == OperateMode.头盔式)
        {
            jd["CPX"] = "0.00";
            jd["CPY"] = "0.00";
            jd["CPZ"] = "0.00";
            jd["CRX"] = "0.00";
            jd["CRY"] = "0.00";
            jd["CRZ"] = "0.00";
            jd["EPX"] = UIManagerGlobal._instance.VrCameraEyeTransform.position.x.ToString();
            jd["EPY"] = UIManagerGlobal._instance.VrCameraEyeTransform.position.y.ToString();
            jd["EPZ"] = UIManagerGlobal._instance.VrCameraEyeTransform.position.z.ToString();
            jd["ERX"] = UIManagerGlobal._instance.VrCameraEyeTransform.eulerAngles.x.ToString();
            jd["ERY"] = UIManagerGlobal._instance.VrCameraEyeTransform.eulerAngles.y.ToString();
            jd["ERZ"] = UIManagerGlobal._instance.VrCameraEyeTransform.eulerAngles.z.ToString();
            jd["RPX"] = UIManagerGlobal._instance.CameraRigTransform.position.x.ToString();
            jd["RPY"] = UIManagerGlobal._instance.CameraRigTransform.position.y.ToString();
            jd["RPZ"] = UIManagerGlobal._instance.CameraRigTransform.position.z.ToString();
            jd["RRX"] = UIManagerGlobal._instance.CameraRigTransform.eulerAngles.x.ToString();
            jd["RRY"] = UIManagerGlobal._instance.CameraRigTransform.eulerAngles.y.ToString();
            jd["RRZ"] = UIManagerGlobal._instance.CameraRigTransform.eulerAngles.z.ToString();
        }

        //Debug.Log("打包数据发送");
        return Regex.Unescape(jd.ToString());//序列化JSON对象
    }
}

public class CameraTransformData : BaseData
{
    public int ID;
    public OperateMode operateMode;

    public float MainCameraPositionX;
    public float MainCameraPositionY;
    public float MainCameraPositionZ;
    public float MainCameraEulerAnglesX;
    public float MainCameraEulerAnglesY;
    public float MainCameraEulerAnglesZ;

    public float EyePositionX;
    public float EyePositionY;
    public float EyePositionZ;
    public float EyeEulerAnglesX;
    public float EyeEulerAnglesY;
    public float EyeEulerAnglesZ;

    public float CameraRigPositionX;
    public float CameraRigPositionY;
    public float CameraRigPositionZ;
    public float CameraRigEulerAnglesX;
    public float CameraRigEulerAnglesY;
    public float CameraRigEulerAnglesZ;

    public CameraTransformData(int ID, OperateMode operateMode, float MainCameraPositionX, float MainCameraPositionY,
        float MainCameraPositionZ, float MainCameraEulerAnglesX, float MainCameraEulerAnglesY,
        float MainCameraEulerAnglesZ, float EyePositionX, float EyePositionY, float EyePositionZ,
        float EyeEulerAnglesX, float EyeEulerAnglesY, float EyeEulerAnglesZ, float CameraRigPositionX,
        float CameraRigPositionY, float CameraRigPositionZ, float CameraRigEulerAnglesX, float CameraRigEulerAnglesY,
        float CameraRigEulerAnglesZ)
    {
        this.ID = ID;
        this.operateMode = operateMode;
        this.MainCameraPositionX = MainCameraPositionX;
        this.MainCameraPositionY = MainCameraPositionY;
        this.MainCameraPositionZ = MainCameraPositionZ;
        this.MainCameraEulerAnglesX = MainCameraEulerAnglesX;
        this.MainCameraEulerAnglesY = MainCameraEulerAnglesY;
        this.MainCameraEulerAnglesZ = MainCameraEulerAnglesZ;
        this.EyePositionX = EyePositionX;
        this.EyePositionY = EyePositionY;
        this.EyePositionZ = EyePositionZ;
        this.EyeEulerAnglesX = EyeEulerAnglesX;
        this.EyeEulerAnglesY = EyeEulerAnglesY;
        this.EyeEulerAnglesZ = EyeEulerAnglesZ;
        this.CameraRigPositionX = CameraRigPositionX;
        this.CameraRigPositionY = CameraRigPositionY;
        this.CameraRigPositionZ = CameraRigPositionZ;
        this.CameraRigEulerAnglesX = CameraRigEulerAnglesX;
        this.CameraRigEulerAnglesY = CameraRigEulerAnglesY;
        this.CameraRigEulerAnglesZ = CameraRigEulerAnglesZ;
    }
}
