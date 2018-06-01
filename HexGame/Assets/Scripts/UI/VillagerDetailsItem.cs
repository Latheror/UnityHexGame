using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class VillagerDetailsItem : MonoBehaviour {

    public Sprite villager1Sprite;

    public TextMeshProUGUI villagerAgeValue;
    public TextMeshProUGUI villagerNameValue;
    public Image villagerImage;
    public Image hungerLevelImage;

    public enum HungerLevel{ Good, Medium, Bad };
    public HungerLevel hungerLevelIndication;

    public Color hungerLevelGoodColor = Color.green;
    public Color hungerLevelMediumColor = Color.yellow;
    public Color hungerLevelBadColor = Color.red;



    // Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetAgeValue(int age){ villagerAgeValue.SetText(age.ToString()); }
    public void SetNameValue(string name){ villagerNameValue.SetText(name.ToString()); }

    // TODO : Change it
    public void SetImage(){ villagerImage.sprite = villager1Sprite; }

    public void setHungerLevelIndicator(Villager villager)
    {
        int hungerLevel = villager.GetHungerLevel();
        hungerLevelIndication = HungerLevel.Good;
        Color colorToDisplay = hungerLevelGoodColor; 

        if (hungerLevel >= 0 && hungerLevel <= 10)
        {
            if (hungerLevel < 3)
            {
                hungerLevelIndication = HungerLevel.Bad;
            }
            else if (hungerLevel < 6)
            {
                hungerLevelIndication = HungerLevel.Medium;
            }
            else
            {
                hungerLevelIndication = HungerLevel.Good;
            }

            switch (hungerLevelIndication)
            {
                case HungerLevel.Good:
                    {
                        colorToDisplay = hungerLevelGoodColor;
                        break;
                    }
                case HungerLevel.Medium:
                    {
                        colorToDisplay = hungerLevelMediumColor;
                        break;
                    }
                case HungerLevel.Bad:
                    {
                        colorToDisplay = hungerLevelBadColor;
                        break;
                    }
                default :
                    {
                        colorToDisplay = hungerLevelGoodColor;
                        break;
                    }
            }

            hungerLevelImage.color = colorToDisplay;

        }
        else
        {
            Debug.LogError("setHungerLevelIndicator | HungerLevel out of bounds !");
        }
    }

}
