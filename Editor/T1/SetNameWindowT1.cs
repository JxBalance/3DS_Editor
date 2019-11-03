using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SetNameWindowT1 : EditorWindow
{
    public static SetNameWindowT1 _instance;
    public string nameText = "";

    public void Awake()
    {
        _instance = this;
        nameText = Window_T1._instance.GM.nameStr;
    }

    void OnGUI()
    {
        GUILayout.Space(20);
        GUILayout.BeginHorizontal();
        {
            nameText = EditorGUILayout.TextField("课件名称", nameText);//保存所导入模型名称
            if (GUI.changed)
            {
                Window_T1._instance.GM.nameStr = nameText;
            }

        }
        GUILayout.EndHorizontal();
        GUILayout.Space(35);
        GUILayout.BeginHorizontal();
        {
            GUILayout.Space(position.width * 0.75f-5);
            if (GUILayout.Button("关闭", GUILayout.Width(position.width * 0.25f - 5)))
            {
                Close();
            }
        }
        GUILayout.EndHorizontal();
    }
}
