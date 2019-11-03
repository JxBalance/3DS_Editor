using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.UI;

public class ImportCabinWindow : EditorWindow
{

    public static ImportCabinWindow _instance;
	// Use this for initialization
    public GUISkin CabinSkin;
    private GameObject CabinModel;
    private string CabinName;
    private GameManagerT2 gm;
    private MonoScript addScript;
    private GameObject partOb;
    private string partName;
    private Camera m_MainCamera;

	void Awake () 
    {
        _instance = this;
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManagerT2>();
        m_MainCamera = Camera.main;
	}

    void OnGUI()
    {
        GUILayout.Space(20);
        GUILayout.BeginHorizontal();
        {
            EditorGUILayout.PrefixLabel("机舱模型");
            CabinModel = EditorGUILayout.ObjectField(CabinModel, typeof(GameObject), true) as GameObject;
            CabinName = "Cabin";
        }

        GUILayout.EndHorizontal();
        GUILayout.Space(10);

        GUILayout.BeginHorizontal();
        {
            GUILayout.Space(position.width * 0.5f);
            if (GUILayout.Button("导入", GUILayout.Width(position.width * 0.25f - 5)))
            {
                if (CabinModel)
                {
                    if (CabinName != null)
                    {
                        GameObject go = Instantiate(CabinModel);
                        go.name = CabinName;
                        go.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
                        go.transform.position = new Vector3(0f, -4.5f, -9.5f);
                        go.transform.rotation = Quaternion.Euler(0, -90, 0);
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

	// Update is called once per frame
	void Update () {
		
	}
}
