using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour {

    // Make sure there is only one TimeManager
    public static TimeManager instance;

    void Awake()
    {   
        if(instance != null)
        {
            Debug.LogError("More than one TimeManager in scene !");
            return;
        }
        instance = this;
    }

    public float normalSpeed = 1f;
    public float fastSpeed = 2f;
    public float veryFastSpeed = 6f;

    // Time after which resources are produced
    public float turnTime = 2f;
    private float turnTimer = 0f;

    public float yearTime = 40f;
    private float yearTimer = 0f;
    public float yearTimeStep = 2f;
    public float yearTimerStep = 0f;

    // How many times the clock is refreshed in a year
    public int nbTimeYearRefresh = 40;

    // Year number in the game
    public int yearNumber = 1;
    public GameObject yearIndicatorText;

    // Seasons
    public enum Season { Spring, Summer, Autumn, Winter};
    public Season currentSeason;
    public GameObject seasonIndicatorText;

    public GameObject clockImage;
    public Dictionary<string, float> seasonClockAngleDictionary = new Dictionary<string, float>();


	// Use this for initialization
	void Start () {
        currentSeason = Season.Spring;

        BuildSeasonClockAngleDictionary();

        yearTimeStep = yearTime / nbTimeYearRefresh;

	}
	
	// Update is called once per frame
	void Update () {

        if (GameManager.instance.gameState == GameManager.GameState.Play)
        {
            turnTimer += Time.deltaTime;
            if(turnTimer >= turnTime)
            {
                Debug.Log(turnTime + " secondes passed. Calling CityManager");
                turnTimer = 0f;
                CityManager.instance.UpdateWork();

                // Test purpose - TODO remove
                //VillagersManager.instance.ChoseVillagersRandomDestination();

            }
           
            yearTimer += Time.deltaTime * GetGameSpeed();
            yearTimerStep += Time.deltaTime;

            if(yearTimerStep >= yearTimeStep)
            {
                // Debug.Log("Refreshing clock");
                yearTimerStep = 0f;
                SetClockToAngleFromTime(yearTimer);
                UpdateSeasonFromYearTime();  
            }

            if(yearTimer >= yearTime)
            {
                Debug.Log("A year passed !");
                yearTimer = 0f;

                // Increment yearNumber
                yearNumber++;

                NewYearActions();

                EventsManager.instance.PrintEvent("Happy new year !");
            }
        }


	}

    public void NewYearActions()
    {
        // Update yearText
        UpdateYearIndicatorText();
        VillagersManager.instance.IncrementAllAges();

        // TEST | TODO : Remove
        VillagersManager.instance.MakeVillagersLoseHungerPoint();

    }

    public void SetSeason(Season season)
    {
        currentSeason = season;
    }

    public void GoToNextSeason()
    {
        switch (currentSeason)
        {
            case Season.Spring:
            {
                currentSeason = Season.Summer;
                break;
            }
            case Season.Summer:
            {
                currentSeason = Season.Autumn;
                break;
            }
            case Season.Autumn:
            {
                currentSeason = Season.Winter;
                break;
            }
            case Season.Winter:
            {
               currentSeason = Season.Spring;
               break;
            }
        }

        Debug.Log("We are now in " + currentSeason);

        NewSeasonEvents();  
    }


    public void SetClockToSeason(Season season)
    {
        float angle = 0f;

        switch (season)
        {
            case Season.Spring:
            {
                    angle = seasonClockAngleDictionary["springStart"];
                    break;
            }
            case Season.Summer:
            {
                    angle = seasonClockAngleDictionary["summerStart"];
                    break;
            }
            case Season.Autumn:
            {
                    angle = seasonClockAngleDictionary["autumnStart"];
                    break;
            }
            case Season.Winter:
            {
                    angle = seasonClockAngleDictionary["winterStart"];
                    break;
            }
        }

        Debug.Log("New angle : " + angle);

        // Change the angle of the image
        clockImage.transform.localEulerAngles = new Vector3(0, 0, angle);
    }


    public void SetClockToAngle(float angle)
    {
        // Change the angle of the image
        clockImage.transform.localEulerAngles = new Vector3(0, 0, angle);
    }

    public void SetClockToAngleFromTime(float time)
    {
        // Change the angle of the image
        clockImage.transform.localEulerAngles = new Vector3(0, 0, YearTimeToAngle(time));
    }
        

    public void NextSeasonButton()
    {
        GoToNextSeason();
        SetClockToSeason(currentSeason);
    }



    public void BuildSeasonClockAngleDictionary()
    {
        seasonClockAngleDictionary.Add("yearStart", 0);
        seasonClockAngleDictionary.Add("springStart", 0);
        seasonClockAngleDictionary.Add("midSpring", -43.8f);
        seasonClockAngleDictionary.Add("summerStart", -90f);
        seasonClockAngleDictionary.Add("midSummer", -134f);
        seasonClockAngleDictionary.Add("autumnStart", -180f);
        seasonClockAngleDictionary.Add("midAutumn", -223f);
        seasonClockAngleDictionary.Add("winterStart", -270f);
        seasonClockAngleDictionary.Add("midWinter", -315f);
    }


    public float YearTimeToAngle()
    {
        float angle = - ((yearTimer) * (360) / (yearTime));
        //Debug.Log("Angle corresponding to time of the year : " + angle);
        return angle;
    }

    public float YearTimeToAngle(float time)
    {
        float angle = - ((time) * (360) / (yearTime));
        //Debug.Log("Angle corresponding to time of the year : " + angle);
        return angle;
    }

    public float GetGameSpeed()
    {   
        float speed = 1f;
        if(GameManager.instance.gameSpeed == GameManager.GameSpeed.NormalSpeed)
        {
            speed = normalSpeed;
        }
        else if(GameManager.instance.gameSpeed == GameManager.GameSpeed.FastSpeed)
        {
            speed = fastSpeed;
        }
        else if(GameManager.instance.gameSpeed == GameManager.GameSpeed.VeryFastSpeed)
        {
            speed = veryFastSpeed;
        }
        return speed;
    }

    public void UpdateSeasonFromYearTime()
    {
        // If we are in spring
        if(yearTimer < yearTime / 4)
        {
            SetSeason(Season.Spring);
        }
        else if((yearTimer >= yearTime / 4) && (yearTimer < yearTime / 2))
        {
            SetSeason(Season.Summer);
        }
        else if((yearTimer >= yearTime / 2) && (yearTimer < yearTime * 0.75))
        {
            SetSeason(Season.Autumn);
        }
        else if((yearTimer >= yearTime * 0.75))
        {
            SetSeason(Season.Winter);
        }
        else
        {
            Debug.Log("Time error...");
        }

        SetSeasonIndicatorText(currentSeason);

        // Update the color of the hexes
        HexManager.instance.UpdateHexesSeasonalColor(currentSeason);

        // TEST --- TODO : Remove
        //NewSeasonEvents();

    }

    public void SetSeasonIndicatorText(Season season)
    {
        string seasonText = "NoSeason";

        switch (season)
        {
            case Season.Spring:
            {
                    seasonText = "Spring";
                    break;
            }
            case Season.Summer:
            {
                    seasonText = "Summer";
                    break;
            }
            case Season.Autumn:
            {
                    seasonText = "Autumn";
                    break;
            }
            case Season.Winter:
            {
                    seasonText = "Winter";
                    break;
            }
        }

        seasonIndicatorText.GetComponent<Text>().text = seasonText;
    }

    public void SetYearIndicatorText(int yearNb)
    {
        yearIndicatorText.GetComponent<Text>().text = ("Year " + yearNb.ToString() + " - ");
    }

    public void UpdateYearIndicatorText()
    {
        SetYearIndicatorText(yearNumber);
    }

    public void NewSeasonEvents()
    {
        //VillagersManager.instance.MakeVillagersLoseHungerPoint();
    }

}