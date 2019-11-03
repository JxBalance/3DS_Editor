using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UI;



public class AddDisassemblyWindow : EditorWindow
{
    public static AddDisassemblyWindow _instance;
    //GUI皮肤

    public GUISkin meSkin;
    public GameObject DAunitButton;

    public List<string> actuatorModelName;

    private GameManagerT2 gm;
    private UIControllerT2 uiCtrl;

    private MonoScript addScript;

    private TextAsset motionTrailFile;

    private string mechanismName;

    private Transform part;
    private GameObject partOb;
    private string partName;

    private int currentModelIndex = -1;
    private int toolIndex = -1;

    private Vector3 partStartPos;
    private Vector3 partEndPos;

    public List<DisassemblyRoute> disassemblyRouteList;

    void Awake()
    {
        _instance = this;
        actuatorModelName = new List<string>();
        disassemblyRouteList = new List<DisassemblyRoute>();
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManagerT2>();
        uiCtrl = gm.GetComponent<UIControllerT2>();
    }

    void OnGUI()
    {
        GUILayout.Space(20);
        /// <summary>
        /// 选择机构
        /// </summary>
        GUILayout.BeginHorizontal();
        {
            int mechModelCount = gm.mechanismModels.Count;

            string[] modelsName = new string[mechModelCount];
            for (int i = 0; i < mechModelCount; i++)
            {
                modelsName[i] = gm.mechanismModels[i].name;
            }
            EditorGUILayout.PrefixLabel("机构模型");
            currentModelIndex = EditorGUILayout.Popup(currentModelIndex, modelsName, EditorStyles.popup, GUILayout.Height(25));
        }
        GUILayout.EndHorizontal();


        GUILayout.Space(20);
        /// <summary>
        /// 选择零件
        /// </summary>
        GUILayout.BeginHorizontal();
        {
            EditorGUILayout.PrefixLabel("当前选择零件");

            Transform[] selected = Selection.transforms;
            GameObject[] selectedob = Selection.gameObjects;

            if (selected.Length == 0)
            {
                GUILayout.Label("无");
            }
            else
            {
                string selected_part_name = selected[0].name;
                string show_name;

                if (selected_part_name.Length > 15)
                {
                    show_name = selected_part_name.Substring(0, 10) + "...";
                }
                else
                {
                    show_name = selected_part_name;
                }

                GUILayout.Label(show_name);
            }

            if (GUILayout.Button("确定", GUILayout.Width(position.width * 0.25f - 5)))
            {
                if (selected.Length == 0)
                {
                    //提示
                    EditorUtility.DisplayDialog("提示", "未选择驱动零件", "确定");
                }
                else
                {
                    part = selected[0];
                    partOb = selectedob[0];
                    partName = selected[0].name;
                    partStartPos = part.position;
                }
            }

        }
        GUILayout.EndHorizontal();

        /// <summary>
        /// 选择拆装工具
        /// </summary>
        GUILayout.Space(20);
        GUILayout.BeginHorizontal();
        {
            int toolNum = 3;
            string[] toolsName = new string[toolNum];
            toolsName[0] = "手";
            toolsName[1] = "螺丝刀";
            toolsName[2] = "扳手";

            EditorGUILayout.PrefixLabel("选择拆装工具");
            toolIndex = EditorGUILayout.Popup(toolIndex, toolsName, EditorStyles.popup, GUILayout.Height(25));
        }
        GUILayout.EndHorizontal();

        GUILayout.Space(10);
        /**/
        GUILayout.BeginHorizontal();
        {
            EditorGUILayout.PrefixLabel("选择拆装模式");
            addScript = EditorGUILayout.ObjectField(addScript, typeof(MonoScript), true) as MonoScript;
        }


        GUILayout.EndHorizontal();

        /**/
        GUILayout.Space(20);
        /// <summary>
        /// 定义拆装轨迹
        /// </summary>
        GUILayout.BeginHorizontal();
        {
            GUILayout.Space(position.width * 0.1f);

            if (GUILayout.Button("从当前位置开始", GUILayout.Width(position.width * 0.35f)))
            {
                Transform mechanismModel = gm.mechanismModels[currentModelIndex].transform;
                partStartPos = part.position;

            }

            GUILayout.Space(position.width * 0.1f);

            if (GUILayout.Button("以当前位置结束", GUILayout.Width(position.width * 0.35f)))
            {
                Transform mechanismModel = gm.mechanismModels[currentModelIndex].transform;
                partEndPos = part.position;
            }
        }
        GUILayout.EndHorizontal();


        GUILayout.Space(10);
        /// <summary>
        /// 显示轨迹起止点
        /// </summary>
        /// 
        if (part != null)
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.Space(position.width * 0.2f);
                GUILayout.Label("起始点");
                GUILayout.Space(position.width * 0.2f);
                GUILayout.Label("终止点");
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            {
                GUILayout.Space(position.width * 0.1f);
                GUILayout.Label("X:");
                GUILayout.Space(position.width * 0.1f);
                GUILayout.Label(partStartPos.x.ToString());
                GUILayout.Label("X:");
                GUILayout.Space(position.width * 0.1f);
                GUILayout.Label(part.position.x.ToString());
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            {
                GUILayout.Space(position.width * 0.1f);
                GUILayout.Label("Y:");
                GUILayout.Space(position.width * 0.1f);
                GUILayout.Label(partStartPos.y.ToString());
                GUILayout.Label("Y:");
                GUILayout.Space(position.width * 0.1f);
                GUILayout.Label(part.position.y.ToString());
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            {
                GUILayout.Space(position.width * 0.1f);
                GUILayout.Label("Z:");
                GUILayout.Space(position.width * 0.1f);
                GUILayout.Label(partStartPos.z.ToString());
                GUILayout.Label("Z:");
                GUILayout.Space(position.width * 0.1f);
                GUILayout.Label(part.position.z.ToString());
            }
            GUILayout.EndHorizontal();
        }

        GUILayout.Space(10);
        GUILayout.BeginHorizontal();
        {
            GUILayout.Space(position.width * 0.2f);

            if (GUILayout.Button("添加轨迹", GUILayout.Width(position.width * 0.2f)))
            {
                DisassemblyRoute nr = new DisassemblyRoute();
                nr.StartPos = partStartPos;
                nr.EndPos = partEndPos;
                disassemblyRouteList.Add(nr);
            }

            GUILayout.Space(position.width * 0.2f);

            if (GUILayout.Button("取消", GUILayout.Width(position.width * 0.2f)))
            {
                part.position = partStartPos;
            }

        }
        GUILayout.EndHorizontal();

        GUILayout.Space(20);
        GUILayout.BeginHorizontal();
        {
            GUILayout.Space(position.width * 0.375f);
            if (GUILayout.Button("保存拆装流程", GUILayout.Width(position.width * 0.25f)))
            {
                //add controler
                Type monoScriptClass = addScript.GetClass();
                gm.mechanismModels[currentModelIndex].AddComponent(monoScriptClass);

                DisassemblyStep ds = new DisassemblyStep();
                ds.PartName = partName;
                ds.ToolIndex = toolIndex;
                ds.DisassemblyRouteList = disassemblyRouteList;
                gm.disassemblySteps.Add(ds);
                part.position = disassemblyRouteList[0].StartPos;
                gm.toolIndexList.Add(toolIndex);
                gm.startPosList.Add(partStartPos);
                gm.endPosList.Add(partEndPos);
                gm.partList.Add(partOb);

                //加入控制列表 TODO
                //Transform PCparent = GameObject.FindGameObjectWithTag("PC_UI_T2").transform.GetChild(1).GetChild(1).GetChild(0);
                Transform PCparent = uiCtrl.GamePanelCanvas_PC.transform.GetChild(1).GetChild(1).GetChild(0);
                //Transform VRparent = GameObject.FindGameObjectWithTag("VR_UI_T2").transform.GetChild(1).GetChild(1).GetChild(0);
                Transform VRparent = uiCtrl.GamePanelCanvas_VR.transform.GetChild(1).GetChild(1).GetChild(0);
                GameObject newButtonPC = Instantiate(DAunitButton);
                newButtonPC.transform.SetParent(PCparent);
                newButtonPC.transform.GetChild(0).GetComponent<Text>().text = partName;
                newButtonPC.transform.GetComponent<DAUnitButtonT2>().setDisassemblyId(gm.disassemblySteps.Count);

                GameObject newButtonVR = Instantiate(DAunitButton);    
                newButtonVR.transform.SetParent(VRparent,false);
                newButtonVR.transform.GetChild(0).GetComponent<Text>().text = partName;
                newButtonVR.transform.GetComponent<DAUnitButtonT2>().setDisassemblyId(gm.disassemblySteps.Count);



                Close();
            }
        }
        GUILayout.EndHorizontal();
    }
}
