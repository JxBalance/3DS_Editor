using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;

public class MenuCtrl : EditorWindow {

    public List<EditorWindow> editorWindows = new List<EditorWindow>();

    [MenuItem("3DS Window/开始 &`")]
    private static void OpenWelcomeWindow()
    {
        EditorWindow.GetWindow<WelcomeWindow>(false, "3Ds开始窗口");
    }

    [MenuItem("3DS Window/位置识别 &1")]
    private static void Mywindow_T1()
    {
        EditorWindow.GetWindow<Window_T1>(false, "位置识别");
    }

    [MenuItem("3DS Window/机构运动 &2")]
    private static void Mywindow_T2()
    {
        EditorWindow.GetWindow<Window_T2>(false, "机构运动");
    }

    [MenuItem("3DS Window/气液流动 &3")]
    private static void Mywindow_T3()
    {
        EditorWindow.GetWindow<Window_T3>(false, "气液流动");
    }

    [MenuItem("3DS Window/标注 &4")]
    private static void Mywindow_T4()
    {
        EditorWindow.GetWindow<OpenNoteWindow>(false, "标注");
    }
    [MenuItem("3DS Window/多人协同 &5")]
    private static void Mywindow_T5()
    {
        GMSWindow window = EditorWindow.GetWindow<GMSWindow>(false, "多人协同");
        window.minSize = new Vector2(350,150);
    }

}

