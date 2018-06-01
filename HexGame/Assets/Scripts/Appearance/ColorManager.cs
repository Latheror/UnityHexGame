using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorManager : MonoBehaviour {

    public static ColorManager instance;

    public Color selectedColor = Color.yellow;
    public Color hoverColor = Color.grey;
    public Color baseColor = Color.green;
    public Color highlightedColor = Color.blue;
    public Color errorColor = Color.red;

    public Color hexBorderColor;
    public Color hexCenterColor;

    // Default color of building types
    public Color mountainDefaultColor = Color.grey;
    public Color plainDefaultColor = Color.green;
    public Color forestDefaultColor = Color.black;
    public Color waterDefaultColor = Color.blue;

    // Building operation
    public Color canBuildColor = Color.blue;
    public Color cantBuildColor = Color.red;

    // Seasons colors
    public SeasonsColors seasonColor1;

    void Awake()
    {   
        if(instance != null)
        {
            Debug.LogError("More than one ColorManager in scene !");
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


    [System.Serializable]
    public class SeasonsColors
    {
        public string name;

        public Color springColor;
        public Color summerColor;
        public Color autumnColor;
        public Color winterColor;

        public Color GetSpringColor(){ return springColor; }
        public Color GetSummerColor(){ return summerColor; }
        public Color GetAutumnColor(){ return autumnColor; }
        public Color GetWinterColor(){ return winterColor; }

        public SeasonsColors(Color springC, Color summerC, Color autumnC, Color winterC)
        {
            this.name = "noName";
            this.springColor = springC;
            this.summerColor = summerC;
            this.autumnColor = autumnC;
            this.winterColor = winterC;
        }

        public SeasonsColors(string name, Color springC, Color summerC, Color autumnC, Color winterC)
        {
            this.name = name;
            this.springColor = springC;
            this.summerColor = summerC;
            this.autumnColor = autumnC;
            this.winterColor = winterC;
        }

        public Color GetColorFromSeason(TimeManager.Season season)
        {
            switch(season)
            {
                case TimeManager.Season.Spring :
                {
                        return GetSpringColor();
                        break;
                }
                case TimeManager.Season.Summer :
                {
                        return GetSummerColor();
                        break;
                }
                case TimeManager.Season.Autumn :
                {
                        return GetAutumnColor();
                        break;
                }
                case TimeManager.Season.Winter :
                {
                        return GetWinterColor();
                        break;
                }
                default :
                {
                        return Color.white;
                        break;
                }
            }
        }

        public Color GetColorFromCurrentSeason()
        {
            return GetColorFromSeason(TimeManager.instance.currentSeason);
        }


    }

}
