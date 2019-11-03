
using System;
using System.Collections;
using System.Collections.Generic;
using Global;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
public class DisassemblyWindow : ScriptableObject
{
    [NonSerialized]
    public bool ShowEditor;
    [TitleGroup("拆卸安装")]
    [ListDrawerSettings(ShowIndexLabels = true)]
    public Disassembly[] DisassemblyList;
}

 