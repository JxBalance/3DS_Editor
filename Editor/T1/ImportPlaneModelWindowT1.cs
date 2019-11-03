using UnityEngine;
using System.Collections;
using UnityEditor;

public class ImportPlaneModelWindowT1 : EditorWindow
{
    public static ImportPlaneModelWindowT1 _instance;
    //GUI皮肤
    public GUISkin mySkin;

    private GameObject planeModel;
    private string planeName;
    private string[] btnsText = {"导入", "关闭"};

    void Awake()
    {
        _instance = this;
    }

    void OnGUI()
    {
        GUILayout.Space(20);
        planeModel = GUITools.ObjectField<GameObject>("飞机/场景模型", 0, 0, planeModel, typeof (GameObject), true);
        GUILayout.Space(5);
        planeName = GUITools.TextField("模型名称", 0, 0, planeName);
        GUILayout.Space(5);
        bool[] b = GUITools.HorizontalButtons("", position.width*0.5f, 0, btnsText, 0);

        if (b[0])
        {
            if (planeModel)
            {
                if (planeName == null || planeName == "")
                {
                    EditorUtility.DisplayDialog("提示", "模型名称不能为空", "确定");
                }
                else
                {
                    GameObject go = Instantiate(planeModel);
                    Selection.activeGameObject = go;
                    SceneView.lastActiveSceneView.FrameSelected();
                    go.name = planeName;
                    Window_T1._instance.GM.models.Add(go);
                    ModelManagerWindowT1.Instance.UpdateWindow();
                    Close();
                }
            }
            else
            {
                EditorUtility.DisplayDialog("提示", "模型不能为空", "确定");
            }
        }

        if (b[1])
        {
            Close();
        }
    }
}


