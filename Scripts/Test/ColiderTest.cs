using UnityEngine;
using System.Collections;

public class ColiderTest : MonoBehaviour {


	bool isCollision;
	// Use this for initialization
	void Start () {
		isCollision = false;
	}

	void OnCollisionEnter (Collision e){
		Debug.Log (e.gameObject.name);
		if(e.gameObject.name != "DrawCall_0529_047")
			isCollision = true;
	}



	float angY;
	float pi = Mathf.PI;
	float sin, cos;

	void Update () {
		if (!isCollision) {

			if(Input.GetKey(KeyCode.W))    //.GetKey 是必须一直按下，.GetKeyDown是只需按下就可以进行
			{
				angY = transform.eulerAngles.y - 90;
				sin = Mathf.Sin (pi*angY/180);
				cos = Mathf.Cos (pi*angY/180);
				//camera.transform.eulerAngles = new Vector3 (0f,0f,180f);
				transform.localPosition = new Vector3(transform.localPosition.x+(0.15f*cos),
					transform.localPosition.y,
					transform.localPosition.z-(0.15f*sin));
			}
			else if(Input.GetKey(KeyCode.S))  
			{
				angY = transform.eulerAngles.y - 90;
				sin = Mathf.Sin (pi*angY/180);
				cos = Mathf.Cos (pi*angY/180);
				//camera.transform.eulerAngles = new Vector3 (0f,0f,180f);
				transform.localPosition = new Vector3(transform.localPosition.x-(0.15f*cos),
					transform.localPosition.y,
					transform.localPosition.z+(0.15f*sin));
			}
			else if(Input.GetKey(KeyCode.A))  
			{
				angY = transform.eulerAngles.y - 90;
				sin = Mathf.Sin (pi*angY/180);
				cos = Mathf.Cos (pi*angY/180);
				//camera.transform.eulerAngles = new Vector3 (0f,0f,180f);
				transform.localPosition = new Vector3(transform.localPosition.x+(0.15f*sin),
					transform.localPosition.y,
					transform.localPosition.z+(0.15f*cos));
			}
			else if(Input.GetKey(KeyCode.D)) 
			{
				angY = transform.eulerAngles.y - 90;
				sin = Mathf.Sin (pi*angY/180);
				cos = Mathf.Cos (pi*angY/180);
				//camera.transform.eulerAngles = new Vector3 (0f,0f,180f);
				transform.localPosition = new Vector3(transform.localPosition.x-(0.15f*sin),
					transform.localPosition.y,
					transform.localPosition.z-(0.15f*cos));
			}

		}
		isCollision = false;
	}
}
