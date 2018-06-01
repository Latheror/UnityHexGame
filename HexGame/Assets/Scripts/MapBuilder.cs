    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class MapBuilder : MonoBehaviour {

        public static MapBuilder instance;

        void Awake()
        {   
            if(instance != null)
            {
                Debug.LogError("More than one MapBuilder in scene !");
                return;
            }
            instance = this;
        }

        public int mapSize = 10;
        public GameObject hexPrefab;
        public float padding = 0f;
        //private float size = 1f;

        public Color tileBorderColor;
        public Color tileCenterColor;

        public float perlinXOffset = 3.5f;
        public float perlinYOffset = 4.5f;

        public int perlinCoordScale = 100;

        public float minMountainAltitude = 0.5f;

        public bool usingBiomes = true;



    	// Use this for initialization
    	void Start () {

    	}
    	
    	// Update is called once per frame
    	void Update () {
    		
    	}

        public void SetMapSize(int size)
        {
            mapSize = size;
        }

        public void GetMapSizeFromStartSettings()
        {
            mapSize = StartSettingsManager.instance.mapSize;
        }

        public void MapStartingOperations()
        {
            GetMapSizeFromStartSettings();


            Material[] tileMaterials = hexPrefab.GetComponentInChildren<MeshRenderer>().sharedMaterials;
            tileBorderColor = tileMaterials[0].color;
            tileCenterColor = tileMaterials[1].color;

            ColorManager.instance.hexBorderColor = tileBorderColor;
            ColorManager.instance.hexCenterColor = tileCenterColor;

            BuildTileMap();
        }

        public void BuildTileMap()
        {

            for (int x = 0; x < mapSize; x++)
            {
                for (int y = 0; y < mapSize; y++)
                {
                    // Creating a new hex with its coordinates
                    Hex newHex = new Hex(x, y);

                    // Instantiate a hex prefab at corresponding location on the map
                    Vector3 tilePos = calculateTilePos(newHex);
                    GameObject instantiatedHexGO = Instantiate(hexPrefab, tilePos, hexPrefab.transform.rotation);

                    // Attaching the instantiated prefab to the new Hex
                    newHex.associatedGO = instantiatedHexGO;

                    // Putting a reference to the hex into the instantiated GameObject
                    instantiatedHexGO.GetComponent<HexGO>().ownerHex = newHex;
                    instantiatedHexGO.GetComponent<HexGO>().x = x;
                    instantiatedHexGO.GetComponent<HexGO>().y = y;

                    // Add the hex to the list
                    HexManager.instance.AddHexToList(newHex);
                }
            }

            if (usingBiomes == false)
            {
                Debug.Log("MB | Building map without using biomes.");
                foreach (var hex in HexManager.instance.hexList)
                {
                    // Randomly chose a terrain
                    hex.terrainType = TerrainsManager.instance.RandomTerrainChoiceOne(); 
                }
            }
            else   
            // We are using biomes
            {
                //Debug.Log("MB | Building map using biomes.");
                // ----- Temporary ------
                int biomeSquareSize = (int)mapSize / 2; // Let's divide the map into 4 biomes

                // First biome, bottom left corner
                BiomeManager.BiomeType firstBiome = BiomeManager.instance.availableBiomeTypesDictionary["plainBiome"];
                List<TerrainsManager.TerrainType> firstBiomeTerrainsList = BiomeManager.instance.BuildBiomeOfSquareSize(firstBiome, biomeSquareSize);
                //Debug.Log("MB | firstBiomeTerrainList has [" + firstBiomeTerrainsList.Count + "] elements (should have 25).");

                // Second biome, top left corner
                BiomeManager.BiomeType secondBiome = BiomeManager.instance.availableBiomeTypesDictionary["waterBiome"];
                List<TerrainsManager.TerrainType> secondBiomeTerrainsList = BiomeManager.instance.BuildBiomeOfSquareSize(secondBiome, biomeSquareSize);
                //Debug.Log("MB | secondBiomeTerrainList has [" + firstBiomeTerrainsList.Count + "] elements (should have 25).");

                // Third biome, bottom right corner
                BiomeManager.BiomeType thirdBiome = BiomeManager.instance.availableBiomeTypesDictionary["forestBiome"];
                List<TerrainsManager.TerrainType> thirdBiomeTerrainsList = BiomeManager.instance.BuildBiomeOfSquareSize(thirdBiome, biomeSquareSize);

                // Fourth biome, top right corner
                BiomeManager.BiomeType fourthBiome = BiomeManager.instance.availableBiomeTypesDictionary["mountainBiome"];
                List<TerrainsManager.TerrainType> fourthBiomeTerrainsList = BiomeManager.instance.BuildBiomeOfSquareSize(fourthBiome, biomeSquareSize);
           

                // Apply the biome terrains on the hexes of the bottom left corner
                for(int x=0; x<biomeSquareSize ;x++)
                {
                    for (int y = 0; y < biomeSquareSize; y++)
                    {
                        Hex hex = HexManager.instance.GetHexFromCoordinates(x, y);
                        hex.terrainType = firstBiomeTerrainsList[x * biomeSquareSize + y];
                    }
                }
                for(int x=0; x<biomeSquareSize ;x++)
                {
                    for (int y = biomeSquareSize; y < mapSize; y++)
                    {
                        Hex hex = HexManager.instance.GetHexFromCoordinates(x, y);
                        hex.terrainType = secondBiomeTerrainsList[x * biomeSquareSize + (y - biomeSquareSize)];
                    }
                }
                for(int x=biomeSquareSize; x<mapSize ;x++)
                {
                    for (int y = 0; y < biomeSquareSize; y++)
                    {
                        Hex hex = HexManager.instance.GetHexFromCoordinates(x, y);
                        hex.terrainType = thirdBiomeTerrainsList[(x - biomeSquareSize) * biomeSquareSize + y];
                    }
                }
                for(int x=biomeSquareSize; x<mapSize ;x++)
                {
                    for (int y = biomeSquareSize; y < mapSize; y++)
                    {
                        Hex hex = HexManager.instance.GetHexFromCoordinates(x, y);
                        hex.terrainType = fourthBiomeTerrainsList[(x - biomeSquareSize) * biomeSquareSize + (y - biomeSquareSize)];
                    }
                }
 }

            // Intermediate operations 

            // -------- DEBUG -------- //
            Debug.Log("DEBUG - Verifying list of map Hexes | ");
            Debug.Log(HexManager.instance.PrintListOfMapHexes());


            // Adjust hexes heights based on the terrain
            AdjustHexHeights();

            // TEST ----- //
            ArrangeTerrainGroups();

            // Following operations to do on each hex
            foreach (var hex in HexManager.instance.GetHexList())
            {
                    // Set the color of the hex corresponding to its type
                    hex.ApplyDefaultColor();

                    // Add the corresponding decor on top of the hex
                    DecorManager.instance.BuildDecor(hex);

                    // Put the instantiated Hex GameObject under the HexManager
                    hex.GetAssociatedGO().transform.SetParent(HexManager.instance.transform);
            }

            // Build lakes/rivers
            DecorManager.instance.ArrangeLakes();
            // Build mountains
            DecorManager.instance.ArrangeMountains();

        }

        // Tile position (X and Z only, Y is adjusted with the terrain
        public Vector3 calculateTilePos(Hex hex)
        {
            Vector3 pos = new Vector3();
            int hexX = hex.x;
            int hexY = hex.y;

            if(hexY % 2 == 0)
            {
                pos.x = (float)(hexX * (Mathf.Sqrt(3) + padding));
            }else{
                pos.x = (float)(hexX * (Mathf.Sqrt(3) + padding) + Mathf.Sqrt(3)/2) + padding/2;
            }

            pos.z = (float) (hexY * (1.5 + padding));

            // Elevation of the hex
            //pos.y = CalculateHexHeight(hexX, hexY);

            pos.y = 0;

            return pos;
        }

        public float CalculateHexHeight(int x, int y)
        {
            float height = Mathf.PerlinNoise((float)x / mapSize * perlinCoordScale + perlinXOffset, (float)y / mapSize * perlinCoordScale + perlinYOffset);

            //Debug.Log("Calculating Perlin Height for Hex(" + x + ";" + y + "), MapSize = " + mapSize + " : " + height);

            return Mathf.Max(height, 0f);
        }


        // Adjust Hexes height related to terrains
        public void AdjustHexHeights()
        {
            List<Hex> hexList = HexManager.instance.GetHexList();

            foreach (var hex in hexList)
            {
                // Mountains and rivers/lakes have to be on the same level
                if(! (hex.GetTerrainType().name.Equals("mountain") || hex.GetTerrainType().name.Equals("water")))
                {
                    float currentHexX = hex.GetAssociatedGO().transform.position.x;
                    float currentHexZ = hex.GetAssociatedGO().transform.position.z;
                    float hexHeight = CalculateHexHeight(hex.x, hex.y);

                    hex.GetAssociatedGO().transform.position = new Vector3(currentHexX, hexHeight, currentHexZ);
                    hex.GetAssociatedGO().transform.localScale = new Vector3(1, 1 + 2* hexHeight, 1);
                }
            }
        }


        public void ArrangeTerrainGroups()
        {
            List<Hex> mapHexList = HexManager.instance.GetHexList();
            List<Hex> visitedHexes = new List<Hex>();


            foreach (var hex in mapHexList)
            {
                if (!hex.IsHexInList(visitedHexes))
                {
                    List<Hex> localGroup = HexManager.instance.GetGroupOfSameTerrainHexes(hex);

                    ArrangeHeightOfLocalGroup(localGroup);

                    HexManager.instance.AddListToList(localGroup, visitedHexes);
                }
            }


        }

        public void ArrangeHeightOfLocalGroup(List<Hex> localGroup)
        {
            TerrainsManager.TerrainType localGroupTerrain = HexManager.instance.GetTerrainTypeFromLocalGroup(localGroup);

            switch(localGroupTerrain.name)
            {
                case "mountain" :
                {
                        foreach (var hex in localGroup)
                        {
                            hex.ModifyHeight(Random.Range(minMountainAltitude, 1.5f * minMountainAltitude));
                        }
                        break;
                }
                default :
                {
                        break;
                }
            }

        }

    }
