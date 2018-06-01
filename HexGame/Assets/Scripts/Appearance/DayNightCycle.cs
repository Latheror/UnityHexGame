using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class DayNightCycle : MonoBehaviour {

    public float time;
    public TimeSpan currentTime;
    public Transform sunTransform;
    public Transform moonTransform;
    public Light sun;
    public float intensity;
    public Color fogDay = Color.grey;
    public Color fogNight = Color.black;
    public float speed = 3f;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        ChangeDayTime();
	}

    public void ChangeDayTime()
    {
        // Debug.Log("Rotating Sun and Moon");

        sunTransform.RotateAround(Vector3.zero, Vector3.forward, Time.deltaTime * speed);
        sunTransform.LookAt(Vector3.zero);

        moonTransform.RotateAround(Vector3.zero, Vector3.forward, Time.deltaTime * speed);
        moonTransform.LookAt(Vector3.zero);
    }
}
