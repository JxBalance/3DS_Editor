using UnityEngine;
using System.IO;
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public class OpenCourseWindow : EditorWindow 
{
	public static OpenCourseWindow _instance;
	public GUISkin mySkin;
	private GUIStyle titleLabelStyle, subtitleLabelStyle, toolBarStyle, buttonStyle, 
	smallLabelStyle, partTitleStyle, boldLabelStyle,
	textAreaStyle;
	public Font myFont;

	private bool isShowIntroducePart = true;
	private bool isShowModelPart = true;
	private bool isShowPartsPart = true;

	private string courseTitle = "";
	private string courseInfo = "";
	private string partsName = "";

	private Vector3 modelPosition;
	private Vector3 modelRotation;
	private Vector3 modelScale;
	private Vector3 partsPosition;
	private Vector3 partsRotation;
	private Vector3 partsScale;

	public void Awake()
	{
		_instance = this;
	}

	void OnGUI()
	{
		if (!_instance)
		{
			_instance = this;
		}
		ScenePage();
	}
		
	#region 场景搭建界面
	private void ScenePage()
	{
		InitGUIStyle();

		GUILayout.Space(25);
		GUILayout.Label("场景搭建编辑器", subtitleLabelStyle);
		GUILayout.Space(15);

		#region 总体说明
		isShowIntroducePart = EditorGUILayout.Foldout(isShowIntroducePart, "总体介绍", partTitleStyle); // 定义折叠菜单
		if (isShowIntroducePart) 
		{
			GUILayout.Space(10);
			GUILayout.BeginHorizontal();
			{
				GUILayout.Space(20);
				GUILayout.Label("课件标题：",GUILayout.Width(70));
				GUILayout.Space(10);
				courseTitle = GUILayout.TextField(courseTitle);
				GUILayout.Space(20);
			}
			GUILayout.EndHorizontal();

			GUILayout.Space(5);
			GUILayout.BeginHorizontal();
			{
				GUILayout.Space(20);
				GUILayout.Label("课件介绍：",GUILayout.Width(70));
				GUILayout.Space(10);
				courseInfo = GUILayout.TextField(courseInfo);
				GUILayout.Space(20);
			}
			GUILayout.EndHorizontal();
		}
		GUILayout.Space(20);
		#endregion

		#region 模型管理
		isShowModelPart = EditorGUILayout.Foldout(isShowModelPart, "模型管理", partTitleStyle);
		if(isShowModelPart)
		{
			GUILayout.Space(10);
			GUILayout.BeginHorizontal();
			{
				GUILayout.Space(20);
				GUILayout.Label("当前模型名称："+ "AirBus");
				GUILayout.Space(20);
			}
			GUILayout.EndHorizontal();

			GUILayout.Space(10);
			GUILayout.BeginHorizontal();
			{
				GUILayout.Space(20);
				GUILayout.Label("位置");
				GUILayout.Space(10);
				modelPosition = EditorGUILayout.Vector3Field("", modelPosition,GUILayout.Height(20));
				GUILayout.Space(20);
			}
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			{
				GUILayout.Space(20);
				GUILayout.Label("旋转");
				GUILayout.Space(10);
				modelRotation = EditorGUILayout.Vector3Field("", modelRotation, GUILayout.Height(20));
				GUILayout.Space(20);
			}
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			{
				GUILayout.Space(20);
				GUILayout.Label("缩放");
				GUILayout.Space(10);
				modelScale = EditorGUILayout.Vector3Field("", modelScale, GUILayout.Height(10));
				GUILayout.Space(20);
			}
			GUILayout.EndHorizontal();

			GUILayout.Space(10);
			GUILayout.BeginHorizontal();
			{
				GUILayout.Space(20);
				if (GUILayout.Button("导入飞机模型", GUILayout.Width(position.width * 0.5f - 25)))
				{

				}
				if (GUILayout.Button("删除飞机模型", GUILayout.Width(position.width * 0.5f - 25)))
				{

				}
				GUILayout.Space(20);
			}
			GUILayout.EndHorizontal();

		}
		GUILayout.Space(20);
		#endregion

		#region 部件管理
		isShowPartsPart = EditorGUILayout.Foldout(isShowPartsPart, "模型管理", partTitleStyle);
		if (isShowPartsPart) 
		{
			GUILayout.Space(10);
			GUILayout.BeginHorizontal();
			{
				GUILayout.Space(20);
				GUILayout.Label("部件名：",GUILayout.Width(70));
				GUILayout.Space(position.width * 0.1f);
				partsName = GUILayout.TextField(partsName);
				GUILayout.Space(20);
			}
			GUILayout.EndHorizontal();

			GUILayout.Space(10);
			GUILayout.BeginHorizontal();
			{
				GUILayout.Space(20);
				GUILayout.Label("位置");
				GUILayout.Space(10);
				partsPosition = EditorGUILayout.Vector3Field("", partsPosition,GUILayout.Height(20));
				GUILayout.Space(20);
			}
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			{
				GUILayout.Space(20);
				GUILayout.Label("旋转");
				GUILayout.Space(10);
				partsRotation = EditorGUILayout.Vector3Field("", partsRotation, GUILayout.Height(20));
				GUILayout.Space(20);
			}
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			{
				GUILayout.Space(20);
				GUILayout.Label("缩放");
				GUILayout.Space(10);
				partsScale = EditorGUILayout.Vector3Field("", partsScale, GUILayout.Height(10));
				GUILayout.Space(20);
			}
			GUILayout.EndHorizontal();

			GUILayout.Space(10);
			GUILayout.BeginHorizontal();
			{
				GUILayout.Space(20);
				if (GUILayout.Button("添加部件", GUILayout.Width(position.width * 0.5f - 25)))
				{
					AddPartsTest();
				}
				if (GUILayout.Button("删除部件", GUILayout.Width(position.width * 0.5f - 25)))
				{
					AddPartsNewTest();
				}
				GUILayout.Space(20);
			}
			GUILayout.EndHorizontal();
		}
		GUILayout.Space(20);
		#endregion

	}
	#endregion

	void AddPartsNewTest()
	{
		GameObject partPerfab = Instantiate (Resources.Load <GameObject> ("prefab/PartPerfab"));
		GameObject partSets = GameObject.Find ("Parts");
		//partPerfab.transform.parent = partParent.transform;
		foreach (Transform child in partSets.transform) 
		{
			partPerfab.transform.parent = child.transform;
		}
	}

	void AddPartsTest()
	{
		GameObject partPerfab = Instantiate (Resources.Load <GameObject> ("prefab/PartPerfab"));
		GameObject partSets = GameObject.Find ("Parts");
		//partPerfab.transform.parent = partParent.transform;
		foreach (Transform child in partSets.transform) 
		{
			string addd = child.name;
			GameObject testA = GameObject.Find (addd);
			if (testA) {
				Debug.Log (addd + " is SHIT!!!!!");
				//																	???????!!!!!!!?!??!?!??!!?!??!?!?!?!
			}
			/*
			if (testA != null) 
			{
				partPerfab.transform.parent = child.transform;
			}*/

		}
		if (partsName == "") 
		{
			partPerfab.name = "PartPerfab";
		}
		else
		{
			partPerfab.name = partsName;
		}
		GameObject mainScript = GameObject.Find ("Scripts");
		mainScript.GetComponent<MainSystemScript> ().AddParts (partPerfab);

		partsPosition = partPerfab.transform.localPosition;
		partsRotation = partPerfab.transform.eulerAngles;
		partsScale = partPerfab.transform.localScale;

	}

	#region 界面变量初始化
	private void InitGUIStyle()
	{
		titleLabelStyle = new GUIStyle(mySkin.label);
		titleLabelStyle.fontSize = 30;
		subtitleLabelStyle = new GUIStyle (mySkin.label);
		subtitleLabelStyle.fontSize = 20;

		partTitleStyle = new GUIStyle (EditorStyles.foldout);
		partTitleStyle.font = myFont;
		partTitleStyle.alignment = TextAnchor.MiddleLeft;
		partTitleStyle.fontStyle = FontStyle.Bold;
		partTitleStyle.fontSize = 15;

		boldLabelStyle = new GUIStyle(mySkin.label);
		boldLabelStyle.alignment = TextAnchor.MiddleLeft;
		boldLabelStyle.font = myFont;
		boldLabelStyle.fontStyle = FontStyle.Bold;

		buttonStyle = new GUIStyle(mySkin.button);

		textAreaStyle = new GUIStyle (GUI.skin.textArea);
		textAreaStyle.alignment = TextAnchor.UpperLeft;
		textAreaStyle.stretchWidth = true;
	}
	#endregion

	// Use this for initialization
	void Start () 
	{
	
	}

	string objName="";
	int startFlag = 0;
	void Update ()
	{
		GameObject copyText = GameObject.Find ("Course Title");
		if (startFlag == 0) {
			courseTitle = copyText.GetComponent<UILabel> ().text;
		}
		if (courseTitle != copyText.GetComponent<UILabel> ().text) {
			copyText.GetComponent<UILabel> ().text = courseTitle;
		}

		copyText = GameObject.Find ("Course Info");
		if (startFlag == 0) {
			courseInfo = copyText.GetComponent<UILabel> ().text;
		}
		if (courseInfo != copyText.GetComponent<UILabel> ().text) {
			copyText.GetComponent<UILabel> ().text = courseInfo;
		}

		GameObject airBus = GameObject.Find ("AirBus");
		if (startFlag == 0) {
			modelPosition = airBus.transform.localPosition;
			modelRotation = airBus.transform.eulerAngles;
			modelScale = airBus.transform.localScale;
			startFlag = 1;
		}

		if (modelPosition != airBus.transform.localPosition) {
			airBus.transform.localPosition = modelPosition;
		}
		if (modelRotation != airBus.transform.eulerAngles) {
			airBus.transform.eulerAngles = modelRotation;
		}
		if (modelScale != airBus.transform.localScale) {
			airBus.transform.localScale = modelScale;
		}
		/*
		if (partsName != "") 
		{
			GameObject partNew = GameObject.Find (partsName);
			if (partNew && partsPosition != partNew.transform.localPosition) {
				partNew.transform.localPosition = partsPosition;
			}
			if (partNew && partsRotation != partNew.transform.eulerAngles) {
				partNew.transform.eulerAngles = partsRotation;
			}
			if (partNew && partsScale != partNew.transform.localScale) {
				partNew.transform.localScale = partsScale;
			}
		}
		*/

	}
}
