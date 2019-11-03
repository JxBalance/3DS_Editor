using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UI;



public class AddButtonPanel : EditorWindow
{

    public static AddButtonPanel _instance;
    // Use this for initialization
    public GUISkin meSkin;
    public float vSbarValue;
    public Material m_Material;

    private GameObject ButtonModel;
    private string ButtonName;
    private GameManagerT2 gm;
	// Use this for initialization
	void Awake () 
    {
        _instance = this;
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManagerT2>();			
	}

    void OnGUI()
    {
        GUILayout.Space(20);
        /// <summary>
        /// 选择按钮
        /// </summary>
        GUILayout.BeginHorizontal();
       {
           EditorGUILayout.PrefixLabel("机舱模型");
           ButtonModel = EditorGUILayout.ObjectField(ButtonModel, typeof(GameObject), true) as GameObject;
           ButtonName = "M_button";
        }

         GUILayout.EndHorizontal();
        GUILayout.Space(10);

        GUILayout.BeginHorizontal();
        {
            GUILayout.Space(position.width * 0.5f);
            if (GUILayout.Button("导入", GUILayout.Width(position.width * 0.25f - 5)))
            {
                if (ButtonModel)
                {
                    if (ButtonName != null)
                    {
                        GameObject go = Instantiate(ButtonModel);
                        go.name = ButtonName;
                        go.transform.position = new Vector3(0, 0, 0);
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
                Close();
            }
            if (GUILayout.Button("关闭", GUILayout.Width(position.width * 0.25f - 5)))
            {
                Close();
            }
        }


       // GUILayout.BeginHorizontal();
       // {
            /*针对分离模型*/
            //EditorGUILayout.PrefixLabel("当前选择按钮");

            //Transform[] selected = Selection.transforms;
            //GameObject[] selectedob = Selection.gameObjects;

            //if (selected.Length == 0)
            //{
            //    GUILayout.Label("无");
            //}
            //else
            //{
            //    string selected_part_name = selected[0].name;
            //    string show_name;

            //    if (selected_part_name.Length > 15)
            //    {
            //        show_name = selected_part_name.Substring(0, 10) + "...";
            //    }
            //    else
            //    {
            //        show_name = selected_part_name;
            //    }

            //    GUILayout.Label(show_name);
            //}

            //if (GUILayout.Button("确定", GUILayout.Width(position.width * 0.25f - 5)))
            //{
            //    if (selected.Length == 0)
            //    {
            //        //提示
            //        EditorUtility.DisplayDialog("提示", "未选择驱动按钮", "确定");
            //    }
            //    else
            //    {
            //        ButtonModel = selectedob[0];
            //        ButtonName = selected[0].name;
            //        m_Material= ButtonModel.GetComponent<Renderer>().material;
            //        m_Material.color = Color.red;
            //        ButtonModel.AddComponent<switchcameraT2>();
                
            //    }
            //}
       // }

        //GUILayout.EndHorizontal();
        //GUILayout.Space(20);

        //GUILayout.BeginHorizontal();
        //{
        //    GUILayout.Space(position.width * 0.5f);
        //    if (GUILayout.Button("导入", GUILayout.Width(position.width * 0.25f - 5)))
        //    {
        //        if (ButtonModel)
        //        {
        //            if (ButtonName != null)
        //            {
        //                GameObject go = Instantiate(ButtonModel);
        //                go.name = ButtonName;
        //                go.transform.position = new Vector3(-99, 1, 2);
        //                go.transform.rotation = Quaternion.Euler(0, 90, 0);
        //                Close();
        //            }
        //            else
        //            {
        //                //没有名字
        //                EditorUtility.DisplayDialog("提示", "名称不能为空", "确定");
        //            }

        //        }
        //        else
        //        {
        //            //提示
        //            EditorUtility.DisplayDialog("提示", "模型不能为空", "确定");
        //        }

        //    }
        //    if (GUILayout.Button("关闭", GUILayout.Width(position.width * 0.25f - 5)))
        //    {
        //        Close();
        //    }
        //}
        GUILayout.EndHorizontal();

    }



	// Update is called once per frame
	void Update () {
		
	}
}
