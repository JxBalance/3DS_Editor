using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;


public class CreatePipeGroupsT3 : EditorWindow {

    public static CreatePipeGroupsT3 _instance;

    //GUI皮肤
    public GUISkin mySkin;
    public GUIStyle smalltitleLabelStyle;

    private List<GameObject> pipeGroup = new List<GameObject>();
    private string pipeGroupName;
    private string pipesName;
    private GameObject pipeModel;
    private int selPipeInt = -1;

    private GameObject selModel = null;     //在场景中选择的物体
    private GameObject selPipeModel;        //通过界面选择的物体
    private Vector2 scroll;

    private bool isHidePlane = true;

    void Awake()
    {
        _instance = this;
        Window_T3._instance.GM.airplaneModel.SetActive(false);
    }

    private void OnGUI()
    {
        InitGUIStyle();

        GUILayout.Space(10);
        GUILayout.BeginHorizontal();
        {
            GUILayout.Label("添加管道组", smalltitleLabelStyle,GUILayout.Width(100));
            //GUILayout.Space(position.width * 0.4f);
            GUILayout.BeginVertical();
            {
                GUILayout.Space(15);
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Space(120);
                    if (isHidePlane)
                    {
                        if (GUILayout.Button("不隐藏飞机模型")){
                            Window_T3._instance.GM.airplaneModel.SetActive(true);
                            isHidePlane = false;
                        }
                    }
                    else
                    {
                        if (GUILayout.Button("隐藏飞机模型"))
                        {
                            Window_T3._instance.GM.airplaneModel.SetActive(false);
                            isHidePlane = true;
                        }
                    }
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();
        }
        GUILayout.EndHorizontal();

        GUILayout.Space(5);
        pipeGroupName = EditorGUILayout.TextField("管道组名称", pipeGroupName);
        GUILayout.Space(5);

        GUILayout.BeginHorizontal();
        {
            GameObject oldPipeModel = pipeModel;
            pipeModel = (GameObject)EditorGUILayout.ObjectField("包含管道", pipeModel, typeof(GameObject), true);
            if(oldPipeModel != pipeModel)
            {
                Selection.activeGameObject = pipeModel;
                SceneView.lastActiveSceneView.FrameSelected();
            }
            GUILayout.Space(5);
            if (GUILayout.Button("添加", GUILayout.Width(50)))
            {
                if(pipeModel)
                {
                    int i = 0;
                    for(i = 0; i < pipeGroup.Count; i++)
                    {
                        if(pipeGroup[i] == pipeModel)
                        {
                            break;
                        }
                    }
                    if(i == pipeGroup.Count)
                    {
                        pipeGroup.Add(pipeModel);
                    }
                }
                else
                {
                    EditorUtility.DisplayDialog("提示", "请选择管道模型", "确定");
                }
            }
        }
        GUILayout.EndHorizontal();
        GUILayout.Space(5);

        GUILayout.BeginHorizontal();
        {
            GUILayout.Space(100);
            scroll = EditorGUILayout.BeginScrollView(scroll, GUILayout.Height(150));
            GUILayout.BeginVertical();
            {
                int count = pipeGroup.Count;
                string[] allPipes = new string[count];
                int old = selPipeInt;
                int i = 0;
                for (i = 0; i < count; i++)
                {
                    allPipes[i] = pipeGroup[i].name;
                }
                selPipeInt = GUILayout.SelectionGrid(selPipeInt, allPipes, 2);
                if (old != selPipeInt)
                {
                    selPipeModel = pipeGroup[selPipeInt];
                    Selection.activeGameObject = selPipeModel;
                    SceneView.lastActiveSceneView.FrameSelected();
                    old = selPipeInt;
                }
            }
            GUILayout.EndVertical();
            EditorGUILayout.EndScrollView();
        }
        GUILayout.EndHorizontal();
        GUILayout.Space(10);

        GUILayout.BeginHorizontal();
        {
            GUILayout.Space(60);
            if (GUILayout.Button("添加管道组", GUILayout.Width(140)))
            {
                if (pipeGroup.Count > 0)
                {
                    if (pipeGroupName == null || pipeGroupName == "")
                    {
                        EditorUtility.DisplayDialog("提示", "管道组名称不能为空", "确定");
                    }
                    else
                    {
                        GameObject go = Instantiate(Window_T3._instance.pipeGroup);
                        go.name = pipeGroupName;
                        go.transform.parent = GameObject.Find("PipeGroupParentT3(Clone)").transform;
                        PipeGroupT3 newpipeGroup = go.GetComponent<PipeGroupT3>();
                        newpipeGroup.pipeGroupName = pipeGroupName;
                        newpipeGroup.pipeModels = pipeGroup;
                        Window_T3._instance.GM.pipegroups.Add(newpipeGroup);
                        Window_T3._instance.GM.airplaneModel.SetActive(true);
                        Close();
                    }
                }
                else
                {
                    EditorUtility.DisplayDialog("提示", "管道组必须包含管道模型", "确定");
                }
            }
            GUILayout.Space(5);
            if (GUILayout.Button("关闭", GUILayout.Width(50)))
            {
                Window_T3._instance.GM.airplaneModel.SetActive(true);
                Close();
            }
        }
    }

    private void InitGUIStyle()
    {
        smalltitleLabelStyle = new GUIStyle(mySkin.label);
        smalltitleLabelStyle.fontSize = 15;

    }
        // Use this for initialization
        void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        //场景中选中其他物体 或 没有选中的物体，则在界面中取消选中的物体按钮
        if (!Selection.activeGameObject || Selection.activeGameObject != selPipeModel)
        {
            selPipeInt = -1;
        }
        if(selModel != Selection.activeGameObject || !Selection.activeGameObject)
        {
            pipeModel = Selection.activeGameObject;
            Selection.activeGameObject = pipeModel;
            SceneView.lastActiveSceneView.FrameSelected();
            selModel = Selection.activeGameObject;
            Repaint();
        }
    }
}
