using UnityEngine;
using System.Collections;
using UnityEditor;

public class UnitManagerWindowT1 : EditorWindow
{
    private static UnitManagerWindowT1 instance;
    public static UnitManagerWindowT1 Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new UnitManagerWindowT1();
            }
            return instance;
        }
    }

    //public GameManagerT1 gm;
    public GUISkin mySkin;
    private GUIStyle title3LabelStyle;
    private string[] names = new string[0];
    public int selGridInt = -1;               //列表选择索引 
    private Vector2 scrollPosition;

    private UnitManagerWindowT1()
    {
    }


    // Update is called once per frame
    public void OnGUI()
    {
        InitGUIStyle();
        GUILayout.Space(5);
        GUILayout.Label("部件列表", title3LabelStyle);
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
            if (old >= 0)
            {
                Window_T1._instance.UnitPositionToOriginal(Window_T1._instance.GM.unitGroups[old]);
            }

            if (selGridInt >= 0)
            {
                int count = Window_T1._instance.GM.unitGroups[selGridInt].unitMembers.Count;
                GameObject[] gameObjects = new GameObject[count];
                for (int i = 0; i < count; i++)
                {
                    gameObjects[i] = Window_T1._instance.GM.unitGroups[selGridInt].unitMembers[i].gameObject;
                }
                Selection.objects = gameObjects;
                Window_T1._instance.ShowUnitPartInfo(Window_T1._instance.GM.unitGroups[selGridInt]);
            }
            
        }


        GUILayout.EndScrollView();
        GUILayout.BeginHorizontal();
        {
            //GUILayout.Space(20);
            //if (GUILayout.Button("上移", GUILayout.Width(73.5f)))
            //{
            //    if (selGridInt == 0)
            //    {
            //        EditorUtility.DisplayDialog("提示", "无法上移", "确定");
            //    }
            //    else
            //    {
            //        UnitGroupT1 tempUnit = gm.unitGroups[selGridInt - 1];
            //        gm.unitGroups[selGridInt - 1] = gm.unitGroups[selGridInt];
            //        gm.unitGroups[selGridInt] = tempUnit;
            //        selGridInt--;
            //        InitWindow();
            //    }
            //}

            //if (GUILayout.Button("下移", GUILayout.Width(73.5f)))
            //{
            //    if (selGridInt == gm.unitGroups.Count - 1)
            //    {
            //        EditorUtility.DisplayDialog("提示", "无法下移", "确定");
            //    }
            //    else
            //    {
            //        UnitGroupT1 tempUnit = gm.unitGroups[selGridInt + 1];
            //        gm.unitGroups[selGridInt + 1] = gm.unitGroups[selGridInt];
            //        gm.unitGroups[selGridInt] = tempUnit;
            //        selGridInt++;
            //        InitWindow();
            //    }
            //}
            GUILayout.Space(20f);
            if (GUILayout.Button("批量导入"))
            {
                //TODO

            }
            GUILayout.Space(26.5f);
            if (GUILayout.Button("删除"))
            {
                if (EditorUtility.DisplayDialog("提示", "确定要删除 " + Window_T1._instance.GM.unitGroups[selGridInt].unitName + " 吗？", "确定", "取消"))
                {
                    foreach (UnitMemberT1 mem in Window_T1._instance.GM.unitGroups[selGridInt].unitMembers)
                    {
                        DestroyImmediate(mem.GetComponent<MeshCollider>());
                        DestroyImmediate(mem);
                    }
                    DestroyImmediate(Window_T1._instance.GM.unitGroups[selGridInt].button);
                    DestroyImmediate(Window_T1._instance.GM.unitGroups[selGridInt].gameObject);
                    Window_T1._instance.GM.unitGroups.RemoveAt(selGridInt);
                    selGridInt = -1;
                    Window_T1._instance.ClearUnitPart();
                    UpdateWindow();
                }
            }

            if (GUILayout.Button("清空"))
            {
                if (EditorUtility.DisplayDialog("提示", "确定要清空所有信息部件吗？", "确定", "取消"))
                {
                    foreach (UnitGroupT1 u in Window_T1._instance.GM.unitGroups)
                    {
                        foreach (UnitMemberT1 m in u.unitMembers)
                        {
                            DestroyImmediate(m.GetComponent<MeshCollider>());
                            DestroyImmediate(m);
                        }
                        DestroyImmediate(u.button);
                        DestroyImmediate(u.gameObject);
                    }
                    Window_T1._instance.GM.unitGroups.Clear();
                    selGridInt = -1;
                    Window_T1._instance.ClearUnitPart();
                    UpdateWindow();
                }
            }
            GUILayout.Space(20);
        }
        GUILayout.EndHorizontal();
        GUILayout.Space(10);
    }

    void OnDestroy()
    {
        //Window_T1._instance.ClearUnitPart();
    }

    /// <summary>
    /// 初始化GUI格式
    /// </summary>
    private void InitGUIStyle()
    {
        title3LabelStyle = new GUIStyle(mySkin.label);
        title3LabelStyle.fontSize = 18;
    }

    /// <summary>
    /// 刷新
    /// </summary>
    public void UpdateWindow()
    {
        int count = Window_T1._instance.GM.unitGroups.Count;
        names = new string[count];
        for (int i = 0; i < count; i++)
        {
            names[i] = Window_T1._instance.GM.unitGroups[i].unitName;
        }
        Repaint();
    }
}
