using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingShopButton : MonoBehaviour {

    public string buildingName;

    public GameObject buildingNameUI;
    public GameObject buildingImageUI;

    public GameObject costsLayout;


    public void SelectBuildingButton()
    {

        // Go into Building Mode (BuildManager)
        BuildManager.instance.SetBuildingState(BuildManager.BuildingState.BuildingOperation);

        //Debug.Log("New building selected: " + buildingName);
        BuildManager.instance.SetSelectedBuildingType(buildingName);
    }    
}
