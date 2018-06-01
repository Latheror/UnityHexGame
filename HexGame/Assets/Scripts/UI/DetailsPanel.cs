using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DetailsPanel : MonoBehaviour {

    // Panel used as a parent of UI details prefabs
    public GameObject detailsInfoLocation;
    // UI prefab to display building details
    public GameObject buildingDetailsItem;
    // UI prefab to display villager details
    public GameObject villagerDetailsItem;

    public bool IsDisplayedDetailItem;
    public GameObject currentlyDisplayedDetailItem;


	// Use this for initialization
	void Start () {
        currentlyDisplayedDetailItem = null;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void HexWithBuildingSelected(Hex hex)
    {
        Building building = hex.getCurrentBuilding();
        DisplayBuildingDetails(building);
    }

    public void DisplayBuildingDetails(Building b)
    {
        Debug.Log("Displaying details of building : " + b.GetBuildingType().name);

        // Debug.Log("Position of buildingDetailsPanel is : " + detailsInfoLocation.transform.position.ToString());

        // Instantiate the BuildingDetail item and position it in the detail panel
        GameObject instantiatedBuildingDetailsItem = Instantiate(buildingDetailsItem, detailsInfoLocation.transform.position, Quaternion.identity);
        instantiatedBuildingDetailsItem.transform.SetParent(detailsInfoLocation.transform, false);
        instantiatedBuildingDetailsItem.transform.localPosition = Vector3.zero;
        instantiatedBuildingDetailsItem.transform.localRotation = Quaternion.identity;
        instantiatedBuildingDetailsItem.transform.localScale = Vector3.one;

        // Reference the currently displayed building details after removing the previous one
        StopDisplaying();
        currentlyDisplayedDetailItem = instantiatedBuildingDetailsItem;

        BuildingDetailsItem buildingDetailsItemScript = instantiatedBuildingDetailsItem.GetComponent<BuildingDetailsItem>();

        // Get panel components
        TextMeshProUGUI buildingTypeUI = buildingDetailsItemScript.GetBuildingTypeText();
        Image buildingImageUI = buildingDetailsItemScript.GetImage();
        TextMeshProUGUI buildingNbWorkerUI = buildingDetailsItemScript.GetNbWorkersText();
        TextMeshProUGUI buildingNbWorkerValue = buildingDetailsItemScript.GetNbWorkersValue();

        // Fill components
        buildingDetailsItemScript.SetBuildingTypeText(b.GetBuildingType().name);
        buildingDetailsItemScript.SetNbWorkersValue(b.GetNbWorkers().ToString());
        buildingDetailsItemScript.SetImage(b.GetBuildingType().image);


        /*
            instantiatedShopItemUI.GetComponent<BuildingShopButton>().buildingImageUI.GetComponent<Image>().sprite = buildingType.Value.image;
       */
    }

    public void VillagerSelected(Villager v)
    {
        DisplayVillagerDetails(v);
    }

    public void DisplayVillagerDetails(Villager v)
    {
        StopDisplaying();
        Debug.Log("Displaying details of villager : " + v.GetName());

        // Instantiate the BuildingDetail item and position it in the detail panel
        GameObject instantiatedVillagerDetailsItem = Instantiate(villagerDetailsItem, detailsInfoLocation.transform.position, Quaternion.identity);
        instantiatedVillagerDetailsItem.transform.SetParent(detailsInfoLocation.transform, false);
        instantiatedVillagerDetailsItem.transform.localPosition = Vector3.zero;
        instantiatedVillagerDetailsItem.transform.localRotation = Quaternion.identity;
        instantiatedVillagerDetailsItem.transform.localScale = Vector3.one;

        currentlyDisplayedDetailItem = instantiatedVillagerDetailsItem;

        VillagerDetailsItem villagerDetailsItemScript = instantiatedVillagerDetailsItem.GetComponent<VillagerDetailsItem>();

        villagerDetailsItemScript.SetAgeValue(v.GetAge());
        villagerDetailsItemScript.SetNameValue(v.GetName());
        villagerDetailsItemScript.SetImage();
        villagerDetailsItemScript.setHungerLevelIndicator(v);
    }

    public void StopDisplaying()
    {
        Destroy(currentlyDisplayedDetailItem);
    }


}
