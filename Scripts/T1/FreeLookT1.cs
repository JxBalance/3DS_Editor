using UnityEngine;
using System.Collections;

public class FreeLookT1 : MonoBehaviour {

    //方向灵敏度  
    public float sensitivityX = 5F;
    public float sensitivityY = 5F;

    //上下最大视角(Y视角)  
    public float minimumY = -90F;
    public float maximumY = 90F;

    //float rotationY_touch = 0F;

    public float moveSpeed = 1f;
    public float rotateSpeed = 1f;


    // Update is called once per frame
    void Update () {
        
        //鼠标右键控制旋转
        if (Input.GetMouseButton(1))
        {
            //根据鼠标移动的快慢(增量), 获得相机左右旋转的角度(处理X)  
            float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX * rotateSpeed;

            //根据鼠标移动的快慢(增量), 获得相机上下旋转的角度(处理Y)  
            float rotationY = transform.localEulerAngles.x - Input.GetAxis("Mouse Y") * sensitivityY * rotateSpeed;

            //角度限制. rotationY小于min,返回min. 大于max,返回max. 否则返回value   
            //rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

            //总体设置一下相机角度  
            transform.localEulerAngles = new Vector3(rotationY, rotationX, transform.localEulerAngles.z);

        }

        ////ETCInput控制旋转
        //float rotationX_touch = transform.localEulerAngles.y + ETCInput.GetAxisSpeed("HorizontalRight") * sensitivityX * rotateSpeed* 0.1f;
        //float rotationY_touch = transform.localEulerAngles.x + ETCInput.GetAxisSpeed("VerticalRight") * sensitivityY * rotateSpeed* 0.1f;
        //transform.localEulerAngles = new Vector3(rotationY_touch, rotationX_touch, transform.localEulerAngles.z);

        //前后左右
        transform.localPosition += transform.forward*Input.GetAxis("Vertical")*0.1f*moveSpeed;
	    transform.localPosition += transform.right*Input.GetAxis("Horizontal")*0.1f*moveSpeed;

        //transform.localPosition += transform.forward * ETCInput.GetAxisSpeed("VerticalLeft") * 0.3f * moveSpeed;
        //transform.localPosition += transform.right * ETCInput.GetAxisSpeed("HorizontalLeft") * 0.3f * moveSpeed; 
        //向下
        if (Input.GetKey(KeyCode.Q))
	    {
	        transform.localPosition -= transform.up*0.1f*moveSpeed;
	    }
        //向上
	    if (Input.GetKey(KeyCode.E))
	    {
	        transform.localPosition += transform.up*0.1f*moveSpeed;
	    }

    }
    
    //public void LeftEasyTouchCtrl(Vector2 vector2)
    //{
    //    print(vector2);
    //}

    //public void RightEasyTouchCtrl(Vector2 vector2)
    //{
    //    print(vector2);
    //}
}
