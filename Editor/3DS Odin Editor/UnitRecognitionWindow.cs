using System;
using System.Collections;
using System.Collections.Generic;
using Global;
using Sirenix.OdinInspector;
using UnityEngine;

public class UnitRecognitionWindow : BaseWindow
{
    [Title("部件识别")]
    [ListDrawerSettings(ShowIndexLabels = true)]
    public UnitGroup[] UnitGroupList;

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
            this.UnitGroupList = gmGlobal.UnitGroupList;
        }
    }

    protected override void SetData()
    {
        if (gmGlobal)
        {
            //删除检测
            if (UnitGroupList.Length < gmGlobal.UnitGroupList.Length)
            {
                //元素遍历 找出被删除的元素
                for (int i = 0; i < gmGlobal.UnitGroupList.Length; i++)
                {
                    //如果是最后一个元素被删除
                    if (i > UnitGroupList.Length - 1)
                    {
                        //销毁一切该group内所有相关实例化物体及添加的脚本
                        gmGlobal.UnitGroupList[i].DestroyGroup();
                        break;
                    }
                    //如果中间某一个元素被删除
                    if (UnitGroupList[i] != gmGlobal.UnitGroupList[i])
                    {
                        gmGlobal.UnitGroupList[i].DestroyGroup();
                        break;
                    }
                }
            }
            //最后同步
            gmGlobal.UnitGroupList = this.UnitGroupList;
        }
    }



}
