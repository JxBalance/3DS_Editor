using UnityEngine;
using System.Collections;

public class ChangeMouseCursor : MonoBehaviour {

	public Texture2D mouseTexture01;
	public Texture2D mouseTexture021;
	public Texture2D mouseTexture022;
	public Texture2D mouseTexture023;
	public Texture2D mouseTexture024;

	int mouseType = -1;

	public void ChangeFlag( int type){

		mouseType = type;

		if (mouseType == 0) {
			CancelInvoke ("mouseFlash");
			Cursor.SetCursor (mouseTexture01, Vector2.zero, CursorMode.Auto);
		} 
		else {
			Cursor.SetCursor (mouseTexture021, Vector2.zero, CursorMode.Auto);
			mouseRepeat = 1;
			InvokeRepeating ("mouseFlash", 0.1f, 0.1f);
		}

	}

	int mouseRepeat = 1;
	void mouseFlash(){
		if (mouseRepeat == 1) {
			mouseRepeat++;
			Cursor.SetCursor (mouseTexture022, Vector2.zero, CursorMode.Auto);
		} 
		else if(mouseRepeat == 2){
			mouseRepeat++;
			Cursor.SetCursor (mouseTexture023, Vector2.zero, CursorMode.Auto);
		}
		else if(mouseRepeat == 3){
			mouseRepeat++;
			Cursor.SetCursor (mouseTexture024, Vector2.zero, CursorMode.Auto);
		}
		else if(mouseRepeat == 4){
			mouseRepeat++;
		}
		else if(mouseRepeat == 5){
			mouseRepeat = 1;
		}
	}

	void Start(){
		ChangeFlag (0);
	}

	void Update(){
		
	}


	/* 第二种更换鼠标箭头方式：隐藏鼠标，画面上出现代替的鼠标图片
	public Texture mouseTexture1;
	public Texture mouseTexture2;

	int mouseType = 0;

	public void Invisible(){
		Cursor.visible = false;
	}

	public void ChangeType(){
		if (mouseType == 0) {
			mouseType = 1;
		} 
		else {
			mouseType = 0;
		}
	}

	// Use this for initialization
	void Start () {
		Invisible ();	
	}
	
	// Update is called once per frame
	void Update () {
		Invisible ();
	}

	void OnGUI(){
		if (mouseType == 0) {
			Vector3 mousePos = Input.mousePosition;
			GUI.DrawTexture (new Rect (mousePos.x - 15f, Screen.height - mousePos.y, mouseTexture1.width/10, mouseTexture1.height/10), mouseTexture1);
		}
		else if (mouseType == 1) {
			Vector3 mousePos = Input.mousePosition;
			GUI.DrawTexture (new Rect (mousePos.x - 15f, Screen.height - mousePos.y, mouseTexture2.width/10, mouseTexture2.height/10), mouseTexture2);
		}
	}
	*/

}
