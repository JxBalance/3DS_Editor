using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Boo.Lang.Environments;
using Sirenix.OdinInspector.Demos;
using Sirenix.OdinInspector.Editor;
using Sirenix.Serialization;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

public class MainWindow : OdinMenuEditorWindow
{


    public void RestartWindow()
    {
        Close();
        MainMenu.OpenMainWindow();
    }


    protected override OdinMenuTree BuildMenuTree()
    {
        OdinMenuTree tree = new OdinMenuTree(supportsMultiSelect: true)
        {
            { "主菜单  ",   new HomeWindow(this),      EditorIcons.House},
            { "基本设置",   new BasicSettingWindow(),      EditorIcons.SettingsCog},
            { "模型管理",   new ModelManagerWindow(),      EditorIcons.Airplane},
            { "介绍面板",   new IntroBoardWindow(),      EditorIcons.Info},
            { "部件识别",   new UnitRecognitionWindow(),      EditorIcons.Tag},
            { "机构控制",   new DrivingPrincipleWindows(),   EditorIcons.Rotate},
            { "拆卸安装",   new DisassemblyWindow(),   EditorIcons.LockLocked},
            { "管道管理",   new PipeManagerWindow(),      EditorIcons.TestTube},
            { "气液流动",   new HydraulicWindow(),      EditorIcons.Pen}
        };

        return tree;
    }

    //void Update()
    //{
        
    //}

    //void OnFocus()
    //{
    //    Debug.Log("OnFocus");
    //}

    //void OnLostFocus()
    //{
    //    Debug.Log("OnLostFocus");
    //}

    //void OnHierarchyChange()
    //{
    //    Debug.Log("OnHierarchyChange");
    //}

    //void OnProjectChange()
    //{
    //    Debug.Log("OnProjectChange");
    //}

    //void OnInspectorGUI()
    //{
    //    Debug.Log("OnInspectorGUI");
    //}

    //void OnSelectionChange()
    //{
    //    Debug.Log("OnSelectionChange");
    //}

    //void OnDestroy()
    //{
    //    Debug.Log("OnDestroy");
    //}
}
