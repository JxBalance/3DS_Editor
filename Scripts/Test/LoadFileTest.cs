using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Text;

public class LoadFileTest : MonoBehaviour {

	public TextAsset testText;
	public GameObject testLabel;
	public GameObject testPic;
	public GameObject testVideo;

	public GameObject testButton;
	public GameObject testPlayButton;
	public AudioSource audioSound;

	MovieTexture videoSource;

	public void testStart(){
		testLabel.GetComponent<UILabel> ().text = testText.text;
		//testPic.GetComponent<UISprite>().spriteName = "Engine";
		//videoSource = (MovieTexture)Resources.Load ("Engine Introduction");
		//testVideo.GetComponent<UITexture> ().mainTexture = videoSource;
		//videoSource.Play ();
		//TextAsset assetText = (TextAsset)Resources.Load ("AirPipe 1");
		//testLabel.GetComponent<UILabel> ().text = assetText.text;

		string m_path = Application.streamingAssetsPath;
		string m_name = "AirPipe 1.txt";
		string ttttText = "";
		ttttText = fnLoadFile (m_path, m_name);
		//Debug.Log (ttttText);

		testLabel.GetComponent<UILabel> ().text = ttttText;

		fnCreateFile (m_path,"AirPipe 2.txt","I Am Air Pipe 2...............");
		//Debug.Log ("HERE!?!??!?!?!??!");

	}

	void PlayAudioTest(GameObject button){
		string filename = "AudioTest11.wav";
		string filepath = "file://"+Application.streamingAssetsPath + "/" + filename;
		//var filepath = Path.Combine(Application.streamingAssetsPath,filename);
		//Debug.Log (filepath);
		StartCoroutine( LoadAudio(filepath) );/*
		if (testFlag == 0) {
			Debug.Log ("WHAT?!?!?!??!?!??!");
			StartCoroutine( LoadAudio(filepath) );
			testFlag++;
		}*/
		audioSound.clip = clip;
		audioSound.Play ();
		Debug.Log ("Finishi?!?!?!??!");

	}

	IEnumerator LoadAudio(string url){
		Debug.Log ("HERE?!?!??!");
		WWW www = new WWW (url);
		yield return www;
		if (www.error != null) {
			Debug.Log("Error: " + www.error);  
		}
		clip = www.GetAudioClip();
		//Debug.Log (www);
	}

	int i = 1;
	public const int SamplingRate = 8000; 
	private AudioClip clip;  
	private byte[] recordData; 
	void AddTestAudio(GameObject button,bool flag){
		if (flag) {
			Microphone.End(null);//这句可以不需要，但是在开始录音以前调用一次是好习惯 
			clip = Microphone.Start(null,false,10,SamplingRate); 
			Debug.Log ("HERE?!!!!!!!! 02 "+System.DateTime.Now);
		} 
		else {
			int audioLength;//录音的长度，单位为秒，ui上可能需要显示  
			int lastPos = Microphone.GetPosition(null);  
			if(Microphone.IsRecording(null))	//录音小于10秒  
			{
				audioLength = lastPos/SamplingRate;//录音时长  
			}
			else
			{  
				audioLength = 10;  
			}  
			Microphone.End(null);//此时录音结束，clip已可以播放了  

			if (audioLength < 1.0f) 
			{
				return;//录音小于1秒就不处理了  
			}

			SavWav.Save ("AudioTest1"+i,clip);
			i++;
			//PlayAudioTest (testPlayButton);
		}
	}

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
			//fnDeleteFile (sPath,sName);		//如果此文件存在则先删除再重新创建
			t_sStreamWriter = t_fFileInfo.CreateText ();
		}
		t_sStreamWriter.WriteLine (nData);//以行的形式写入信息
		t_sStreamWriter.Close();//关闭流
		t_sStreamWriter.Dispose();//销毁流
		//Debug.Log("FINISH?!!!");
	}

	void Awake()
	{
		UIEventListener.Get (testButton).onPress = AddTestAudio;
		UIEventListener.Get (testPlayButton).onClick = PlayAudioTest;

	}

	// Use this for initialization
	void Start () {
		//系统开始时调用一次麦克风，以防止系统运行期间调用麦克风卡顿
		AudioClip clip;
		clip = Microphone.Start(null,false,10,SamplingRate); 
		Debug.Log ("START"+" "+System.DateTime.Now);
		Microphone.End (null);

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
