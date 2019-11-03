using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;


public class OpenSwitchWindow : EditorWindow {

    public static OpenSwitchWindow _instance;

    //GUI皮肤
    public GUISkin mySkin;
    public Font myFont;
    private GUIStyle titleLabelStyle, subtitleLabelStyle, toolBarStyle, buttonStyle,
                     smallLabelStyle, partTitleStyle, boldLabelStyle;
        
    //开关状态变量
    private bool[] switchTurn = { true, true, true, true };
    private bool[] switchOn = { true, true, true, true };

    //开关集合变量
    GameObject switchGrp;

    private bool isShowStartPart = true;
    private bool isShowAttPart = true;
    private bool isShowSwitchPart = true;

    //开关名变量
    private string switchText = "";
    private char[] switchCH = { '开', '关' };  
    
    public void Awake()
    {
        _instance = this;
    }
    
    void OnGUI()
    {
        if(!_instance)
        {
            _instance = this;
        }
        //SwitchPage();
        //switchGrp = GameObject.Find("switch");
        //switchGrp.GetComponent<SwitchCtrl>().switchStatus();
    }

    //气液体流动演示编辑器窗口
    private void SwitchPage()
    {
        InitGUIStyle();

        #region 标题
        GUILayout.Space(position.height * 0.05f);
        GUILayout.Label("气/液体流动演示编辑器", subtitleLabelStyle);
        GUILayout.Space(position.height * 0.05f);
        #endregion

        #region 开始
        isShowStartPart = EditorGUILayout.Foldout(isShowStartPart, "开始", partTitleStyle); // 定义折叠菜单
        if (isShowStartPart)
        {
            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            {
                GUILayout.Space(position.width * 0.1f);
                EditorGUILayout.PrefixLabel("课件操作");
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            {
                GUILayout.Space(position.width * 0.1f);
                if (GUILayout.Button("重置课件", GUILayout.Width(position.width * 0.2f)))
                {
                    if (EditorUtility.DisplayDialog("提示", "重置课件将会抹去当前编辑的所有信息，确定要重置课件吗？", "确定", "取消"))
                    {

                    }
                }
                GUILayout.Space(position.width * 0.05f);
                if (GUILayout.Button("导入管道/场景模型", GUILayout.Width(position.width * 0.3f)))
                {

                }
                GUILayout.Space(position.width * 0.05f);
                if (GUILayout.Button("导入素材", GUILayout.Width(position.width * 0.2f)))
                {
                    EditorApplication.ExecuteMenuItem("Assets/Import New Asset...");
                }
            }
            GUILayout.EndHorizontal();
        }
        GUILayout.Space(position.height * 0.05f);
        #endregion

        #region 流动属性
        isShowAttPart = EditorGUILayout.Foldout(isShowAttPart, "流动属性", partTitleStyle); // 定义折叠菜单
        if (isShowAttPart)
        {
            EditorGUILayout.BeginHorizontal();
            {
                GUILayout.Space(position.width * 0.35f);
                if (GUILayout.Button("流动液体", GUILayout.Height(20), GUILayout.Width(position.width * 0.3f)))
                {

                }
            }
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(10);

            EditorGUILayout.BeginHorizontal();
            {
                GUILayout.Space(position.width * 0.35f);
                if (GUILayout.Button("流动气体", GUILayout.Height(20), GUILayout.Width(position.width * 0.3f)))
                {

                }
            }
            EditorGUILayout.EndHorizontal();
        }
        GUILayout.Space(position.height * 0.05f);
        #endregion

        #region 开关设置
        isShowSwitchPart = EditorGUILayout.Foldout(isShowSwitchPart, "开关设置", partTitleStyle); // 定义折叠菜单
        if (isShowSwitchPart)
        {
            EditorGUILayout.BeginHorizontal();
            {
                GUILayout.Space(position.width * 0.35f);
                if (GUILayout.Button("全部打开", GUILayout.Height(20), GUILayout.Width(position.width * 0.3f)))
                {
                    switchRotation(2);
                }
            }
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(10);

            EditorGUILayout.BeginHorizontal();
            {
                GUILayout.Space(position.width * 0.35f);
                if (GUILayout.Button("全部关闭", GUILayout.Height(20), GUILayout.Width(position.width * 0.3f)))
                {
                    switchRotation(3);
                }
            }
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(20);

            #region 选中开关
            EditorGUILayout.BeginHorizontal();
            {
                GUILayout.Space(position.width * 0.28f);

                //获取选中开关信息
                switchText = GUILayout.TextField(switchText, GUILayout.Height(20), GUILayout.Width(position.width * 0.2f));
                GUILayout.Space(position.width * 0.04f);

                int nums = 0;

                //无选中开关
                if (switchText == "")
                {
                    GUILayout.Button("", GUILayout.Height(20), GUILayout.Width(position.width * 0.2f));
                }
                //有选中开关
                else
                {
                    //分解开关名，获得对应开关状态
                    nums = int.Parse(switchText.Trim(switchCH)) - 1;

                    //判断按钮状状态，"开"或"关"
                    if (switchTurn[nums] == true)
                    {
                        if (GUILayout.Button("关", GUILayout.Height(20), GUILayout.Width(position.width * 0.2f)))
                        {
                            switchTurn[nums] = false;
                            switchRotation(0);
                        }
                    }
                    else
                    {
                        if (GUILayout.Button("开", GUILayout.Height(20), GUILayout.Width(position.width * 0.2f)))
                        {
                            switchTurn[nums] = true;
                            switchRotation(1);
                        }
                    }
                }
                
            }
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(20);
            #endregion
        }
        #endregion


    }

    //旋转开关，变量a为目标开关状态，0关，1开，2全部打开，3全部关闭
    private void switchRotation(int a)
    {
        //旋转开关变量
        GameObject rotSwitch = null;

        //目前开关1至4部件:Cylinder011,Cylinder010,Cylinder007,Cylinder006


        if (a == 2)
        {
            //全部打开
            rotSwitch = GameObject.Find("Cylinder011");
            rotSwitch.transform.eulerAngles = new Vector3(0f, -90f, 0f);
            rotSwitch = GameObject.Find("Cylinder010");
            rotSwitch.transform.eulerAngles = new Vector3(0f, -90f, 0f);
            rotSwitch = GameObject.Find("Cylinder007");
            rotSwitch.transform.eulerAngles = new Vector3(0f, -90f, 0f);
            rotSwitch = GameObject.Find("Cylinder006");
            rotSwitch.transform.eulerAngles = new Vector3(0f, -90f, 0f);
            for (int i=0;i<4;i++)
            {
                switchTurn[i] = true;
            }

            //调用开关全部打开函数
            switchGrp.GetComponent<SwitchCtrl>().openAllSwitch();
        }
        else if (a == 3)
        {
            //全部关闭
            rotSwitch = GameObject.Find("Cylinder011");
            rotSwitch.transform.eulerAngles = new Vector3(0f, 0f, 0f);
            rotSwitch = GameObject.Find("Cylinder010");
            rotSwitch.transform.eulerAngles = new Vector3(0f, 0f, 0f);
            rotSwitch = GameObject.Find("Cylinder007");
            rotSwitch.transform.eulerAngles = new Vector3(0f, 0f, 0f);
            rotSwitch = GameObject.Find("Cylinder006");
            rotSwitch.transform.eulerAngles = new Vector3(0f, 0f, 0f);
            for (int i = 0; i < 4; i++)
            {
                switchTurn[i] = false;
            }

            //调用开关全部关闭函数
            switchGrp.GetComponent<SwitchCtrl>().closeAllSwitch();
        }
        else
        {
            //判断旋转的开关，通过判断读取的选中物体信息
            switch (switchText)
            {
                case ("开关1"):
                    rotSwitch = GameObject.Find("Cylinder011");
                    switchGrp.GetComponent<SwitchCtrl>().setPipe(1);  //setPipe()函数控制水管的流通
                    break;
                case ("开关2"):
                    rotSwitch = GameObject.Find("Cylinder010");
                    switchGrp.GetComponent<SwitchCtrl>().setPipe(2);
                    break;
                case ("开关3"):
                    rotSwitch = GameObject.Find("Cylinder007");
                    switchGrp.GetComponent<SwitchCtrl>().setPipe(3);
                    break;
                case ("开关4"):
                    rotSwitch = GameObject.Find("Cylinder006");
                    switchGrp.GetComponent<SwitchCtrl>().setPipe(4);
                    break;
            }

            if (a == 0)
            {
                rotSwitch.transform.eulerAngles = new Vector3(0f, 0f, 0f);      //打开
            }
            else
            {
                rotSwitch.transform.eulerAngles = new Vector3(0f, -90f, 0f);    //关闭
            }

        }        

    }

    //初始化GUI格式
    private void InitGUIStyle()
    {
        titleLabelStyle = new GUIStyle(mySkin.label);
        titleLabelStyle.fontSize = 30;
        subtitleLabelStyle = new GUIStyle(mySkin.label);
        subtitleLabelStyle.fontSize = 20;
        smallLabelStyle = new GUIStyle(mySkin.label);
        smallLabelStyle.fontSize = 10;

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

    }

    //控制水管流通
    private void pipeCtrl(int a)
    {
        GameObject pipeE = GameObject.Find("pipeExit");
        GameObject pipea = GameObject.Find("pipeA");
        GameObject pipeb = GameObject.Find("pipeB");

        a -= 1;
        if (switchOn[a] == true)
        {
            switchOn[a] = false;
        }
        else
        {
            switchOn[a] = true;
        }

        if (switchOn[0] == false || switchOn[2] == false)
        {
            pipea.SetActive(true);
            if (switchOn[1] == false || switchOn[3] == false)
            {
                pipeE.SetActive(true);
            }
        }
        else
        {
            pipea.SetActive(false);
            pipeE.SetActive(false);
        }
        if (switchOn[1] == false || switchOn[3] == false)
        {
            pipeb.SetActive(true);
        }
        else
        {
            pipeb.SetActive(false);
            pipeE.SetActive(false);
        }

    }
    
    //读取选中物体信息并显示
    private void ShowSelectedSwitch(GameObject sel)
    {
        switch (sel.name)
        {
            case "Tube031":
                switchText = "开关1";
                break;
            case "Tube032":
                switchText = "开关2";
                break;
            case "Tube025":
                switchText = "开关3";
                break;
            case "Tube026":
                switchText = "开关4";
                break;
        }

        //Debug.Log(switchText);
        EditorWindow.GetWindow<OpenSwitchWindow>(false, "水流控制窗口");

    }
    

    // Use this for initialization
    void Start () {

    }

    string switchName = "";
    // Update is called once per frame
    void Update () {

        //获取选中物体信息
        GameObject selObj = Selection.activeGameObject;
        if(selObj == null)
        {

        }
        else if (selObj.name != switchName)
        {
            switchName = selObj.name;
            if (selObj.name == "Tube031" || selObj.name == "Tube032" ||
                selObj.name == "Tube025" || selObj.name == "Tube026")
            {
                ShowSelectedSwitch(selObj);
            }
            else
            {
                switchText = "";
            }
        }

    }

}