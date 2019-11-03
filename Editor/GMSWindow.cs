using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 多人协同GMS窗口
/// </summary>
public class GMSWindow : EditorWindow
{
    public static GMSWindow _instance;//单例模式

    //GUI皮肤
    public GUISkin mySkin;
    private GUIStyle titleLabelStyle, subtitleLabelStyle, toolBarStyle, partTitleStyle, boldLabelStyle;
    public Font myFont;
    private Vector2 scrollPos = Vector2.zero;//窗口滚动条

    //界面控制变量
    private bool isAddGMS = false;
    private int crewSize = 2;
    private List<Crew> crew = new List<Crew>();

    //场景内部控制变量
    public GameManager gm;

    void Awake()
    {
        _instance = this;
        GetGameManager();
        GetGMSSceneData();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnGUI()
    {
        #region 单例 格式 标题
        if (!_instance)
        {
            Awake();
        }
        if (!gm)
        {
            GetGameManager();
        }
        InitGUIStyle();
        scrollPos = GUI.BeginScrollView(new Rect(0, 0, position.width, position.height), scrollPos,
            new Rect(0, 0, 320, 230));

        GUILayout.Label("多人协同配置", subtitleLabelStyle);
        #endregion

        #region 界面
        GUILayout.BeginArea(new Rect(30,30,position.width-60,170));
        {
            GUILayout.Space(20);
            bool old = isAddGMS;
            isAddGMS = EditorGUILayout.Toggle(new GUIContent("使用多人协同"), isAddGMS);
            GUILayout.Space(20);
            //GMS配置界面
            if (isAddGMS)
            {
                //如果是从false刚过渡过来，需要读取已存脚本信息
                if (!old)
                {
                    //基本初始化
                    InitCrewComplete();
                }
                //选择人数
                crewSize =  EditorGUILayout.IntSlider(new GUIContent("人数"), crewSize, 2, 4);
                GUILayout.Space(10);
                //Debug.Log(crew.Count());
                //角色分配
                for (int i = 0; i < crewSize; i++)
                {
                    crew[i] = (Crew)EditorGUILayout.EnumPopup(new GUIContent("人员" + (i + 1)), crew[i]);
                }
            }

            //if (GUI.Button(new Rect(position.width/2-80, 200, 100, 18), "保存"))
            //{
            //    SaveGMSSceneData();
            //}
        }
        GUILayout.EndArea();
        GUI.EndScrollView();
        #endregion

        //界面发生改变即实时同步
        if (GUI.changed)
        {
            SaveGMSSceneData();
        }
    }

    /// <summary>
    /// 初始化GUI格式
    /// </summary>
    private void InitGUIStyle()
    {
        titleLabelStyle = new GUIStyle(mySkin.label);
        titleLabelStyle.fontSize = 30;

        subtitleLabelStyle = new GUIStyle(mySkin.label);
        subtitleLabelStyle.fontSize = 20;

        partTitleStyle = new GUIStyle(EditorStyles.foldout);
        partTitleStyle.font = myFont;
        partTitleStyle.alignment = TextAnchor.MiddleLeft;
        partTitleStyle.fontStyle = FontStyle.Bold;
        partTitleStyle.fontSize = 15;

        boldLabelStyle = new GUIStyle(mySkin.label);
        boldLabelStyle.alignment = TextAnchor.MiddleLeft;
        boldLabelStyle.font = myFont;
        boldLabelStyle.fontStyle = FontStyle.Bold;
    }
    
    /// <summary>
    /// 基本初始化
    /// </summary>
    private void InitCrewComplete()
    {
        //Debug.Log("基本初始化");
        isAddGMS = true;
        crewSize = 2;
        crew.Clear();
        crew.Add(Crew.操作端);
        crew.Add(Crew.监控端);
        crew.Add(Crew.监控端);
        crew.Add(Crew.监控端);
    }

    /// <summary>
    /// 获取GM
    /// </summary>
    private void GetGameManager()
    {
        GameObject _gm = GameObject.FindGameObjectWithTag("GameManager");
        if (_gm)
        {
            gm = _gm.GetComponent<GameManager>();
        }
    }

    /// <summary>
    /// 获取场景数据
    /// </summary>
    private void GetGMSSceneData()
    {
        //Debug.Log("获取场景数据");
        isAddGMS = gm.isAddGMS;
        crew.Clear();
        crewSize = gm.crew.Count;
        for (int i = 0; i < gm.crew.Count; i++)
        {
            crew.Add(gm.crew[i]);
        }
        //补齐
        while (crew.Count<4)
        {
            crew.Add(Crew.监控端);
        }
    }

    /// <summary>
    /// 保存场景数据
    /// </summary>
    private void SaveGMSSceneData()
    {
        gm.isAddGMS = isAddGMS;
        gm.crew.Clear();
        if (gm.isAddGMS)
        {
            for (int i = 0; i < crewSize; i++)
            {
                gm.crew.Add(crew[i]);
            }
        }
    }

}
