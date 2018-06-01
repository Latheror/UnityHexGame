using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsManager : MonoBehaviour {

    // Make sure there is only one ControlsManager
    public static ControlsManager instance;

    void Awake()
    {   
        if(instance != null)
        {
            Debug.LogError("More than one ControlsManager in scene !");
            return;
        }
        instance = this;
    }

    BuildManager buildManager = BuildManager.instance;

    void Start()
    {
        buildManager = BuildManager.instance;
    }

	// Update is called once per frame
	void Update()
    {

        // Change building construction orientation
        if (Input.GetKeyDown("r"))
        {
            	// Has an effect only if BuildManager is currently in build mode
                if(buildManager.buildingState == BuildManager.BuildingState.BuildingOperation)
                {
                    Debug.Log("Building Rotation");
                    buildManager.ChangeConstructionOrientation();
                }
        }

        else     
            
        // Escape key - Go into default mode
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            BuildManager.instance.SetBuildingState(BuildManager.BuildingState.NoOperation);
            BuildManager.instance.RemovePreviewBuilding();
        }

    }

}
