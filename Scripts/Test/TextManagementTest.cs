using UnityEngine;
using System.IO;
using System;
using System.Collections;

public class TextManagementTest : MonoBehaviour {

	private string m_sFileName;//文件名
	private string m_sPath;//路径
	//private ArrayList m_aArray;//文本中每行的内容
	private string m_tText;

	/* 写入（创建）文件
	 * sPath:文件创建目录	* sName:文件的名称 * nData:数据	 */
	void fnCreateFile(string sPath, string sName, string nData)
	{
		StreamWriter t_sStreamWriter;//文件流信息
		FileInfo t_fFileInfo = new FileInfo(sPath + "//" + sName);
		if (!t_fFileInfo.Exists) 
		{
			t_sStreamWriter = t_fFileInfo.CreateText ();//如果文件不存在则创建
		} 
		else 
		{
			t_sStreamWriter = t_fFileInfo.AppendText ();//如果此文件存在则打开
		}
		t_sStreamWriter.WriteLine (nData);//以行的形式写入信息
		t_sStreamWriter.Close();//关闭流
		t_sStreamWriter.Dispose();//销毁流
		Debug.Log("FINISH?!!!");
	}

	/*读取文件内容
	 * path:读取文件的路径	* name:读取文件的名称  */
	string fnLoadFile(String sPath, string sName)
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
		//string t_sLine;//每行的内容
		//ArrayList t_aArrayList = new ArrayList();//容器

		string tText;
		tText = t_sStreamReader.ReadToEnd();
		/*
		while ((t_sLine = t_sStreamReader.ReadLine ()) != null) 
		{
			tText = t_sLine;
			Debug.Log(tText);
			t_aArrayList.Add (t_sLine);//将每一行的内容存入数组链表容器中
		}
		*/
		t_sStreamReader.Close ();//关闭流
		t_sStreamReader.Dispose ();//销毁流

		//return t_aArrayList;//将数组链表容器返回
		return tText;
	}

	/*删除文件
	 * sPath:删除文件的路径	 * sName:删除文件的名称	*/
	void fnDeleteFile(string sPath, string sName)
	{
		File.Delete (sPath + "//" + sName);
	}

	// Use this for initialization
	void Start () {
		m_sFileName = "RCZHAfile.txt";
		m_sPath = Application.dataPath;

		string inputText = "My Name Is RCZHA.\n我的名字叫查睿成";
		fnCreateFile (m_sPath,m_sFileName,inputText);	//写入（创建）文件,默认Assests文件夹

		m_tText = fnLoadFile (m_sPath, m_sFileName);//读取文件内容
		Debug.Log (m_tText);

		//fnDeleteFile (m_sPath, m_sFileName);	//删除文件

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
