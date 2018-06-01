using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuildingDetailsItem : MonoBehaviour {

    public GameObject buildingDetailsPanel;
    public TextMeshProUGUI buildingTypeText;
    public Image buildingImage;
    public TextMeshProUGUI nbWorkerText;
    public TextMeshProUGUI nbWorkerValue;

    public GameObject GetBuildingDetailsPanel(){ return this.buildingDetailsPanel; }
    public void SetBuildingDetailsPanel(GameObject p){ this.buildingDetailsPanel = p; }
    public TextMeshProUGUI GetBuildingTypeText(){ return this.buildingTypeText; }
    public void SetBuildingTypeText(string text){ this.buildingTypeText.SetText(text); }
    public Image GetImage(){ return this.buildingImage; }
    public void SetImage(Sprite im){ this.buildingImage.sprite = im; }
    public TextMeshProUGUI GetNbWorkersText(){ return this.nbWorkerText; }
    public void SetNbWorkersText(string text){ this.nbWorkerText.SetText(text); }
    public TextMeshProUGUI GetNbWorkersValue(){ return this.nbWorkerValue; }
    public void SetNbWorkersValue(string text){ this.nbWorkerValue.SetText(text); }


    public BuildingDetailsItem(string text, Sprite image)
    {
        this.SetBuildingTypeText(text);
        this.SetImage(image);
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
