using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ControlSliderT2 : MonoBehaviour {

    public Slider sd;
    public MultiControlerBase me;
    public int controlSliderID;

    // Use this for initialization
    void Start ()
    {
        sd = transform.GetComponent<Slider>();
        me = GameManagerT2._instance.mechanismModels[0].GetComponent<MultiControlerBase>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnSliderValueChanged()
    {
        //Debug.Log(sd.value);
        me.sliderControl(sd.value,controlSliderID);
    }

    public void setControlSliderID(int id)
    {
        controlSliderID = id;
    }
}
