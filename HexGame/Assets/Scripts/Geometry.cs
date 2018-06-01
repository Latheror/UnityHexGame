using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Geometry : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    public static string Vector3ToString(Vector3 vector)
    {
        return ("(" + vector.x + "," + vector.y + "," + vector.z + ")");
    }

    public static int GetRandomOrientationAngle()
    {
        return (Random.Range(0, 359));
    }

    public static void FaceDestinationOnXZ(Transform toRotate, Transform target)
    {
        toRotate.LookAt(new Vector3(target.position.x, toRotate.position.y, target.position.z));
    }


}
