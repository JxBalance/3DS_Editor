using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateSetT3 : MonoBehaviour {

    public string stateName;
    public string stateTitle;
    public string hydrauForm = "Water";
    public List<PipeGroupT3> statePipegroups = new List<PipeGroupT3>();

    public List<pipeStateSets> statesVertix = new List<pipeStateSets>();

    public class pipeStateSets
    {

        //public List<GameObject> stateModels = new List<GameObject>();
        public GameObject stateSphere;
        public int stateOrder;
        public int stateIndex;
        
    };

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
