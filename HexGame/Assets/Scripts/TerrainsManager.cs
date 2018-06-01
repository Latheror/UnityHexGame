using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TerrainsManager : MonoBehaviour
{

    // Make sure there is only one TerrainsManager
    public static TerrainsManager instance;

    void Awake()
    {   
        if(instance != null)
        {
            Debug.LogError("More than one TerrainsManager in scene !");
            return;
        }
        instance = this;

    }


    public Dictionary<string, TerrainType> terrainTypesDictionary = new Dictionary<string, TerrainType>();


    public void BuildTerrainTypesDictionary()
    {
        terrainTypesDictionary.Add("plain", new TerrainType("plain", ColorManager.instance.plainDefaultColor, DecorManager.instance.plainGO, 1));
        terrainTypesDictionary.Add("mountain", new TerrainType("mountain", ColorManager.instance.mountainDefaultColor, DecorManager.instance.mountainGO, 10));
        terrainTypesDictionary.Add("forest", new TerrainType("forest", ColorManager.instance.forestDefaultColor, DecorManager.instance.forestGO, 2));
        terrainTypesDictionary.Add("water", new TerrainType("water", ColorManager.instance.waterDefaultColor, DecorManager.instance.mountainGO, 10));
    }

    public TerrainType RandomTerrainChoiceOne()
    {
        int waterPercentage = StartSettingsManager.instance.waterMapPercentage;

        // Debug.Log("Water should fill " + waterPercentage + " % of the map.");

        TerrainType terrain = null;

        int randomWaterDetermination = Random.Range(1,101);

        // Water
        if(randomWaterDetermination <= waterPercentage)
        {
            // This will be water
            terrain = terrainTypesDictionary["water"];
        }
        else
        {
            // This won't be water

            int randomTerrain = Random.Range(1,4);

            switch(randomTerrain)
            {
                case 1 :
                    terrain = terrainTypesDictionary["plain"];
                    break;
                case 2 :
                    terrain = terrainTypesDictionary["mountain"];
                    break;
                case 3 :
                    terrain = terrainTypesDictionary["forest"];
                    break;
            }
        }
        return terrain;
    }


    public bool CanBuildOnThisTerrain(Hex hex)
    {
        return ( ! hex.terrainType.name.Equals("water")  ); 
    }


    [System.Serializable]
    public class TerrainType
    {
        public string name;

        public Color associatedDefaultColor = Color.green;

        public ColorManager.SeasonsColors seasonsColors;

        public GameObject decorGO;

        public int pathFindingWalkingCost;

        public string Name()
        {
            return name;
        }


        public TerrainType(string name)
        {
            this.name = name;
            associatedDefaultColor = Color.green;
        }

        public TerrainType(string name, Color defaultColor)
        {
            this.name = name;
            this.associatedDefaultColor = defaultColor;
        }

        public TerrainType(string name, Color defaultColor, GameObject decor, int walkingCost)
        {
            this.name = name;
            this.associatedDefaultColor = defaultColor;
            this.decorGO = decor;
            this.pathFindingWalkingCost = walkingCost;
        }

        public bool Equals(TerrainType terrainTypeToCompare)
        {
            return (this.name == terrainTypeToCompare.name);
        }


    }
    



}

