using UnityEngine;
using System.IO;
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;


public class OpenNoteWindow : EditorWindow
{
	public static OpenNoteWindow _instance;
	public GUISkin mySkin;
	private GUIStyle titleLabelStyle, subtitleLabelStyle, toolBarStyle, buttonStyle, 
	smallLabelStyle, partTitleStyle, boldLabelStyle,
	textAreaStyle;
	public Font myFont;

	private Vector2 scroll;

	private MovieTexture noteVideo;
	private Texture2D notePicture;
	private AudioClip audioNote;

	private string N_Text = "";

	private bool isShowObjectPart = true;
	private bool isShowTextPart = true;
	private bool isShowAudioPart = true;
	private bool isShowVideoPart = true;
	private bool isShowPicPart = true;

	public string objTitle = "";
	public string objName = "";

	private string m_path;
	private string m_name = "";

	public UIAtlas atlas;

	static string titleNameText = "Title Saved.txt";	//定义一个静态变量，代表预制保存标题文档的名字
	static string videoNameText = "Video Saved.txt";	//定义一个静态变量，代表预制导入视频文档的名字
	static string pictureNameText = "Picture Saved.txt";	//定义一个静态变量，代表预制导入图片文档的名字


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
		NotePage();
	}

	#region 标注页面
	private void NotePage()
	{
		InitGUIStyle();

		GUILayout.Space(25);
		GUILayout.Label("初始标注编辑器", subtitleLabelStyle);
		GUILayout.Space(15);

		#region 部件信息
		GUILayout.BeginHorizontal();
		{
			isShowObjectPart = EditorGUILayout.Foldout(isShowObjectPart, "部件组信息", partTitleStyle); // 定义折叠菜单
			GUILayout.Space(position.width*0.5f);
			GUI.skin.label.fontStyle = FontStyle.Bold;
			GUILayout.Label(objName);
			GUI.skin.label.fontStyle  = FontStyle.Normal;
		}
		GUILayout.EndHorizontal();
		if(isShowObjectPart)
		{
			GUILayout.Space(10);
			GUILayout.BeginHorizontal();
			{
				GUILayout.Space(40);
				GUILayout.Label("部件组名称：",GUILayout.Width(70));
				objTitle = GUILayout.TextField(objTitle,GUILayout.Width(position.width*0.7f-140));
				GUILayout.Space(position.width*0.05f);
				if(	GUILayout.Button("保存名称",GUILayout.Width(position.width*0.25f)) )
				{
					saveTitle();
				}
				GUILayout.Space(20);
			}
			GUILayout.EndHorizontal();
		}
		GUILayout.Space(20);
		#endregion

		#region 文本
		isShowTextPart = EditorGUILayout.Foldout(isShowTextPart, "文本", partTitleStyle); // 定义折叠菜单
		if (isShowTextPart)
		{
			GUILayout.Space(10);
			GUILayout.BeginHorizontal();
			{
				GUILayout.Space(20);
				scroll = EditorGUILayout.BeginScrollView(scroll,GUILayout.Height(80));
				N_Text = EditorGUILayout.TextArea(N_Text,textAreaStyle,GUILayout.Width(position.width*0.6f),GUILayout.ExpandHeight(true));
				EditorGUILayout.EndScrollView();
				if(	GUILayout.Button("保存文本",GUILayout.Width(position.width*0.25f)) )
				{
					SaveText();
				}
				GUILayout.Space(20);
			}
			GUILayout.EndHorizontal();

		}
		GUILayout.Space(20);
		#endregion

		#region 语音
		isShowAudioPart = EditorGUILayout.Foldout(isShowAudioPart, "语音", partTitleStyle); // 定义折叠菜单
		if (isShowAudioPart)
		{
			GUILayout.Space(10);
			GUILayout.BeginHorizontal();
			{
				GUILayout.Space(20);
				//临时
				audioNote = (AudioClip)EditorGUILayout.ObjectField(audioNote,typeof(AudioClip),true);
				GUILayout.Button("删除语音",GUILayout.Width(position.width*0.25f));
				GUILayout.Space(20);
			}
			GUILayout.EndHorizontal();

		}
		GUILayout.Space(20);
		#endregion

		#region 视频
		isShowVideoPart = EditorGUILayout.Foldout(isShowVideoPart, "视频", partTitleStyle); // 定义折叠菜单
		if (isShowVideoPart)
		{
			GUILayout.Space(10);
			GUILayout.BeginHorizontal();
			{
				GUILayout.Space(20);
				noteVideo = (MovieTexture)EditorGUILayout.ObjectField(noteVideo, typeof(MovieTexture), true);
				if( GUILayout.Button("导入视频",GUILayout.Width(position.width*0.25f)) )
				{
					if(noteVideo) SaveVideo(noteVideo.name);
				}
				GUILayout.Space(20);
			}
			GUILayout.EndHorizontal();

		}
		GUILayout.Space(20);
		#endregion

		#region 图片
		isShowPicPart = EditorGUILayout.Foldout(isShowPicPart, "图片", partTitleStyle); // 定义折叠菜单
		if (isShowPicPart)
		{
			GUILayout.Space(10);
			GUILayout.BeginHorizontal();
			{
				GUILayout.Space(20);
				notePicture = (Texture2D)EditorGUILayout.ObjectField(notePicture, typeof(Texture2D), true);
				if( GUILayout.Button("导入图片",GUILayout.Width(position.width*0.25f)) )
				{
					if(notePicture) SavePicture(notePicture.name);
				}
				GUILayout.Space(20);
			}
			GUILayout.EndHorizontal();

		}
		GUILayout.Space(20);
		#endregion

		#region 拷贝粘贴与删除
		GUILayout.Space(20);
		GUILayout.BeginHorizontal();
		{
			GUILayout.Space(20);
			if( GUILayout.Button("拷贝所有标注",GUILayout.Width(position.width*0.3f)) )
			{
				if(objName != "") CopyAllNotes(objName);
			}
			if( GUILayout.Button("粘贴所有标注",GUILayout.Width(position.width*0.3f)) )
			{
				if(objName != "") PasteAllNotes(objName);
			}
			GUILayout.Button("删除所有标注",GUILayout.Width(position.width*0.3f));
			GUILayout.Space(20);
		}
		GUILayout.EndHorizontal();
		#endregion


	}
	#endregion

	#region 拷贝，粘贴，删除所有标注
	string copyNoteName = "";
	private void CopyAllNotes(string coName)
	{
		copyNoteName = coName;
	}

	private void PasteAllNotes(string paName)
	{
		string m_path;
		string m_name;
		m_path = Application.dataPath;

		//复制标题
		m_name = titleNameText;
		string copyNoteTitle = fnLoadFile (m_path,m_name);
		int n = copyNoteTitle.IndexOf (copyNoteName,0);
		if (n != -1) {
			int m = copyNoteTitle.IndexOf ('-', n);
			int k = copyNoteTitle.IndexOf (';', n);
			copyNoteTitle = copyNoteTitle.Substring (m + 1, k - m - 1);
			objTitle = copyNoteTitle;
			saveTitle ();
		} 
		else 
		{
			objTitle = "";
		}

		//复制文本
		m_name = copyNoteName + ".txt";
		N_Text = fnLoadFile ("Assets/Resources",m_name);
		SaveText ();

		//复制视频
		m_name = videoNameText;
		string copyNoteVideo = fnLoadFile (m_path,m_name);
		n = copyNoteVideo.IndexOf (copyNoteName,0);
		if (n != -1) {
			int m = copyNoteVideo.IndexOf ('-', n);
			int k = copyNoteVideo.IndexOf (';', n);
			copyNoteVideo = copyNoteVideo.Substring (m + 1, k - m - 1);
			SaveVideo (copyNoteVideo);
		} 

		//复制图片
		m_name = pictureNameText;
		string copyNotePic = fnLoadFile (m_path,m_name);
		n = copyNotePic.IndexOf (copyNoteName,0);
		if (n != -1) {
			int m = copyNotePic.IndexOf ('-', n);
			int k = copyNotePic.IndexOf (';', n);
			copyNotePic = copyNotePic.Substring (m + 1, k - m - 1);
			SavePicture (copyNotePic);
		} 

	}
	#endregion


	#region 存取文件
	//存入（创建）文件
	private void fnCreateFile(string sPath, string sName, string nData)
	{
		StreamWriter t_sStreamWriter;//文件流信息
		FileInfo t_fFileInfo = new FileInfo(sPath + "//" + sName);
		if (!t_fFileInfo.Exists) 
		{
			t_sStreamWriter = t_fFileInfo.CreateText ();//如果文件不存在则创建
		} 
		else 
		{
			fnDeleteFile (sPath,sName);		//如果此文件存在则先删除再重新创建
			t_sStreamWriter = t_fFileInfo.CreateText ();
		}
		t_sStreamWriter.WriteLine (nData);//以行的形式写入信息
		t_sStreamWriter.Close();//关闭流
		t_sStreamWriter.Dispose();//销毁流
		//Debug.Log("FINISH?!!!");
	}

	//读取文件内容
	private string fnLoadFile(String sPath, string sName)
	{
		StreamReader t_sStreamReader = null;//使用流的形式读取
		try
		{
			t_sStreamReader = File.OpenText(sPath + "//" + sName);
		}
		catch(Exception ex) 
		{
			return null;
		}
		string tText;
		tText = t_sStreamReader.ReadToEnd();

		t_sStreamReader.Close ();//关闭流
		t_sStreamReader.Dispose ();//销毁流
		return tText;
	}

	//删除文件
	private void fnDeleteFile(string sPath, string sName)
	{
		File.Delete (sPath + "//" + sName);
	}
	#endregion

	#region 存取文本信息
	private void SaveText()
	{
		string m_path;
		string m_name;
		//m_path = Application.dataPath;//默认Assests文件夹
		m_path = "Assets/Resources";	//放入Resources文件夹便于发布时读取
		m_name = objName + ".txt";
		fnCreateFile (m_path,m_name,N_Text);

	}

	private void ReadText()
	{
		string m_path;
		string m_name;
		//m_path = Application.dataPath;//默认Assests文件夹
		m_path = "Assets/Resources";
		m_name = objName + ".txt";
		N_Text = fnLoadFile (m_path, m_name);
	}
	#endregion

	#region 保存标题信息
	private void saveTitle ()
	{
		m_path = Application.dataPath;
		m_name = titleNameText;	//预制的一个用来保存标题集合的txt

		//读取标题集合，看是否有已存标题，若有则用新标题替代
		string titleCollect = fnLoadFile(m_path,m_name);

		//从字符串第0位开始检查其中是否包括objName
		int n = titleCollect.IndexOf (objName,0);	
		if (n != -1) {
			//从第n位开始找第一个出现的';'
			int m = titleCollect.IndexOf (";", n);	
			//获取从第n位开始到m-n位前的这段字符串，即原物体-标题
			string reName = titleCollect.Substring (n, (m - n));	
			//将旧的物体-标题reName换成新的物体-标题
			titleCollect = titleCollect.Replace (reName, (objName + "-" + objTitle));	
		} 
		else 
		{
			//没有已存标题，将新的物体-标题加到最尾
			titleCollect = titleCollect.Replace ("#", (objName + "-" + objTitle + ";#"));	
		}
		fnCreateFile (m_path,m_name,titleCollect);
	}
	#endregion

	#region 保存视频信息
	private void SaveVideo(string vName)
	{

		m_path = Application.dataPath;
		m_name = videoNameText;	

		string videoCollect = fnLoadFile(m_path,m_name);

		int n = videoCollect.IndexOf (objName,0);	
		if (n != -1) {
			int m = videoCollect.IndexOf (";", n);	
			string reName = videoCollect.Substring (n, (m - n));	
			if (vName != "") 
			{
				videoCollect = videoCollect.Replace (reName, (objName + "-" + vName));	
			}
			else
			{
				videoCollect = videoCollect.Replace ((reName+";"), "");	
			}
		} 
		else 
		{
			if (vName != "") 
			{
				videoCollect = videoCollect.Replace ("#", (objName + "-" + vName + ";#"));	
			}
		}
		fnCreateFile (m_path,m_name,videoCollect);
	}
	#endregion

	#region 保存图片信息
	private void SavePicture(string picName)
	{
		m_path = Application.dataPath;
		m_name = pictureNameText;	

		string picCollect = fnLoadFile(m_path,m_name);

		int n = picCollect.IndexOf (objName,0);	
		if (n != -1) {
			int m = picCollect.IndexOf (";", n);	
			string reName = picCollect.Substring (n, (m - n));	
			if (picName != "") 
			{
				picCollect = picCollect.Replace (reName, (objName + "-" + picName));	
			}
			else
			{
				picCollect = picCollect.Replace ((reName+";"), "");	
			}
		} 
		else 
		{
			if (picName != "") 
			{
				picCollect = picCollect.Replace ("#", (objName + "-" + picName + ";#"));	
			}
		}
		fnCreateFile (m_path,m_name,picCollect);
	}
	#endregion



	#region 界面变量初始化
	private void InitGUIStyle()
	{
		titleLabelStyle = new GUIStyle(mySkin.label);
		titleLabelStyle.fontSize = 30;
		subtitleLabelStyle = new GUIStyle(mySkin.label);
		subtitleLabelStyle.fontSize = 20;
		smallLabelStyle = new GUIStyle(mySkin.label);
		smallLabelStyle.fontSize = 10;

		partTitleStyle = new GUIStyle(EditorStyles.foldout);
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
	void Start () {

	}

	string videoName = "";

	void Update () 
	{
		//上一帧物体名,视频名,照片名备份
		string objNameCopy = objName;

		//获取场景中选中物体名
		GameObject objSel = Selection.activeGameObject;
		if (objSel && objSel.layer == 8) 
		{
			objName = objSel.name;
			//Debug.Log (objName);
		} 
		else 
		{
			objName = "";
		}

		//若上一帧物体名与这一帧物体名不同，则改变标题与文本
		if (objName != "" && objName != objNameCopy) 
		{
			//检查该物体是否有已存标题
			m_path = Application.dataPath;
			m_name = titleNameText;
			string titleCollect = fnLoadFile (m_path,m_name);
			//Debug.Log (titleCollect);
			int n = titleCollect.IndexOf (objName, 0);	//从字符串第0位开始检查其中是否包括objName
			if ( n == -1) //如果不存在标题，则标题为空
			{
				objTitle = "";
			}
			else 
			{
				int m = titleCollect.IndexOf (';',n);	//从第n位开始找第一个出现的';'
				int k = titleCollect.IndexOf ('-',n);	//从第n位开始找第一个出现的'-'
				objTitle = titleCollect.Substring(k+1,m-k-1);	//提取出'-'和';'间的字符串,即为标题
			}

			//检查此物体是否有已存信息
			m_name = objName + ".txt";
			FileInfo t_fFileInfo = new FileInfo("Assets/Resources//" + m_name);	//检查是否存在保存此物体信息的文本
			//FileInfo t_fFileInfo = new FileInfo("Assets/Resources//" + m_name);
			if (t_fFileInfo.Exists) 
			{
				//Debug.Log ("EXIST!!!!!!!!!!!!!!!!");
				ReadText ();
			} 
			else 
			{
				//Debug.Log ("NO exist................");
				N_Text = "";
			}

			//检查此物体是否有对应视频	
			m_name = videoNameText;
			string videoCollect = fnLoadFile (m_path,m_name);
			//Debug.Log (videoCollect);
			n = videoCollect.IndexOf (objName, 0);
			if ( n == -1) 
			{
				noteVideo = null;
			}
			else 
			{
				int m = videoCollect.IndexOf (';',n);	
				int k = videoCollect.IndexOf ('-',n);	
				string noteVideoName = videoCollect.Substring(k+1,m-k-1);	

				//LoadAssetAtPath 只能编辑器模式下！！！！
				noteVideo = (MovieTexture)AssetDatabase.LoadAssetAtPath("Assets/"+noteVideoName+".ogv",typeof(MovieTexture));

				if (noteVideo) {
					AssetDatabase.MoveAsset ("Assets/" + noteVideoName + ".ogv", "Assets/Resources/" + noteVideoName + ".ogv");
				} 
				else 
				{
					noteVideo = (MovieTexture)Resources.Load (noteVideoName);
				}

				//Debug.Log("noteVideo : "+noteVideo);
				Resources.UnloadAsset (noteVideo);
			}

			//检查此物体是否有对应图片	
			m_name = pictureNameText;
			string picCollect = fnLoadFile (m_path,m_name);

			n = picCollect.IndexOf (objName, 0);
			if ( n == -1) 
			{
				notePicture = null;
			}
			else 
			{
				int m = picCollect.IndexOf (';',n);	
				int k = picCollect.IndexOf ('-',n);	
				string notePicName = picCollect.Substring(k+1,m-k-1);	
				//Debug.Log ("notePicName : "+notePicName);

				//LoadAssetAtPath 只能编辑器模式下！！！！
				notePicture = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/"+notePicName+".png",typeof(Texture2D));

				if (notePicture) {
					UIAtlasMaker.AddOrUpdate (atlas,notePicture);
					AssetDatabase.MoveAsset ("Assets/" + notePicName + ".png", "Assets/Resources/" + notePicName + ".png");
				} 
				else 
				{
					notePicture = (Texture2D)Resources.Load (notePicName);
				}

				//Debug.Log("notePicture : "+notePicture);
				Resources.UnloadAsset (notePicture);
			}

		}
		/*
		//若有选中视频，且同个物体的上一帧视频名与这一帧视频名不同，则改变存储视频信息
		if (noteVideo) 
		{
			if (videoName != noteVideo.name && objName == objNameCopy) 
			{
				videoName = noteVideo.name;
				SaveVideo (videoName);
			}
		}
		else 
		{
			if (videoName != "" && objName == objNameCopy) 
			{
				videoName = "";
				Debug.Log ("videoName is NONE!!!!!!!!!!!" + videoName);
				SaveVideo (videoName);
			}
		}
		*/

		//string notePictureCopy = notePicture.name;

	}

}
