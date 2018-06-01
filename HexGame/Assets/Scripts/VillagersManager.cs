using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class VillagersManager : MonoBehaviour {

    public static VillagersManager instance;

    void Awake()
    {   
        if(instance != null)
        {
            Debug.LogError("More than one VillagersManager in scene !");
            return;
        }
        instance = this;

    }

    public enum VillagerStatus {Child, Student, Worker};

    public GameObject villagerGO;
    public GameObject defaultSpawnPoint;

    public List<Villager> villagersList = new List<Villager>();
    public int startVillagersNumber = 10;
    public int currentVillagersNumber = 0;

    public int studentNumber;
    public List<Villager> studentList = new List<Villager>();

    public GameObject villagerNumberIndicator;
    public GameObject studentNumberIndicator;

    public float villagersYOffset = 1f;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        ManageVillagersMovement();
	}


    // Initialize existing villagers at the start of the game
    public void InitializeStartVillagers()
    {
        for (int i = 0; i < startVillagersNumber; i++)
        {
            villagersList.Add(new Villager());
        }
        currentVillagersNumber = startVillagersNumber;
        UpdateVillagerNumberIndicator();
        InstantiateStartingVillagers();
        DisplayStartVillagersList();
    }

    public void AddNewVillager()
    {
        Villager newVillager = new Villager();
        villagersList.Add(newVillager);
        currentVillagersNumber++;
        UpdateVillagerNumberIndicator();
        InstantiateVillager(newVillager);
    }

    public void DisplayStartVillagersList()
    {
        Debug.Log("Starting Villagers : ");
        foreach (var villager in villagersList)
        {
            Debug.Log(villager.ToString());
        }
    }

    public void UpdateVillagerNumberIndicator()
    {
        villagerNumberIndicator.GetComponent<Text>().text = currentVillagersNumber.ToString();
    }

    public void InstantiateVillager(Villager villager)
    {
        Hex randomHex = HexManager.instance.SelectRandomHex();

        villager.SetHexPosition(randomHex);

        Vector3 destination = randomHex.GetTopPointOfHex().transform.position;

        int randomOrientationAngle = Geometry.GetRandomOrientationAngle();

        GameObject instantiatedVGO = Instantiate(villagerGO, destination, Quaternion.Euler(new Vector3(0,randomOrientationAngle,0)));
        villager.SetAssociatedGO(instantiatedVGO);
        instantiatedVGO.GetComponent<VillagerGO>().SetAssociatedVillager(villager);
        instantiatedVGO.transform.SetParent(instance.transform, false);
    }

    public void InstantiateStartingVillagers()
    {
        foreach (var villager in villagersList)
        {
            InstantiateVillager(villager);
        }
    }

    public void IncrementAllAges()
    {
        foreach (var villager in villagersList)
        {
            villager.IncrementAge();
            if(villager.GetAge() == 20)
            {
                villager.BecomeWorker();
            }
            else if (villager.GetAge() == 40)
            {
                villager.Die();
            }
        }
    }

    public void MoveVillagersRandomly()
    {
        foreach (var villager in villagersList)
        {
            MoveVillagerRandomly(villager);  
        }
    }

    public void MoveVillagerRandomly(Villager villager)
    {
        Hex newHex = HexManager.instance.GetRandomAdjacentHex(villager.GetHexPosition());
        villager.MoveVillagerOverHex(newHex);
    }

    public void ChoseVillagersRandomDestination()
    {
        foreach (var villager in villagersList)
        {
            ChoseVillagerRandomDestination(villager);  
        }
    }

    public void ChoseVillagerRandomDestination(Villager villager)
    {
        villager.SetHexDestination(HexManager.instance.GetRandomHex());

    }

    public void ManageVillagersMovement()
    {
        foreach (var villager in villagersList)
        {
            // If the villager's destination is different than his current position
            if(villager.IsDestinationDifferentThanPosition())
            {
                //villager.GetAssociatedGO().GetComponent<VillagerAnimation>().StartWalking();
                villager.MoveVillagerTowardDestination();
            }
        }
    }

    public void AddNewBornVillager()
    {
        Villager newVillager = new Villager(0);
        villagersList.Add(newVillager);
        currentVillagersNumber++;
        UpdateVillagerNumberIndicator();
        InstantiateVillager(newVillager);
    }

    public void RemoveVillager(Villager villager)
    {
        villagersList.Remove(villager);
        Debug.Log("Removed villager from list. List now contains " + villagersList.Count + " villagers.");
        UpdateVillagerCount();
    }

    public void UpdateVillagerCount()
    {
        currentVillagersNumber = villagersList.Count;
        UpdateVillagerNumberIndicator();
    }

    public void UpdateStudentCount()
    {
        studentNumber = studentList.Count;
        UpdateStudentNumberIndicator();
    }

    public void UpdateStudentNumberIndicator()
    {
        studentNumberIndicator.GetComponent<Text>().text = studentNumber.ToString();
    }

    // ---- TEST --- //
    public void MakeVillagersLoseHungerPoint()
    {
        foreach (var villager in villagersList)
        {
            villager.LoseHungerPoints(1);
        }
    }

}
