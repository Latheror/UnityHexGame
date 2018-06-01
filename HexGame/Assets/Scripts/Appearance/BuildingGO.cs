using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGO : MonoBehaviour {

    public List<GameObject> gOworkerSpotList = new List<GameObject>();


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public List<GameObject> GetWorkerSpotListGO(){ return gOworkerSpotList; }

    public void DisplayGOworkerSpotList()
    {
        Debug.Log("BUIGO | DisplayGOworkerSpotList :");
        foreach (var goWorkerSpot in gOworkerSpotList) {
            Debug.Log("GOWorkerSpot position : " + Geometry.Vector3ToString(goWorkerSpot.transform.position));
        }
    }
}
