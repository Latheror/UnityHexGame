using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiomeManager : MonoBehaviour {

    // Make sure there is only one BiomeManager
    public static BiomeManager instance;

    void Awake()
    {   
        if (instance != null)
        {
            Debug.LogError("More than one BiomeManager in scene !");
            return;
        }
        instance = this;
    }

    public List<BiomeType> availableBiomeTypesList = new List<BiomeType>();
    public Dictionary<string, BiomeType> availableBiomeTypesDictionary = new Dictionary<string, BiomeType>();

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void InitialiseBiomeTypesList()
    {
        Debug.Log("BIM | Initializing biome types list and dictionary.");
        availableBiomeTypesList.Add(new BiomeType("plainBiome", new List<TerrainPercentage>()
                {
                    new TerrainPercentage(TerrainsManager.instance.terrainTypesDictionary["plain"], 60),
                    new TerrainPercentage(TerrainsManager.instance.terrainTypesDictionary["water"], 15),
                    new TerrainPercentage(TerrainsManager.instance.terrainTypesDictionary["forest"], 20),
                    new TerrainPercentage(TerrainsManager.instance.terrainTypesDictionary["mountain"], 5)
                }                  
            )
        );
        availableBiomeTypesList.Add(new BiomeType("forestBiome", new List<TerrainPercentage>()
                {
                    new TerrainPercentage(TerrainsManager.instance.terrainTypesDictionary["plain"], 20),
                    new TerrainPercentage(TerrainsManager.instance.terrainTypesDictionary["water"], 10),
                    new TerrainPercentage(TerrainsManager.instance.terrainTypesDictionary["forest"], 65),
                    new TerrainPercentage(TerrainsManager.instance.terrainTypesDictionary["mountain"], 5)
                }                  
            )
        );
        availableBiomeTypesList.Add(new BiomeType("waterBiome", new List<TerrainPercentage>()
                {
                    new TerrainPercentage(TerrainsManager.instance.terrainTypesDictionary["plain"], 15),
                    new TerrainPercentage(TerrainsManager.instance.terrainTypesDictionary["water"], 70),
                    new TerrainPercentage(TerrainsManager.instance.terrainTypesDictionary["forest"], 10),
                    new TerrainPercentage(TerrainsManager.instance.terrainTypesDictionary["mountain"], 5)
                }                  
            )
        );
        availableBiomeTypesList.Add(new BiomeType("mountainBiome", new List<TerrainPercentage>()
                {
                    new TerrainPercentage(TerrainsManager.instance.terrainTypesDictionary["plain"], 5),
                    new TerrainPercentage(TerrainsManager.instance.terrainTypesDictionary["water"], 5),
                    new TerrainPercentage(TerrainsManager.instance.terrainTypesDictionary["forest"], 20),
                    new TerrainPercentage(TerrainsManager.instance.terrainTypesDictionary["mountain"], 70)
                }                  
            )
        );

        InitialiseBiomeTypesDictionary();
        DisplayBiomeTypesList();
    }


    public void InitialiseBiomeTypesDictionary()
    {
        foreach (var biome in availableBiomeTypesList) {
            availableBiomeTypesDictionary.Add(biome.name, biome);
        }
    }

    public void DisplayBiomeTypesList()
    {
        string toPrint = "Biome Types : ";
        foreach (var biomeType in availableBiomeTypesList)
        {
            toPrint += biomeType.name + " | ";
        }
        Debug.Log(toPrint);
    }

    public class Biome
    {
        public BiomeType biomeType;
        public int biomeDiameter;    // Consider a square, a circle ?...
    }

    public class BiomeType
    {

        public string name;

        public List<TerrainPercentage> terrainPercentages = new List<TerrainPercentage>();

        public BiomeType(string biomeName, List<TerrainPercentage> biomeTerrainPercentages)
        {
            this.name = biomeName;
            this.terrainPercentages = biomeTerrainPercentages;
        }

    }


    public struct TerrainPercentage
    {
        public TerrainsManager.TerrainType terrainType;
        public int percentage;

        public TerrainPercentage(TerrainsManager.TerrainType tType, int percent)
        {
            this.terrainType = tType;
            if(percent >= 0 && percent <= 100)
            {
                this.percentage = percent;
            }else{
                Debug.Log("Invalid terrain parameters !");
                this.percentage = 0;
            }
        }
    }


    public List<TerrainsManager.TerrainType> BuildBiomeOfLinearSize(BiomeType biomeType, int linearSize)
    {
        //Debug.Log("BIM | Building biome of linear size [" + linearSize + "].");

        List<TerrainsManager.TerrainType> listToReturn = new List<TerrainsManager.TerrainType>();
        // Iterate through the tiles
        for (int i = 0; i < linearSize; i++)
        {
            int randomNumber = (int)Random.Range(0, 100);
            bool hasTerrainTypeBeenAttributed = false;
            // Set result to a default value
            TerrainsManager.TerrainType chosenTerrainType = TerrainsManager.instance.terrainTypesDictionary["plain"];
            int index = 0;
            int sumOfPreviousTerrainPercentages = 0;

            //Debug.Log("BIM | BiomeType : " + biomeType.name + " | Random number : " + randomNumber);

            while(hasTerrainTypeBeenAttributed == false /* || sumOfPreviousTerrainPercentages < 101 */)
            {
                int terrainPercentage = biomeType.terrainPercentages[index].percentage;
                sumOfPreviousTerrainPercentages += terrainPercentage;

                if(randomNumber <= sumOfPreviousTerrainPercentages)
                {
                    hasTerrainTypeBeenAttributed = true;
                    chosenTerrainType = biomeType.terrainPercentages[index].terrainType;
                }

                index++;
            }

            //Debug.Log("BIM | ChosenTerrainType : " + chosenTerrainType.name);

            // Add the TerrainType to the list to return
            listToReturn.Add(chosenTerrainType);
        }

        return listToReturn;
    }

    public List<TerrainsManager.TerrainType> BuildBiomeOfSquareSize(BiomeType biomeType, int squareSize)
    {
        //Debug.Log("Building biome of square size [" + squareSize + "].");
        return BuildBiomeOfLinearSize(biomeType, (int)Mathf.Pow(squareSize, 2));
    }


}
