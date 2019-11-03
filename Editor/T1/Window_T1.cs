using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VRTK;

public class Window_T1 : EditorWindow
{
    public static Window_T1 _instance;//单例模式
    public string currentScenePath = "";//当前场景路径

    //public event UpdateLabelsDelegate updateSceneLabelsEvent;

    //GUI皮肤
    public GUISkin mySkin;
    private GUIStyle titleLabelStyle, subtitleLabelStyle, toolBarStyle, partTitleStyle, boldLabelStyle, title3LabelStyle;
    private static GUIStyle labelStyle;
    public Font myFont;
    private Vector2 scrollPos = Vector2.zero;//窗口滚动条


    //场景管理器
    public GameObject GameManager;
    private GameManagerT1 gm;

    public GameManagerT1 GM
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

    private UIControllerT1 uiCtrl;

    public GameObject DesktopGameCanvasPrefab;
    public GameObject DesktopGameCanvasGameObject;

    public GameObject StartUICanvasPrefab;
    public GameObject StartUICanvasGameObject;

    public GameObject EventSystem;
    private Transform infoBoardParentTransform;

    public Transform InfoBoardParentTransform
    {
        get
        {
            if (infoBoardParentTransform)
            {
                return infoBoardParentTransform;
            }
            else
            {
                if (GameObject.Find("InfoBoardParent(Clone)"))
                {
                    infoBoardParentTransform = GameObject.Find("InfoBoardParent(Clone)").transform;
                    return infoBoardParentTransform;
                }
                else
                {
                    Debug.Log("InfoBoardParent(Clone)不存在");
                }
                return null;
            }
        }
        set { infoBoardParentTransform = value; }
    }
    private Transform unitParentTransform;

    public Transform UnitParentTransform
    {
        get
        {
            if (unitParentTransform)
            {
                return unitParentTransform;
            }
            else
            {
                if (GameObject.Find("ObservePointParent(Clone)"))
                {
                    unitParentTransform = GameObject.Find("ObservePointParent(Clone)").transform;
                    return unitParentTransform;
                }
                else
                {
                    Debug.Log("ObservePointParent(Clone)不存在");
                }
                return null;
            }
            
        }
        set { unitParentTransform = value; }
    }

    //桌面式场景预制体
    public GameObject unitButton;
    public GameObject pointButton;
    public GameObject InfoBoardParent;
    public GameObject InfoBoardGameObject;
    public GameObject ObservePointParent;
    public GameObject ObservePointGameObject;
    public GameObject UnitParent;
    public GameObject UnitGameObject;
    public GameObject DesktopStartUI;

    //头盔式场景预制体
    public GameObject VRCameraRigGameObject;
    public GameObject VRTKGameObject;
    public GameObject VRCanvas_LeftHandMenu;

    public GameObject VRTK_SDKManagerPrefab;
    public GameObject VRTK_ScriptsPrefab;


    //各模块折叠变量
    private bool isShowStartPart = true;
    private bool isShowIntroducePart = true;
    private bool isShowTransformPart = true;
    private bool isShowUnitPart = true;
    private bool isShowObservePointPart = true;

    //开始
    private string[] startBtnsText = {"课件名称", "导入飞机/场景模型", "课件设置", "导入素材", "保存场景"};

    //总体介绍变量
    private string[] introBtnsText = { "添加提示板", "更新提示板" };
    private bool[] introBtnsDisable = { false, false, true };
    public string introTitleText = "";
    public string introContentText = "";
    public Texture2D introLoadtexture2D;

    

    //基础信息变量
    public GameObject currentModel;
    public Vector3 modelPosition = Vector3.zero;
    public Vector3 modelRotation = Vector3.zero;
    public Vector3 modelScale = Vector3.one;

    //部件编辑变量
    private string[] unitBtnsText = {"添加部件", "更新部件" };
    private bool[] unitBtnsDisable = { false, false, true };

    public string unitName = "";
    public string unitContent = "";
    public Color unitColor = new Color(0.38f, 0.97f, 0.44f);
    public Texture2D unitLoadtexture2D;
    public int unitObservePointCount = 0;
    public List<Vector3> observePointPositionList = new List<Vector3>();
    public List<Vector3> observePointRotationList = new List<Vector3>();

    public bool groupOperation = false;
    private string[] operationToolbarTexts = {"旋转式", "按压式", "旋转并按压式"};
    public int operationToolbarOption = 0;

    //旋转式
    private string[] gearsToolbarText = {"无级", "有级"};
    public int hasGears = 0;
    public float angle_R = 0.0f;
    public int axis_R = 0;
    private string[] axisRToolbarTexts = { "X", "Y", "Z" };
    public int gearCount = 1;

    //按压式
    private string[] isUpToolbarText = {"部分弹起", "完全弹起"};
    public int isUpComplete = 0;
    public float depth = 0f;
    public int axis_P = 0;
    private string[] axisPToolbarTexts = { "X", "Y", "Z" };

    //旋转并按压式
    public float angle_RP_R = 0.0f;                 //旋转角
    public int axis_RP_R = 0;                       //旋转轴
    public float depth_RP_P = 0f;                   //按压深度
    public int axis_RP_P = 0;                       //按压轴

    private bool isLockGameObjects = false;
    private float temp = 0;
    private float speed = 0.8f;         //角度平滑过渡所需时间
    private float totalAngle = 0f;
    private GameObject[] currentGameObjects;
    private Vector3[] position_original;
    private Quaternion[] rotation_original;

    private Vector3 rotation_target_R;
    private Vector3 position_add_P;
    

     

    public void Awake()
    {
        _instance = this;
        GetGameManager();

        if(!gm) return;
        UnitManagerWindowT1.Instance.UpdateWindow();
        ModelManagerWindowT1.Instance.UpdateWindow();
        IntroManagerWindowT1.Instance.UpdateWindow();
    }

    void Update()
    {
        //isLockGameObjects = false;
        if (isLockGameObjects)
        {
            //Selection.objects = currentGameObjects;
            //预览操作效果
            switch (operationToolbarOption)
            {
                case 0:
                    //旋转
                    switch (axis_R)
                    {
                        case 0:
                            rotation_target_R = new Vector3(angle_R, 0, 0);
                            break;
                        case 1:
                            rotation_target_R = new Vector3(0, angle_R, 0);
                            break;
                        case 2:
                            rotation_target_R = new Vector3( 0, 0, angle_R);
                            break;
                    }
                    for (int i = 0; i < currentGameObjects.Length; i++)
                    {
                        currentGameObjects[i].transform.localPosition = position_original[i];
                        currentGameObjects[i].transform.localRotation = rotation_original[i];
                        currentGameObjects[i].transform.Rotate(rotation_target_R, Space.Self);
                    }
                    break;
                case 1:
                    //按压
                    switch (axis_P)
                    {
                        case 0:
                            position_add_P = new Vector3(depth, 0, 0);
                            break;
                        case 1:
                            position_add_P = new Vector3(0, depth, 0);
                            break;
                        case 2:
                            position_add_P = new Vector3(0, 0, depth);
                            break;
                    }
                    for (int i = 0; i < currentGameObjects.Length; i++)
                    {
                        currentGameObjects[i].transform.localPosition = position_original[i] + position_add_P;
                        currentGameObjects[i].transform.localRotation = rotation_original[i];
                    }
                    break;
                case 2:
                    //旋转并且按压
                    switch (axis_RP_R)
                    {
                        case 0:
                            rotation_target_R = new Vector3(angle_RP_R, 0, 0);
                            break;
                        case 1:
                            rotation_target_R = new Vector3(0, angle_RP_R, 0);
                            break;
                        case 2:
                            rotation_target_R = new Vector3(0, 0, angle_RP_R);
                            break;
                    }
                    switch (axis_RP_P)
                    {
                        case 0:
                            position_add_P = new Vector3(depth_RP_P, 0, 0);
                            break;
                        case 1:
                            position_add_P = new Vector3(0, depth_RP_P, 0);
                            break;
                        case 2:
                            position_add_P = new Vector3(0, 0, depth_RP_P);
                            break;
                    }

                    for (int i = 0; i < currentGameObjects.Length; i++)
                    {
                        currentGameObjects[i].transform.localPosition = position_original[i] + position_add_P;
                        currentGameObjects[i].transform.localRotation = rotation_original[i];
                        currentGameObjects[i].transform.Rotate(rotation_target_R, Space.Self);
                    }
                    break;
            }
        }
    }

    void OnGUI()
    {
        #region 单例 格式 标题
        GetEditorController();
        InitGUIStyle();
        CheckScenePath();
        if(!gm) return; 


        float h = 1200;
        float w = position.width;
        if (position.height < h)
        {
            w -= 15;
        }
        scrollPos = GUI.BeginScrollView(new Rect(0, 0, position.width, position.height), scrollPos,
            new Rect(0, 0, w, h));
        GUILayout.BeginArea(new Rect(5,5,w-10,h));
        GUILayout.Label("位置识别展示编辑器", subtitleLabelStyle);
        #endregion
        
        #region 开始
        isShowStartPart = GUITools.DrawBox(isShowStartPart, "开始", partTitleStyle, StartBox);
        #endregion

        #region 总体介绍
        isShowIntroducePart = GUITools.DrawBox(isShowIntroducePart, "总体介绍", partTitleStyle, IntroduceBox);
        #endregion

        #region 模型管理
        isShowTransformPart = GUITools.DrawBox(isShowTransformPart, "模型管理", partTitleStyle, TransformBox);
        #endregion

        #region 部件管理
        isShowUnitPart = GUITools.DrawBox(isShowUnitPart, "部件管理", partTitleStyle, UnitBox);
        #endregion

        GUILayout.EndArea();
        GUI.EndScrollView();
    }

    void OnFocus()
    {
        //Debug.Log("当窗口获得焦点时调用一次");
    }

    void OnLostFocus()
    {
        //Debug.Log("当窗口丢失焦点时调用一次");
    }

    void OnHierarchyChange()
    {
        //Debug.Log("当Hierarchy视图中的任何对象发生改变时调用一次");
    }

    void OnProjectChange()
    {
        //Debug.Log("当Project视图中的资源发生改变时调用一次");
    }

    void OnInspectorUpdate()
    {
        //Debug.Log("窗口面板的更新");
        //这里开启窗口的重绘，不然窗口信息不会刷新
        this.Repaint();
    }

    void OnSelectionChange()
    {
        //当窗口出去开启状态，并且在Hierarchy视图中选择某游戏对象时调用
        //foreach (Transform t in Selection.transforms)
        //{
        //    //有可能是多选，这里开启一个循环打印选中游戏对象的名称
        //    Debug.Log("OnSelectionChange" + t.name);
        //}

        //校验是否点击了标签物体
        //if (Selection.transforms.Length == 1)
        //{
            
        //    if (Selection.activeGameObject.GetComponent<UnitGroupT1>())
        //    {
        //        List<UnitMemberT1> mems = Selection.activeGameObject.GetComponent<UnitGroupT1>().unitMembers;
        //        GameObject[] objects = new GameObject[mems.Count];
        //        for (int i = 0; i < mems.Count; i++)
        //        {
        //            objects[i] = mems[i].gameObject;
        //        }
                
        //        Selection.objects = objects;
        //        //Debug.Log(Selection.objects.Length);
        //    }
        //}

    }

    void OnDestroy()
    {
        //Debug.Log("当窗口关闭时调用");
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
    /// 旋转界面
    /// </summary>
    private void ChooseRotate()
    {
        GUILayout.Space(10);
        hasGears = GUITools.Toolbar("分级", 30, 30, hasGears, gearsToolbarText);
        GUILayout.Space(5);
        float old_angle_R = angle_R;
        angle_R = GUITools.FloatField("角度", 30, 30, angle_R);
        GUILayout.Space(5);
        if (old_angle_R != angle_R)
        {
            
        }

        axis_R = GUITools.Toolbar("旋转轴", 30, 30, axis_R, axisRToolbarTexts);

        if (hasGears == 1)
        {
            if (gearCount < 1)
            {
                gearCount = 1;
            }
            if (gearCount > 10)
            {
                gearCount = 10;
            }
            GUILayout.Space(5);
            gearCount = GUITools.IntField("级数", 30, 30, gearCount);
        }
        GUILayout.Space(10);
    }

    /// <summary>
    /// 按压界面
    /// </summary>
    private void ChoosePress()
    {
        GUILayout.Space(10);
        isUpComplete = GUITools.Toolbar("弹起", 30, 30, isUpComplete, isUpToolbarText);
        GUILayout.Space(5);
        depth = GUITools.FloatField("按压深度", 30, 30, depth);
        GUILayout.Space(5);
        axis_P = GUITools.Toolbar("按压轴", 30, 30, axis_P, axisPToolbarTexts);
        GUILayout.Space(10);
    }

    /// <summary>
    /// 旋转并按压界面
    /// </summary>
    private void ChooseRotateAndPress()
    {
        GUILayout.Space(10);
        angle_RP_R = GUITools.FloatField("角度", 30, 30, angle_RP_R);
        GUILayout.Space(5);
        axis_RP_R = GUITools.Toolbar("旋转轴", 30, 30, axis_RP_R, axisRToolbarTexts);
        GUILayout.Space(5);
        depth_RP_P = GUITools.FloatField("按压深度", 30, 30, depth_RP_P);
        GUILayout.Space(5);
        axis_RP_P = GUITools.Toolbar("按压轴", 30, 30, axis_RP_P, axisPToolbarTexts);
        GUILayout.Space(10);
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


        title3LabelStyle = new GUIStyle(mySkin.label);
        title3LabelStyle.fontSize = 18;


        partTitleStyle = new GUIStyle(EditorStyles.foldout);
        partTitleStyle.font = myFont;
        partTitleStyle.alignment = TextAnchor.MiddleLeft;
        partTitleStyle.fontStyle = FontStyle.Bold;
        partTitleStyle.fontSize = 15;

        boldLabelStyle = new GUIStyle(mySkin.label);
        boldLabelStyle.alignment = TextAnchor.MiddleLeft;
        boldLabelStyle.font = myFont;
        boldLabelStyle.fontStyle = FontStyle.Bold;

        labelStyle = new GUIStyle(mySkin.label);
        labelStyle.fontSize = 30;
    }

    /// <summary>
    /// 判断当前打开的场景和窗口是否对应
    /// </summary>
    private void CheckScenePath()
    {
        //该判断用于强制用户保存场景，否则无法锁定当前窗口，为方便调试先注释
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
            }
        //}
        
    }

    /// <summary>
    /// 创建新场景
    /// </summary>
    public void CreateNewScene(SubjectType sjType,OperateType opType)
    {
        //实例化PC和VR必备物体
        StartUICanvasGameObject = Instantiate(StartUICanvasPrefab);
        infoBoardParentTransform = Instantiate(InfoBoardParent).transform;
        unitParentTransform = Instantiate(UnitParent).transform;
        GameObject EventSystemGameObject = Instantiate(EventSystem);
        DesktopGameCanvasGameObject = Instantiate(DesktopGameCanvasPrefab);
        GameObject VRTK_SDKManagerGameObject = Instantiate(VRTK_SDKManagerPrefab);
        GameObject VRTK_ScriptsGameObject = Instantiate(VRTK_ScriptsPrefab);


        //属性赋值
        gm = Instantiate(GameManager).GetComponent<GameManagerT1>();
        gm.InitCameraGameObject(GameObject.Find("Main Camera"), VRTK_SDKManagerGameObject, VRTK_ScriptsGameObject);
        gm.subjectType = sjType;

        //操作交互方式由课件启动的时候决定，这里不在定义
        //gm.operateType = opType;

        uiCtrl = gm.GetComponent<UIControllerT1>();
        uiCtrl.InitUICanvas(StartUICanvasGameObject, DesktopGameCanvasGameObject,
            VRTK_SDKManagerGameObject.transform.GetChild(0).GetChild(3).GetChild(0).GetChild(0).gameObject,
            VRTK_SDKManagerGameObject.transform.GetChild(0).GetChild(3).GetChild(0).GetChild(3).GetChild(0).GetChild(0).gameObject);


        VRTK_SDKManagerGameObject.GetComponent<VRTK_SDKManager>().scriptAliasLeftController =
                VRTK_ScriptsGameObject.transform.Find("LeftController").gameObject;
        VRTK_SDKManagerGameObject.GetComponent<VRTK_SDKManager>().scriptAliasRightController =
            VRTK_ScriptsGameObject.transform.Find("RightController").gameObject;

        ClearIntroPart();
        ClearShowTransformPart();
        ClearUnitPart();
    }

    /// <summary>
    /// 初始化（重新载入）界面数据
    /// </summary>
    public void Init()
    {
        GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
        camera.AddComponent<PhysicsRaycaster>();
        camera.AddComponent<FreeLookT1>();
    }

    /// <summary>
    /// 清理总体介绍
    /// </summary>
    public void ClearIntroPart()
    {
        introTitleText = "";
        introContentText = "";
        introLoadtexture2D = null;
        IntroManagerWindowT1.Instance.UpdateWindow();
        Repaint();
    }

    /// <summary>
    /// 清理模型管理
    /// </summary>
    public void ClearShowTransformPart()
    {
        currentModel = null;
        modelPosition = Vector3.zero;
        modelRotation = Vector3.zero;
        modelScale = Vector3.one;
        ModelManagerWindowT1.Instance.UpdateWindow();
        Repaint();
    }

    /// <summary>
    /// 清理部件管理
    /// </summary>
    public void ClearUnitPart()
    {
        unitName = "";
        unitContent = "";
        unitColor = new Color(0.38f, 0.97f, 0.44f);
        unitLoadtexture2D = null;
        groupOperation = false;
        operationToolbarOption = 0;
        hasGears = 0;
        angle_R = 0.0f;
        axis_R = 0;
        gearCount = 1;
        isUpComplete = 0;
        depth = 0f;
        axis_P = 0;
        angle_RP_R = 0.0f; //旋转角
        axis_RP_R = 0; //旋转轴
        depth_RP_P = 0f; //按压深度
        axis_RP_P = 0; //按压轴
        unitObservePointCount = 0;
        observePointPositionList.Clear();
        observePointRotationList.Clear();
        UnitManagerWindowT1.Instance.UpdateWindow();
        Repaint();
    }

    /// <summary>
    /// 部件信息展示
    /// </summary>
    /// <param name="unitGroup"></param>
    public void ShowUnitPartInfo(UnitGroupT1 unitGroup)
    {
        unitName = unitGroup.unitName;
        unitContent = unitGroup.unitContent;
        unitColor = unitGroup.unitColor;
        unitLoadtexture2D = unitGroup.unitLoadtexture2D;
        unitObservePointCount = unitGroup.pointPositionList.Count;
        observePointPositionList = unitGroup.pointPositionList;
        observePointRotationList = unitGroup.pointRotationList;
        groupOperation = unitGroup.groupOperation;
        operationToolbarOption = unitGroup.operationToolbarOption;
        hasGears = unitGroup.hasGears;
        angle_R = unitGroup.angle_R;
        axis_R = unitGroup.axis_R;
        gearCount = unitGroup.gearCount;
        isUpComplete = unitGroup.isUpComplete;
        depth = unitGroup.depth;
        axis_P = unitGroup.axis_P;
        angle_RP_R = unitGroup.angle_RP_R;
        axis_RP_R = unitGroup.axis_RP_R;
        depth_RP_P = unitGroup.depth_RP_P;
        axis_RP_P = unitGroup.axis_RP_P;

        //observePointPositionList = unitGroup.observePointPositionList;
        if (groupOperation)
        {
            if (Selection.activeGameObject)
            {
                currentGameObjects = Selection.gameObjects;
                position_original = new Vector3[currentGameObjects.Length];
                rotation_original = new Quaternion[currentGameObjects.Length];
                for (int i = 0; i < currentGameObjects.Length; i++)
                {
                    //记录初始位置
                    position_original[i] = currentGameObjects[i].transform.localPosition;
                    rotation_original[i] = currentGameObjects[i].transform.localRotation;
                }
            }
        }
        Repaint();
    }

    /// <summary>
    /// 部件位置归位
    /// </summary>
    /// <param name="unitGroup"></param>
    public void UnitPositionToOriginal(UnitGroupT1 unitGroup)
    {
        if (groupOperation)
        {
            if (Selection.activeGameObject)
            {
                for (int i = 0; i < currentGameObjects.Length; i++)
                {
                    //记录初始位置
                    currentGameObjects[i].transform.localPosition = position_original[i];
                    currentGameObjects[i].transform.localRotation = rotation_original[i];
                }
            }
        }
    }

    /// <summary>
    /// 判断两组物体是否是由同样的物体组成
    /// </summary>
    /// <param name="a">物体数组A</param>
    /// <param name="b">物体数组B</param>
    /// <returns></returns>
    private bool GameObjectsIsSame(GameObject[] a, GameObject[] b)
    {
        if (a != null && b != null)
        {
            if (a.Length == b.Length)
            {
                for (int i = 0; i < a.Length; i++)
                {
                    if (a[i] != b[i])
                    {
                        return false;
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
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
            gm = _gm.GetComponent<GameManagerT1>();
            uiCtrl = _gm.GetComponent<UIControllerT1>();
            //Debug.Log("获取T1 gm");
        }
        else
        {
            //Debug.Log("GameManager物体不存在");
        }
    }

    /// <summary>
    /// 开始
    /// </summary>
    private void StartBox()
    {
        GUILayout.Space(10);

        bool[] b = GUITools.HorizontalButtons("课件操作", 30, 30, startBtnsText, 10);
        GUILayout.Space(5);

        for (int i = 0; i < b.Length; i++)
        {
            if (b[i])
            {
                switch (i)
                {
                    case 0:
                        CreateWindow<SetNameWindowT1>(true, "课件名称", new Vector2(350, 100), new Vector2(350, 100),
                            new Rect(600, 400, 350, 100));
                        break;
                    case 1:
                        CreateWindow<ImportPlaneModelWindowT1>(true, "导入飞机/场景模型", new Vector2(350, 100), new Vector2(350, 100),
                            new Rect(600, 400, 350, 100));
                        break;
                    case 2:
                        CreateWindow<SettingsWindowT1>(true, "课件设置", new Vector2(350, 300), new Vector2(350, 300),
                            new Rect(600, 400, 350, 100));
                        break;
                    case 3:
                        EditorApplication.ExecuteMenuItem("Assets/Import New Asset...");
                        break;
                    case 4:
                        EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
                        break;
                }
            }
        }
    }

    /// <summary>
    /// 创建EditorWindow
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="utility"></param>
    /// <param name="title"></param>
    /// <param name="minSize"></param>
    /// <param name="maxSize"></param>
    /// <param name="position"></param>
    private void CreateWindow<T>(bool utility, string title, Vector2 minSize, Vector2 maxSize, Rect position) where T : EditorWindow
    {
        T tWindow = EditorWindow.GetWindow<T>(utility, title);
        tWindow.minSize = minSize;
        tWindow.maxSize = maxSize;
        tWindow.position = position;
        tWindow.Show();
    }

    /// <summary>
    /// 总体介绍
    /// </summary>
    private void IntroduceBox()
    {
        EditorGUILayout.BeginHorizontal();
        //左
        EditorGUILayout.BeginVertical(GUILayout.Width(position.width * 0.6f), GUILayout.Height(250));
        {
            GUILayout.Space(5);
            GUILayout.Label("属性编辑", title3LabelStyle);
            GUILayout.Space(10);
            introTitleText = GUITools.TextField("标题", 30, 30, introTitleText);
            GUILayout.Space(5);
            introContentText = GUITools.TextField("文字", 30, 30, introContentText);
            GUILayout.Space(5);
            introLoadtexture2D = GUITools.ObjectField<Texture2D>("图片", 30, 30, introLoadtexture2D, typeof(Texture2D),
                true);
            GUILayout.Space(110);
            //introBtnsDisable[2] = !IntroManagerWindowT1.Instance;
            bool[] b = GUITools.HorizontalButtons(30, 30, introBtnsText, 5, introBtnsDisable);
            GUILayout.Space(10);
            if (b[0])
            {
                OnAddBoardButtonClick();
            }
            if (b[1])
            {
                OnUpdateBoardButtonClick();
            }
        }
        EditorGUILayout.EndVertical();
        //右
        EditorGUILayout.BeginVertical(GUILayout.Width(position.width * 0.4f - 25), GUILayout.Height(250));
        {
            IntroManagerWindowT1.Instance.OnGUI();
        }
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();
    }

    /// <summary>
    /// 添加提示板
    /// </summary>
    private void OnAddBoardButtonClick()
    {
        GameObject infoBoardGameObject = Instantiate(InfoBoardGameObject, InfoBoardParentTransform);
        IntroduceBoardT1 board = infoBoardGameObject.GetComponent<IntroduceBoardT1>();
        board.introTitleText = introTitleText;
        infoBoardGameObject.name = introTitleText;
        board.introContentText = introContentText;
        board.introLoadtexture2D = introLoadtexture2D;
        gm.introBoard.Add(board);
        
        if (IntroManagerWindowT1.Instance)
        {
            IntroManagerWindowT1.Instance.UpdateWindow();
            IntroManagerWindowT1.Instance.selGridInt = -1;
            IntroManagerWindowT1.Instance.Repaint();
        }
        ClearIntroPart();
        GUI.FocusControl("");
    }

    /// <summary>
    /// 更新提示板
    /// </summary>
    private void OnUpdateBoardButtonClick()
    {
        if (IntroManagerWindowT1.Instance.selGridInt >= 0)
        {
            gm.introBoard[IntroManagerWindowT1.Instance.selGridInt].introTitleText = introTitleText;
            gm.introBoard[IntroManagerWindowT1.Instance.selGridInt].introContentText = introContentText;
            gm.introBoard[IntroManagerWindowT1.Instance.selGridInt].introLoadtexture2D = introLoadtexture2D;
            gm.introBoard[IntroManagerWindowT1.Instance.selGridInt].gameObject.name = introTitleText;
        }
        IntroManagerWindowT1.Instance.UpdateWindow();
        IntroManagerWindowT1.Instance.selGridInt = -1;
        IntroManagerWindowT1.Instance.Repaint();

        ClearIntroPart();
        GUI.FocusControl("");
    }

    /// <summary>
    /// 模型信息
    /// </summary>
    private void TransformBox()
    {
        EditorGUILayout.BeginHorizontal();
        //左
        EditorGUILayout.BeginVertical(GUILayout.Width(position.width *0.6f), GUILayout.Height(250));
        {
            GUILayout.Space(5);
            GUILayout.Label("属性编辑", title3LabelStyle);
            GUILayout.Space(10);
            string str = "模型名称：";
            if (currentModel)
            {
                str += currentModel.name;
            }
            GUITools.Label(str, 30, 30);
            GUILayout.Space(10);
            if (currentModel)
            {
                modelPosition = currentModel.transform.position;
                modelRotation = currentModel.transform.eulerAngles;
                modelScale = currentModel.transform.localScale;
            }
            modelPosition = GUITools.Vector3Field("位置", 30, 30, modelPosition, GUILayout.Height(20));
            modelRotation = GUITools.Vector3Field("旋转", 30, 30, modelRotation, GUILayout.Height(20));
            modelScale = GUITools.Vector3Field("缩放", 30, 30, modelScale, GUILayout.Height(10));

            if (currentModel)
            {
                currentModel.transform.position = modelPosition;
                currentModel.transform.eulerAngles = modelRotation;
                currentModel.transform.localScale = modelScale;
                Repaint();
            }
            GUILayout.Space(5);
        }
        EditorGUILayout.EndVertical();
        //右
        EditorGUILayout.BeginVertical(GUILayout.Width(position.width *0.4f - 25), GUILayout.Height(250));
        {
            ModelManagerWindowT1.Instance.OnGUI();
        }
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();
    }

    /// <summary>
    /// 部件信息
    /// </summary>
    private void UnitBox()
    {
        EditorGUILayout.BeginHorizontal();
        //左
        EditorGUILayout.BeginVertical(GUILayout.Width(position.width * 0.6f), GUILayout.Height(250));
        {
            GUILayout.Space(5);
            GUILayout.Label("属性编辑", title3LabelStyle);
            GUILayout.Space(10);
            unitName = GUITools.TextField("部件名称", 30, 30, unitName);
            GUILayout.Space(5);
            unitColor = GUITools.ColorField("颜色变化", 30, 30, unitColor);
            GUILayout.Space(5);
            unitContent = GUITools.TextField("文字说明", 30, 30, unitContent);
            GUILayout.Space(5);
            unitLoadtexture2D = GUITools.ObjectField<Texture2D>("图片说明", 30, 30, unitLoadtexture2D, typeof(Texture2D), true);
            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            {
                GUILayout.Space(30);
                EditorGUILayout.PrefixLabel("观察点");
                EditorGUI.BeginDisabledGroup(unitObservePointCount==5);
                if (GUILayout.Button("添加观察点"))
                {
                    unitObservePointCount++;
                    GUI.FocusControl("");
                }
                EditorGUI.EndDisabledGroup();
                GUILayout.Space(30);
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(5);
            if (unitObservePointCount < 0) 
                unitObservePointCount = 0;
            if (unitObservePointCount > 5) 
                unitObservePointCount = 5;

            observePointPositionList = GUITools.ObservePointListPanel(observePointPositionList, observePointRotationList, unitObservePointCount);
            unitObservePointCount = observePointPositionList.Count;
            GUILayout.Space(5);
            bool old_groupOperation = groupOperation;
            groupOperation = GUITools.Toggle("添加操作信息", 30, 30, groupOperation);
            CheckOperationBool(old_groupOperation, groupOperation);
            GUILayout.Space(5);

            if (groupOperation)
            {
                isLockGameObjects = true;
                int old = operationToolbarOption;
                operationToolbarOption = GUITools.Toolbar(30, 30, operationToolbarOption,
                    operationToolbarTexts);

                //改变选择
                if (old != operationToolbarOption)
                {
                    GUI.FocusControl("");
                    for (int i = 0; i < currentGameObjects.Length; i++)
                    {
                        currentGameObjects[i].transform.localPosition = position_original[i];
                        currentGameObjects[i].transform.localRotation = rotation_original[i];
                    }
                }

                switch (operationToolbarOption)
                {
                    case 0: //旋转
                        ChooseRotate();
                        break;
                    case 1: //按压
                        ChoosePress();
                        break;
                    case 2: //旋转并按压
                        ChooseRotateAndPress();
                        break;
                }
                GUILayout.Space(5);
            }
            else
            {
                GUILayout.Space(145);
            }
            //unitBtnsDisable[2] = !UnitManagerWindowT1.Instance;
            bool[] b = GUITools.HorizontalButtons(30, 30, unitBtnsText, 10, unitBtnsDisable);
            GUILayout.Space(10);
            if (b[0])
            {
                OnAddUnitButtonClick();
            }
            if (b[1])
            {
                OnUpdateUnitButtonClick();
            }
        }
        EditorGUILayout.EndVertical();
        //右
        EditorGUILayout.BeginVertical(GUILayout.Width(position.width * 0.4f - 25), GUILayout.Height(355));
        {
            UnitManagerWindowT1.Instance.OnGUI();
        }
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();
    }

    /// <summary>
    /// 检查是否添加操作信息
    /// </summary>
    /// <param name="oldBool"></param>
    /// <param name="newBool"></param>
    private void CheckOperationBool(bool oldBool,bool newBool)
    {
        //从无到有
        if (!oldBool && newBool)
        {
            if (Selection.activeGameObject)
            {
                currentGameObjects = Selection.gameObjects;
                position_original = new Vector3[currentGameObjects.Length];
                rotation_original = new Quaternion[currentGameObjects.Length];
                for (int i = 0; i < currentGameObjects.Length; i++)
                {
                    //记录初始位置
                    position_original[i] = currentGameObjects[i].transform.localPosition;
                    rotation_original[i] = currentGameObjects[i].transform.localRotation;
                }
            }
            else
            {
                EditorUtility.DisplayDialog("提示", "请选择部件!", "确定");
                groupOperation = false;
            }
        }
        //从有到无
        if (oldBool && !newBool)
        {
            if (Selection.activeGameObject)
            {
                isLockGameObjects = false;
                currentGameObjects = Selection.gameObjects;
                for (int i = 0; i < currentGameObjects.Length; i++)
                {
                    //记录初始位置
                    currentGameObjects[i].transform.localPosition = position_original[i];
                    currentGameObjects[i].transform.localRotation = rotation_original[i];
                }
                position_original = new Vector3[0];
                rotation_original = new Quaternion[0];
            }
        }
    }

    /// <summary>
    /// 添加部件信息
    /// </summary>
    private void OnAddUnitButtonClick()
    {
        if (Selection.gameObjects.Length != 0)
        {
            bool canAdd = true;
            foreach (GameObject go in Selection.gameObjects)
            {
                if (go.GetComponent<UnitMemberT1>())
                {
                    canAdd = false;
                }
            }
            if (canAdd)
            {
                GameObject unitLabelGameObject = Instantiate(UnitGameObject, unitParentTransform, UnitParentTransform);
                UnitGroupT1 unitGroup = unitLabelGameObject.GetComponent<UnitGroupT1>();

                List<UnitMemberT1> mems = new List<UnitMemberT1>();
                mems.Clear();
                foreach (GameObject go in Selection.gameObjects)
                {
                    mems.Add(go.AddComponent<UnitMemberT1>());
                    go.AddComponent<MeshCollider>();
                }
                //关于button
                GameObject button = Instantiate(unitButton);
                button.transform.parent =
                    GameObject.FindGameObjectWithTag("GameUI_PC_T1").transform.GetChild(1).GetChild(1).GetChild(0).GetChild(0);
                button.transform.localScale = new Vector3(1, 1, 1);
                button.transform.GetChild(0).GetComponent<Text>().text = unitName;
                button.GetComponent<ForUnitButtonT1>().group = unitGroup;
               
                unitGroup.SaveUnitGroupValue(unitName, unitContent, unitColor, unitLoadtexture2D, 
                    groupOperation, operationToolbarOption, observePointPositionList, observePointRotationList, hasGears,
                    angle_R, axis_R, gearCount, isUpComplete, depth, axis_P, angle_RP_R, axis_RP_R, depth_RP_P,
                    axis_RP_P, mems, button);
                
                unitGroup.MemberInfoSave();
                gm.unitGroups.Add(unitGroup);

                if (UnitManagerWindowT1.Instance)
                {
                    UnitManagerWindowT1.Instance.UpdateWindow();
                    UnitManagerWindowT1.Instance.selGridInt = -1;
                    UnitManagerWindowT1.Instance.Repaint();
                }

                if (isLockGameObjects)
                {
                    isLockGameObjects = false;
                    for (int i = 0; i < currentGameObjects.Length; i++)
                    {
                        currentGameObjects[i].transform.localPosition = position_original[i];
                        currentGameObjects[i].transform.localRotation = rotation_original[i];
                    }
                    position_original = new Vector3[0];
                    rotation_original = new Quaternion[0];
                }

                ClearUnitPart();

                GUI.FocusControl("");
            }
            else
            {
                EditorUtility.DisplayDialog("提示", "存在已编辑的部件", "确定");
            }

        }
        else
        {
            
        }
    }

    /// <summary>
    /// 更新部件信息
    /// </summary>
    private void OnUpdateUnitButtonClick()
    {
        if (UnitManagerWindowT1.Instance.selGridInt >= 0)
        {
            gm.unitGroups[UnitManagerWindowT1.Instance.selGridInt].SaveUnitGroupValue(unitName, unitContent, unitColor,
                unitLoadtexture2D, groupOperation, operationToolbarOption, observePointPositionList,
                observePointRotationList, hasGears, angle_R, axis_R, gearCount, isUpComplete, depth, axis_P, angle_RP_R,
                axis_RP_R, depth_RP_P, axis_RP_P);
            gm.unitGroups[UnitManagerWindowT1.Instance.selGridInt].MemberInfoSave();

            UnitManagerWindowT1.Instance.UpdateWindow();
            UnitManagerWindowT1.Instance.selGridInt = -1;
            UnitManagerWindowT1.Instance.Repaint();

            if (isLockGameObjects)
            {
                isLockGameObjects = false;
                for (int i = 0; i < currentGameObjects.Length; i++)
                {
                    currentGameObjects[i].transform.localPosition = position_original[i];
                    currentGameObjects[i].transform.localRotation = rotation_original[i];
                }
                position_original = new Vector3[0];
                rotation_original = new Quaternion[0];
            }

            ClearUnitPart();
            GUI.FocusControl("");
        }
    }

}
