using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorkersManager : MonoBehaviour {

    // Make sure there is only one UIManager
    public static WorkersManager instance;

    void Awake()
    {   
        if(instance != null)
        {
            Debug.LogError("More than one WorkersManager in scene !");
            return;
        }
        instance = this;
    }

    // Panels containing the number of workers setting for differents buildings
    public GameObject buildingJobListLeft;
    public GameObject buildingJobListRight;

    // The worker number setting UI prefab
    public GameObject buildingJobPrefab;

    public int maxNumberOfSettingPerPanel = 6;

    public int workersNumber = 0;
    public int workersIdleNumber = 0;

    //public Dictionary<string> workersPerBuildingType = new Dictionary<>();


    [SerializeField]
    private int currentWorkerID;

    [SerializeField]
    private List<Villager> workerList = new List<Villager>();

    public List<Villager> idlingWorkerList = new List<Villager>();

    // Worker count UI
    public GameObject workerText;
    // Worker idling count UI
    public GameObject workerIdleNumberText;


    public int GetCurrentWorkerID()
    {
        return currentWorkerID;
    }

    public void SetWorkerID(int id)
    {
        currentWorkerID = id;
    }

    public void IncrementWorkerID()
    {
        currentWorkerID++;
    }


    public void InitializeJobSettings()
    {
        foreach (var buildingType in BuildManager.instance.availableBuildingsDictionary)
        {
            GameObject instantiatedBuildingJobPanel;

            // Debug.Log("Number of panels in LeftLayout : " + buildingJobListLeft.transform.childCount);
            if(buildingJobListLeft.transform.childCount < maxNumberOfSettingPerPanel)
            {
                instantiatedBuildingJobPanel = Instantiate(buildingJobPrefab, buildingJobListLeft.transform.position, Quaternion.identity);
                instantiatedBuildingJobPanel.transform.SetParent(buildingJobListLeft.transform, false);
            }else{
                instantiatedBuildingJobPanel = Instantiate(buildingJobPrefab, buildingJobListRight.transform.position, Quaternion.identity);
                instantiatedBuildingJobPanel.transform.SetParent(buildingJobListRight.transform, false);
            }

            instantiatedBuildingJobPanel.GetComponent<BuildingJobSetting>().buildingType = buildingType.Value;
            instantiatedBuildingJobPanel.GetComponent<BuildingJobSetting>().buildingImage.GetComponent<Image>().sprite = buildingType.Value.imageWorkerSetting;

        }
    }


    // Initialize list of workers from list of villagers at the start of the game
    public void InitializeStartingWorkers()
    {
        workerList = new List<Villager>();

        // All villagers having the role Worker become workers
        foreach (var villager in VillagersManager.instance.villagersList)
        {
            if(villager.GetRole().Equals(Villager.Role.Worker))
            {
                workerList.Add(villager);
                villager.SetHasJob(false);
            }
        }

        workersNumber = workerList.Count;
        UpdateWorkersNumberDisplay();

        DisplayStartingWorkers();

        InitializeIdleWorkers();
    }

    public void InitializeIdleWorkers()
    {
        workersIdleNumber = workersNumber;
        idlingWorkerList = workerList;
        UpdateIdleWorkersNumber();

    }

    public void DisplayStartingWorkers()
    {
        Debug.Log("WM | Displaying Starting Workers : ");
        foreach (var worker in workerList)
        {
            Debug.Log(worker.ToString());
        }
    }

    public void AddWorker(Villager newWorker)
    {
        workerList.Add(newWorker);

        AddWorkerToIdlingList(newWorker);

        UpdateWorkersNumber();
        UpdateWorkersNumberDisplay();
    }

    public void RemoveWorker(Villager worker)
    {
        workerList.Remove(worker);
        RemoveIdleWorker(worker);
        UpdateWorkersNumber();
    }


    public bool IncreaseWorkersNumberOfBuildingType(BuildManager.BuildingType bType, int nb)
    {   
        bool succeeded = false;
        // Checks if there is enough workers available
        if (workersIdleNumber >= nb)
        {   
            // Check if there is enough total room for all workers in this type of building
            if(bType.totalWorkersNumber + nb <= bType.buildingList.Count * bType.maxWorkersPerBuildingNumber)
            {
                bType.totalWorkersNumber += nb;

                Debug.Log(bType.totalWorkersNumber + " workers now work in a " + bType.name +".");

                AttributeWorkersNb();

                // We succeeded to increase number of workers
                succeeded = true;
            }
            else
            {
                Debug.Log("There is not enough slot for more workers in this type of building.");
                succeeded = false;
            }

        }else{
            Debug.Log("No worker is available !");
            succeeded = false;
        }

        return succeeded;
    }

    public bool DecreaseWorkersNumberOfBuildingType(BuildManager.BuildingType bType, int nb)
    {
        // Checks if there are workers working in this type of building
        if(bType.totalWorkersNumber > 0)
        {
            bType.totalWorkersNumber -= nb;

            Debug.Log(bType.totalWorkersNumber + " workers now work in a " + bType.name +".");

            AttributeWorkersNb();

            // We succeeded to decrease number of workers
            return true;

        }else{
            Debug.Log("Can't go below 0 !..");
            return false;
        }

    }


    public void DecreaseWorkersIdleNumber(int nb)
    {
        if(workersIdleNumber >= nb)
        {
            workersIdleNumber -= nb;
            UpdateIdleWorkersNumber();
        }
    }

    public void IncreaseWorkersIdleNumber(int nb)
    {
            workersIdleNumber += nb;
            UpdateIdleWorkersNumber();
    }

    public void UpdateWorkersNumberDisplay()
    {
        workerText.GetComponent<Text>().text = workersNumber.ToString();
    }

    public void UpdateIdleWorkersNumber()
    {
        workerIdleNumberText.GetComponent<Text>().text = workersIdleNumber.ToString();
    }


    // Attribute the workers equally in the buildings
    public void AttributeWorkersNb()
    {
        // Loop through all the building types
        foreach (var buildingType in BuildManager.instance.availableBuildingsDictionary)
        {
            // Total number of workers working in this type of building
            int nbWorkersInThisBuildingType = buildingType.Value.totalWorkersNumber;
            Debug.Log("There is a total of " + nbWorkersInThisBuildingType + " workers in all " + buildingType.Value.name + "s.");

            // Number of buildings of this type
            int nbBuildingsOfThisType = buildingType.Value.buildingList.Count;
            Debug.Log("There is a total of " + nbBuildingsOfThisType + " " + buildingType.Value.name + " on the map.");

            if(nbBuildingsOfThisType > 0)
            {
                int nbWorkersDivision = nbWorkersInThisBuildingType / nbBuildingsOfThisType;
                int nbWorkersLeft = nbWorkersInThisBuildingType % nbBuildingsOfThisType;
                Debug.Log("We can put " + nbWorkersDivision + " workers in each building. There will be " + nbWorkersLeft + " workers left.");

                // Loop through all buildings of this type
                foreach (var building in buildingType.Value.buildingList)
                {
                  building.attributedWorkersNb = nbWorkersDivision;
                }

                // We still need to attribute this number of workers
                int workersLeftToAttribute = nbWorkersLeft;
                int buildingNumberInList = 0;

                while(workersLeftToAttribute > 0)
                {
                    buildingType.Value.buildingList[buildingNumberInList].attributedWorkersNb += 1;
                    workersLeftToAttribute -= 1;
                    buildingNumberInList++;
                }


            }else{
                Debug.Log("There isn't a single " + buildingType.Value.name + " on the map !");
            }

        }

        PlaceWorkersInBuildings();

    }

    // Effectively place workers in buildings
    public void PlaceWorkersInBuildings()
    {
        Debug.Log("WM | PlaceWorkersInBuildings ");

        foreach (var building in BuildingsManager.instance.buildingList)
        {
            building.PrintBuildingInfo();

            // Reset workers list -- TODO Do it differently later
            //building.workerList = new List<Villager>();
            int missingWorkersNb = building.attributedWorkersNb - building.workerList.Count;

            Debug.Log("Missing workers = " + missingWorkersNb);

            // We need to put workers in this building
            if(missingWorkersNb > 0)
            {
                for(int i=0 ; i<missingWorkersNb ; i++)
                {
                    Debug.Log("Adding a worker to " + building.GetBuildingType().name + " building.");
                    Debug.Log("Number of idling workers was : " + workersIdleNumber);

                    // Add worker to building's list
                    Villager newWorker = idlingWorkerList[i];
                    building.workerList.Add(newWorker);
                    // Remove previously idling worker form list
                    RemoveIdleWorker(newWorker);
                    // Set building as worker's job
                    newWorker.SetJobBuilding(building);

                    Debug.Log("Number of idling workers is now : " + workersIdleNumber);

                    building.AddWorkerToWorkerSpots(newWorker);
                    newWorker.SetDestination(newWorker.GetWorkerSpot().spot.transform);
                    newWorker.SetIsDestinationDifferentThanPosition(true);
                }
            }
            // We need to remove workers from this building
            else if(missingWorkersNb < 0)
            {
                for (int i = 0; i < - missingWorkersNb; i++)
                {
                    Debug.Log("Removing a worker from " + building.GetBuildingType().name + " building.");
                    Debug.Log("Number of idling workers was : " + workersIdleNumber);
                    Villager removedWorker = building.workerList[building.workerList.Count - 1];
                    building.workerList.Remove(removedWorker);
                    AddWorkerToIdlingList(removedWorker);
                    Debug.Log("Number of idling workers is now : " + workersIdleNumber);

                    building.RemoveWorkerFromWorkerSpots(removedWorker);

                    // TODO : Modify
                    removedWorker.SetDestination(HexManager.instance.GetHexList()[0].GetTopPointOfHex().transform);
                }
            }

        }
    }

    public void WorkerDies(Villager worker)
    {
        Debug.Log("WM | WorkerDies : " + worker.ToString());
        RemoveWorker(worker);

        // Re attribute workers
        AttributeWorkersNb();
    }

    public void UpdateWorkersNumber()
    {
        workersNumber = workerList.Count;
        UpdateWorkersNumberDisplay();
    }

    public void RemoveIdleWorker(Villager worker)
    {
        if(worker.IsVillagerInList(idlingWorkerList))
        {   
            idlingWorkerList.Remove(worker);
            DecreaseWorkersIdleNumber(1);
        }

    }

    public void AddWorkerToIdlingList(Villager worker)
    {
        idlingWorkerList.Add(worker);
        IncreaseWorkersIdleNumber(1);

    }



}
