using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventObject : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public GameObject eventIcon;
    public GameObject eventText;
    public GameObject closeButton;


    public void DeleteAlertButton()
    {
        Destroy(gameObject);
    }
}
