using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Collections;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VRTK;




public class Window_T2 : EditorWindow
{
    public static Window_T2 _instance;
    public string currentScenePath = "";

    //字体 皮肤
    public GUISkin meSkin;
    public GUIStyle titleLabelStyle0, titleLabelStyle1;
    public Font meFont;

    //场景管理器
    public GameObject GameManager;
    public GameManagerT2 gm;

    private UIControllerT2 uiCtrl;
    public  GameObject DeskCanvasGameObject;
    public GameObject StartUICanvasGameObject ;
    //桌面式场景预制体
    public GameObject DeskCanvasGamePrefab;
    public GameObject eventSystemGamePrefab;
    public GameObject StartUICanvasPrefab;
    //头盔式场景预制体
    public GameObject VRCameraRigGameObject;
    public GameObject VRTKGameObject;
    public GameObject VRCanvas_LeftHandMenu;

    public GameObject VRTK_SDKManagerPrefab;
    public GameObject VRTK_ScriptsPrefab;

    //折叠标签
    private bool isShowModleImport = true;
    private bool isShowMechanismControl = true;
    private bool isShowDisassembly = true;

    //窗口
    private ImportCabinWindow importCabinWindow;
    private AddButtonPanel addButtonPanel;
    private ImportMechanismModelWindow importMechanismModelWindow;
    private ImpotEnvironmentalModel impotEnvironmentalModel;
    private AddDriveWindow addDriveWindow;
    private AddAotuDriveWindow addAutoDriveWindow;
    private AddDisassemblyWindow addDisassemblyWindow;

    //example
    public GameObject unitButton;
    public List<string> testUnits = new List<string>();

    //位置控制变量
    Vector2 scrollPosition = new Vector2(200,200) ;

    public void Awake()
    {
        _instance = this;
        GameObject CameraRigGameObject;
        CameraRigGameObject = GameObject.FindGameObjectWithTag("CameraRig");
    }

    private void InitGUIStyle()
    {
        titleLabelStyle0 = new GUIStyle(meSkin.label);
        titleLabelStyle0.fontSize = 20;
        titleLabelStyle0.alignment = TextAnchor.MiddleCenter;//居中

        titleLabelStyle1 = new GUIStyle(EditorStyles.foldout);
        titleLabelStyle1.font = meFont;
        titleLabelStyle1.alignment = TextAnchor.MiddleLeft;
        titleLabelStyle1.fontStyle = FontStyle.Bold;
        titleLabelStyle1.fontSize = 15;
    }


    void OnGUI()
    {
        if (_instance)
        {
            _instance = this;
            //gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManagerT2>();
        }

        CheckScenePath();
        InitGUIStyle();

        GUILayout.Space(20);
        GUILayout.Label("机构动作展示课件编辑器",titleLabelStyle0);

         GUILayout.Space(20);
        isShowMechanismControl = EditorGUILayout.Foldout(isShowMechanismControl, "机舱", titleLabelStyle1); // 定义折叠菜单
        if (isShowMechanismControl)
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.Space(position.width * 0.2f);
                if (GUILayout.Button("导入驾驶舱界面", GUILayout.Height(20), GUILayout.Width(position.width * 0.2f)))
                {

                    importCabinWindow = EditorWindow.GetWindow<ImportCabinWindow>(true, "导入机舱界面");
                    importCabinWindow.minSize = new Vector2(350, 100);
                    importCabinWindow.maxSize = new Vector2(350, 100);
                    importCabinWindow.position = new Rect(600, 400, 350, 100);
                    importCabinWindow.Show();
           
                }
                GUILayout.Space(position.width * 0.2f);
                if (GUILayout.Button("添加控制按钮", GUILayout.Height(20), GUILayout.Width(position.width * 0.2f)))
                {

                    addButtonPanel = EditorWindow.GetWindow<AddButtonPanel>(true, "添加控制按钮");
                    addButtonPanel.minSize = new Vector2(400, 400);
                    addButtonPanel.maxSize = new Vector2(400, 400);
                    addButtonPanel.position = new Rect(600, 400, 400, 400);
                    addButtonPanel.Show();

                }
            }
            GUILayout.EndHorizontal();
        }
        GUILayout.Space(20);
        isShowModleImport = EditorGUILayout.Foldout(isShowModleImport, "模型导入", titleLabelStyle1); // 定义折叠菜单
        if(isShowModleImport)
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.Space(position.width * 0.2f);

                if (GUILayout.Button("添加环境模型", GUILayout.Width(position.width * 0.2f)))
                {
                    impotEnvironmentalModel = EditorWindow.GetWindow<ImpotEnvironmentalModel>(true, "导入机构模型");
                    impotEnvironmentalModel.minSize = new Vector2(350, 100);
                    impotEnvironmentalModel.maxSize = new Vector2(350, 100);
                    impotEnvironmentalModel.position = new Rect(600, 400, 350, 100);
                    impotEnvironmentalModel.Show();
                }

                GUILayout.Space(position.width * 0.2f);

                if (GUILayout.Button("导入机构模型", GUILayout.Width(position.width * 0.2f)))
                {
                    importMechanismModelWindow = EditorWindow.GetWindow<ImportMechanismModelWindow>(true, "导入机构模型");
                    importMechanismModelWindow.minSize = new Vector2(350, 100);
                    importMechanismModelWindow.maxSize = new Vector2(350, 100);
                    importMechanismModelWindow.position = new Rect(600, 400, 350, 100);
                    importMechanismModelWindow.Show();
                }
            }
            GUILayout.EndHorizontal();
        }

        GUILayout.Space(20);

        isShowMechanismControl = EditorGUILayout.Foldout(isShowMechanismControl, "机构控制", titleLabelStyle1); // 定义折叠菜单
        if(isShowMechanismControl)
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.Space(position.width * 0.2f);
                if (GUILayout.Button("机构运动控制分步模式", GUILayout.Width(position.width * 0.2f)))
                {
                    addDriveWindow = EditorWindow.GetWindow<AddDriveWindow>(true, "机构运动控制分步模式");
                    addDriveWindow.minSize = new Vector2(400, 400);
                    addDriveWindow.maxSize = new Vector2(400, 400);
                    addDriveWindow.position = new Rect(600, 400, 400, 400);
                    addDriveWindow.Show();
                }

                GUILayout.Space(position.width * 0.2f);
                if (GUILayout.Button("机构运动控制演示模式", GUILayout.Width(position.width * 0.2f)))
                {
                    addAutoDriveWindow = EditorWindow.GetWindow<AddAotuDriveWindow>(true, "机构运动控制演示模");
                    addAutoDriveWindow.minSize = new Vector2(350, 130);
                    addAutoDriveWindow.maxSize = new Vector2(350, 130);
                    addAutoDriveWindow.position = new Rect(600, 400, 350, 130);
                    addAutoDriveWindow.Show();
                }
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(20);
            GUILayout.BeginHorizontal();
            {
                GUILayout.Space(position.width * 0.1f);
                GUILayout.Label("控制流程");
                GUILayout.Space(position.width * 0.1f);
                GUILayout.Label("零件名");
                GUILayout.Space(position.width * 0.1f);
                GUILayout.Label("控制器");
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(10);
        }
        

        GUILayout.Space(20);

        isShowDisassembly = EditorGUILayout.Foldout(isShowDisassembly, "拆卸安装", titleLabelStyle1); // 定义折叠菜单
        if(isShowDisassembly)
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.Space(position.width * 0.2f);
                if (GUILayout.Button("添加拆装流程", GUILayout.Width(position.width * 0.2f)))
                {
                    addDisassemblyWindow = EditorWindow.GetWindow<AddDisassemblyWindow>(true, "添加拆装流程");
                    addDisassemblyWindow.minSize = new Vector2(400, 400);
                    addDisassemblyWindow.maxSize = new Vector2(400, 400);
                    addDisassemblyWindow.position = new Rect(600, 400, 400, 400);
                    addDisassemblyWindow.Show();

                }

                GUILayout.Space(position.width * 0.2f);
                if (GUILayout.Button("删除拆装流程", GUILayout.Width(position.width * 0.2f)))
                {


                }
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(20);
            GUILayout.BeginHorizontal();
            {
                GUILayout.Space(position.width * 0.1f);
                GUILayout.Label("拆装流程");
                GUILayout.Space(position.width * 0.1f);
                GUILayout.Label("零件名");
                GUILayout.Space(position.width * 0.1f);
                GUILayout.Label("拆装工具");
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(10);
            /*
            int dsCount = gm.disassemblySteps.Count;
            for (int i = 0; i < dsCount; i++)
            {
                GUILayout.BeginHorizontal();
                {
                    string part_name = gm.disassemblySteps[i].PartName;
                    int toolIndex = gm.disassemblySteps[i].ToolIndex;
                    string show_name;
                    string tool_name = " ";

                    if (part_name.Length > 15)
                    {
                        show_name = part_name.Substring(0, 10) + "...";
                    }
                    else
                    {
                        show_name = part_name;
                    }

                    if(toolIndex==0)
                    {
                        tool_name = "手";
                    }

                    if (toolIndex == 1)
                    {
                        tool_name = "螺丝刀";
                    }

                    if (toolIndex == 2)
                    {
                        tool_name = "扳手";
                    }

                    GUILayout.Space(position.width * 0.1f);
                    GUILayout.Label((i+1).ToString());
                    GUILayout.Space(position.width * 0.1f);
                    GUILayout.Label(show_name);
                    GUILayout.Space(position.width * 0.1f);
                    GUILayout.Label(tool_name);
                }
                GUILayout.EndHorizontal();
            }*/
        }
        

        GUILayout.Space(20);
        GUILayout.BeginHorizontal();
        {
            GUILayout.Space(position.width * 0.375f);
            if (GUILayout.Button("保存场景", GUILayout.Width(position.width * 0.25f)))
            {
                EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
                //输出拆装流程
            }
        }
        GUILayout.EndHorizontal();
        
    }


    /// <summary>
    /// 判断当前打开的场景和窗口是否对应
    /// </summary>
    private void CheckScenePath()
    {

        //if (EditorApplication.currentScene == "")
        //{
        //    if (EditorUtility.DisplayDialog("提示", "请先保存场景再编辑！", "确定"))
        //    {
        //        EditorSceneManager.SaveOpenScenes();
        //    }
        //}
        //else
        //{
            if (currentScenePath != EditorApplication.currentScene)
            {
                GameObject gm = GameObject.FindGameObjectWithTag("GameManager");
                if (gm)
                {
                    switch (gm.GetComponent<GameManager>().type)
                    {
                        case 1:
                            Window_T1._instance.Focus();
                            currentScenePath = EditorApplication.currentScene;
                            Debug.Log("已切换至编辑页面1");
                            break;
                        case 2:
                            Window_T2._instance.Focus();
                            currentScenePath = "";
                            Window_T2._instance.currentScenePath = EditorApplication.currentScene;
                            Debug.Log("已切换至编辑页面2");
                            break;
                        case 3:
                            Window_T3._instance.Focus();
                            currentScenePath = "";
                            Window_T3._instance.currentScenePath = EditorApplication.currentScene;
                            Debug.Log("已切换至编辑页面3");
                            break;
                    }
                }


                //switch (GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().type)
                //{
                //    case 1:
                //        Window_T1._instance.Focus();
                //        currentScenePath = "";
                //        Window_T1._instance.currentScenePath = EditorApplication.currentScene;
                //        Debug.Log("已切换至编辑页面1");
                //        break;
                //    case 2:
                //        Window_T2._instance.Focus();
                //        currentScenePath = EditorApplication.currentScene;
                //        Debug.Log("已切换至编辑页面2");
                //        break;
                //    case 3:
                //        Window_T3._instance.Focus();
                //        currentScenePath = "";
                //        Window_T3._instance.currentScenePath = EditorApplication.currentScene;
                //        Debug.Log("已切换至编辑页面3");
                //        break;
                //}
            }
        //}
    }

    /// <summary>
    /// 创建新场景
    /// </summary>
    public void CreateNewScene(SubjectType sjType, OperateType opType)
    {

        //实例化PC和VR必备物体
        DeskCanvasGameObject = Instantiate(DeskCanvasGamePrefab);
        StartUICanvasGameObject = Instantiate(StartUICanvasPrefab);
        GameObject EventSystemGameObject = Instantiate(eventSystemGamePrefab);
        GameObject VRTK_SDKManagerGameObject = Instantiate(VRTK_SDKManagerPrefab);
        GameObject VRTK_ScriptsGameObject = Instantiate(VRTK_ScriptsPrefab);
       
        //加载场景所需必要的脚本和物体
        gm = Instantiate(GameManager).GetComponent<GameManagerT2>();
        gm.InitCameraGameObject(GameObject.Find("Main Camera"), VRTK_SDKManagerGameObject, VRTK_ScriptsGameObject);
        gm.subjectType = sjType;
     
        uiCtrl = gm.GetComponent<UIControllerT2>();
        uiCtrl.InitUICanvas(StartUICanvasGameObject, DeskCanvasGameObject,
            VRTK_SDKManagerGameObject.transform.GetChild(0).GetChild(3).GetChild(0).GetChild(0).gameObject,
            VRTK_SDKManagerGameObject.transform.GetChild(0).GetChild(3).GetChild(0).GetChild(3).GetChild(0).GetChild(0).gameObject);
 
        VRTK_SDKManagerGameObject.GetComponent<VRTK_SDKManager>().scriptAliasLeftController = VRTK_ScriptsGameObject.transform.Find("LeftController").gameObject;
        VRTK_SDKManagerGameObject.GetComponent<VRTK_SDKManager>().scriptAliasRightController = VRTK_ScriptsGameObject.transform.Find("RightController").gameObject;
      
    }

    /// <summary>
    /// 初始化界面信息
    /// </summary>
    public void Init()
    {
        GameObject.FindGameObjectWithTag("MainCamera").AddComponent<FreeLookT2>();
    }
}
