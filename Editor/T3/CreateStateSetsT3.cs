using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;


public class CreateStateSetsT3 : EditorWindow {
    
    public static CreateStateSetsT3 _instance;

    //GUI皮肤
    public GUISkin mySkin;

    private string stateName;
    private string stateTitle;
    
    void Awake()
    {
        _instance = this;
    }

    private void OnGUI()
    {
        GUILayout.Space(20);
        stateName = EditorGUILayout.TextField("状态名称 （创建状态）", stateName);

        GUILayout.Space(5);
        stateTitle = EditorGUILayout.TextField("状态标题 （界面标题）", stateTitle);

        GUILayout.Space(20);
        GUILayout.BeginHorizontal();
        {
            GUILayout.Space(120);
            if (GUILayout.Button("确定添加", GUILayout.Width(80)))
            {
                if (stateName == null)
                {
                    EditorUtility.DisplayDialog("提示", "状态名称不能为空", "确定");
                }
                else if(stateTitle == null)
                {
                    EditorUtility.DisplayDialog("提示", "状态标题不能为空", "确定");
                }
                else
                {
                    GameObject go = Instantiate(Window_T3._instance.stateSetPrefab);
                    go.name = stateName;
                    go.transform.parent = GameObject.Find("StateSetsParentT3(Clone)").transform;
                    StateSetT3 newstateSet = go.GetComponent<StateSetT3>();
                    newstateSet.stateName = stateName;
                    newstateSet.stateTitle = stateTitle;
                    Window_T3._instance.GM.stateSets.Add(newstateSet);

                    int oldState = Window_T3._instance.stateIndex;
                    Window_T3._instance.stateIndex = (Window_T3._instance.GM.stateSets.Count - 1);
                    Window_T3._instance.GM.stateSets[oldState].transform.GetChild(0).gameObject.SetActive(false);
                    Window_T3._instance.GM.stateSets[Window_T3._instance.stateIndex].transform.GetChild(0).gameObject.SetActive(true);
                    Window_T3._instance.UIdefaultSettings();
                    Close();
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
        // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
