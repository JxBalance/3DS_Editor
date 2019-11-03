

using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class switchcameraT2 : MonoBehaviour {
    public GameObject MainCamera_VR;
    public GameObject CabinCamera;
    GameObject MainCamera_PC;
    public AutoMulticControl me;
    public bool starmove;
    void Start()
    {
        MainCamera_PC = GameManagerT2._instance.MainCameraGameObject_PC;
        MainCamera_VR = UIControllerT2._instance.CameraRigGameObject;
        me = GameManagerT2._instance.mechanismModels[0].GetComponent<AutoMulticControl>();
        starmove = false;

        }

    void OnMouseDown()
    {
        Debug.Log("start");
        MainCamera_PC.transform.position = new Vector3(5, -2.5f, -25);
        MainCamera_PC.transform.rotation = Quaternion.Euler(0, 200, 0);
        starmove = true;
        me.controlMove = starmove;
    }

    /// <summary>
    /// 有问题 --- 已解决
    /// </summary>
    public void OnRayPointInSphere()
    {
        Debug.Log("start");
        MainCamera_VR.transform.position = new Vector3(5, -2.5f, -25);
        MainCamera_VR.transform.rotation = Quaternion.Euler(0, 200, 0);
        starmove = true;
        me.controlMove = starmove;     
    }


}
