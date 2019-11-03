using UnityEngine;
using System.Collections;
using UnityEditor;

public class ModelManagerWindowT1 : EditorWindow
{
    private static ModelManagerWindowT1 instance;

    public static ModelManagerWindowT1 Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new ModelManagerWindowT1();
            }
            return instance;
        }
    }
    public GUISkin mySkin;
    private GUIStyle title3LabelStyle;
    private string[] names = new string[0];
    public int selGridInt = -1;               //列表选择索引 
    private Vector2 scrollPosition;


    private ModelManagerWindowT1()
    {

    }

    // Update is called once per frame
    public void OnGUI()
    {
        InitGUIStyle();
        GUILayout.Space(5);
        GUILayout.Label("模型列表", title3LabelStyle);
        if (Window_T1._instance.GM)
        {
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
                Selection.activeGameObject = Window_T1._instance.currentModel = Window_T1._instance.GM.models[selGridInt];
                SceneView.lastActiveSceneView.FrameSelected();
                Window_T1._instance.modelPosition = Window_T1._instance.GM.models[selGridInt].transform.position;
                Window_T1._instance.modelRotation = Window_T1._instance.GM.models[selGridInt].transform.eulerAngles;
                Window_T1._instance.modelScale = Window_T1._instance.GM.models[selGridInt].transform.localScale;
                Window_T1._instance.Repaint();
            }
            GUILayout.EndScrollView();
        }
        
        GUILayout.BeginHorizontal();
        {
            GUILayout.Space(75);
            if (GUILayout.Button("删除"))
            {
                if (EditorUtility.DisplayDialog("提示", "确定要删除 " + Window_T1._instance.GM.models[selGridInt].name + " 吗？", "确定", "取消"))
                {
                    DestroyImmediate(Window_T1._instance.GM.models[selGridInt]);
                    Window_T1._instance.GM.models.RemoveAt(selGridInt);
                    selGridInt = -1;
                    UpdateWindow();
                }
            }

            if (GUILayout.Button("清空"))
            {
                if (EditorUtility.DisplayDialog("提示", "确定要清空所有模型吗？", "确定", "取消"))
                {
                    foreach (GameObject g in Window_T1._instance.GM.models)
                    {
                        DestroyImmediate(g);
                    }
                    Window_T1._instance.GM.introBoard.Clear();
                    selGridInt = -1;
                    UpdateWindow();
                }
            }
            GUILayout.Space(75);
        }
        GUILayout.EndHorizontal();
        GUILayout.Space(10);
    }

    /// <summary>
    /// 初始化GUI格式
    /// </summary>
    private void InitGUIStyle()
    {
        title3LabelStyle = new GUIStyle(mySkin.label);
        title3LabelStyle.fontSize = 18;
    }

    public void UpdateWindow()
    {
        int count = Window_T1._instance.GM.models.Count;
        names = new string[count];
        for (int i = 0; i < count; i++)
        {
            names[i] = Window_T1._instance.GM.models[i].name;
        }
        if (Window_T1._instance.currentModel)
        {
            for (int i = 0; i < count; i++)
            {
                if (Window_T1._instance.currentModel == Window_T1._instance.GM.models[i])
                {
                    selGridInt = i;
                    break;
                }
            }
        }
        Repaint();
    }

}
