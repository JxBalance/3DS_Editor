using UnityEngine;
using System.IO;
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VRTK;

public class Window_T3 : EditorWindow {

    public static Window_T3 _instance;//单例模式
    public string currentScenePath = "";//当前场景路径

    //GUI皮肤
    public GUISkin mySkin;
    private GUIStyle titleLabelStyle, subtitleLabelStyle, toolBarStyle, buttonStyle,
                     smallLabelStyle, partTitleStyle, boldLabelStyle, textAreaStyle;
    public Font myFont;

    public GameObject waterLine;
    /*
    public GameObject waterPipe;
    public GameObject waterLineSets;
    */

    //场景管理器
    public GameObject GameManager;

    private GameManagerT3 gm;
    public GameManagerT3 GM
    {
        get
        {
            if (!gm)
            {
                GetGameManager();
            }
            return gm;
        }
    }

    public GameObject DesktopGameCanvasPrefab;
    public GameObject DesktopGameCanvasGameObject;

    public GameObject StartUICanvasPrefab;
    public GameObject StartUICanvasGameObject;

    //头盔式场景预制体
    public GameObject VRCameraRigGameObject;
    public GameObject VRTKGameObject;
    public GameObject VRCanvas_LeftHandMenu;

    public GameObject EventSystem;

    public GameObject VRTK_SDKManagerPrefab;
    public GameObject VRTK_ScriptsPrefab;

    private UIControllerT3 uiCtrl;

    //预制体
    public GameObject mainCamera;

    public GameObject pipeGroupParent;
    public GameObject pipeGroup;

    public GameObject stateSetParent;
    public GameObject stateSetPrefab;

    public GameObject uiSetParent;
    public GameObject uiSetPrefab;

    private bool isShowStartPart = true;
    private bool isShowPlanePart = true;
    private bool isShowModelPart = true;
    private bool isShowHydrauPart = true;
    private bool isShowUIPart = true;

    private GameObject pipeModel;

    private Vector2 scrollGUI_T3;
    private Vector2 scrollPipe;
    private Vector2 scrollGroup;
    private Vector2 scrollHydrau;
    private Vector2 scrollUI;

    private string N_Text;

    private int transparentPer = 127;
    private int defaultTransPer = 127;

    private bool isPreviewMode = false;
    public bool isPreviewEnable = true;
    private bool isTest = true;

    private bool isWater = false;
    private bool isGas = false;

    private int pipeIndex = 0;          //管道模型管理界面中，选择的管道组索引
    private int selPipeInt = -1;        //管道模型管理界面中，选择的管道模型索引

    private int hydraupipeIndex = 0;    //气液流动编辑界面中，选择的管道组索引
    private int selPipeGroupInt = -1;   //气液流动编辑界面中，选中的管道组索引

    private string thisStateTitle;

    private int sphereIndex = 0;
    private int beginIndex = 0;
    private int endIndex = 0;

    public  int stateIndex = 0;         //气液流动编辑界面中，选中的流动状态索引
    private int currentState = -1;

    private int uiStateIndex = 0;       //界面编辑界面中，选择的流动状态索引
    private int selUISteteInt = -1;     //界面编辑界面中，选中的流动状态索引

    private GameObject selPipeModel;    //通过界面选择的物体

    private EditorWindow AddPipeWindow;
    private bool isPlaneModel = true;

    public void Awake()
    {
        _instance = this;
        GetGameManager();
        if (GM)
        {
            UIdefaultSettings();
        }
    }

    void OnGUI()
    {
        if (!_instance)
        {
            _instance = this;
        }
        if (GM)
        {
            HydrauScene();
        }
        else
        {
            NoGMScene();
        }
    }

    //气液流动编辑器主界面
    private void HydrauScene()
    {
        InitGUIStyle();

        scrollGUI_T3 = EditorGUILayout.BeginScrollView(scrollGUI_T3);

        GUILayout.Space(25);
        GUILayout.Label("气液流动展示编辑器", subtitleLabelStyle);
        GUILayout.Space(15);

        #region 开始
        isShowStartPart = EditorGUILayout.Foldout(isShowStartPart, "开始", partTitleStyle);
        if (isShowStartPart)
        {
            GUILayout.Space(20);
            GUILayout.BeginHorizontal();
            {
                GUILayout.Space(20);
                GUILayout.Label("导入带/不带管道飞机模型", GUILayout.Width(180));
                if (GUILayout.Button("导入飞机模型"))
                {
                    CreateWindow<ImportModelWindowT3>(true, "导入飞机模型", new Vector2(350, 100), new Vector2(350, 100),
                        new Rect(600, 400, 350, 100));
                }
                GUILayout.Space(20);
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            {
                GUILayout.Space(20);
                GUILayout.Label("(若不带管道)导入管道模型", GUILayout.Width(180));
                if (GUILayout.Button("导入管道模型"))
                {
                    CreateWindow<ImportModelWindowT3>(true, "导入管道模型", new Vector2(350, 100), new Vector2(350, 100),
                        new Rect(600, 400, 350, 100));
                }
                GUILayout.Space(20);
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(20);
        }
        #endregion

        #region 飞机模型管理
        isShowPlanePart = EditorGUILayout.Foldout(isShowPlanePart, "飞机模型管理", partTitleStyle);
        if (isShowPlanePart)
        {
            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            {
                GUILayout.Space(20);
                GUILayout.Label("进入场景后飞机透明度", GUILayout.Width(120));
                GUILayout.Space(5);
                transparentPer = EditorGUILayout.IntSlider(transparentPer, 0, 255);
                GUILayout.Space(20);
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            {
                GUILayout.Space(20);
                if (GUILayout.Button("选中飞机模型"))
                {
                    Selection.activeGameObject = GM.airplaneModel;
                    SceneView.lastActiveSceneView.FrameSelected();
                }
                EditorGUI.BeginDisabledGroup(isPreviewEnable == false);
                if (isPreviewMode = GUILayout.Button("效果预览"))
                {
                    TransparentSetting(1);
                    isPreviewEnable = false;
                }
                EditorGUI.EndDisabledGroup();
                if (GUILayout.Button("结束预览"))
                {
                    TransparentSetting(0);
                    isPreviewEnable = true;
                }
                GUILayout.Space(20);
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(20);

        }
        #endregion

        #region 管道模型管理
        isShowModelPart = EditorGUILayout.Foldout(isShowModelPart, "管道模型管理", partTitleStyle);
        if (isShowModelPart)
        {
            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            {
                int count = 0;
                count = GM.pipegroups.Count;

                string[] pipeNameSets = new string[count];
                for (int i = 0; i < count; i++)
                {
                    pipeNameSets[i] = GM.pipegroups[i].pipeGroupName;
                }

                GUILayout.Space(20);
                GUILayout.Label("管道组", GUILayout.Width(80));
                GUILayout.Space(5);
                pipeIndex = EditorGUILayout.Popup(pipeIndex, pipeNameSets);
                GUILayout.Space(5);
                if (GUILayout.Button("添加管道组", GUILayout.Width(position.width * 0.2f)))
                {
                    isPlaneModel = false;
                    CreateWindow<CreatePipeGroupsT3>(true, "添加管道组", new Vector2(350, 300), new Vector2(350, 300),
                        new Rect(600, 300, 350, 100));
                }
                GUILayout.Space(20);
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(5);

            GUILayout.BeginHorizontal();
            {
                GUILayout.Space(20);
                GUILayout.Label("包含管道", GUILayout.Width(80));
                //GUILayout.Space(5);
                scrollPipe = EditorGUILayout.BeginScrollView(scrollPipe, GUILayout.Height(80));
                GUILayout.BeginVertical();
                {
                    int count = 0;
                    if (GM.pipegroups.Count > 0)
                    {
                        count = GM.pipegroups[pipeIndex].pipeModels.Count;
                    }
                    string[] allPipesName = new string[count];
                    int old = selPipeInt;
                    for (int i = 0; i < count; i++)
                    {
                        allPipesName[i] = GM.pipegroups[pipeIndex].pipeModels[i].name;
                    }
                    selPipeInt = GUILayout.SelectionGrid(selPipeInt, allPipesName, 2);
                    if (old != selPipeInt)
                    {
                        selPipeModel = GM.pipegroups[pipeIndex].pipeModels[selPipeInt];
                        Selection.activeGameObject = selPipeModel;
                        SceneView.lastActiveSceneView.FrameSelected();
                        old = selPipeInt;
                    }
                }
                GUILayout.EndVertical();
                EditorGUILayout.EndScrollView();
                GUILayout.Space(20);
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(20);
        }
        #endregion

        #region 气液流动编辑
        isShowHydrauPart = EditorGUILayout.Foldout(isShowModelPart, "气液流动编辑", partTitleStyle);
        if (isShowHydrauPart)
        {
            #region 选择流动状态与形式
            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            {
                int count = 0;
                count = GM.stateSets.Count;

                string[] stateNameSets = new string[count];
                for (int i = 0; i < count; i++)
                {
                    stateNameSets[i] = GM.stateSets[i].stateName;
                }

                GUILayout.Space(20);
                GUILayout.Label("选择流动状态", GUILayout.Width(80));
                GUILayout.Space(5);
                int oldState = stateIndex;
                stateIndex = EditorGUILayout.Popup(stateIndex, stateNameSets);
                if(oldState != stateIndex)
                {                                                                              //改变当前状态,显示当前状态中的流水点,隐藏上一状态流水点
                    GM.stateSets[oldState].transform.GetChild(0).gameObject.SetActive(false);
                    GM.stateSets[stateIndex].transform.GetChild(0).gameObject.SetActive(true);
                }
                if (GUILayout.Button("添加流动状态", GUILayout.Width(position.width * 0.2f)))
                {
                    CreateWindow<CreateStateSetsT3>(true, "添加流动状态", new Vector2(350, 110), new Vector2(350, 110),
                        new Rect(600, 300, 350, 100));                    
                }
                GUILayout.Space(20);
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            {
                GUILayout.Space(20);
                GUILayout.Label("状态标题", GUILayout.Width(80));
                GUILayout.Space(5);
                string oldTitle = thisStateTitle;
                thisStateTitle = GUILayout.TextField(thisStateTitle, GUILayout.Width(position.width * 0.5f));
                if (oldTitle != thisStateTitle)
                {
                    if (GM.stateSets[stateIndex].stateTitle != thisStateTitle)
                    {
                        GM.stateSets[stateIndex].stateTitle = thisStateTitle;
                    }
                }
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            {
                GUILayout.Space(20);
                GUILayout.Label("流动形式", GUILayout.Width(80));
                GUILayout.Space(40);

                if (currentState != stateIndex)             //转换状态，更新流动编辑界面内容
                {
                    UIdefaultSettings();
                    currentState = stateIndex;
                }
                if (isWater = GUILayout.Toggle(isWater, "液体", GUILayout.Width(position.width * 0.3f)))
                {
                    isGas = false;
                    if (GM.stateSets.Count > 0)
                    {
                        GM.stateSets[stateIndex].hydrauForm = "Water";
                    }
                }
                if (isGas = GUILayout.Toggle(isGas, "气体", GUILayout.Width(position.width * 0.3f)))
                {
                    isWater = false;
                    if (GM.stateSets.Count > 0)
                    {
                        GM.stateSets[stateIndex].hydrauForm = "Gas";
                    }
                }
            }
            GUILayout.EndHorizontal();
            #endregion

            #region 管道组添加与选中
            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            {
                int count = 0;
                count = GM.pipegroups.Count;

                string[] pipeSets = new string[count];
                for (int i = 0; i < count; i++)
                {
                    pipeSets[i] = GM.pipegroups[i].pipeGroupName;
                }

                GUILayout.Space(20);
                GUILayout.Label("管道组", GUILayout.Width(80));
                GUILayout.Space(5);
                hydraupipeIndex = EditorGUILayout.Popup(hydraupipeIndex, pipeSets);
                GUILayout.Space(5);
                if (GUILayout.Button("添加到状态", GUILayout.Width(position.width * 0.2f)))
                {
                    if (GM.stateSets[stateIndex].statePipegroups.Count > 0)         //已存在状态中的管道组不再添加
                    {
                        int i = 0;
                        for (i = 0; i < GM.stateSets[stateIndex].statePipegroups.Count; i++)
                        {
                            if (GM.stateSets[stateIndex].statePipegroups[i].pipeGroupName == pipeSets[hydraupipeIndex])
                            {
                                break;
                            }
                        }
                        if (i == GM.stateSets[stateIndex].statePipegroups.Count)
                        {
                            GM.stateSets[stateIndex].statePipegroups.Add(GM.pipegroups[hydraupipeIndex]);
                        }
                    }
                    else
                    {
                        GM.stateSets[stateIndex].statePipegroups.Add(GM.pipegroups[hydraupipeIndex]);

                    }
                }
                GUILayout.Space(20);
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            {
                GUILayout.Space(20);
                GUILayout.Label("已包含管道组", GUILayout.Width(80));
                scrollGroup = EditorGUILayout.BeginScrollView(scrollGroup, GUILayout.Height(60));
                GUILayout.BeginVertical();
                {
                    int count = 0;
                    if (GM.stateSets.Count > 0)
                    {
                        count = GM.stateSets[stateIndex].statePipegroups.Count;
                    }
                    string[] allPipeGroups = new string[count];
                    int old = selPipeGroupInt;
                    for (int i = 0; i < count; i++)
                    {
                        allPipeGroups[i] = GM.stateSets[stateIndex].statePipegroups[i].pipeGroupName;
                    }
                    selPipeGroupInt = GUILayout.SelectionGrid(selPipeGroupInt, allPipeGroups, 2);
                }
                GUILayout.EndVertical();
                EditorGUILayout.EndScrollView();
                GUILayout.Space(20);
            }
            GUILayout.EndHorizontal();
            #endregion

            #region 流动线路与流水点匹配
            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            {
                GUILayout.Space(20);
                GUILayout.Label("流动线路", GUILayout.Width(80));
                GUILayout.Space(5);
                scrollHydrau = EditorGUILayout.BeginScrollView(scrollHydrau, GUILayout.Height(120));
                GUILayout.BeginVertical(GUILayout.Width(position.width * 0.52f));
                {
                    int count = 0;
                    if (GM.stateSets.Count > 0)
                    {
                        count = GM.stateSets[stateIndex].statesVertix.Count;
                    }
                    string[] stateGroup = new string[count];            //该集合包含该状态中的所有管道组流水点
                    for (int i = 0; i < count; i++)
                    {
                        stateGroup[i] = GM.stateSets[stateIndex].statesVertix[i].stateSphere.name;
                    }

                    bool isbreak = false;
                    List<int> sts = new List<int>();
                    for (int i = 0; i < count; i++)
                    {
                        for (int j = 0; j < GM.stateSets[stateIndex].statesVertix.Count; j++)
                        {
                            if (i == GM.stateSets[stateIndex].statesVertix[j].stateOrder)
                            {
                                sts.Add(GM.stateSets[stateIndex].statesVertix[j].stateIndex);
                                break;
                            }
                            if (j == (count - 1))
                            {
                                isbreak = true;
                                break;
                            }
                        }
                        if (isbreak)
                        {
                            break;
                        }
                    }
                    for (int i = 0; i < sts.Count; i++)                  //流水线路排列
                    {
                        GUILayout.BeginHorizontal();
                        {
                            beginIndex = EditorGUILayout.Popup(sts[i], stateGroup, GUILayout.Width(position.width * 0.16f));
                            GUILayout.Label("      —> ", GUILayout.Width(position.width * 0.12f));
                            if (i == sts.Count - 1)
                            {
                                string[] x = { "" };
                                endIndex = EditorGUILayout.Popup(endIndex, x, GUILayout.Width(position.width * 0.16f));
                            }
                            else
                            {
                                endIndex = EditorGUILayout.Popup(sts[i + 1], stateGroup, GUILayout.Width(position.width * 0.16f));
                            }
                            //GUILayout.Space(10);
                        }
                        GUILayout.EndHorizontal();
                    }
                    if (sts.Count == 0)
                    {
                        GUILayout.BeginHorizontal();
                        {
                            beginIndex = EditorGUILayout.Popup(beginIndex, stateGroup, GUILayout.Width(position.width * 0.16f));
                            GUILayout.Label("      —> ", GUILayout.Width(position.width * 0.12f));
                            endIndex = EditorGUILayout.Popup(endIndex, stateGroup, GUILayout.Width(position.width * 0.16f));
                            //GUILayout.Space(10);
                        }
                        GUILayout.EndHorizontal();
                    }
                }
                GUILayout.EndVertical();
                EditorGUILayout.EndScrollView();

                GUILayout.BeginVertical();
                {
                    GUILayout.BeginHorizontal();
                    {
                        if (GUILayout.Button("自动匹配流水点"))
                        {
                            int k = 0;
                            GameObject stateSetParent = GameObject.Find("StateSetsParentT3(Clone)");
                            if (selPipeGroupInt == -1)
                            {
                                EditorUtility.DisplayDialog("提示", "请选择已包含的管道组", "确定");
                            }
                            else
                            {
                                for (int i = 0; i < GM.stateSets[stateIndex].statePipegroups[selPipeGroupInt].pipeModels.Count; i++)        //计算生成管道的顶点集合
                                {
                                    stateSetParent.GetComponent<CreateSphereT3>().GetVertix(GM.stateSets[stateIndex].statePipegroups[selPipeGroupInt].pipeModels[i]);

                                    for (int j = 0; j < 100; j++)
                                    {
                                        string oldName = "sphere0" + j;
                                        GameObject go = GameObject.Find(oldName);
                                        if (!go)
                                        {
                                            k += j;
                                            break;
                                        }
                                        go.name = "sphere00" + (j + k);
                                        GameObject parent = GameObject.Find(GM.stateSets[stateIndex].stateName);
                                        if(parent.transform.GetChild(0).gameObject.name == "SphereSet")
                                        {
                                            parent = parent.transform.GetChild(0).gameObject;
                                        }
                                        else
                                        {
                                            parent = parent.transform.GetChild(1).gameObject;
                                        }
                                        go.transform.parent = parent.transform;
                                        if (GM.stateSets[stateIndex].statesVertix.Count > (j + k))
                                        {
                                            GM.stateSets[stateIndex].statesVertix[j + k] = new StateSetT3.pipeStateSets();
                                        }
                                        else
                                        {
                                            GM.stateSets[stateIndex].statesVertix.Add(new StateSetT3.pipeStateSets());
                                        }
                                        GM.stateSets[stateIndex].statesVertix[j + k].stateSphere = go;
                                        GM.stateSets[stateIndex].statesVertix[j + k].stateIndex = (j + k);
                                    }
                                }
                            }
                        }
                    }
                    GUILayout.EndHorizontal();
                    GUILayout.Space(5);
                    GUILayout.BeginHorizontal();
                    {
                        if (GUILayout.Button("自动生成流水线"))
                        {
                            for (int i = 0; i < GM.stateSets[stateIndex].statesVertix.Count; i++)
                            {
                                GM.stateSets[stateIndex].statesVertix[i].stateOrder = i;
                            }
                            CreateWater();

                        }
                    }
                    GUILayout.EndHorizontal();
                }
                GUILayout.EndVertical();
                GUILayout.Space(20);
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(20);
            #endregion
        }
        #endregion

        #region 界面编辑
        isShowUIPart = EditorGUILayout.Foldout(isShowUIPart, "界面编辑", partTitleStyle);
        if (isShowUIPart)
        {
            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            {
                int count = 0;
                count = GM.stateSets.Count;

                string[] stateNameSets = new string[count];
                for (int i = 0; i < count; i++)
                {
                    stateNameSets[i] = GM.stateSets[i].stateName;
                }

                GUILayout.Space(20);
                GUILayout.Label("选择流动状态", GUILayout.Width(80));
                GUILayout.Space(5);
                uiStateIndex = EditorGUILayout.Popup(uiStateIndex, stateNameSets);
                GUILayout.Space(5);
                if (GUILayout.Button("添加状态至界面", GUILayout.Width(position.width * 0.2f)))
                {
                    GameObject go;
                    if (!GM.uiSets)
                    {
                        go = Instantiate(uiSetPrefab);
                        go.name = "UISet";
                        go.transform.parent = GameObject.Find("UISetParentT3(Clone)").transform;
                    }
                    else
                    {
                        go = GameObject.Find("UISet");
                    }
                    UISetT3 newUISet = go.GetComponent<UISetT3>();
                    newUISet.uiStates.Add(GM.stateSets[uiStateIndex]);
                    GM.uiSets = newUISet;
                }
                GUILayout.Space(20);
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(5);

            GUILayout.BeginHorizontal();
            {
                GUILayout.Space(20);
                GUILayout.Label("已包含流水状态", GUILayout.Width(80));
                scrollUI = EditorGUILayout.BeginScrollView(scrollUI, GUILayout.Height(60));
                GUILayout.BeginVertical();
                {
                    int count = 0;
                    if (GM.uiSets)
                    {
                        count = GM.uiSets.uiStates.Count;
                    }

                    string[] uiGroups = new string[count];
                    int old = selUISteteInt;
                    for (int i = 0; i < count; i++)
                    {
                        uiGroups[i] = GM.uiSets.uiStates[i].stateName;
                    }
                    selUISteteInt = GUILayout.SelectionGrid(selUISteteInt, uiGroups, 2);
                    selUISteteInt = -1;
                }
                GUILayout.EndVertical();
                EditorGUILayout.EndScrollView();
                GUILayout.Space(20);
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(20);
        }
        #endregion
        
        EditorGUILayout.EndScrollView();

    }

    //若没有实例化场景，则输出提示
    private void NoGMScene()
    {
        InitGUIStyle();

        GUILayout.Space(25);
        GUILayout.Label("请打开或新建气液流动场景！", subtitleLabelStyle);
        GUILayout.Space(15);
    }
    
    //创建水流
    private void CreateWater()
    {
        float x, y, z;
        int i, j, k = 0;
        for (i = 0; i < GM.stateSets[stateIndex].statePipegroups[selPipeGroupInt].pipeModels.Count; i++)
        {
            float delay = 0f;
            for (j = 0; j < 4; j++)
            {
                if (j == 3)
                {
                    k++;
                }
                else
                {
                    x = GM.stateSets[stateIndex].statesVertix[k + 1].stateSphere.transform.position.x -
                        GM.stateSets[stateIndex].statesVertix[k].stateSphere.transform.position.x;
                    y = GM.stateSets[stateIndex].statesVertix[k + 1].stateSphere.transform.position.y -
                        GM.stateSets[stateIndex].statesVertix[k].stateSphere.transform.position.y;
                    z = GM.stateSets[stateIndex].statesVertix[k + 1].stateSphere.transform.position.z -
                        GM.stateSets[stateIndex].statesVertix[k].stateSphere.transform.position.z;

                    float waterLength = Mathf.Sqrt(Mathf.Pow(x, 2) + Mathf.Pow(y, 2) + Mathf.Pow(z, 2));

                    GameObject go = Instantiate(waterLine);
                    go.transform.position = GM.stateSets[stateIndex].statesVertix[i].stateSphere.transform.position;
                    string newName = "waterLine00" + k;
                    go.name = newName;
                    GameObject parent = GameObject.Find(GM.stateSets[stateIndex].stateName);
                    if(parent.transform.GetChild(0).gameObject.name == "WaterLineSet")
                    {
                        parent = parent.transform.GetChild(0).gameObject;
                    }
                    else
                    {
                        parent = parent.transform.GetChild(1).gameObject;
                    }
                    go.transform.parent = parent.transform;
                    go.transform.position = new Vector3(GM.stateSets[stateIndex].statesVertix[k].stateSphere.transform.position.x,
                                                        GM.stateSets[stateIndex].statesVertix[k].stateSphere.transform.position.y,
                                                        GM.stateSets[stateIndex].statesVertix[k].stateSphere.transform.position.z);

                    //长度求值
                    //f1.GetComponent<ParticleSystem>().startLifetime = (xxx) * 0.8678f - 0.4975f;
                    go.GetComponent<ParticleSystem>().startLifetime = 0.00001f * waterLength * waterLength + 0.8222f * waterLength - 0.0919f + 0.01f * waterLength;

                    //延时，使水流依次流动
                    if (j != 0)
                    {
                        go.GetComponent<ParticleSystem>().startDelay = delay;
                    }
                    delay += go.GetComponent<ParticleSystem>().startLifetime;

                    float rotateX = 0f, rotateY = 0f;

                    if (z > 0 && x > 0)
                    {
                        rotateY = (180 / (Mathf.PI / Mathf.Atan(Mathf.Abs(x) / Mathf.Abs(z))));
                    }
                    else if (z < 0 && x > 0)
                    {
                        rotateY = 180 - (180 / (Mathf.PI / Mathf.Atan(Mathf.Abs(x) / Mathf.Abs(z))));
                    }
                    else if (z > 0 && x < 0)
                    {
                        rotateY = -(180 / (Mathf.PI / Mathf.Atan(Mathf.Abs(x) / Mathf.Abs(z))));
                    }
                    else if (z < 0 && x < 0)
                    {
                        rotateY = -180 + (180 / (Mathf.PI / Mathf.Atan(Mathf.Abs(x) / Mathf.Abs(z))));
                    }
                    else if (z == 0 && x != 0)
                    {
                        if (x > 0)
                        {
                            rotateY = 90;
                        }
                        else
                        {
                            rotateY = -90;
                        }
                    }
                    else if (x == 0 && z != 0)
                    {
                        if (z > 0)
                        {
                            rotateY = 0;
                        }
                        else
                        {
                            rotateY = 180;
                        }
                    }
                    if (y > 0)
                    {
                        rotateX = -180 / (Mathf.PI / Mathf.Atan(y / Mathf.Sqrt((Mathf.Pow(x, 2) + Mathf.Pow(z, 2)))));
                    }
                    else if (y < 0)
                    {
                        rotateX = -180 / (Mathf.PI / Mathf.Atan(y / Mathf.Sqrt((Mathf.Pow(x, 2) + Mathf.Pow(z, 2)))));
                    }
                    else
                    {
                        rotateX = 0;
                    }

                    go.transform.eulerAngles = new Vector3(rotateX, rotateY, 0f);
                    k++;
                }

            }

        }


        /*
        for (int i = 0; i < GM.stateSets[stateIndex].statesVertix.Count-1; i++)
        {
            GameObject go = Instantiate(waterLine);
            go.transform.position = GM.stateSets[stateIndex].statesVertix[i].stateSphere.transform.position;
            string newName = "waterLine00" + i;
            go.name = newName;
            Vector3 fVertix, bVertix;
            fVertix = GM.stateSets[stateIndex].statesVertix[i].stateSphere.transform.position;
            bVertix = GM.stateSets[stateIndex].statesVertix[i].stateSphere.transform.position;
            float distance = Mathf.Pow((fVertix.x - bVertix.x), 2) + Mathf.Pow((fVertix.y - bVertix.y), 2) + Mathf.Pow((fVertix.z - bVertix.z), 2);

            
        }
        for (int i = 0; i < 5; i++)
        {
            GameObject aas = GameObject.Find("FuelPipe00"+(i+1));
            aas.GetComponent<Renderer>().material.shader = Shader.Find("Transparent/Diffuse");

            Color colorChange = new Color(aas.GetComponent<Renderer>().material.color.r,
                                          aas.GetComponent<Renderer>().material.color.g,
                                          aas.GetComponent<Renderer>().material.color.b, 60/255f);

            aas.GetComponent<Renderer>().material.color = colorChange;
        }

        GameObject gooo = Instantiate(waterLineSets);
        */
    }

    //设置飞机模型透明度
    public void TransparentSetting(int isPreview)
    {
        if (isPreview == 1)
        {
            if (GM && GM.airplaneModel && GM.airplaneModel.transform.childCount > 0)
            {
                SetAllChild(GM.airplaneModel.transform, "Transparent/Diffuse", (transparentPer / 255f));
            }
        }
        else
        {
            SetAllChild(GM.airplaneModel.transform, "Standard", 1f);
        }

    }

    //设置飞机模型所有子节点的材质透明度
    private void SetAllChild(Transform parent, string shaderType, float trans)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            SetAllChild(parent.GetChild(i), shaderType, trans);
        }
        if (parent.GetComponent<Renderer>())
        {
            for (int i = 0; i < parent.GetComponent<Renderer>().sharedMaterials.Length; i++)
            {
                parent.GetComponent<Renderer>().sharedMaterials[i].shader = Shader.Find(shaderType);
                Color colorChange = new Color(parent.GetComponent<Renderer>().sharedMaterials[i].color.r,
                                               parent.GetComponent<Renderer>().sharedMaterials[i].color.g,
                                                parent.GetComponent<Renderer>().sharedMaterials[i].color.b, trans);
                parent.GetComponent<Renderer>().sharedMaterials[i].color = colorChange;
            }
        }
    }

    //创建新窗口
    private void CreateWindow<T>(bool utility, string title, Vector2 minSize, Vector2 maxSize, Rect position) where T : EditorWindow
    {
        T tWindow = EditorWindow.GetWindow<T>(utility, title);
        tWindow.minSize = minSize;
        tWindow.maxSize = maxSize;
        tWindow.position = position;
        tWindow.Show();
        AddPipeWindow = tWindow;
    }

    private void GetEditorController()
    {
        if (!_instance)
        {
            Awake();
        }
        if (!gm)
        {
            GetGameManager();
        }
    }

    /// <summary>
    /// 获取GM 
    /// </summary>
    /// <returns></returns>
    private void GetGameManager()
    {
        GameObject _gm = GameObject.FindGameObjectWithTag("GameManager");
        if (_gm)
        {
            gm = _gm.GetComponent<GameManagerT3>();
            uiCtrl = _gm.GetComponent<UIControllerT3>();
            //Debug.Log("获取T1 gm");
        }
        else
        {
            //Debug.Log("GameManager物体不存在");
        }
    }

    //设置界面的初始状态
    public void UIdefaultSettings()
    {
        transparentPer = GM.airplaneTransparent;                //设置飞机透明度

        if (GM.stateSets.Count > 0)
        {
            thisStateTitle = GM.stateSets[stateIndex].stateTitle;            //设置状态标题

            if (GM.stateSets[stateIndex].hydrauForm == "Water")     //设置选中液体或气体
            {
                isWater = true;
                isGas = false;
            }
            else
            {
                isGas = true;
                isWater = false;
            }
        }
        else
        {
            thisStateTitle = "";
        }

    }

    // Use this for initialization
    void Start()
    {

    }

    //界面变量初始化
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

        buttonStyle = new GUIStyle(mySkin.button);

        textAreaStyle = new GUIStyle(GUI.skin.textArea);
        textAreaStyle.alignment = TextAnchor.UpperLeft;
        textAreaStyle.stretchWidth = true;
    }


    void Update()
    {
        if(!GM) return;

        if (defaultTransPer != transparentPer)
        {
            defaultTransPer = transparentPer;
            if (!isPreviewEnable)
            {
                TransparentSetting(1);
            }
            GM.airplaneTransparent = transparentPer;
        }
        if (isPreviewMode)
        {
            isPreviewMode = true;
        }

        //场景中选中其他物体 或 没有选中的物体，则在界面中取消选中的物体按钮
        if (!Selection.activeGameObject || Selection.activeGameObject != selPipeModel)
        {
            selPipeInt = -1;
        }

        if (!AddPipeWindow && !isPlaneModel)
        {
            gm.airplaneModel.SetActive(true);
            isPlaneModel = true;
        }

        //新建场景,数值初始化
        if (GM.stateSets.Count == 0)
        {
            stateIndex = 0;
            thisStateTitle = "";
            isWater = false;
            isGas = false;
        }
        else if(GM.stateSets.Count < stateIndex)
        {
            stateIndex = 0;
        }
        if(GM.pipegroups.Count == 0)
        {
            pipeIndex = 0;
        }
        else if (GM.pipegroups.Count < pipeIndex)
        {
            pipeIndex = 0;
        }
    }


    /// <summary>
    /// 初始化（重新载入）界面数据
    /// </summary>
    public void Init()
    {
        /*
        GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
        camera.AddComponent<PhysicsRaycaster>();
        camera.AddComponent<FreeLookT1>();
        */
    }
    
    /// <summary>
    /// 创建新场景
    /// </summary>
    public void CreateNewScene(SubjectType sjType, OperateType opType)
    {
        //实例化PC和VR必备物体,加载场景所需必要的脚本和物体
        GameObject camera = Instantiate(mainCamera);
        camera.name = "Main Camera T3";
        GameObject oldCamera = GameObject.Find("Main Camera");        
        oldCamera.SetActive(false);

        GameObject EventSystemGameObject = Instantiate(EventSystem);
        StartUICanvasGameObject = Instantiate(StartUICanvasPrefab);
        DesktopGameCanvasGameObject = Instantiate(DesktopGameCanvasPrefab);
        GameObject VRTK_SDKManagerGameObject = Instantiate(VRTK_SDKManagerPrefab);
        GameObject VRTK_ScriptsGameObject = Instantiate(VRTK_ScriptsPrefab);

        gm = Instantiate(GameManager).GetComponent<GameManagerT3>();
        gm.InitCameraGameObject(camera, StartUICanvasGameObject, DesktopGameCanvasGameObject, VRTK_SDKManagerGameObject, VRTK_ScriptsGameObject);
        gm.subjectType = sjType;

        GameObject PipeGroupParentGameObject = Instantiate(pipeGroupParent);
        GameObject StateSetsParentGameObject = Instantiate(stateSetParent);
        GameObject UISetParentGameObject = Instantiate(uiSetParent);

        
        uiCtrl = gm.GetComponent<UIControllerT3>();
        uiCtrl.InitUICanvas(StartUICanvasGameObject, DesktopGameCanvasGameObject, 
            VRTK_SDKManagerGameObject.transform.GetChild(0).GetChild(3).GetChild(0).GetChild(0).gameObject,
            VRTK_SDKManagerGameObject.transform.GetChild(0).GetChild(3).GetChild(0).GetChild(3).GetChild(0).GetChild(0).gameObject);


        VRTK_SDKManagerGameObject.GetComponent<VRTK_SDKManager>().scriptAliasLeftController =
            VRTK_ScriptsGameObject.transform.Find("LeftController").gameObject;
        VRTK_SDKManagerGameObject.GetComponent<VRTK_SDKManager>().scriptAliasRightController =
            VRTK_ScriptsGameObject.transform.Find("RightController").gameObject;


    }

    
}
