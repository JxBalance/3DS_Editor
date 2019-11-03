using System.Collections;
using System.Collections.Generic;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

public class MainMenu : EditorWindow
{
    [MenuItem("3DS Window/主界面")]
    public static void OpenMainWindow()
    {
        var window = GetWindow<MainWindow>();
        window.position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 600);
        window.titleContent = new GUIContent("3DS 主界面");
    }
}
