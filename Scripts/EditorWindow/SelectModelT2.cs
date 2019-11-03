#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using Global;
using UnityEditor;
using UnityEngine;

public class SelectModelT2 : EditorWindow {

    private static SelectModelT2 instance;
    public GUISkin mySkin;
    private GUIStyle titleLabelStyle;


    private string[] names = new string[0];
    public int selGridInt = -1;               //列表选择索引 
    private Vector2 scrollPosition;

    //private List<GameObject> gameObjectList = new List<GameObject>();
    private List<UnitGroupT1> groupList = new List<UnitGroupT1>();

    public static SelectModelT2 Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new SelectModelT2();
            }
            return instance;
        }
    }

    public SelectModelT2()
    {
        instance = this;
    }

    private Drive currentDrive;

    public Drive CurrentDrive
    {
        set { currentDrive = value; }
    }

    private void Awake()
    {
        GetManager();
        InitNames();
    }

    private void OnGUI()
    {
        InitGUIStyle();
        GUILayout.Space(5);
        GUILayout.Label("选择作动机构", titleLabelStyle);

        scrollPosition = GUILayout.BeginScrollView(scrollPosition);
        int old = selGridInt;
        GUILayout.BeginHorizontal();
        {
            GUILayout.Space(20);
            selGridInt = GUILayout.SelectionGrid(selGridInt, names, 1);
            GUILayout.Space(20);
        }
        GUILayout.EndHorizontal();
        if (selGridInt != old)
        {
            //按钮响应
            //当前选中物体设置为 机构作动物体
            currentDrive.mechanismMode = groupList[selGridInt].unitMembers[0].gameObject;
            
            if (currentDrive.mechanismMode)
            {
                //如果是导入的模型，并且有可能位置发生了变化，重置位置和旋转
                if (currentDrive.mechanismMode.GetComponentInChildren<ModelInfoT1>())
                {
                    currentDrive.mechanismMode.GetComponentInChildren<ModelInfoT1>().ResetTransform();
                }
                //名称搜索XML  注意：mechanismMode.name 是场景中模型的名称 并未做实际修改
                string name = currentDrive.mechanismMode.name;
                if (name.Contains("Clone"))
                {
                    currentDrive.mechanismMode.name = name.Replace("(Clone)", "");
                }
                currentDrive.motionTrailFilePath = Application.streamingAssetsPath + "/" + currentDrive.mechanismMode.name + ".xml";
                MultiControlerBase ctrl = currentDrive.mechanismMode.AddComponent<MultiControlerBase>();
                groupList[selGridInt].multiControlerBase = ctrl;
                ctrl.drive = currentDrive;
                currentDrive.multiControlerBase = ctrl;
                ctrl.setMotionTrailFilePath(currentDrive.motionTrailFilePath);
            }
            Close();
        }
        GUILayout.EndScrollView();

    }

    /// <summary>
    /// 初始化GUI格式
    /// </summary>
    private void InitGUIStyle()
    {
        titleLabelStyle = new GUIStyle(mySkin.label);
        titleLabelStyle.fontSize = 18;
    }

    /// <summary>
    /// 初始化名称数组
    /// </summary>
    private void InitNames()
    {
        if (gmGlobal)
        {
            List<string> nameList = new List<string>();
            groupList.Clear();
            foreach (var unitGroup in gmGlobal.UnitGroupList)
            {
                if (unitGroup.unitMembers.Count == 1)
                {
                    //if (unitGroup.unitMembers[0].tag == "124344123423")
                    //{
                        
                    //}
                    groupList.Add(unitGroup.unitGroup);
                    nameList.Add(unitGroup.unitMembers[0].gameObject.name);
                }
            }
            names = nameList.ToArray();
        }
    }


    private GamaManagerGlobal gmGlobal;
    private void GetManager()
    {
        if (!gmGlobal)
        {
            GameObject go = GameObject.FindGameObjectWithTag("GameManager");
            if (go)
            {
                gmGlobal = go.GetComponent<GamaManagerGlobal>();
            }
        }
    }

}
#endif