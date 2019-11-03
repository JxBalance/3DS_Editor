using UnityEngine;
using System.Collections;

public class SetItActive : MonoBehaviour {

	public void SetActive(){
		gameObject.SetActive (true);
	}

	public void SetQuiet(){
		gameObject.SetActive (false);
	}

	public void SetButtonDisabled(){
		gameObject.GetComponent<UIButton> ().isEnabled = false;
	}

	public void SetButtonEnabled(){
		gameObject.GetComponent<UIButton> ().isEnabled = true;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
