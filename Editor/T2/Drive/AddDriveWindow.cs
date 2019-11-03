using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UI;

public class AddDriveWindow : EditorWindow
{
    public static AddDriveWindow _instance;
    //GUI皮肤
    public GUISkin meSkin;
    public GameObject unitButton;

    public List<string> actuatorModelName;

    private GameManagerT2 gm;
    private UIControllerT2 uiCtrl;

    private MonoScript addScript;

    private TextAsset motionTrailFile;

    private string mechanismName;
    private int currentModelIndex = -1;

    void Awake()
    {
        _instance = this;
        actuatorModelName = new List<string>();
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
        GUILayout.Space(10);

        /// <summary>
        /// 添加驱动零件
        /// </summary>
        GUILayout.BeginHorizontal();
        {
            EditorGUILayout.PrefixLabel("当前选择零件");

            Transform[] selected = Selection.transforms;

            if(selected.Length == 0)
            {
                GUILayout.Label("无");
            }
            else
            {
                string selected_part_name = selected[0].name;
                string show_name;
                if(selected.Length == 1)
                {
                    if(selected_part_name.Length>15)
                    {
                        show_name = selected_part_name.Substring(0, 10) + "...";
                    }
                    else
                    {
                        show_name = selected_part_name;
                    }
                }
                else
                {
                    if (selected_part_name.Length > 15)
                    {
                        show_name = selected_part_name.Substring(0, 10)+ "..." + "等共" + selected.Length.ToString() + "个零件";
                    }
                    else
                    {
                        show_name = selected_part_name + "等共" + selected.Length.ToString() + "个零件";
                    }
                }
                GUILayout.Label(show_name);
            }
            
            if (GUILayout.Button("添加驱动零件", GUILayout.Width(position.width * 0.25f - 5)))
            {
                if(selected.Length == 0)
                {
                    //提示
                    EditorUtility.DisplayDialog("提示", "未选择驱动零件", "确定");
                }
                else
                {
                    for(int i = 0 ; i < selected.Length ; i++)
                    {
                        actuatorModelName.Add(selected[i].name);
                    }
                }
            }
        }
        GUILayout.EndHorizontal();
        /**/
        GUILayout.Space(10);

        GUILayout.BeginHorizontal();
        {
            EditorGUILayout.PrefixLabel("已选驱动零件");
            Vector2 scrollPosition = new Vector2(200, 200);
            GUILayout.BeginScrollView(scrollPosition,GUILayout.Width(200), GUILayout.Height(200));
            {
                for (int i = 0; i < actuatorModelName.Count; i++)
                {
                    GUILayout.Label(actuatorModelName[i]);
                }
            }
            GUILayout.EndScrollView();
        }
        GUILayout.EndHorizontal();
        GUILayout.Space(10);
        /**/
        GUILayout.BeginHorizontal();
        {
            EditorGUILayout.PrefixLabel("选择控制模式");
            addScript = EditorGUILayout.ObjectField(addScript, typeof(MonoScript), true) as MonoScript;
        }
        GUILayout.EndHorizontal();
        /**/

        GUILayout.Space(10);


        GUILayout.BeginHorizontal();
        {
            EditorGUILayout.PrefixLabel("加载运动轨迹");
            motionTrailFile =  EditorGUILayout.ObjectField(motionTrailFile, typeof(TextAsset), true) as TextAsset; 
        }
        GUILayout.EndHorizontal();
        /**/

        GUILayout.Space(10);

        /**/
        GUILayout.BeginHorizontal();
        {
            GUILayout.Space(position.width * 0.5f);
            if (GUILayout.Button("添加", GUILayout.Width(position.width * 0.25f-5)))
            {
                if (addScript != null)
                {
                    if (currentModelIndex >= 0)
                    {
                        //add controler
                        Type monoScriptClass = addScript.GetClass();
                        gm.mechanismModels[currentModelIndex].AddComponent(monoScriptClass);
                        gm.mechanismModels[currentModelIndex].GetComponent<MultiControlerBase>().setMotionTrailFilePath(AssetDatabase.GetAssetPath(motionTrailFile));

                        //加入选择 TODO
                        //Transform PCparent = GameObject.FindGameObjectWithTag("PC_UI_T2").transform.GetChild(0).GetChild(1).GetChild(0);
                        Transform PCparent = uiCtrl.GamePanelCanvas_PC.transform.GetChild(0).GetChild(1).GetChild(0);
                        //Transform VRparent = GameObject.FindGameObjectWithTag("VR_UI_T2").transform.GetChild(0).GetChild(1).GetChild(0);
                        Transform VRparent = uiCtrl.GamePanelCanvas_VR.transform.GetChild(0).GetChild(1).GetChild(0);


                        for (int i = 0; i < actuatorModelName.Count; i++)
                        {
                            gm.controlSliderStatus.Add(false);

                            //GameObject newButtonPC = Instantiate(unitButton);
                            //newButtonPC.transform.SetParent(PCparent);
                            //newButtonPC.transform.GetChild(0).GetComponent<Text>().text = actuatorModelName[i];
                            //newButtonPC.transform.GetComponent<UnitButtonT2>().setButtonId(i);

                            //GameObject newButtonVR = Instantiate(unitButton);   
                            //newButtonVR.transform.SetParent(VRparent,false);
                            //newButtonVR.transform.GetChild(0).GetComponent<Text>().text = actuatorModelName[i];
                            //newButtonVR.transform.transform.GetComponent<UnitButtonT2>().setButtonId(i);

                        }

                        //关闭
                        Close();
                    }
                    else
                    {
                        //提示
                        EditorUtility.DisplayDialog("提示", "模型不能为空", "确定");         
                    }
                }
                else
                {
                    //提示
                    EditorUtility.DisplayDialog("提示", "驱动器不能为空", "确定");
                }
            }
            if (GUILayout.Button("关闭", GUILayout.Width(position.width * 0.25f - 5)))
            {
                Close();
            }
        }
        GUILayout.EndHorizontal();
    }

}
