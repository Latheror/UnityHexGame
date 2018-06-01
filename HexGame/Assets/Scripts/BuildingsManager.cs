using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingsManager : MonoBehaviour {

    public static BuildingsManager instance;

    void Awake()
    {   
        if (instance != null)
        {
            Debug.LogError("More than one BuildingsManager in scene !");
            return;
        }
        instance = this;
    }

    public List<Building> buildingList = new List<Building>();


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    public void AddBuilding(Building b)
    {
        buildingList.Add(b);
        WorkersManager.instance.AttributeWorkersNb();
    }

    public void RemoveBuilding(Building b)
    {
        //foreach (var building in buildingList)
        //{
        //    if(building.buildingNumber == buildingNb)
        //    {
        buildingList.Remove(b);

        Debug.Log("There is now " + buildingList.Count + " buildings in the city.");
        //        WorkersManager.instance.AttributeWorkers();
        //        break;
        //    }
        //}
    }
}
