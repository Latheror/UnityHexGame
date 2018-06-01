using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopPanel : MonoBehaviour {

    public int nbBuildingShopItems;

    public List<GameObject> buildingShopPanels;

    public int currentDisplayedShopPanelNumber;

    public GameObject buildingShopPanel1;
    public GameObject buildingShopPanel2;


	// Use this for initialization
	void Start () {
        buildingShopPanels = new List<GameObject>();
        buildingShopPanels.Add(buildingShopPanel1);
        buildingShopPanels.Add(buildingShopPanel2);


        currentDisplayedShopPanelNumber = 1;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void DisplayPanelNumber(int number)
    {
        GameObject panelToDisplay = GetPanelFromNumber(number);
        panelToDisplay.SetActive(true);
        foreach (var panel in buildingShopPanels)
        {
            panel.SetActive(false);
        }
    }

    public GameObject GetPanelFromNumber(int number)
    {
        return buildingShopPanels[number - 1];
    }

    public int GetCurrentPanelNumber()
    {
        return currentDisplayedShopPanelNumber;
    }

    public void PreviousPanelShopButtonActions()
    {
        Debug.Log("Previous shop panel");
        DisplayPreviousPanel();
    }

    public void NextPanelShopButtonActions()
    {
        Debug.Log("Next shop panel");
        DisplayNextPanel();
    }

    public void DisplayPreviousPanel(){
        int currentNumber = GetCurrentPanelNumber();
        if(currentNumber > 1)
        {
            
        }
        else
        {
            Debug.Log("Can't display previous panel, already displaying first one");
        }
    }


    public void DisplayNextPanel(){


    }

}
