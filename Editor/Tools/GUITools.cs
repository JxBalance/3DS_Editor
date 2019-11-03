using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// GUI封装类，自定义
/// </summary>
public class GUITools :EditorWindow{

    public static Enum EnumPopup(GUIContent label, Enum selected, float leftSpacePixels, float rightSpacePixels,
        params GUILayoutOption[] options)
    {
        Enum e;
        GUILayout.BeginHorizontal();
        {
            GUILayout.Space(leftSpacePixels);
            e = EditorGUILayout.EnumPopup(label, selected, options);
            GUILayout.Space(rightSpacePixels);
        }
        GUILayout.EndHorizontal();
        return e; 
    }


    public static bool Button(string text, float leftSpacePixels, float rightSpacePixels,
        params GUILayoutOption[] options)
    {
        bool b = false;
        GUILayout.BeginHorizontal();
        {
            GUILayout.Space(leftSpacePixels);
            b = GUILayout.Button(text, options);
            GUILayout.Space(rightSpacePixels);
        }
        GUILayout.EndHorizontal();
        return b; 
    }

    public static bool DrawBox(bool foldout, string content, GUIStyle style,Action processCallback)
    {
        EditorGUILayout.BeginVertical("Box");
        EditorGUILayout.Space();
        foldout = EditorGUILayout.Foldout(foldout, content, style);
        if (foldout)
        {
            processCallback();
        }
        EditorGUILayout.Space();
        EditorGUILayout.EndVertical();
        return foldout;
    }

    public static bool[] HorizontalButtons(string prefixLabeltext, float leftSpacePixels, float rightSpacePixels,
        string[] buttonsText, int spacePixels)
    {
        bool[] b = new bool[buttonsText.Length];
        GUILayout.BeginHorizontal();
        {
            GUILayout.Space(leftSpacePixels);
            if (prefixLabeltext != "")
            {
                EditorGUILayout.PrefixLabel(prefixLabeltext);
            }
            for (int i = 0; i < buttonsText.Length; i++)
            {
                if (i != 0)
                {
                    GUILayout.Space(spacePixels);
                }
                b[i] = GUILayout.Button(buttonsText[i]);
                if (i != buttonsText.Length - 1)
                {
                    GUILayout.Space(spacePixels);
                }
            }
            GUILayout.Space(rightSpacePixels);
        }
        GUILayout.EndHorizontal();
        return b;
    }

    public static bool[] HorizontalButtons(float leftSpacePixels, float rightSpacePixels, string[] buttonsText,
        int spacePixels,bool[] disableButtons)
    {
        bool[] b = new bool[buttonsText.Length];
        GUILayout.BeginHorizontal();
        {
            GUILayout.Space(leftSpacePixels);
            for (int i = 0; i < buttonsText.Length; i++)
            {
                if (i != 0)
                {
                    GUILayout.Space(spacePixels);
                }
                EditorGUI.BeginDisabledGroup(disableButtons[i]);
                b[i] = GUILayout.Button(buttonsText[i]);
                EditorGUI.EndDisabledGroup();

                if (i != buttonsText.Length - 1)
                {
                    GUILayout.Space(spacePixels);
                }
            }
            GUILayout.Space(rightSpacePixels);
        }
        GUILayout.EndHorizontal();
        return b;
    }

    public static string TextField(string label, float leftSpacePixels, float rightSpacePixels, string text)
    {
        GUILayout.BeginHorizontal();
        {
            GUILayout.Space(leftSpacePixels);
            text = EditorGUILayout.TextField(label, text);
            GUILayout.Space(rightSpacePixels);
        }
        GUILayout.EndHorizontal();
        return text;
    }

    public static T ObjectField<T>(string label, float leftSpacePixels, float rightSpacePixels,
        UnityEngine.Object obj, Type objType, bool allowSceneObjects) where T:UnityEngine.Object
    {
        System.Object sObj;
        GUILayout.BeginHorizontal();
        {
            GUILayout.Space(leftSpacePixels);
            EditorGUILayout.PrefixLabel(label);
            //图片选择框
            sObj = (UnityEngine.Object)EditorGUILayout.ObjectField(obj, objType, allowSceneObjects);
            GUILayout.Space(rightSpacePixels);
        }
        GUILayout.EndHorizontal();
        T tt = (T)sObj;
        return tt;
    }

    public static void Label(string text,float leftSpacePixels, float rightSpacePixels)
    {
        GUILayout.BeginHorizontal();
        {
            GUILayout.Space(leftSpacePixels);
            GUILayout.Label(text);
            GUILayout.Space(rightSpacePixels);
        }
        GUILayout.EndHorizontal();
    }

    public static Vector3 Vector3Field(string label, float leftSpacePixels, float rightSpacePixels, Vector3 value,
        params GUILayoutOption[] options)
    {
        Vector3 v = value;
        GUILayout.BeginHorizontal();
        {
            GUILayout.Space(leftSpacePixels);
            EditorGUILayout.PrefixLabel(label);
            v = EditorGUILayout.Vector3Field("", value, options);
            GUILayout.Space(rightSpacePixels);
        }
        GUILayout.EndHorizontal();
        return v;
    }

    public static Color ColorField(string label, float leftSpacePixels, float rightSpacePixels, Color value,
        params GUILayoutOption[] options)
    {
        Color c = value;
        GUILayout.BeginHorizontal();
        {
            GUILayout.Space(leftSpacePixels);
            c = EditorGUILayout.ColorField(label, value);
            GUILayout.Space(rightSpacePixels);
        }
        GUILayout.EndHorizontal();
        return c;
    }

    public static bool Toggle(string label, float leftSpacePixels, float rightSpacePixels, bool value,
        params GUILayoutOption[] options)
    {
        GUILayout.BeginHorizontal();
        {
            GUILayout.Space(leftSpacePixels);
            value = EditorGUILayout.Toggle(label, value);
            GUILayout.Space(rightSpacePixels);
        }
        GUILayout.EndHorizontal();
        return value;
    }

    public static int Toolbar(float leftSpacePixels, float rightSpacePixels, int selected, string[] texts,
        params GUILayoutOption[] options)
    {
        GUILayout.BeginHorizontal();
        {
            GUILayout.Space(leftSpacePixels);
            selected = GUILayout.Toolbar(selected, texts, options);
            GUILayout.Space(rightSpacePixels);
        }
        GUILayout.EndHorizontal();
        return selected;
    }

    public static int Toolbar(string label, float leftSpacePixels, float rightSpacePixels, int selected, string[] texts,
        params GUILayoutOption[] options)
    {
        GUILayout.BeginHorizontal();
        {
            GUILayout.Space(leftSpacePixels);
            EditorGUILayout.PrefixLabel(label);

            selected = GUILayout.Toolbar(selected, texts, options);
            GUILayout.Space(rightSpacePixels);
        }
        GUILayout.EndHorizontal();
        return selected;
    }

    public static bool BoolDoubleToggle(string label, float leftSpacePixels, float rightSpacePixels, bool value, string[] texts, params GUILayoutOption[] options)
    {
        GUILayout.BeginHorizontal();
        {
            GUILayout.Space(leftSpacePixels);
            EditorGUILayout.PrefixLabel(label);
            value = GUILayout.Toggle(value, texts[0], options);
            value = !GUILayout.Toggle(!value, texts[1], options);
            GUILayout.Space(rightSpacePixels);
        }
        GUILayout.EndHorizontal();
        return value;
    }

    public static float FloatField(string label, float leftSpacePixels, float rightSpacePixels, float value)
    {
        
        GUILayout.BeginHorizontal();
        {
            GUILayout.Space(leftSpacePixels);
            value = EditorGUILayout.FloatField(label, value);
            GUILayout.Space(rightSpacePixels);
        }
        GUILayout.EndHorizontal();
        return value;
    }

    public static int IntField(string label, float leftSpacePixels, float rightSpacePixels, int value)
    {
        GUILayout.BeginHorizontal();
        {
            GUILayout.Space(leftSpacePixels);
            value = EditorGUILayout.IntField(label, value);
            GUILayout.Space(rightSpacePixels);
        }
        GUILayout.EndHorizontal();
        return value;
    }

    public static List<Vector3> ObservePointListPanel(List<Vector3> pointPositionList, List<Vector3> pointRotationList,int count)
    {
        if (count < pointPositionList.Count && count > -1)
        {
            //移除多余的元素
            int removeCount = pointPositionList.Count - count;
            //Debug.Log("移除多余" + removeCount +"个元素");
            pointPositionList.RemoveRange(count, removeCount);
            pointRotationList.RemoveRange(count, removeCount);
        }
        else if (count > pointPositionList.Count)
        {
            //填补不足的元素
            //Debug.Log("填补不足" + (count - pointPositionList.Count) + "元素" + count + "  "+  pointPositionList.Count);
            for (int i = 0; i < count - pointPositionList.Count; i++)
            {
                Debug.Log("Add");
                pointPositionList.Add(Vector3.zero);
                pointRotationList.Add(Vector3.zero);
            }
        }

        for (int i = 0; i < pointPositionList.Count; i++)
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.Space(180);
                GUILayout.Label(i.ToString());
                pointPositionList[i] = EditorGUILayout.Vector3Field("", pointPositionList[i]);
                GUILayout.Space(10);
                if (GUILayout.Button("设置"))
                {
                    UnityEngine.Object[] tempObjects = Selection.objects;
                    GameObject camGameObject = GameObject.FindGameObjectWithTag("MainCamera");
                    Selection.activeGameObject = camGameObject;
                    EditorApplication.ExecuteMenuItem("GameObject/Align With View");
                    Selection.objects = tempObjects;
                    pointPositionList[i] = camGameObject.transform.position;
                    pointRotationList[i] = camGameObject.transform.eulerAngles;
                }
                GUILayout.Space(10);
                if (GUILayout.Button("删除"))
                {
                    pointPositionList.RemoveAt(i);
                    pointRotationList.RemoveAt(i);
                    GUI.FocusControl("");
                }

                GUILayout.Space(30);
            }
            GUILayout.EndHorizontal();
        }
        return pointPositionList;
    }


}
