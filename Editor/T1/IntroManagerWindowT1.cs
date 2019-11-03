using UnityEngine;
using System.Collections;
using System.Text;
using UnityEditor;

public class IntroManagerWindowT1 : EditorWindow
{
    private static IntroManagerWindowT1 instance;

    public static IntroManagerWindowT1 Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new IntroManagerWindowT1();
            }
            return instance;
        }
    }
    
    public GUISkin mySkin;
    private GUIStyle title3LabelStyle;
    private string[] names = new string[0];
    public int selGridInt = -1;               //列表选择索引 
    private Vector2 scrollPosition;


    private IntroManagerWindowT1()
    {
    }
	
	// Update is called once per frame
	public void OnGUI ()
    {
	    InitGUIStyle();
        GUILayout.Space(2);
        GUILayout.Label("提示板列表", title3LabelStyle);
	    if (Window_T1._instance.GM)
	    {
            UpdateWindow();
            scrollPosition = GUILayout.BeginScrollView(scrollPosition);
            int old = selGridInt;
            GUILayout.BeginHorizontal();
            {
                GUILayout.Space(20);
                selGridInt = GUILayout.SelectionGrid(selGridInt, names, 1);
                GUILayout.Space(20);
            }
            GUILayout.EndHorizontal();
            if (selGridInt != old)
            {
                if (selGridInt >= 0)
                {
                    Window_T1._instance.introTitleText = Window_T1._instance.GM.introBoard[selGridInt].introTitleText;
                    Window_T1._instance.introContentText = Window_T1._instance.GM.introBoard[selGridInt].introContentText;
                    Window_T1._instance.introLoadtexture2D = Window_T1._instance.GM.introBoard[selGridInt].introLoadtexture2D;
                    Window_T1._instance.Repaint();
                }
            }
            GUILayout.EndScrollView();
	    }
        
        GUILayout.BeginHorizontal();
	    {
            GUILayout.Space(20);
            if (GUILayout.Button("上移"))
	        {
	            if (selGridInt == 0)
	            {
	                EditorUtility.DisplayDialog("提示", "无法上移", "确定");
	            }
	            else
	            {
                    IntroduceBoardT1 tempBoard = Window_T1._instance.GM.introBoard[selGridInt - 1];
                    Window_T1._instance.GM.introBoard[selGridInt - 1] = Window_T1._instance.GM.introBoard[selGridInt];
                    Window_T1._instance.GM.introBoard[selGridInt] = tempBoard;
	                selGridInt--;
                    UpdateWindow();

	            }
            }
            if (GUILayout.Button("下移"))
            {
                if (selGridInt == Window_T1._instance.GM.introBoard.Count - 1)
                {
                    EditorUtility.DisplayDialog("提示", "无法下移", "确定");
                }
                else
                {
                    IntroduceBoardT1 tempBoard = Window_T1._instance.GM.introBoard[selGridInt + 1];
                    Window_T1._instance.GM.introBoard[selGridInt + 1] = Window_T1._instance.GM.introBoard[selGridInt];
                    Window_T1._instance.GM.introBoard[selGridInt] = tempBoard;
                    selGridInt++;
                    UpdateWindow();
                }
            }

            if (GUILayout.Button("删除"))
            {
                if (EditorUtility.DisplayDialog("提示", "确定要删除 " + Window_T1._instance.GM.introBoard[selGridInt].introTitleText + " 吗？", "确定", "取消"))
                {
                    DestroyImmediate(Window_T1._instance.GM.introBoard[selGridInt].gameObject);
                    Window_T1._instance.GM.introBoard.RemoveAt(selGridInt);
                    selGridInt = -1;
                    Window_T1._instance.ClearIntroPart();
                    UpdateWindow();
                }
            }

            if (GUILayout.Button("清空"))
            {
                if (EditorUtility.DisplayDialog("提示", "确定要清空所有总体介绍信息吗？", "确定", "取消"))
                {
                    foreach (IntroduceBoardT1 b in Window_T1._instance.GM.introBoard)
                    {
                        DestroyImmediate(b.gameObject);
                    }
                    Window_T1._instance.GM.introBoard.Clear();
                    selGridInt = -1;
                    Window_T1._instance.ClearIntroPart();
                    UpdateWindow();
                }
            }
            GUILayout.Space(20);
        }
        GUILayout.EndHorizontal();
        GUILayout.Space(13);
    }

    void UpdateBoardChildOrder()
    {
        foreach (var board in Window_T1._instance.GM.introBoard)
        {
            board.transform.parent = null;
        }
        foreach (var board in Window_T1._instance.GM.introBoard)
        {
            board.transform.parent = Window_T1._instance.InfoBoardParentTransform;
        }
    }

    void OnDestroy()
    {
        //Window_T1._instance.ClearIntroPart();
    }

    /// <summary>
    /// 初始化GUI格式
    /// </summary>
    private void InitGUIStyle()
    {
        title3LabelStyle = new GUIStyle(mySkin.label);
        title3LabelStyle.fontSize = 18;
    }

    public void UpdateWindow()
    {
        int count = Window_T1._instance.GM.introBoard.Count;
        names = new string[count];
        for (int i = 0; i < count; i++)
        {
            names[i] = Window_T1._instance.GM.introBoard[i].introTitleText;
        }
        UpdateBoardChildOrder();
        Repaint();
    }
}
