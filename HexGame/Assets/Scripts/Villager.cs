using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Villager {

    // List of all possible names
    static string[] villagerMaleNamesList = new string[]{"Jean-Mich", "Kevin", "Trucmuche"};
    static string[] villagerFemaleNamesList = new string[]{"Machine", "Chieuse", "Pimbeche"};    

    public enum Sex {Male, Female};
    public enum Role {Worker, Student, Child};

    private int id;
    private int age;
    private string name;
    private Sex sex;
    private Role role;
    private int health;

    private GameObject associatedGO;

    // Villager movements
    private Hex positionHex;
    private Hex destinationHex;

    private Transform destination;

    private bool isDestinationDifferentThanPosition;
    private bool hasReachedDestination;
    private bool isMovingTowardDestination;
    private float movementSpeed = 2f;

    private Building.WorkerSpot workerSpot;

    // Hunger level of a villager - 0 is bad, 10 is good
    private int hungerLevel;
    private bool hasJob;
    private Building jobBuilding;

    private List<Transform> hopPointsListTowardDestination = new List<Transform>();

    // Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    public string GetName(){ return this.name; }
    public int GetAge(){ return this.age; }
    public int GetHungerLevel(){ return this.hungerLevel; }
    public bool HasJob(){ return this.hasJob; }
    public Building GetJobBuilding(){ return this.jobBuilding; }
    public GameObject GetAssociatedGO(){ return this.associatedGO; }
    public Hex GetHexPosition(){ return this.positionHex; }
    public bool HasReachedDestination(){ return this.hasReachedDestination; }
    public Hex GetHexDestination(){ return this.destinationHex; }
    public bool IsDestinationDifferentThanPosition(){ return this.isDestinationDifferentThanPosition; }
    public float GetMovementSpeed(){ return this.movementSpeed; }
    public Role GetRole(){ return this.role; }
    public Sex GetSexe(){ return this.sex; }
    public Building.WorkerSpot GetWorkerSpot(){ return this.workerSpot; }
    public Transform GetDestination(){ return this.destination; }
    public int GetHealth(){ return this.health; }

    public void SetAge(int age){ this.age = age; }
    public void SetHungerLevel(int level){ this.hungerLevel = level; }
    public void SetHasJob(bool hasJob){ this.hasJob = hasJob; }
    public void SetJobBuilding(Building b){ this.jobBuilding = b; }
    public void SetAssociatedGO(GameObject go){ this.associatedGO = go; }
    public void SetHexPosition(Hex hex){ this.positionHex = hex; }
    public void SetHasReachedDestination(bool destinationReached){ this.hasReachedDestination = destinationReached; }
    public void SetHexDestination(Hex destination){
        this.destinationHex = destination;
        if(! this.destinationHex.Equals(positionHex))
        {
            SetIsDestinationDifferentThanPosition(true);
            // Make the villager face the destination
            Geometry.FaceDestinationOnXZ(this.GetAssociatedGO().transform, destination.GetAssociatedGO().transform);
        }
    }
    public void SetDestination(Transform destinationTransform){ this.destination = destinationTransform;
                                                        Debug.Log("Villager : " + this.GetName() + " now has the destination : " + Geometry.Vector3ToString(this.destination.position)); }
    public void SetIsDestinationDifferentThanPosition(bool isDifferent){ this.isDestinationDifferentThanPosition = isDifferent; }
    public void SetRole(Role role){ this.role = role; }
    public void SetWorkerSpot(Building.WorkerSpot spot){ this.workerSpot = spot; }
    public void SetHealth(int villagerHealth){ this.health = villagerHealth; }



    public string ChooseRandomName()
    {
        string nameToReturn = "noName";
        if (this.GetSexe().Equals(Villager.Sex.Male))
        {
            nameToReturn = villagerMaleNamesList[Random.Range(0, villagerMaleNamesList.Length)];
        }
        else
        {
            nameToReturn = villagerFemaleNamesList[Random.Range(0, villagerFemaleNamesList.Length)];
        }
        return nameToReturn;
    }

    public Sex ChooseRandomSex()
    {
        return (Sex)Random.Range(0, System.Enum.GetValues(typeof(Sex)).Length);
    }

    public Villager(int age){
        this.id = WorkersManager.instance.GetCurrentWorkerID();
        WorkersManager.instance.IncrementWorkerID();
        this.age = age;
        this.hungerLevel = 10;
        this.sex = ChooseRandomSex();
        this.name = ChooseRandomName();
        if(age <= 19){ this.role = Role.Child; } else { this.role = Role.Worker; }
        this.hasJob = false;
    }

    public Villager(){
        this.id = WorkersManager.instance.GetCurrentWorkerID();
        WorkersManager.instance.IncrementWorkerID();
        this.age = Random.Range(0, 40);
        this.health = 100;
        this.hungerLevel = 10;
        this.sex = ChooseRandomSex();
        this.name = ChooseRandomName();
        if(age <= 19){ this.role = Role.Child; } else { this.role = Role.Worker; }
        this.hasJob = false;
    }

    public override string ToString()
    {
        return ("Name : " + this.name + " | Age : " + this.age + " | Sex : " + this.sex.ToString() + " | Role : " + this.role.ToString() + " | Hunger level : " + this.hungerLevel);
    }

    public void Eat(int nb)
    {
        this.hungerLevel = Mathf.Min(this.hungerLevel + nb, 10);
    }

    public void DecreaseHungerLevel(int nb)
    {
        this.hungerLevel = Mathf.Max(this.hungerLevel - nb, 0);
    }

    public void IncrementAge()
    {
        this.age++;
    }

    public void MoveVillagerOverHex(Hex hex)
    {
        //Debug.Log("Moving villager " + this.GetName() + " on " + hex.ToString());
        Vector3 newPos = new Vector3(hex.GetAssociatedGO().transform.position.x, VillagersManager.instance.villagersYOffset, hex.GetAssociatedGO().transform.position.z);
        this.GetAssociatedGO().transform.position = newPos;
        this.SetHexPosition(hex);
    }

    public void MoveVillagerTowardHex(Hex destination)
    {
        // If the villager is not already there
        if (!IsPhysicallyOnHex(destination))
        {
            Debug.Log("VM | MoveVillagerTowardHex");
            //Debug.Log("Moving Villager [" + this.GetName() + "] toward " + destination.ToString());
            Vector3 villagerPosition = this.GetAssociatedGO().transform.position;
            //Debug.Log("Villager position : " + Geometry.Vector3ToString(villagerPosition));
            Vector3 destinationPosition = destination.GetTopPointOfHex().transform.position;
            Debug.Log("DestinationPosition : " + Geometry.Vector3ToString(destinationPosition));
            //Debug.Log("Destination position : " + Geometry.Vector3ToString(destinationPosition));

            Vector3 newVillagerPosition = Vector3.MoveTowards(villagerPosition, destinationPosition, Time.deltaTime * GetMovementSpeed());
            //Debug.Log("New villager position : " + Geometry.Vector3ToString(newVillagerPosition));
            this.GetAssociatedGO().transform.position = newVillagerPosition;
        }
        else
        {
            SetIsDestinationDifferentThanPosition(false);
        }
    }

    public void MoveVillagerTowardAbsoluteDestination()
    {
        if (!IsPhysicallyOnPosition(this.destination))
        {
            this.OrientateTowardDestination();

            this.GetAssociatedGO().GetComponent<VillagerGO>().StartWalking();

            //Debug.Log("VM | MoveVillagerTowardAbsoluteDestination");
            Vector3 villagerPosition = this.GetAssociatedGO().transform.position;

            Vector3 destinationPosition = destination.position;

            Vector3 newVillagerPosition = Vector3.MoveTowards(villagerPosition, destinationPosition, Time.deltaTime * GetMovementSpeed());

            this.GetAssociatedGO().transform.position = newVillagerPosition;
        }
        else
        {
            this.GetAssociatedGO().GetComponent<VillagerGO>().StopWalking();
        }
    }

    public void MoveVillagerTowardDestination()
    {
        // No longer using hexes
        //MoveVillagerTowardHex(destinationHex);
        MoveVillagerTowardAbsoluteDestination();
    }

    public bool IsPhysicallyOnHex(Hex hex)
    {
        return (this.GetAssociatedGO().transform.position == hex.GetAssociatedGO().transform.position);
    }

    public bool IsPhysicallyOnPosition(Transform positionToCompare)
    {
        return (this.GetAssociatedGO().transform.position == positionToCompare.transform.position);
    }

    public void BecomeWorker()
    {
        this.SetRole(Villager.Role.Worker);
        WorkersManager.instance.AddWorker(this);

        Debug.Log(this.name + " has become a worker !");
    }

    public void Die()
    {
        UnityEngine.Object.Destroy(this.GetAssociatedGO());

        Debug.Log("V | DIE | Before the death, there were " + VillagersManager.instance.villagersList.Count + " villagers.");

        VillagersManager.instance.RemoveVillager(this);

        // Also remove him from worker list if he was a worker
        if(this.GetRole().Equals(Villager.Role.Worker))
        {
            WorkersManager.instance.WorkerDies(this);
        }

        EventsManager.instance.DisplayVillagerDeathMessage(this);
    }

    public bool IsVillagerInList(List<Villager> list)
    {
        return (list.Contains(this));
    }

    public void OrientateTowardDestination()
    {
        Transform destinationToLookAt = this.destination;
        Vector3 destinationPosition = destinationToLookAt.position;
        this.GetAssociatedGO().transform.LookAt(new Vector3(destinationPosition.x, this.GetAssociatedGO().transform.position.y, destinationPosition.z));
    }


    public void GainHealth(int healthPointsToGain)
    {
        this.health = Mathf.Min(this.health + healthPointsToGain, 100);
    }

    public void LoseHealth(int healthPointsToLose)
    {
        this.health -= healthPointsToLose;
        Debug.Log("Villager " + this.name + " health is now : " + this.health);

        // The villager dies if its healthpoints go down to 0
        if(this.health <= 0)
        {
            Die();
        }
    }

    public void GainHungerPoints(int hungerPointsToGain)
    {
        this.hungerLevel = Mathf.Min(this.hungerLevel + hungerPointsToGain, 10);
    }

    public void LoseHungerPoints(int hungerPointsToLose)
    {
        this.hungerLevel = Mathf.Max(hungerLevel - hungerPointsToLose, 0);
        Debug.Log("Villager " + this.name + " hunger points is now : " + hungerLevel);

        // The villager dies if its healthpoints go down to 0
        if(this.hungerLevel == 0)
        {
            LoseHealth(40);
        }
    }
}
