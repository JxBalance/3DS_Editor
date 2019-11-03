using UnityEngine;
using System.Collections;
using UnityEditor;

public class ImportModelWindowT3 : EditorWindow
{
    public static ImportModelWindowT3 _instance;

    //GUI皮肤
    public GUISkin mySkin;

    private GameObject planeModel;
    private string planeName;
    //private string[] btnsText = { "导入", "关闭" };

    //public GameObject importModel;

    void Awake()
    {
        _instance = this;
    }

    void OnGUI()
    {
        GUILayout.Space(20);
        planeModel = (GameObject)EditorGUILayout.ObjectField("选择模型",planeModel,typeof(GameObject),true);
        GUILayout.Space(5);
        planeName = EditorGUILayout.TextField("模型名称", planeName);
        GUILayout.Space(5);
        GUILayout.BeginHorizontal();
        {
            GUILayout.Space(100);
            if (GUILayout.Button("导入", GUILayout.Width(50)))
            {
                if(planeModel)
                {
                    if (planeName == null || planeName == "")
                    {
                        EditorUtility.DisplayDialog("提示", "模型名称不能为空", "确定");
                    }
                    else
                    {
                        if (_instance.titleContent.text == "导入管道模型" && !Window_T3._instance.GM.airplaneModel)
                        {
                            EditorUtility.DisplayDialog("提示", "请先导入飞机模型", "确定");
                            return;
                        }
                        GameObject go = Instantiate(planeModel);
                        Selection.activeGameObject = go;
                        SceneView.lastActiveSceneView.FrameSelected();
                        go.name = planeName;
                        if (_instance.titleContent.text == "导入飞机模型")
                        {
                            Window_T3._instance.GM.airplaneModel = go;
                            Window_T3._instance.TransparentSetting(0);
                            Window_T3._instance.isPreviewEnable = true;
                        }
                        //Window_T1._instance.GM.models.Add(go);
                        //ModelManagerWindowT1.Instance.UpdateWindow();
                        Close();
                    }
                }
                else
                {
                    EditorUtility.DisplayDialog("提示", "模型不能为空", "确定");
                }
            }
            GUILayout.Space(5);
            if (GUILayout.Button("关闭", GUILayout.Width(50)))
            {
                Close();
            }
        }
        GUILayout.EndHorizontal();
    }
}


