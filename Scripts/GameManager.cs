using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
//using VRTK;

/// <summary>
/// 该脚本为场景控制器，所有课件必须添加专用物体挂载子类
/// </summary>
public class GameManager : MonoBehaviour
{

    public int type = -1; //课件类型
    public SubjectType subjectType;
    public OperateType operateType;

    //课件GMS信息
    public bool isAddGMS = false;
    public List<Crew> crew = new List<Crew>();
}

/// <summary>
/// 角色
/// </summary>
public enum Crew
{
    操作端 = 0,
    监控端 = 1,
};

/// <summary>
/// 操作方式
/// </summary>
public enum OperateType
{
    桌面式 = 0,
    头盔式 = 1,
};

/// <summary>
/// 课件类型
/// </summary>
public enum SubjectType
{
    位置识别 = 0,
    机构运动 = 1,
    气液流动 = 2,
};