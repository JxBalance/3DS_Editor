using System;
using System.Collections;
using System.Collections.Generic;
using Global;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Demos;
using UnityEngine;

public class IntroBoardWindow : BaseWindow
{

    [Title("介绍面板")]
    [ListDrawerSettings(ShowIndexLabels = true)]
    public IntroBoard[] IntroBoardList;

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
            this.IntroBoardList = gmGlobal.IntroBoardList;
        }
    }

    protected override void SetData()
    {
        if (gmGlobal)
        {
            //最后同步
            gmGlobal.IntroBoardList = this.IntroBoardList;
        }
    }
}
