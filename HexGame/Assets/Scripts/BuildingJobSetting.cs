using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingJobSetting : MonoBehaviour {

    public GameObject buildingImage;
    public GameObject inputField;
    public GameObject numberText;
    public GameObject arrowUp;
    public GameObject arrowDown;

    public int workersNumber = 0;

    // Building type referred by this setting
    public BuildManager.BuildingType buildingType;


    public void ArrowUp()
    {
        if(WorkersManager.instance.IncreaseWorkersNumberOfBuildingType(buildingType, 1))
        {
            workersNumber = buildingType.totalWorkersNumber;

            inputField.GetComponent<InputField>().text = workersNumber.ToString();
        }else{
            Debug.Log("Cant increase this number...");
        }

    }

    public void ArrowDown()
    {
        if(WorkersManager.instance.DecreaseWorkersNumberOfBuildingType(buildingType, 1))
        {
            workersNumber = buildingType.totalWorkersNumber;

            inputField.GetComponent<InputField>().text = workersNumber.ToString();
        }else{
            Debug.Log("Cant decrease this number...");
        }
    }


}
