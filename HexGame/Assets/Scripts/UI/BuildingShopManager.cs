using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingShopManager : MonoBehaviour {

    public static BuildingShopManager instance;

    void Awake()
    {   
        if(instance != null)
        {
            Debug.LogError("More than one BuildingShopManager in scene !");
            return;
        }
        instance = this;
    }

    public GameObject buildingLayoutGroup1;
    public GameObject buildingLayoutGroup2;

    public int nbBuildingsPerPanel = 3;

    // Panel used to select a building
    public GameObject shopBuildingUI;

    // Panel indicating the resources costs to build a building
    public GameObject buildingCostUI;

    public int currentlyDisplayedPanelNumber = 1;

    void Start()
    {
        currentlyDisplayedPanelNumber = 1;
    }

	// Update is called once per frame
	void Update () {
		
	}


    public void BuildBuildingShop()
    {
        foreach (var buildingType in BuildManager.instance.availableBuildingsDictionary)
        {   
            GameObject placeToInstantiate = buildingLayoutGroup1;
            if(GetNbBuildingShopButtonsInPanel(1) < nbBuildingsPerPanel)
            {

                placeToInstantiate = buildingLayoutGroup1;
            }
            else
            {
                placeToInstantiate = buildingLayoutGroup2;
            }

            // Instantiate the shop item and position it in the shop layout
            GameObject instantiatedShopItemUI = Instantiate(shopBuildingUI, placeToInstantiate.transform.position, Quaternion.identity);
            instantiatedShopItemUI.transform.SetParent(placeToInstantiate.transform, false);

            BuildingShopButton shopButton = instantiatedShopItemUI.GetComponent<BuildingShopButton>();
            shopButton.buildingNameUI.GetComponent<Text>().text = buildingType.Value.name.ToString();
            shopButton.buildingName = buildingType.Value.name;
            instantiatedShopItemUI.GetComponent<BuildingShopButton>().buildingImageUI.GetComponent<Image>().sprite = buildingType.Value.image;

            // Filling the costs panel
            foreach (var cost in buildingType.Value.resourcesCost)
            {
                Sprite costResourceSprite = cost.resource.image;
                int costAmount = cost.amount;

                // Instantiate a cost panel
                GameObject instantiatedCostPanel = Instantiate(buildingCostUI, instantiatedShopItemUI.GetComponent<BuildingShopButton>().costsLayout.transform.position, Quaternion.identity);
                // Fill it
                instantiatedCostPanel.GetComponent<CostObject>().resourceImage.GetComponent<Image>().sprite = costResourceSprite;
                instantiatedCostPanel.GetComponent<CostObject>().costText.GetComponent<Text>().text = costAmount.ToString();
                // Put it in the costs layout
                instantiatedCostPanel.transform.SetParent(instantiatedShopItemUI.GetComponent<BuildingShopButton>().costsLayout.transform, false);
            }
        }

        DisplayPanelNumber(currentlyDisplayedPanelNumber);
    }

    public int GetNbBuildingShopButtonsInPanel(int panelNb)
    {
        int buildingButtonsNb = 0;
        if(panelNb == 1)
        {
            buildingButtonsNb = buildingLayoutGroup1.transform.childCount;
        }
        else if(panelNb == 2)
        {
            buildingButtonsNb = buildingLayoutGroup2.transform.childCount;
        }
        else
        {
            buildingButtonsNb = 0;
        }
        return buildingButtonsNb;
    }

    public void DisplayPanelNumber(int panelNb)
    {
        if (panelNb == 1)
        {
            buildingLayoutGroup1.SetActive(true);
            buildingLayoutGroup2.SetActive(false);
        }
        else if (panelNb == 2)
        {
            buildingLayoutGroup1.SetActive(false);
            buildingLayoutGroup2.SetActive(true);
        }
        else
        {
            Debug.LogError("Trying to display a non-existing panel");
        }
    }

    public void DisplayNextPanel()
    {
        if (currentlyDisplayedPanelNumber == 1)
        {
            currentlyDisplayedPanelNumber++;
            DisplayPanelNumber(currentlyDisplayedPanelNumber);
        }
        else
        {
            // Nothing for now
        }
    }

    public void DisplayPreviousPanel()
    {
        if (currentlyDisplayedPanelNumber == 2)
        {
            currentlyDisplayedPanelNumber--;
            DisplayPanelNumber(currentlyDisplayedPanelNumber);
        }
        else
        {
            // Nothing for now
        }
    }
            
}
