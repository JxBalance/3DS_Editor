using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelInfoT1 : MonoBehaviour
{
    public string mModelName = "";
    public Vector3 mInitialPosition;
    public Vector3 mInitialEulerAngles;


    public void ResetTransform()
    {
        transform.position = mInitialPosition;
        transform.eulerAngles = mInitialEulerAngles;
    }
}
