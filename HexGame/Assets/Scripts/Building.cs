using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Building {

    public BuildManager.BuildingType type;

    public int buildingNumber;

    public string name;
    public string description;

    // List of Hexes the building is located onto
    public List<Hex> HexList;

    public int attributedWorkersNb = 0;
    public List<Villager> workerList = new List<Villager>();

    public List<WorkerSpot> workerSpotList = new List<WorkerSpot>();
   
    public GameObject instantiatedGO;

    public Text popupText;

    public BuildManager.BuildingType GetBuildingType(){ return this.type; }
    public int GetNbWorkers(){ return this.attributedWorkersNb; }

	// Use this for initialization
	void Start () {
        
	}

    void Update()
    {
        //buildingInfo.Print();
    }


    public Building(BuildManager.BuildingType type, int buildingNumber)
    {
        this.type = type;
        this.buildingNumber = buildingNumber;
        this.workerList = new List<Villager>();
    }

    public void ReferenceWorkerSpots()
    {
        Debug.Log("BUI | ReferenceWorkerSpots :");
        this.workerSpotList = new List<WorkerSpot>();

        List<GameObject> associatedGOWorkerSpotList = this.instantiatedGO.GetComponent<BuildingGO>().GetWorkerSpotListGO();

        this.instantiatedGO.GetComponent<BuildingGO>().DisplayGOworkerSpotList();

        foreach (var GOWorkerSpot in associatedGOWorkerSpotList)
        {
            WorkerSpot newWorkerSpot = new WorkerSpot(GOWorkerSpot, true);
            this.workerSpotList.Add(newWorkerSpot);
        }

    }

    public void AddWorkerToBuilding(int nb)
    {
        attributedWorkersNb += nb;
    }

    public void RemoveWorkerFromBuilding(int nb)
    {
        if(attributedWorkersNb - nb >= 0)
        {
            attributedWorkersNb -= nb;
        }
        else
        {
            Debug.Log("There already is no workers here...");
        }
    }


    public void ExecuteCreationActions()
    {   
        if(type.creationActions != null)
        {
            foreach (var action in type.creationActions)
            {
                action();
            }
        }
    }

    public void ExecuteDeletionActions()
    {
        if(type.creationActions != null)
        {
            foreach (var action in type.creationActions)
            {
                action();
            }
        }
    } 

    public void SetHexList(List<Hex> list)
    {
        HexList = list;
    }

    public List<Hex> GetHexList()
    {
        return HexList;
    }

    public override string ToString()
    {
        string text = ("Building | Type : " + this.type.name + " | Nb workers : " + this.GetNbWorkers());
        return text;
    }

    public void PrintBuildingInfo()
    {
        Debug.Log(this.ToString());
    }

    public void DisplayWorkerSpotsInfo()
    {
        Debug.Log("BUI | DisplayWorkerSpotsInfo : ");
        foreach (var workerSpot in workerSpotList)
        {
            Debug.Log("WorkerSpot | Position : " + Geometry.Vector3ToString(workerSpot.spot.transform.position));
        }
    }

    public void AddWorkerToWorkerSpots(Villager workerToAdd)
    {
        Debug.Log("BUI | AddWorkerToWorkerSpots");
        List<WorkerSpot> buildingWorkerSpots = this.workerSpotList;

        DisplayWorkerSpotsInfo();

        bool workerHasBeenPlaced = false;

        foreach (var buildingWorkerSpot in buildingWorkerSpots)
        {
            if(buildingWorkerSpot.IsEmpty())
            {
                buildingWorkerSpot.SetWorkerToSpot(workerToAdd);
                workerHasBeenPlaced = true;
                break;
            }
        }

        if(! workerHasBeenPlaced)
        {
            Debug.LogError("No WorkerSpot has been found for a worker !");
        }
    }

    public void RemoveWorkerFromWorkerSpots(Villager workerToRemove)
    {
        List<WorkerSpot> buildingWorkerSpots = this.workerSpotList;
        foreach (var buildingWorkerSpot in buildingWorkerSpots)
        {
            if(buildingWorkerSpot.GetAssociatedWorker().Equals(workerToRemove))
            {
                buildingWorkerSpot.RemoveWorkerFromSpot(workerToRemove);
                break;
            }
            Debug.LogError("BUI | Trying to remove a worker from a building's workerSpots, but it isn't on any of them !");
        }
    }



    public class WorkerSpot
    {
        public GameObject spot;
        public bool isEmpty;
        public Villager associatedWorker;

        public Villager GetAssociatedWorker(){ return associatedWorker; }
        public void SetAssociatedWorker(Villager worker){ this.associatedWorker = worker; }

        public bool IsEmpty(){ return this.isEmpty; }
        public void SetIsEmpty(bool spotEmpty){ this.isEmpty = spotEmpty; }

        public WorkerSpot(GameObject workerSpot, bool spotIsEmpty)
        {
            spot = workerSpot;
            isEmpty = spotIsEmpty;
        }

        public void SetWorkerToSpot(Villager workerToAdd)
        {
            if(isEmpty)
            {
                SetAssociatedWorker(workerToAdd);
                workerToAdd.SetWorkerSpot(this);
                SetIsEmpty(false);
                Debug.Log("BUI | SetWorkerToSpot | Worker : " + workerToAdd.GetName() + " has been placed onto workerSpot " + Geometry.Vector3ToString(this.spot.gameObject.transform.position));
            }else{
                Debug.LogError("Trying to set a worker on a not free workerSpot !");
            }
        }

        public void RemoveWorkerFromSpot(Villager workerToRemove)
        {
            if (associatedWorker != null)
            {
                SetAssociatedWorker(null);
                workerToRemove.SetWorkerSpot(null);
                SetIsEmpty(true);
            }else{
                Debug.LogError("Trying to remove a worker from a spot where it isn't !");
            }
        }
    }
}
