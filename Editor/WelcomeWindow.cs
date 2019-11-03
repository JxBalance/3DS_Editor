using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class WelcomeWindow : EditorWindow
{
    public static WelcomeWindow _instance;
    public GUISkin mySkin;
    public Font font;
    private string[] toolbarTexts = { "位置识别", "机构运动", "气/液流动"};
    private GUIStyle titleLabelStyle, subtitleLabelStyle, toolBarStyle, buttonStyle;
    private Vector2 scrollPosition;
    private int selGridInt = -1;
    private static string currentScenePath = "";
    private OperateType opType = OperateType.桌面式;
    private SubjectType sjType = SubjectType.位置识别;


    public void Awake()
    {
        _instance = this;
    }

    void OnGUI()
    {
        WelcomePage();
    }

    /// <summary>
    /// 开始界面
    /// </summary>
    private void WelcomePage()
    {
        InitGUIStyle();
        GUILayout.Space(position.height * 0.2f);
        GUILayout.Label("欢迎使用中国商飞3D虚拟培训课件开发平台", titleLabelStyle);
        GUILayout.Space(20);
        GUILayout.Label("请选择新建课件设计类型", subtitleLabelStyle);
        GUILayout.Space(100);

        //操作方式设定未来可能被移除
        opType =
            (OperateType)
                GUITools.EnumPopup(new GUIContent("操作方式"), opType, position.width*0.33f, position.width*0.33f,
                    GUILayout.Height(20));
        GUILayout.Space(10);

        sjType =
            (SubjectType)
                GUITools.EnumPopup(new GUIContent("课件类型"), sjType, position.width * 0.33f, position.width * 0.33f,
                    GUILayout.Height(20));
        GUILayout.Space(100);

        if (GUITools.Button("新建课件", position.width*0.4f, position.width*0.4f, GUILayout.Height(20)))
        {
            GameObject go = GameObject.FindGameObjectWithTag("GameManager");
                if (go)
                {
                    if (EditorUtility.DisplayDialog("提示", "存在已编辑的场景，确定要新建课件吗？", "确定", "取消"))
                    {
                        //新建
                        EditorApplication.delayCall += NewScene;
                    }
                }
                else
                {
                    if (EditorUtility.DisplayDialog("提示", "确定要新建课件吗？", "确定", "取消"))
                    {
                        //新建
                        EditorApplication.delayCall += NewScene;
                    }
                }
        }

        GUILayout.Space(20);

        if (GUITools.Button("打开已编辑场景", position.width * 0.4f, position.width * 0.4f, GUILayout.Height(20)))
        {
            string openPath = EditorUtility.OpenFilePanel("打开场景", "", "unity");
            if (openPath != "")
            {
                EditorSceneManager.OpenScene(openPath);
                //载入信息，跳转界面
                CheckScenePath();
            }
        }
    }

    /// <summary>
    /// 初始化GUI格式
    /// </summary>
    private void InitGUIStyle()
    {
        //GUI.skin.font = font;
        titleLabelStyle = new GUIStyle(mySkin.label);
        
        titleLabelStyle.fontSize = 30;
        subtitleLabelStyle = new GUIStyle(mySkin.label);
        subtitleLabelStyle.fontSize = 20;
        buttonStyle = new GUIStyle(mySkin.button);
    }

    /// <summary>
    /// 判断当前打开的场景和窗口是否对应
    /// </summary>
    private void CheckScenePath()
    {
        if (currentScenePath != EditorApplication.currentScene)
        {
            switch (GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().type)
            {
                case 1:
                    Window_T1._instance.Focus();
                    currentScenePath = Window_T1._instance.currentScenePath = EditorApplication.currentScene;
                    //Window_T1._instance.Init();
                    Debug.Log("已切换至编辑页面1");
                    break;
                case 2:
                    Window_T2._instance.Focus();
                    currentScenePath = Window_T2._instance.currentScenePath = EditorApplication.currentScene;
                    //Window_T2._instance.Init();
                    Debug.Log("已切换至编辑页面2");
                    break;
                case 3:
                    Window_T3._instance.Focus();
                    currentScenePath = Window_T3._instance.currentScenePath = EditorApplication.currentScene;
                    //Window_T3._instance.Init();
                    Debug.Log("已切换至编辑页面3");
                    break;
            }
        }
    }

    /// <summary>
    /// 创建新场景
    /// </summary>
    private void NewScene()
    {
        EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects);
        //创建新场景
        switch ((int)sjType)
        {
            case 0://部件识别
                Window_T1._instance.CreateNewScene(sjType,opType);//创建预制体
                Window_T1._instance.Init();//数据初始化
                currentScenePath = Window_T1._instance.currentScenePath = EditorApplication.currentScene;
                Window_T1._instance.Focus();//窗口焦点
                Debug.Log("已切换至编辑页面1");
                //EditorSceneManager.SaveOpenScenes();
                EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
                break;
            case 1://拆装
                Window_T2._instance.CreateNewScene(sjType, opType);
                Window_T2._instance.Init();
                currentScenePath = Window_T2._instance.currentScenePath = EditorApplication.currentScene;
                Window_T2._instance.Focus();
                Debug.Log("已切换至编辑页面2");
                EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
                break;
            case 2://原理图
                Window_T3._instance.CreateNewScene(sjType, opType);
                Window_T3._instance.Init();
                currentScenePath = Window_T3._instance.currentScenePath = EditorApplication.currentScene;
                Window_T3._instance.Focus();
                Debug.Log("已切换至编辑页面3");
                EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
                break;
        }
    }

}
