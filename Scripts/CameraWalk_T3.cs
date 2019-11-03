using UnityEngine;
using System.Collections;

public class CameraWalk_T3 : MonoBehaviour {


	// Use this for initialization
	void Start () {

	}
	
	float _angY;	//摄像机角度
	float _pi = Mathf.PI;
	float _sin, _cos;   //保存旋转角度的sin值和cos值
    /*
    void Update () 
	{
		//.GetKey当一直按下W时进行移动
		if(Input.GetKey(KeyCode.W))    
		{
			//获取摄像机相对正前方90度的旋转角
			_angY = camera.transform.eulerAngles.y - 90; 
			//计算此旋转角的sin值和cos值
			_sin = Mathf.Sin (_pi*_angY/180);	
			_cos = Mathf.Cos (_pi*_angY/180);
			float axisX = camera.transform.localPosition.x;
			float axisY = camera.transform.localPosition.y;
			float axisZ = camera.transform.localPosition.z;
			//将sin值和cos值乘上移动系数分别赋给x和z轴的位移值
			camera.transform.localPosition = 
				new Vector3(axisX+(0.15f*_cos),axisY,
							axisZ-(0.15f*_sin));
		}
		//省略其他按键
	}*/
	
	
	float angY,angX;
	float pi = Mathf.PI;
	float sin, cos,sinX;
	void Update ()
    {
        angY = transform.eulerAngles.y - 90;
        sin = Mathf.Sin(pi * angY / 180);
        cos = Mathf.Cos(pi * angY / 180);
        angX = transform.eulerAngles.x;
        sinX = Mathf.Sin(pi * angX / 180);
        if (Input.GetKey(KeyCode.W))    //.GetKey 是必须一直按下，.GetKeyDown是只需按下就可以进行
		{
            transform.localPosition = new Vector3(transform.localPosition.x + (0.15f * cos),
                transform.localPosition.y - (0.15f * sinX),
                transform.localPosition.z - (0.15f * sin));
		}

		else if(Input.GetKey(KeyCode.S))  
		{
            transform.localPosition = new Vector3(transform.localPosition.x - (0.15f * cos),
                transform.localPosition.y + (0.15f * sinX),
                transform.localPosition.z + (0.15f * sin));
		}
		else if(Input.GetKey(KeyCode.A))  
		{
            transform.localPosition = new Vector3(transform.localPosition.x + (0.15f * sin),
                transform.localPosition.y,
                transform.localPosition.z + (0.15f * cos));
		}
		else if(Input.GetKey(KeyCode.D)) 
		{
            transform.localPosition = new Vector3(transform.localPosition.x - (0.15f * sin),
                transform.localPosition.y,
                transform.localPosition.z - (0.15f * cos));
		}
	}
}
