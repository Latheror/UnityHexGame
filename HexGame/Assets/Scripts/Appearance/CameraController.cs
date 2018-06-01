using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public float panSpeed = 20f;
    public float panBorderThickness = 10f;
    public Vector2 panLimit;
    public float scrollSpeed = 20f;
    public float rotationSpeed = 20f;
    public float dragSpeed = 50f;

    public float minRotation = 20;
    public float maxRotation = 90;
    public bool isCameraLocked = false;

    public float minY = 20f;
    public float maxY = 100f;
    public float minX = -20f;
    public float maxX = 20f;
    public float minZ = -20f;
    public float maxZ = 20f;

    // Drag with left button
    public Vector3 posOrigin;
    public Vector3 panOrigin;
    public Vector3 posDifference;
    public bool drag;

	// Use this for initialization
	void Start () {
        drag = false;


        SetMaxCameraPosition(CalculateMaxCameraPositionFromMapSize());

	}
	
	// Update is called once per frame
	void LateUpdate()
    {

        Vector3 pos = transform.position;

        // Camera lock
        if (Input.GetKeyDown("space"))
        {
            if (isCameraLocked)
            {
                isCameraLocked = false;
            }
            else
            {
                isCameraLocked = true;
            }
        }

        if (!isCameraLocked)
        {
            
            // Change position
            if(Input.GetKey("z"))
            {
                pos.z += panSpeed * Time.deltaTime;
            }
            if (Input.mousePosition.y >= Screen.height - panBorderThickness)
            {
                pos.z += panSpeed * YMousePositionToSpeedCoef(Input.mousePosition.y) * Time.deltaTime;
            }
            if (Input.GetKey("s"))
            {
                pos.z -= panSpeed * Time.deltaTime;
            }
            if (Input.mousePosition.y <= panBorderThickness)
            {
                pos.z -= panSpeed * YMousePositionToSpeedCoef(Input.mousePosition.y) * Time.deltaTime;
            }
            if (Input.GetKey("q"))
            {
                pos.x -= panSpeed * Time.deltaTime;
            }
            if (Input.mousePosition.x <= panBorderThickness)
            {
                pos.x -= panSpeed * XMousePositionToSpeedCoef(Input.mousePosition.x) * Time.deltaTime;
            }
            if (Input.GetKey("d"))
            {
                pos.x += panSpeed * Time.deltaTime;
            }
            if (Input.mousePosition.x >= Screen.width - panBorderThickness)
            {
                pos.x += panSpeed * XMousePositionToSpeedCoef(Input.mousePosition.x) * Time.deltaTime;
            }

            if (Input.GetMouseButton(1))
            {
                transform.Rotate(Vector3.right * rotationSpeed * Time.deltaTime * Input.GetAxis("Mouse Y"));
            }

            // Zoom
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            pos.y -= scroll * scrollSpeed * 100 * Time.deltaTime;


            // Drag camera with left button
            if (Input.GetMouseButton(0))
            {
                pos.x -= Input.GetAxis("Mouse X") * dragSpeed * Time.deltaTime;
                pos.z -= Input.GetAxis("Mouse Y") * dragSpeed * Time.deltaTime;
            }


            // Lateral position
            pos.x = Mathf.Clamp(pos.x, minX, maxX);
            pos.z = Mathf.Clamp(pos.z, minZ, maxZ);
            pos.y = Mathf.Clamp(pos.y, minY, maxY);

            transform.position = pos;

        }

	}

    // Adapts the camera speed with the mouse distance from the screen border
    public float XMousePositionToSpeedCoef(float position)
    {   
        float coef;

        if(position <= panBorderThickness)
        {
            coef = (- position) / (panBorderThickness) + 1;
        }else{
            coef = (position - (Screen.width - panBorderThickness)) / (panBorderThickness);
        }

        // Make sure the coef is between 0 and 1
        coef = Mathf.Clamp(coef, 0, 1);

        return coef;
    }

    public float YMousePositionToSpeedCoef(float position)
    {
        float coef;

        if(position <= panBorderThickness)
        {
            coef = (- position) / (panBorderThickness) + 1;
        }else{
            coef = (position - (Screen.height - panBorderThickness)) / (panBorderThickness);
        }

        coef = Mathf.Clamp(coef, 0, 1);

        return coef;
    }

    public struct MaxCameraPositions
    {
        public float minX;
        public float maxX;
        public float minZ;
        public float maxZ;

    }

    public MaxCameraPositions CalculateMaxCameraPositionFromMapSize()
    {
        MaxCameraPositions maxPos = new MaxCameraPositions();
        int mapSize = MapBuilder.instance.mapSize;

        maxPos.minX = -30f;
        maxPos.minZ = -30f;
        maxPos.maxX = 4f * (float) mapSize;
        maxPos.maxZ = 4f * (float) mapSize;

        return maxPos;
    }

    public void SetMaxCameraPosition(MaxCameraPositions maxPos)
    {
        minX = maxPos.minX;
        maxX = maxPos.maxX;
        minZ = maxPos.minZ;
        maxZ = maxPos.maxZ;
    }
}
