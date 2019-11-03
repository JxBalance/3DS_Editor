using System;
using System.Collections;
using System.Collections.Generic;
using Global;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

public class DrivingPrincipleWindows : BaseWindow
{
    [TitleGroup("作动原理")]
    [ListDrawerSettings(ShowIndexLabels = true)]
    public Drive[] DriveList;

    protected override void GetData()
    {
        if (gmGlobal)
        {
            this.DriveList = gmGlobal.DriveList;
        }
    }

    protected override void SetData()
    {
        if (gmGlobal)
        {
            //删除检测
            if (DriveList.Length < gmGlobal.DriveList.Length)
            {
                //元素遍历 找出被删除的元素
                for (int i = 0; i < gmGlobal.DriveList.Length; i++)
                {
                    //如果是最后一个元素被删除
                    if (i > DriveList.Length - 1)
                    {
                        //销毁一切该group内所有相关实例化物体及添加的脚本
                        DestroyImmediate(gmGlobal.DriveList[i].multiControlerBase);

                        break;
                    }
                    //如果中间某一个元素被删除
                    if (DriveList[i] != gmGlobal.DriveList[i])
                    {
                        DestroyImmediate(gmGlobal.DriveList[i].multiControlerBase);

                        break;
                    }
                }
            }
            //最后同步
            gmGlobal.DriveList = this.DriveList;
        }
    }
}