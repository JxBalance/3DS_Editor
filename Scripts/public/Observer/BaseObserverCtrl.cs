using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;

/// <summary>
/// 观察者基类 
/// 子类观察者需派生于该基类
/// 用于在unity主线程中处理数据 
/// 子类脚本挂载于GameObserver物体上
/// </summary>
public class BaseObserverCtrl : MonoBehaviour
{
    //观察者类
    protected GMSObserver mObserver;
    //数据缓冲队列
    protected Queue<BaseData> DataQueue = new Queue<BaseData>();
    //计时器
    protected float timer = 0f;
    //发送间隔
    public float time = 1f;
    //使用计时器
    public bool useTimer = true;

    protected virtual void Start()
    {
       
    }

    protected virtual void Update()
    {

    }

    protected virtual void LateUpdate()
    {
        ProcessingData(DataQueue);
        SendDataByTimer();
    }
    /// <summary>
    /// 注册观察者
    /// </summary>
    /// <param name="observer"></param>
    public void RegisterObserver(GMSObserver observer)
    {
        GMSManagerGlobal._instance.RegisterObserver(observer);
        mObserver = observer; 
    }
    /// <summary>
    /// 移除观察者
    /// </summary>
    public void RemoveObserver()
    {
        GMSManagerGlobal._instance.RemoveObserver(mObserver);
        mObserver = null;
    }

    /// <summary>
    /// 解析数据
    /// </summary>
    /// <param name="jd"></param>
    public virtual void AnalysisData(JObject jd)
    {

    }

    /// <summary>
    /// 处理数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    protected virtual void ProcessingData<T>(Queue<T> dataQueue)
    {
        
    }

    /// <summary>
    /// 计时器发送数据
    /// </summary>
    protected virtual void SendDataByTimer()
    {
        if (mObserver != null)
        {
            if (useTimer)
            {
                if (GMSManagerGlobal._instance.isConnect)
                {
                    if (timer <= 0f)
                    {
                        SendData();
                        timer = time;
                    }
                    timer -= Time.deltaTime;
                }
                else
                {
                    timer = 0f;
                }
            }
        }
    }
    /// <summary>
    /// 发送数据
    /// </summary>
    public virtual void SendData()
    {
        GMSManagerGlobal._instance.SendData(PackageData());
    }


    /// <summary>
    /// 打包数据
    /// </summary>
    protected virtual string PackageData()
    {
        return "";
    }
}

/// <summary>
/// 数据格式基类 便于列表管理
/// </summary>
public abstract class BaseData
{
    
}

/// <summary>
/// 观察者类 用于响应GMS发来的数据
/// </summary>
public class GMSObserver
{
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="obCtrl">构造时需要保留该观察者控制器 才能在主线程中处理</param>
    /// <param name="IDT">该观察者处理响应的识别码</param>
    public GMSObserver(BaseObserverCtrl obCtrl,string IDT)
    {
        this.obCtrl = obCtrl;
        this.IDT = IDT;
    }
    public BaseObserverCtrl obCtrl;//观察者控制器
    public string IDT;//识别码
    
    /// <summary>
    /// 更新数据 让管理类处理
    /// </summary>
    public virtual void Update(JObject jd)
    {
        //识别码判断
        if (jd["PGS"].ToString() == GamaManagerGlobal._instance.Setting.SceneName)
        {
            if (jd["IDT"].ToString() == IDT)
            {
                //识别正确  处理数据
                obCtrl.AnalysisData(jd);
            }
        }
    }
}
