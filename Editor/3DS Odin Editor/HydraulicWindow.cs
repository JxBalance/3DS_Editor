using System;
using System.Collections;
using System.Collections.Generic;
using Global;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

public class HydraulicWindow : ScriptableObject
{

    [NonSerialized]
    public bool ShowEditor;
    [TitleGroup("气液流动")]
    [ListDrawerSettings(ShowIndexLabels = true)]
    public Hydraulic[] HydraulicList;

    [OnInspectorGUI]
    void OnInspectorGUI()
    {
        //删除判断 TODO
    }
}
