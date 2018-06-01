using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventsManager : MonoBehaviour {


    // Make sure there is only one EventsManager
    public static EventsManager instance;

    void Awake()
    {   
        if(instance != null)
        {
            Debug.LogError("More than one EventsManager in scene !");
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


    public GameObject eventObjectPrefab;
    public GameObject eventsVerticalLayout;


    public void PrintEvent()
    {
        GameObject instantiatedEventObject = Instantiate(eventObjectPrefab, eventsVerticalLayout.transform.position, Quaternion.identity);
        instantiatedEventObject.transform.SetParent(eventsVerticalLayout.transform, false);
    }

    public void PrintEvent(string eventText)
    {
        GameObject instantiatedEventObject = Instantiate(eventObjectPrefab, eventsVerticalLayout.transform.position, Quaternion.identity);
        instantiatedEventObject.GetComponent<EventObject>().eventText.GetComponent<Text>().text = eventText;

        // Put the event object in the event layout
        instantiatedEventObject.transform.SetParent(eventsVerticalLayout.transform,false);
    }

    public void DisplayVillagerDeathMessage(Villager villager)
    {
        PrintEvent(villager.GetName() + " has died :( ");
    }

}
