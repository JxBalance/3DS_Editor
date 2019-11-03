using UnityEngine;
using System.Collections;

public class DisassemblyBase : MonoBehaviour {

    private Vector3 startPos;
    private Vector3 endPos;
    public Vector3 v;
    private float t;
    public GameObject part;
    private bool actSi;



	// Use this for initialization
	void Start () {

        actSi = false;
        t = 0;
	}
	
	// Update is called once per frame
	void Update ()
    {
        //Debug.Log(actSi);
        if(actSi)
        {
            Vector3 pos = part.transform.position;
            pos += v * Time.deltaTime;
            part.transform.position = pos;
            t = t + Time.deltaTime;
            if(t>2)
            {
                actSi = false;
                t = 0;
            }
        }       
    }

    public Vector3 StartPos
    {
        get { return startPos; }
        set { startPos = value; }
    }

    public Vector3 EndPos
    {
        get { return endPos; }
        set { endPos = value; }
    }

    public bool ActSi
    {
        get { return actSi; }
        set { actSi = value; }
    }

    public Vector3 V
    {
        get { return v; }
        set { v = value; }
    }

    public GameObject Part
    {
        get { return part; }
        set { part = value; }
    }
}
