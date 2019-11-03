using System;
using System.Collections.Generic;
using Global;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

public class BasicSettingWindow : BaseWindow
{
    [Title("基本设置")]
    [Rename("课件名称")]
    public string SceneName = "";

    //多行Attr不能汉化
    [Multiline]
    public string 课件备注 = "";

    [Title("多人协同角色配置")]
    public ClientMode[] ClientArray = { ClientMode.教员端, ClientMode.学员端, ClientMode.观察端, ClientMode.观察端 };

    //[HideIf("ShowEditor")]
    //[OnInspectorGUI]
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
        if (gmGlobal)
        {
            SceneName = gmGlobal.Setting.SceneName;
            课件备注 = gmGlobal.Setting.SceneRemarks;
            ClientArray = gmGlobal.Setting.ClientArray;
        }
    }
    protected override void SetData()
    {
        if (gmGlobal)
        {
            gmGlobal.Setting.SceneName = SceneName;
            gmGlobal.Setting.SceneRemarks = 课件备注;
            gmGlobal.Setting.ClientArray = ClientArray;
        }
    }
}
