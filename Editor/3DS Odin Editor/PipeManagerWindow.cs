using System;
using System.Collections;
using System.Collections.Generic;
using Global;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

public class PipeManagerWindow : BaseWindow
{

    [NonSerialized]
    public bool ShowEditor;
    [TitleGroup("管道管理")]
    [ListDrawerSettings(ShowIndexLabels = true)]
    public PipeGroup[] PipeGroupList;

    protected override void OnHierarchyChange()
    {
        base.OnHierarchyChange();
    }

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void GetData()
    {
        
    }

    protected override void SetData()
    {
        
    }
}