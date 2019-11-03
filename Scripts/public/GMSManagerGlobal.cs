using System;
using System.Collections.Generic;
using System.Text;
using Global;
using GMSClientLib.GMSClientLib;
using Newtonsoft.Json.Linq;
using TcpHelper;
using UnityEngine;
using TcpClient = TcpHelper.TcpClient;

public class GMSManagerGlobal : MonoBehaviour {

    public static GMSManagerGlobal _instance; //单例模式

    private TcpClient Client; //客户端对象

    public bool isConnect = false; //当前是否处于连接状态
    //int Port = 10010;//目标服务器端口
    //string IP = "127.0.0.1";//目标服务器IP

    private JObject mJObject;//数据接受临时对象
    public JObject jObject
    {
        set
        {
            mJObject = value; //收到数据
            NotifyObserver(value); //通知观察者
        }
        get { return mJObject; }
    }
    private List<GMSObserver> mObservers = new List<GMSObserver>(); //观察者列表

    public List<ClientInfo> clientInfoList = new List<ClientInfo>();//客户端信息表
    private List<float> timerList = new List<float>();//计时器列表

    /// <summary>
    /// 各个观察者控制器 强制private 观察者不允许外部定义！！ 
    /// </summary>
    [SerializeField]
    private ConnectObserverCtrl connectObserverCtrl;
    [SerializeField]
    private StartObserverCtrl startObserverCtrl;
    [SerializeField]
    private UnitRecognitionObserverCtrl unitRecognitionObserverCtrl;
    [SerializeField]
    private SettingsObserverCtrl settingsObserverCtrl;
    [SerializeField]
    private CameraTransformObserverCtrl cameraTransformObserverCtrl;
    [SerializeField]
    private IntroBoardObserverCtrl introBoardObserverCtrl;

    public int ID = 0; //当前ID

    /// <summary>
    /// 当前客户端类型
    /// </summary>
    public ClientMode currentClientMode
    {
        get { return clientInfoList[ID].clientMode; }
    }

    private int globalPositionFollowID = -1; //全局位置跟随ID
    private int globalOperateFollowID = -1;//全局操作跟随ID

    private int personalPositionFollowID = -1;//个人位置跟随ID
    private int personalOperateFollowID = -1;//个人操作跟随ID
    private bool studentsFollowSettings = true;//是否允许学员端跟随设置

    /// <summary>
    /// 全局位置跟随IDglo
    /// </summary>
    public int GlobalPositionFollowID
    {
        get { return globalPositionFollowID; }
        set
        {
            globalPositionFollowID = value;
            if (value == -1)
            {
                //自由移动 显示个人位置跟随设置
                UIManagerGlobal._instance.SetActivePersonalPositionFollowDropdown(true);
            }
            else
            {
                //全局跟随  全局跟随状态 无论任何角色 都不允许个人位置跟随设置
                UIManagerGlobal._instance.SetActivePersonalPositionFollowDropdown(false);
            }
        }
    }
    /// <summary>
    /// 全局操作跟随ID
    /// </summary>
    public int GlobalOperateFollowID
    {
        get { return globalOperateFollowID; }
        set
        {
            globalOperateFollowID = value;
            if (value == -1)
            {
                UIManagerGlobal._instance.SetActivePersonalOperationFollowDropdown(true);
            }
            else
            {
                UIManagerGlobal._instance.SetActivePersonalOperationFollowDropdown(false);
            }
        }
    }
    /// <summary>
    /// 个人位置跟随ID
    /// </summary>
    public int PersonalPositionFollowID
    {
        get { return personalPositionFollowID; }
        set
        {
            if (GlobalPositionFollowID == -1)
            {
                personalPositionFollowID = value;
            }
        }
    }
    /// <summary>
    /// 个人操作跟随ID
    /// </summary>
    public int PersonalOperateFollowID
    {
        get { return personalOperateFollowID; }
        set
        {
            if (GlobalOperateFollowID == -1)
            {
                personalOperateFollowID = value;
            }
        }
    }

    private float jigou_x;

    public float JIGOU_X
    {
        get { return jigou_x; }
        set { jigou_x = value; }
    }

    /// <summary>
    /// 是否允许学员端跟随设置
    /// </summary>
    public bool StudentsFollowSettings
    {
        get { return studentsFollowSettings; }
        set
        {
            studentsFollowSettings = value;
            if (currentClientMode != ClientMode.教员端)
            {
                UIManagerGlobal._instance.SetActivePersonalSettingsPanel(value);
            }
        }
    }
    /// <summary>
    /// 获取当前客户端位置跟随ID
    /// </summary>
    public int CurrentPositionFollowID
    {
        get
        {
            if (GlobalPositionFollowID == -1)
            {
                return PersonalPositionFollowID;
            }
            else
            {
                return GlobalPositionFollowID;
            }
        }
    }
    /// <summary>
    /// 获取当前客户端操作跟随ID
    /// </summary>
    public int CurrentOperateFollowID
    {
        get
        {
            if (GlobalOperateFollowID == -1)
            {
                return PersonalOperateFollowID;
            }
            else
            {
                return GlobalOperateFollowID;
            }
        }
    }

    /// <summary>
    /// 初始化观察者 并管理
    /// </summary>
    public void InitObserverCtrl()
    {
        GameObject GameObserverGameObject = GamaManagerGlobal._instance.GameObserverGameObject;
        connectObserverCtrl = GameObserverGameObject.AddComponent<ConnectObserverCtrl>();
        startObserverCtrl = GameObserverGameObject.AddComponent<StartObserverCtrl>();
        unitRecognitionObserverCtrl = GameObserverGameObject.AddComponent<UnitRecognitionObserverCtrl>();
        settingsObserverCtrl = GameObserverGameObject.AddComponent<SettingsObserverCtrl>();
        cameraTransformObserverCtrl = GameObserverGameObject.AddComponent<CameraTransformObserverCtrl>();
        introBoardObserverCtrl = GameObserverGameObject.AddComponent<IntroBoardObserverCtrl>();
    }
    /// <summary>
    /// 初始化客户端列表并管理
    /// </summary>
    public void InitClientInfoList()
    {
        clientInfoList.Clear();
        for (int i = 0; i < GamaManagerGlobal._instance.Setting.ClientArray.Length; i++)
        {
            clientInfoList.Add(new ClientInfo(GamaManagerGlobal._instance.Setting.ClientArray[i],i));
            timerList.Add(2.5f);
        }
    }

    void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        InitObserverCtrl();
        InitClientInfoList();
    }

    void Update()
    {
        ClientDisconnectionCheckByTimer();
    }

    /// <summary>
    /// 连接服务器
    /// </summary>
    /// <param name="IP">IP</param>
    /// <param name="Port">端口</param>
    public void ConnectServer(string IP,int Port)
    {
        Client = new TcpClient();//新建客户端对象
        Client.BinaryInput = new ClientBinaryInputHandler(ClientMessageHandler);
        Client.MessageInput = new ClientMessageInputHandler(ClientExceptionHandler);
        //Client.Message = new MessageHandler(ClientMessageHandler); //设置数据包处理回调方法
        //Client.Exception = new ExceptionHandler(ClientExceptionHandler); //客户端异常断开处理回调方法


        if (Client.Connect(IP, Port)) //连接到服务器
        {
            //Console.WriteLine("连接服务器成功");
            Debug.Log("连接服务器成功");
            isConnect = true;
            Client.StartRead(); //开始监听读取
            connectObserverCtrl.RegisterObserver(new GMSObserver(connectObserverCtrl, "CON"));
            startObserverCtrl.RegisterObserver(new GMSObserver(startObserverCtrl, "STT"));
        }
        else
        {
            Debug.Log("无法连接服务器");
        }
    }
    /// <summary>
    /// 断开连接
    /// </summary>
    public void DisconnectServer()
    {
        if (Client != null)
        {
            //关闭可能存在的观察者
            connectObserverCtrl.RemoveObserver();
            startObserverCtrl.RemoveObserver();
            unitRecognitionObserverCtrl.RemoveObserver();
            settingsObserverCtrl.RemoveObserver();
            cameraTransformObserverCtrl.RemoveObserver();
            introBoardObserverCtrl.RemoveObserver();
            isConnect = false;
            Client.Close();
            Client = null;
        }
    }

    /// <summary>
    /// 客户端断开
    /// </summary>
    /// <param name="message"></param>
    void ClientExceptionHandler(string message)
    {
        Debug.Log("与服务器端断开连接");
        DisconnectServer();
    }
    
    /// <summary>
    /// U3D程序关闭断开连接
    /// </summary>
    void OnApplicationQuit()
    {
        DisconnectServer();
    }
    
    /// <summary>
    /// 收到数据
    /// </summary>
    /// <param name="data"></param>
    void ClientMessageHandler(byte[] data)
    {
        var csData = Encoding.UTF8.GetString(data, 0, data.Length);//数据包转化为字符串
        JObject jd = JObject.Parse(csData);//反序列化JSON

        //处理收到的数据
        ReceiveData(jd);
    }
    
    /// <summary>
    /// 处理收到的数据
    /// </summary>
    /// <param name="jd"></param>
    void ReceiveData(JObject jd)
    {
        //Debug.Log("收到消息:" + jd.ToString());
        jObject = jd;
    }

    /// <summary>
    /// 处理发送数据
    /// </summary>
    /// <param name="msg"></param>
    public void SendData(String msg)
    {
        try
        {
            Client.SendData(msg);
        }
        catch (Exception e)
        {
            throw;
        }
    }

    /// <summary>
    /// 注册观察者
    /// </summary>
    /// <param name="ob"></param>
    public void RegisterObserver(GMSObserver ob)
    {
        mObservers.Add(ob);
    }

    /// <summary>
    /// 移除观察者
    /// </summary>
    /// <param name="ob"></param>
    public void RemoveObserver(GMSObserver ob)
    {
        if (mObservers.Contains(ob))
        {
            mObservers.Remove(ob);
        }
    }

    /// <summary>
    /// 通知观察者
    /// </summary>
    /// <param name="jd"></param>
    private void NotifyObserver(JObject jd)
    {
        foreach (GMSObserver ob in mObservers)
        {
            ob.Update(jd);
        }
    }

    /// <summary>
    /// StartObserver 发送数据
    /// </summary>
    public void SendStartObserverData()
    {
        startObserverCtrl.SendData();
    }

    /// <summary>
    /// 客户端连接检测
    /// </summary>
    /// <param name="id"></param>
    public void ClientConnectionCheck(int id)
    {
        clientInfoList[id].isConnect = true;
        UIManagerGlobal._instance.ClientConnectionCheck(id);
        timerList[id] = 2.5f;
    }

    /// <summary>
    /// 客户端离线检测
    /// </summary>
    /// <param name="id"></param>
    public void ClientDisconnectionCheck(int id)
    {
        clientInfoList[id].isConnect = false;
        UIManagerGlobal._instance.ClientDisconnectionCheck(id);
        timerList[id] = 0f;
    }

    /// <summary>
    /// 客户端离线检测---计时器
    /// </summary>
    private void ClientDisconnectionCheckByTimer()
    {
        if (!isConnect) return;
        for (int i = 0; i < timerList.Count; i++)
        {
            timerList[i] -= Time.deltaTime;
            if (timerList[i] <= 0f)
            {
                ClientDisconnectionCheck(i);
            }
        }
    }

    /// <summary>
    /// UnitRecognitionObserver 发送数据
    /// </summary>
    public void SendUnitRecognitionObserver()
    {
        unitRecognitionObserverCtrl.SendData();
    }
    /// <summary>
    /// 注册部件识别观察者
    /// </summary>
    public void RegisterUnitRecognitionObserver()
    {
        unitRecognitionObserverCtrl.RegisterObserver(new GMSObserver(unitRecognitionObserverCtrl, "REC"));
    }

    /// <summary>
    /// 获取当前处于连接状态的客户端信息列表
    /// </summary>
    /// <returns></returns>
    public List<ClientInfo> GetConnectClientInfoList()
    {
        List<ClientInfo> connectClientInfoList = new List<ClientInfo>();
        foreach (var c in clientInfoList)
        {
            if (c.isConnect)
            {
                connectClientInfoList.Add(c);
            }
        }
        return connectClientInfoList;
    }

    /// <summary>
    /// 注册设置观察者
    /// </summary>
    public void RegisterSettingsObserver()
    {
        settingsObserverCtrl.RegisterObserver(new GMSObserver(settingsObserverCtrl, "SET"));
    }

    /// <summary>
    /// SettingsObserver 发送数据
    /// </summary>
    public void SendSettingsObserverData()
    {
        settingsObserverCtrl.SendData();
        //权限发生变化需要各种操作的同步

    }

    /// <summary>
    /// 注册相机位置观察者
    /// </summary>
    public void RegisterCameraTransformObserver()
    {
        cameraTransformObserverCtrl.RegisterObserver(new GMSObserver(cameraTransformObserverCtrl, "CMT"));
    }

    public void RegisterIntroBoardCtrl()
    {
        introBoardObserverCtrl.RegisterObserver(new GMSObserver(introBoardObserverCtrl, "INT"));
    }

    public void SendIntroBoardObserverData()
    {
        introBoardObserverCtrl.SendData();
    }

}

/// <summary>
/// 客户端信息类 --- GMSManagerGlobal管理
/// </summary>
public class ClientInfo
{
    public ClientMode clientMode;
    public int ID;
    public ClientInfo(ClientMode clientMode,int ID)
    {
        this.clientMode = clientMode;
        this.ID = ID;
    }
    public bool isConnect = false;
}
