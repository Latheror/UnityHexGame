using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSettingsManager : MonoBehaviour {

    public static StartSettingsManager instance;

    void Awake()
    {   
        if(instance != null)
        {
            Debug.LogError("More than one StartSettingsManager in scene !");
            return;
        }
        instance = this;
    }


    public int mapSize = 10;
    public int waterMapPercentage = 30;

    // TODO : Remove
    public int startNumberOfWorkers = 5;



	// Use this for initialization
	void Start () {
        mapSize = 10;
	}

    public void GetMapSizeFromMenuSettings()
    {
        mapSize = MenuSettingsManager.instance.mapSize;
    }

    public void GetWaterMapPercentageFromMenuSettings()
    {
        waterMapPercentage = MenuSettingsManager.instance.waterProportion;
    }


}
