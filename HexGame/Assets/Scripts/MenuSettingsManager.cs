using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSettingsManager : MonoBehaviour {

    public static MenuSettingsManager instance;

    void Awake()
    {   
        DontDestroyOnLoad (transform.gameObject);

        if(instance != null)
        {
            Debug.LogError("More than one MenuSettingsManager in scene !");
            return;
        }
        instance = this;
    }

    public int waterProportion = 25;
    public int mapSize = 10;



    public void SetWaterProportion(float percentage)
    {
        waterProportion = (int)percentage;
    }

    public void SetMapSize(int size)
    {
        mapSize = size;
    }

}
