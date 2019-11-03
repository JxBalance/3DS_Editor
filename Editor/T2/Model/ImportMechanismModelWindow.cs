using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.UI;

public class ImportMechanismModelWindow : EditorWindow
{
    public static ImportMechanismModelWindow _instance;
    //GUI皮肤
    public GUISkin meSkin;

    private GameObject mechanismModel;
    private string mechanismName;
    private GameManagerT2 gm;

    void Awake()
    {
        _instance = this;
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManagerT2>();
    }

    void OnGUI()
    {
        GUILayout.Space(20);
        GUILayout.BeginHorizontal();
        {
            EditorGUILayout.PrefixLabel("机构模型");
            mechanismModel = EditorGUILayout.ObjectField(mechanismModel, typeof(GameObject), true) as GameObject;
        }
        GUILayout.EndHorizontal();
        GUILayout.Space(5);
        GUILayout.BeginHorizontal();
        {
            mechanismName = EditorGUILayout.TextField("模型名称", mechanismName);//保存所导入模型名称
        }
        GUILayout.EndHorizontal();
        GUILayout.Space(5);
        GUILayout.BeginHorizontal();
        {
            GUILayout.Space(position.width * 0.5f);
            if (GUILayout.Button("导入", GUILayout.Width(position.width * 0.25f-5)))
            {
                if (mechanismModel)
                {
                    if (mechanismName != null)
                    {
                        GameObject go = Instantiate(mechanismModel);
                        go.name = mechanismName;
                        go.transform.position = new Vector3(5.5f, -1.5f, -55.5f);
                        go.transform.rotation = Quaternion.Euler(0, 0, 0);
                        gm.mechanismModels.Add(go);                    
                        Close();
                    }
                    else
                    {
                        //没有名字
                        EditorUtility.DisplayDialog("提示", "名称不能为空", "确定");
                    }
                    
                }
                else
                {
                    //提示
                    EditorUtility.DisplayDialog("提示", "模型不能为空", "确定");
                }
                

            }
            if (GUILayout.Button("关闭", GUILayout.Width(position.width * 0.25f - 5)))
            {
                Close();
            }
        }
        GUILayout.EndHorizontal();
    }

}
