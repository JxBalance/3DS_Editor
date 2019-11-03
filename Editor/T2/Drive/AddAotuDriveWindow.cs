using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UI;

public class AddAotuDriveWindow :EditorWindow
{
    public static AddAotuDriveWindow _instance;
    //GUI皮肤
    public GUISkin meSkin;
    public GameObject unitButton;

    public List<string> actuatorModelName;

    private GameManagerT2 gm;

    private MonoScript addScript;

    private string motionTrailFile;

    private string mechanismName;
    private int currentModelIndex = -1;
    private float movetime ;

    void Awake()
    {
        _instance = this;
        actuatorModelName = new List<string>();
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManagerT2>();
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
     
        /**/
        GUILayout.BeginHorizontal();
        {
            EditorGUILayout.PrefixLabel("选择控制模式");
            addScript = EditorGUILayout.ObjectField(addScript, typeof(MonoScript), true) as MonoScript;
        }
        GUILayout.EndHorizontal();
     
        GUILayout.Space(10);

        /**/

        GUILayout.BeginHorizontal();
        {
            GUILayout.Space(position.width * 0.5f);
            if (GUILayout.Button("添加", GUILayout.Width(position.width * 0.25f-5)))
            {
                if (currentModelIndex >= 0)
                {
                    //add controler
                    Type monoScriptClass = addScript.GetClass();
                    gm.mechanismModels[currentModelIndex].AddComponent(monoScriptClass);  
                    //关闭
                    Close();
                }
                else
                {
                    //提示
                    EditorUtility.DisplayDialog("提示", "模型不能为空", "确定");         
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
