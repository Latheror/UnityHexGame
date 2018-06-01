using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CityManager : MonoBehaviour {

    // Make sure there is only one CityManager
    public static CityManager instance;

    void Awake()
    {   
        if(instance != null)
        {
            Debug.LogError("More than one CityManager in scene !");
            return;
        }
        instance = this;
    }

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {

    }



    // Make building produce resources
    public void UpdateWork()
    {
        foreach (var building in BuildingsManager.instance.buildingList)
        {
        foreach (var production in building.type.productions)
            {   
                ResourcesManager.instance.ProduceResource(building,production);
            }
        }
        ResourcesManager.instance.UpdateResourcesIndicators();
    }






}
