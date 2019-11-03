using UnityEngine;
using System.Collections;

public class CameraWalk : MonoBehaviour {

	public GameObject camera;

	// Use this for initialization
	void Start () {
		Debug.Log ("HAHAHA");
	}

	/*
	float angY;	//摄像机角度
	float pi = Mathf.PI;
	float sin, cos;	//保存旋转角度的sin值和cos值
	void Update () 
	{
		//.GetKey当一直按下W时进行移动
		if(Input.GetKey(KeyCode.W))    
		{
			//获取摄像机相对正前方90度的旋转角
			angY = camera.transform.eulerAngles.y - 90; 
			//计算此旋转角的sin值和cos值
			sin = Mathf.Sin (pi*angY/180);	
			cos = Mathf.Cos (pi*angY/180);
			float axisX = camera.transform.localPosition.x;
			float axisY = camera.transform.localPosition.y;
			float axisZ = camera.transform.localPosition.z;
			//将sin值和cos值乘上移动系数分别赋给x和z轴的位移值
			camera.transform.localPosition = 
				new Vector3(axisX+(0.15f*cos),axisY,
							axisZ-(0.15f*sin));
		}
		//省略其他按键
	}
	*/
	 
	float angY;
	float pi = Mathf.PI;
	float sin, cos;
	void Update () {
		if(Input.GetKey(KeyCode.W))    //.GetKey 是必须一直按下，.GetKeyDown是只需按下就可以进行
		{
			angY = camera.transform.eulerAngles.y - 90;
			sin = Mathf.Sin (pi*angY/180);
			cos = Mathf.Cos (pi*angY/180);
			//camera.transform.eulerAngles = new Vector3 (0f,0f,180f);
			camera.transform.localPosition = new Vector3(camera.transform.localPosition.x+(0.15f*cos),
				camera.transform.localPosition.y,
				camera.transform.localPosition.z-(0.15f*sin));
		}

		else if(Input.GetKey(KeyCode.S))  
		{
			angY = camera.transform.eulerAngles.y - 90;
			sin = Mathf.Sin (pi*angY/180);
			cos = Mathf.Cos (pi*angY/180);
			//camera.transform.eulerAngles = new Vector3 (0f,0f,180f);
			camera.transform.localPosition = new Vector3(camera.transform.localPosition.x-(0.15f*cos),
				camera.transform.localPosition.y,
				camera.transform.localPosition.z+(0.15f*sin));
		}
		else if(Input.GetKey(KeyCode.A))  
		{
			angY = camera.transform.eulerAngles.y - 90;
			sin = Mathf.Sin (pi*angY/180);
			cos = Mathf.Cos (pi*angY/180);
			//camera.transform.eulerAngles = new Vector3 (0f,0f,180f);
			camera.transform.localPosition = new Vector3(camera.transform.localPosition.x+(0.15f*sin),
				camera.transform.localPosition.y,
				camera.transform.localPosition.z+(0.15f*cos));
		}
		else if(Input.GetKey(KeyCode.D)) 
		{
			angY = camera.transform.eulerAngles.y - 90;
			sin = Mathf.Sin (pi*angY/180);
			cos = Mathf.Cos (pi*angY/180);
			//camera.transform.eulerAngles = new Vector3 (0f,0f,180f);
			camera.transform.localPosition = new Vector3(camera.transform.localPosition.x-(0.15f*sin),
				camera.transform.localPosition.y,
				camera.transform.localPosition.z-(0.15f*cos));
		}
	}
}
