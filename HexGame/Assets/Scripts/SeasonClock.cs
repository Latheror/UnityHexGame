using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeasonClock : MonoBehaviour {

    // Make sure there is only one SeasonClock
    public static SeasonClock instance;

    void Awake()
    {   
        if(instance != null)
        {
            Debug.LogError("More than one SeasonClock in scene !");
            return;
        }
        instance = this;
    }

    public bool summerReached = false;
    public bool autumnReached = false;
    public bool winterReached = false;
    public bool springReached = false;


    // Use this for initialization
    void Start () {
        summerReached = autumnReached = winterReached = springReached = false;
    }
    
    // Update is called once per frame
    void Update () {
        
    }

    public void SummerReached()
    {
        summerReached = true;
        autumnReached = false;
        winterReached = false;
        springReached = false;
        Debug.Log("We reached Summer !");
    }

    public void AutumnReached()
    {
        autumnReached = true;
        winterReached = false;
        springReached = false;
        summerReached = false;
        Debug.Log("We reached Autumn !");
    }

    public void WinterReached()
    {
        winterReached = true;
        springReached = false;
        summerReached = false;
        autumnReached = false;
        Debug.Log("We reached Winter !");
    }

    public void SpringReached()
    {
        springReached = true;
        summerReached = false;
        autumnReached = false;
        winterReached = false;
        Debug.Log("We reached Spring !");
    }
}
