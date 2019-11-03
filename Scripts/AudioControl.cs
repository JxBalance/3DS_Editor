using UnityEngine;
using System.Collections;

public class AudioControl : MonoBehaviour {
    
	public void ChooseList(){
		gameObject.GetComponent<UIButton> ().isEnabled = false;
		GameObject deepList = GameObject.Find ("AudioListDeep");
		deepList.GetComponent<UISprite> ().enabled = true;
		deepList.transform.localPosition = new Vector3 (-367.3f, transform.localPosition.y, 0f);
		foreach (Transform child in transform) {
			if (child.name == "AudioPlay") {
				child.GetComponent<UISprite> ().enabled = false;
			} else if (child.name == "AudioPause") {
				child.GetComponent<UISprite> ().enabled = true;
			} else if (child.name == "AudioLabel") {
				child.GetComponent<UILabel> ().text = "正在播放：" + child.GetComponent<UILabel> ().text;
				child.GetComponent<UILabel> ().fontStyle = FontStyle.Bold;
				child.localPosition = new Vector3 (9f,0f,0f);
			} else if (child.name == "AudioSubLabel") {
				child.GetComponent<UILabel> ().enabled = false;
			}
		}
		GameObject mainScript = GameObject.Find ("Scripts");
		mainScript.GetComponent<MainSystemScript> ().StopAudio();
		mainScript.GetComponent<MainSystemScript> ().PlayAudio (gameObject.name);

		Transform parentList = gameObject.transform.parent;
		foreach(Transform childList in parentList){
			if (childList.name != gameObject.name && childList.name != "AudioListDeep") {
				foreach (Transform child in childList) {
					if (child.name == "AudioPlay") {
						child.GetComponent<UISprite> ().enabled = true;
					} else if (child.name == "AudioPause") {
						child.GetComponent<UISprite> ().enabled = false;
					} else if (child.name == "AudioLabel") {
						string n = childList.name.Replace("AudioList","");
						child.GetComponent<UILabel> ().text = "录音" + n;
						child.GetComponent<UILabel> ().fontStyle = FontStyle.Normal;
						child.localPosition = new Vector3 (-167f,0f,0f);
					} else if (child.name == "AudioSubLabel") {
						child.GetComponent<UILabel> ().enabled = true;
					}		
					if(childList)
						childList.GetComponent<UIButton> ().isEnabled = true;
				}
			}
		}

	}

	public void ChooseDeepList(){
		GetComponent<UISprite> ().enabled = false;
		int n = 1+(14 - (int)transform.localPosition.y) / 54;
		GameObject choosenList = GameObject.Find ("AudioList"+n);
		while (!choosenList || (choosenList.transform.localPosition.y != gameObject.transform.localPosition.y) ) {
			n++;
			choosenList = GameObject.Find ("AudioList"+n);
			//Debug.Log (n);
			if (n > 100)
				break;
		}
		if (choosenList) {
			choosenList.GetComponent<UIButton> ().isEnabled = true;
			foreach (Transform child in choosenList.transform) {
				if (child.name == "AudioPlay") {
					child.GetComponent<UISprite> ().enabled = true;
				} else if (child.name == "AudioPause") {
					child.GetComponent<UISprite> ().enabled = false;
				} else if (child.name == "AudioLabel") {
					child.GetComponent<UILabel> ().text = "录音" + n;
					child.GetComponent<UILabel> ().fontStyle = FontStyle.Normal;
					child.localPosition = new Vector3 (-167f,0f,0f);
				} else if (child.name == "AudioSubLabel") {
					child.GetComponent<UILabel> ().enabled = true;
				}		
			}
		}

		GameObject mainScript = GameObject.Find ("Scripts");
		mainScript.GetComponent<MainSystemScript> ().StopAudio ();
	}

	public void DeleteAudio(){
		GameObject parentList = GameObject.Find (gameObject.transform.parent.name);
		GameObject deepList = GameObject.Find ("AudioListDeep");
		if (parentList.transform.localPosition.y == deepList.transform.localPosition.y) {
			deepList.GetComponent<UISprite> ().enabled = false;
		}
		float n = parentList.transform.localPosition.y;
		Destroy (parentList);
		GameObject audioScrollView = GameObject.Find ("Audio Scroll View");
		foreach (Transform child in audioScrollView.transform) {
			if (child.name != "AudioListDeep") {
				if (child.localPosition.y < n) {
					child.localPosition = new Vector3 (-367.3f, (child.localPosition.y+54f), 0f);
					if (child.GetComponent<UISprite> ().color.r == 1) {
						child.GetComponent<UISprite> ().color = new Color (101f / 255f, 101f / 255f, 101f / 255f);
						child.GetComponent<UIButtonColor>().defaultColor = new Color (101f / 255f, 101f / 255f, 101f / 255f, 160f / 255f);
					}
					else {
						child.GetComponent<UISprite> ().color = new Color (1,1,1);
						child.GetComponent<UIButtonColor> ().defaultColor = new Color (1, 1, 1, 160f / 255f);
					}
				}
			}
		}
		GameObject mainScript = GameObject.Find ("Scripts");
		mainScript.GetComponent<MainSystemScript> ().DeleteAudio (parentList.name);

	}


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
