using UnityEngine;
using System.Collections;

public class PipeController : MonoBehaviour {

	public GameObject undercarriageBack;
	public GameObject undercarriageUp;
	public GameObject undercarriageDown;
	public GameObject flapBack;
	public GameObject flapOpen;
	public GameObject oilDrumOut;


	public GameObject undercarriageValve;
	public GameObject underValveCtrl;
	public GameObject flapValveCtrl;

	public GameObject informationLabel;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ShowWaterPipe(){
		undercarriageBack.transform.localPosition = new Vector3 (0f,0f,0f);
		flapBack.transform.localPosition = new Vector3 (0f,0f,0f);
		undercarriageValve.transform.localPosition = new Vector3 (-8.273f,0f,2.393f);
		informationLabel.GetComponent<UILabel> ().text = 
			"起落架与襟翼选择活门处于中立位置，接通动作筒回油";
	}

	public void ShowUnderBack(){
		undercarriageBack.transform.localPosition = new Vector3 (0f, 0f, 0f);
		undercarriageUp.transform.localPosition = new Vector3 (0f, -2000f, 0f);
		undercarriageDown.transform.localPosition = new Vector3 (0f, -2000f, 0f);
		if (flapBack.transform.localPosition.y == 0f) {
			oilDrumOut.transform.localPosition = new Vector3 (0f, -2000f, 0f);
		}
		undercarriageValve.transform.localPosition = new Vector3 (-8.273f, 0f, 2.393f);
		underValveCtrl.transform.eulerAngles = new Vector3 (0f, 0f, 0f);
		informationLabel.GetComponent<UILabel> ().text = 
			"当起落架收放手柄在中立位置，起落架选择活门堵住仅有而接通动作筒回油";
	}

	public void ShowUnderUp(){
		undercarriageUp.transform.localPosition = new Vector3 (0f,0f,0f);
		undercarriageBack.transform.localPosition = new Vector3 (0f,-2000f,0f);
		undercarriageDown.transform.localPosition = new Vector3 (0f,-2000f,0f);
		oilDrumOut.transform.localPosition = new Vector3 (0f,0f,0f);
		undercarriageValve.transform.localPosition = new Vector3 (-8.355f,0f,2.393f);
		underValveCtrl.transform.eulerAngles = new Vector3 (0f, -45f, 0f);
		informationLabel.GetComponent<UILabel> ().text = 
			"当收放手柄在收上位置，选择活门进油并接通动作筒收上端，使起落架收起";
	}

	public void ShowUnderDown(){
		undercarriageDown.transform.localPosition = new Vector3 (0f,0f,0f);
		undercarriageBack.transform.localPosition = new Vector3 (0f,-2000f,0f);
		undercarriageUp.transform.localPosition = new Vector3 (0f,-2000f,0f);
		oilDrumOut.transform.localPosition = new Vector3 (0f,0f,0f);
		undercarriageValve.transform.localPosition = new Vector3 (-8.19f,0f,2.393f);
		underValveCtrl.transform.eulerAngles = new Vector3 (0f, 45f, 0f);
		informationLabel.GetComponent<UILabel> ().text = 
			"当收放手柄在放下位置，选择活门进油并接通动作筒放下端，使起落架放下";
	}

	public void ShowFlapBack(){
		flapBack.transform.localPosition = new Vector3 (0f,0f,0f);
		flapOpen.transform.localPosition = new Vector3 (0f,-2000f,0f);
		if (undercarriageBack.transform.localPosition.y == 0f) {
			oilDrumOut.transform.localPosition = new Vector3 (0f,-2000f,0f);
		}
		flapValveCtrl.transform.eulerAngles = new Vector3 (90f,-135f,0f);
		informationLabel.GetComponent<UILabel> ().text = 
			"当襟翼收放手柄在关闭位置，襟翼选择活门堵住进油而接通液压马达回油";
	}

	public void ShowFlapOpen(){
		flapOpen.transform.localPosition = new Vector3 (0f,0f,0f);
		flapBack.transform.localPosition = new Vector3 (0f,-2000f,0f);
		oilDrumOut.transform.localPosition = new Vector3 (0f,0f,0f);
		flapValveCtrl.transform.eulerAngles = new Vector3 (90f,-45f,0f);
		informationLabel.GetComponent<UILabel> ().text = 
			"当襟翼收放手柄打开，选择活门接通进油，驱动襟翼液压马达，马达带动襟翼收放";
	}

	public void HideWaterPipe(){
		undercarriageBack.transform.localPosition = new Vector3 (0f,-2000f,0f);
		undercarriageDown.transform.localPosition = new Vector3 (0f,-2000f,0f);
		undercarriageUp.transform.localPosition = new Vector3 (0f,-2000f,0f);
		flapBack.transform.localPosition = new Vector3 (0f,-2000f,0f);
		flapOpen.transform.localPosition = new Vector3 (0f,-2000f,0f);
		oilDrumOut.transform.localPosition = new Vector3 (0f,-2000f,0f);
		undercarriageValve.transform.localPosition = new Vector3 (-8.273f,0f,2.393f);
		underValveCtrl.transform.eulerAngles = new Vector3 (0f, 0f, 0f);
		flapValveCtrl.transform.eulerAngles = new Vector3 (90f,-135f,0f);
		informationLabel.GetComponent<UILabel> ().text = "显示液压流动";
	}

}