using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.UI;

public class ImpotEnvironmentalModel  : EditorWindow
{
    public static ImpotEnvironmentalModel _instance;
    //GUI皮肤
    public GUISkin meSkin;

    private GameObject environmentModel;
    private string environmentName;
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
            EditorGUILayout.PrefixLabel("环境模型");
            environmentModel = EditorGUILayout.ObjectField(environmentModel, typeof(GameObject), true) as GameObject;
        }
        GUILayout.EndHorizontal();
        GUILayout.Space(5);
        GUILayout.BeginHorizontal();
        {
            environmentName = EditorGUILayout.TextField("模型名称", environmentName);//保存所导入模型名称
        }
        GUILayout.EndHorizontal();
        GUILayout.Space(5);
        GUILayout.BeginHorizontal();
        {
            GUILayout.Space(position.width * 0.5f);
            if (GUILayout.Button("导入", GUILayout.Width(position.width * 0.25f-5)))
            {
                if (environmentModel)
                {
                    if (environmentName != null)
                    {
                        GameObject go = Instantiate(environmentModel);
                        go.name = environmentName;
                        go.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
                        go.transform.position = new Vector3(-87f, -5, -29f);
                        go.transform.rotation = Quaternion.Euler(0, 0, 0);
                        gm.environmentModels.Add(go);                    
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
