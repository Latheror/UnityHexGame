using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BuildManager : MonoBehaviour
{

    // Make sure there is only one BuildManager
    public static BuildManager instance;

    void Awake()
    {   
        if (instance != null)
        {
            Debug.LogError("More than one BuildManager in scene !");
            return;
        }
        instance = this;
    }

    // BuildingState
    public enum BuildingState
    {
        NoOperation,
        BuildingOperation}

    ;

    public BuildingState buildingState = BuildingState.NoOperation;

    // BuildingOrientation
    public enum ConstructionOrientation
    {
        West,
        NorthWest,
        NorthEast,
        East,
        SouthEast,
        SouthWest}

    ;

    public ConstructionOrientation constructionOrientation = ConstructionOrientation.West;

    // Buildings Prefabs
    public GameObject castleGO;
    public GameObject foresterGO;
    public GameObject farmGO;
    public GameObject quarryGO;
    public GameObject warehouseGO;
    public GameObject orchardGO;
    public GameObject windmillGO;
    public GameObject bakeryGO;

    private Hex selectedHex = null;
    private BuildingType selectedBuildingType = null;

    private GameObject previewBuilding;
    private bool isPreviewBuildingToRemove = false;

    public Hex nullHex;

    private int currentBuildingNumber = 1;

    [SerializeField]
    // List of all available buildings
    private List<BuildingType> availableBuildings = new List<BuildingType>();

    public Dictionary<string, BuildingType> availableBuildingsDictionary = new Dictionary<string, BuildingType>();


    void Start()
    {

        nullHex = new Hex(-1, -1);
        selectedHex = nullHex;

        buildingState = BuildingState.NoOperation;

        isPreviewBuildingToRemove = false;

    }

    public Hex GetSelectedHex()
    {
        return selectedHex;
    }

    public void SetSelectedHex(Hex hex)
    {
        Debug.Log("New Hex selected: " + hex.terrainType.name.ToString());
        selectedHex = hex;
    }

    public BuildingType GetSelectedBuildingType()
    {
        return selectedBuildingType;
    }

    public void SetSelectedBuildingType(BuildingType b)
    {
        Debug.Log("New Building selected: " + b.name);
        selectedBuildingType = b;
    }

    // Set the selected building using its name
    public void SetSelectedBuildingType(string buildingName)
    {
        Debug.Log("New Building selected: " + buildingName);

        selectedBuildingType = availableBuildingsDictionary[buildingName];

        // Switch to construction mode
        buildingState = BuildingState.BuildingOperation;

    }

    public void PlaceBuilding(Building building, Hex hex)
    {
        Debug.Log("Placing building: " + building.name);

        int rotationAngle = getRotationAngleFromConstructionOrientation();

        // Instantiate the building prefab on the map
        GameObject builtGO = Instantiate(building.type.prefab, hex.associatedGO.transform.position, Quaternion.Euler(0, rotationAngle, 0));

        // Link the instantiated GameObject to the building
        building.instantiatedGO = builtGO;

        // Reference the worker spots
        building.ReferenceWorkerSpots();

        hex.SetBuilding(building);

        // Add the building to the CityManager list
        BuildingsManager.instance.AddBuilding(building);

        // Add the building to its BuildingType list
        building.type.AddBuildingOfThisType(building);
    }

    public void BuildBuilding()
    {
        if (!selectedHex.Equals(nullHex) && selectedBuildingType != null)
        {
            if (canBuildOnTiles(selectedBuildingType))
            {
                if (TerrainsManager.instance.CanBuildOnThisTerrain(selectedHex))
                {
                    if (!selectedHex.HasBuilding())
                    {
                        if (ResourcesManager.instance.CanPay(selectedBuildingType))
                        {
                            // FROM HERE - We know we can build

                            // Pay the required resources
                            Debug.Log("Paying the resources");
                            ResourcesManager.instance.Pay(selectedBuildingType);

                            // Create a new building from the chosen type
                            Building newBuilding = new Building(selectedBuildingType, currentBuildingNumber++);

                            // Get all hexes needed to build
                            List<Hex> neededHexes = hexNeededToBuild(selectedBuildingType, selectedHex);

                            // Store the neededHexes list in the building
                            Debug.Log("SetHexList to building: " + MapManager.instance.HexListToString(neededHexes));
                            newBuilding.SetHexList(neededHexes);

                            HexManager.instance.FlattenHexGroup(neededHexes, selectedHex);

                            // Destroy decor on the hexes
                            DestroyHexListDecors(neededHexes);

                            // Add building to Hexes in the list
                            AddBuildingToHexList(newBuilding);

                            // Place the building on the map
                            Debug.Log("Placing the building on the map");
                            PlaceBuilding(newBuilding, selectedHex);

                            newBuilding.ExecuteCreationActions();

                            // Reset the BuildingStage to default
                            BuildManager.instance.SetBuildingState(BuildingState.NoOperation);

                            RemovePreviewBuilding();

                        }
                        else
                        {
                            Debug.Log("You don't have enough resources to build this !");
                        }
                    }
                    else
                    {
                        Debug.Log("There is already a building on this hex !");
                    }
                }
                else
                {
                    Debug.Log("You can't build on this terrain !");
                }
            }
            else
            {
                Debug.Log("You can't build on these tiles !");
            }
        }
        else
        {
            Debug.Log("Can't build : Selected Building or selected Hex missing !");
        }
    }

    public void DestroyBuilding()
    {
        if (!selectedHex.Equals(nullHex))
        {
            if (selectedHex.HasBuilding())
            {
                Building buildingToDestroy = selectedHex.getCurrentBuilding();

                // Execute building type destruction functions
                buildingToDestroy.ExecuteDeletionActions();

                // Destroy building GO
                Destroy(buildingToDestroy.instantiatedGO);

                // Get hex list
                List<Hex> hexList = buildingToDestroy.GetHexList();
                Debug.Log("Removing building from HexList: " + MapManager.instance.HexListToString(hexList));

                // Reset building for the hex list
                foreach (var hex in hexList)
                {
                    hex.SetHasBuilding(false);
                }

                BuildingsManager.instance.RemoveBuilding(buildingToDestroy/*.buildingNumber*/);

                // Remove the building from its BuildingType list
                buildingToDestroy.type.RemoveBuildingOfThisType(buildingToDestroy);

            }
            else
            {
                Debug.Log("There is no building on this hex !");
            }
        }
        else
        {
            Debug.Log("No selected Hex !");
        }
    }

    public void PositionObject(GameObject objToMove, Hex locationObj)
    {
        Vector3 targetPos = locationObj.associatedGO.transform.position;
        objToMove.transform.position = new Vector3(targetPos.x, 1, targetPos.z);
    }

    public void putPreviewBuilding(Hex hexForPreview)
    {
        Debug.Log("Displaying a preview building");
        Debug.Log("TargetedHex: " + MouseManager.instance.GetTargetedHex().ToString());
        if (!MouseManager.instance.GetTargetedHex().Equals(nullHex) && selectedBuildingType != null)
        {
            int rotationAngle = getRotationAngleFromConstructionOrientation();

            previewBuilding = Instantiate(selectedBuildingType.prefab, hexForPreview.associatedGO.transform.position, Quaternion.Euler(0, rotationAngle, 0));
            isPreviewBuildingToRemove = true;
        }
    }

    public void RemovePreviewBuilding()
    {
        Debug.Log("Removing preview building.");
        if (previewBuilding != null)
        {
            Destroy(previewBuilding);
            isPreviewBuildingToRemove = false;
        }
    }

    public void AddBuildingToHexList(Building b)
    {
        List<Hex> hexList = b.GetHexList();
        foreach (var hex in hexList)
        {
            if (!hex.HasBuilding())
            {
                hex.SetBuilding(b);
                hex.SetHasBuilding(true);
            }
            else
            {
                Debug.LogError("Trying to set a building on a hex already having one !");
            }
        }
    }

    public void DestroyHexListDecors(List<Hex> hexList)
    {
        foreach (var hex in hexList)
        {
            if (hex.decor != null)
            {
                DecorManager.instance.DestroyDecor(hex);
            }
        }
    }


    // TODO : To finish
    // Return true if we can build on all hexes needed, false otherwise
    public bool canBuildOnTiles(BuildingType bt)
    {
        bool canBuild = true;

        // The "centre" hex
        Hex targetedHex = MouseManager.instance.targetedHex;
        // The list of all needed hexes
        List<Hex> neededHexes = hexNeededToBuild(bt, targetedHex);
        Debug.Log("CanBuildTest - HexList needed: " + MapManager.instance.HexListToString(neededHexes));

        MapManager.instance.CleanSuroundingHexes(targetedHex);

        foreach (var hex in neededHexes)
        {
            // If the hex is correct
            if (!hex.Equals(nullHex))
            {
                Debug.Log("One of the needed hex is : " + hex.ToString());
                if (!CanBuildOnThisHex(bt, hex))
                {
                    canBuild = false;
                }
            }
        }
        return canBuild;
    }

    public bool CanBuildOnThisHex(BuildingType bt, Hex hex)
    {
        //Debug.Log("CanBuildOnThisHex | " + hex.ToString());
        if(! hex.Equals(nullHex) && hex != null)
        {
            bool alreadyBuildingPresent = hex.HasBuilding();
            bool hexIsWater = (hex.GetTerrainType().name.Equals("water"));
            //Debug.Log("CanBuildOnHexTest: " + hex.ToString() + " | The answer is : " + ((!alreadyBuildingPresent) && (!hexIsWater)));
            return ((!alreadyBuildingPresent) && (!hexIsWater));
        }
        else
        {
            return false;
        }

    }

    // Return a list of all hex on top of which we want to build
    public List<Hex> hexNeededToBuild(BuildingType bt, Hex th)
    {
        List<Hex> hexes = new List<Hex>();
        hexes.Add(th);
        switch (bt.size)
        {
            case 1:
                break;

            case 2:
                {
                    switch (constructionOrientation)
                    {
                        case ConstructionOrientation.West:
                            hexes.Add(th.GetLeftNeighbour());
                            break;
                        case ConstructionOrientation.NorthWest:
                            hexes.Add(th.GetTopleftNeighbour());
                            break;
                        case ConstructionOrientation.NorthEast:
                            hexes.Add(th.GetToprightNeighbour());
                            break;
                        case ConstructionOrientation.East:
                            hexes.Add(th.GetRightNeighbour());
                            break;
                        case ConstructionOrientation.SouthEast:
                            hexes.Add(th.GetBottomrightNeighbour());
                            break;
                        case ConstructionOrientation.SouthWest:
                            hexes.Add(th.GetBottomleftNeighbour());
                            break;
                    }

                    break;
                }
                

            case 3:
                {
                    switch (constructionOrientation)
                    {
                        case ConstructionOrientation.West:
                            hexes.Add(th.GetLeftNeighbour());
                            hexes.Add(th.GetTopleftNeighbour());
                            break;
                        case ConstructionOrientation.NorthWest:
                            hexes.Add(th.GetTopleftNeighbour());
                            hexes.Add(th.GetToprightNeighbour());
                            break;
                        case ConstructionOrientation.NorthEast:
                            hexes.Add(th.GetToprightNeighbour());
                            hexes.Add(th.GetRightNeighbour());
                            break;
                        case ConstructionOrientation.East:
                            hexes.Add(th.GetRightNeighbour());
                            hexes.Add(th.GetBottomrightNeighbour());
                            break;
                        case ConstructionOrientation.SouthEast:
                            hexes.Add(th.GetBottomrightNeighbour());
                            hexes.Add(th.GetBottomleftNeighbour());
                            break;
                        case ConstructionOrientation.SouthWest:
                            hexes.Add(th.GetBottomleftNeighbour());
                            hexes.Add(th.GetLeftNeighbour());
                            break;
                    }

                    break;
                }


        }

        return hexes;
    }


    public void ImportBuildings()
    {
        availableBuildings.Add(
            new BuildingType(1, "Castle", castleGO,
            new List<ResourcesManager.ResourceCost>()
            {
                new ResourcesManager.ResourceCost(ResourcesManager.instance.availableResourcesDictionary["gold"], 500),
                new ResourcesManager.ResourceCost(ResourcesManager.instance.availableResourcesDictionary["wood"], 500)
            },
            new List<ResourcesManager.Production>()
            {
                new ResourcesManager.Production(ResourcesManager.instance.availableResourcesDictionary["gold"], 1, 2, false,
                    new List<ResourcesManager.ResourceAmount>()
                    {
                        //new ResourcesManager.ResourceAmount(ResourcesManager.instance.availableResourcesDictionary["wheat"],2)
                    }
                )
            },
            3,
            Resources.Load<Sprite>("castle"),
            Resources.Load<Sprite>("coins"),
            null,
            null,
            3
            )
        );

        availableBuildings.Add(
            new BuildingType(2, "Forester", foresterGO, 
                new List<ResourcesManager.ResourceCost>()
                {
                    new ResourcesManager.ResourceCost(ResourcesManager.instance.availableResourcesDictionary["gold"], 50),
                    new ResourcesManager.ResourceCost(ResourcesManager.instance.availableResourcesDictionary["wood"], 50)
                },
                new List<ResourcesManager.Production>()
                {
                    new ResourcesManager.Production(ResourcesManager.instance.availableResourcesDictionary["wood"], 1, 2)
                },
                2,
                Resources.Load<Sprite>("farm_image_2"),  // TODO : Put a Forester Imager
                Resources.Load<Sprite>("wood"),
                null,
                new List<System.Action>()
                {
                    //new System.Action(BuildingType.TestFunction)
                },
                1

            )
        );

        availableBuildings.Add(
            new BuildingType(3, "Farm", farmGO, 
                new List<ResourcesManager.ResourceCost>()
                {
                    new ResourcesManager.ResourceCost(ResourcesManager.instance.availableResourcesDictionary["gold"], 50),
                    new ResourcesManager.ResourceCost(ResourcesManager.instance.availableResourcesDictionary["wood"], 50)
                },
                new List<ResourcesManager.Production>()
                {
                    new ResourcesManager.Production(ResourcesManager.instance.availableResourcesDictionary["wheat"], 1, 2)
                },
                2,
                Resources.Load<Sprite>("farm_image_2"),
                Resources.Load<Sprite>("wheat"),
                null,
                new List<System.Action>()
                {
                    //new System.Action(BuildingType.TestFunction)
                },
                1
            )
        );


        availableBuildings.Add(
            new BuildingType(4, "Quarry", quarryGO, 
                new List<ResourcesManager.ResourceCost>()
                {
                    new ResourcesManager.ResourceCost(ResourcesManager.instance.availableResourcesDictionary["gold"], 50),
                    new ResourcesManager.ResourceCost(ResourcesManager.instance.availableResourcesDictionary["wood"], 50)
                },
                new List<ResourcesManager.Production>()
                {
                    new ResourcesManager.Production(ResourcesManager.instance.availableResourcesDictionary["stone"], 1, 2)
                },
                3,
                Resources.Load<Sprite>("quarry_image"),
                Resources.Load<Sprite>("stone"),
                null,
                new List<System.Action>()
                {
                    //new System.Action(BuildingType.TestFunction)
                },
                2
  
            )
        );

        availableBuildings.Add(
            new BuildingType(5, "Warehouse", warehouseGO, 
                new List<ResourcesManager.ResourceCost>()
                {
                    new ResourcesManager.ResourceCost(ResourcesManager.instance.availableResourcesDictionary["gold"], 50),
                    new ResourcesManager.ResourceCost(ResourcesManager.instance.availableResourcesDictionary["wood"], 50)
                },
                new List<ResourcesManager.Production>(),
                0,
                Resources.Load<Sprite>("warehouse_image"),
                null,
                new List<System.Action>()
                {
                    new System.Action(BuildingType.TestFunction),
                    new System.Action(() => ResourcesManager.instance.increaseMaxResourceAmount(ResourcesManager.instance.availableResourcesDictionary["stone"], 5000)),
                    new System.Action(() => ResourcesManager.instance.increaseMaxResourceAmount(ResourcesManager.instance.availableResourcesDictionary["wood"], 5000))
                },
                new List<System.Action>()
                {
                    new System.Action(BuildingType.TestFunction)
                },
                1
  
            )
        );

        availableBuildings.Add(
            new BuildingType(6, "Orchard", orchardGO,
                new List<ResourcesManager.ResourceCost>()
                {
                    new ResourcesManager.ResourceCost(ResourcesManager.instance.availableResourcesDictionary["gold"], 50),
                    new ResourcesManager.ResourceCost(ResourcesManager.instance.availableResourcesDictionary["wood"], 50)
                },
                new List<ResourcesManager.Production>()
                {
                    new ResourcesManager.Production(ResourcesManager.instance.availableResourcesDictionary["food"], 1, 2)
                },
                3,
                Resources.Load<Sprite>("quarry_image"),
                Resources.Load<Sprite>("apple"),
                null,
                null,
                3  
            )
        );

        availableBuildings.Add(
            new BuildingType(7, "Windmill", windmillGO,
                new List<ResourcesManager.ResourceCost>()
                {
                    new ResourcesManager.ResourceCost(ResourcesManager.instance.availableResourcesDictionary["gold"], 50),
                    new ResourcesManager.ResourceCost(ResourcesManager.instance.availableResourcesDictionary["wood"], 50)
                },
                new List<ResourcesManager.Production>()
                {
                    new ResourcesManager.Production(ResourcesManager.instance.availableResourcesDictionary["flour"], 1, 2, true,
                        new List<ResourcesManager.ResourceAmount>()
                        {
                            new ResourcesManager.ResourceAmount(ResourcesManager.instance.availableResourcesDictionary["wheat"],2)
                        }
                    )
                },
                2,
                Resources.Load<Sprite>("windmill2"),
                Resources.Load<Sprite>("flour"),
                null,
                null,
                1  
            )
        );

        availableBuildings.Add(
            new BuildingType(8, "Bakery", bakeryGO,
                new List<ResourcesManager.ResourceCost>()
                {
                    new ResourcesManager.ResourceCost(ResourcesManager.instance.availableResourcesDictionary["gold"], 50),
                    new ResourcesManager.ResourceCost(ResourcesManager.instance.availableResourcesDictionary["wood"], 50)
                },
                new List<ResourcesManager.Production>()
                {
                    new ResourcesManager.Production(ResourcesManager.instance.availableResourcesDictionary["bread"], 1, 2, true,
                        new List<ResourcesManager.ResourceAmount>()
                        {
                            new ResourcesManager.ResourceAmount(ResourcesManager.instance.availableResourcesDictionary["flour"],2)
                        }
                    )
                },
                1,
                Resources.Load<Sprite>("windmill2"),
                Resources.Load<Sprite>("bread"),
                null,
                null,
                1  
            )
        );

    }

    public void FillBuildingDictionary()
    {
        foreach (var b in availableBuildings)
        {
            availableBuildingsDictionary.Add(b.name, b);
        }
    }

    public void SetStartingBuildingType()
    {
        SetSelectedBuildingType(availableBuildingsDictionary["Castle"]);
    }


    public void ChangeConstructionOrientation()
    {
        switch (constructionOrientation)
        {
            case ConstructionOrientation.West:
                {
                    constructionOrientation = ConstructionOrientation.NorthWest;
                    break;
                }
            case ConstructionOrientation.NorthWest:
                {
                    constructionOrientation = ConstructionOrientation.NorthEast;
                    break;
                }
            case ConstructionOrientation.NorthEast:
                {
                    constructionOrientation = ConstructionOrientation.East;
                    break;
                }
            case ConstructionOrientation.East:
                {
                    constructionOrientation = ConstructionOrientation.SouthEast;
                    break;
                }
            case ConstructionOrientation.SouthEast:
                {
                    constructionOrientation = ConstructionOrientation.SouthWest;
                    break;
                }
            case ConstructionOrientation.SouthWest:
                {
                    constructionOrientation = ConstructionOrientation.West;
                    break;
                }
        }
        Debug.Log("Changing construction orientation to " + constructionOrientation.ToString());
    }

    public int getRotationAngleFromConstructionOrientation()
    {
        int angle = 0;

        switch (constructionOrientation)
        {
            case ConstructionOrientation.West:
                {
                    angle = 0;
                    break;
                }
            case ConstructionOrientation.NorthWest:
                {
                    angle = 60;
                    break;
                }
            case ConstructionOrientation.NorthEast:
                {
                    angle = 120;
                    break;
                }
            case ConstructionOrientation.East:
                {
                    angle = 180;
                    break;
                }
            case ConstructionOrientation.SouthEast:
                {
                    angle = 240;
                    break;
                }
            case ConstructionOrientation.SouthWest:
                {
                    angle = 300;
                    break;
                }
        }

        //Debug.Log("Building will be built with an angle of + " + angle + " degres.");
        return angle;
    }

    public int getRandomHexAngle()
    {   
        int angle = 0;
        int r = Random.Range(1, 7);
        switch (r)
        {
            case 1:
                angle = 0;
                break;
            case 2:
                angle = 60;
                break;
            case 3:
                angle = 120;
                break;
            case 4:
                angle = 180;
                break;
            case 5:
                angle = 240;
                break;
            case 6:
                angle = 300;
                break;     
        }
        return angle;
    }

    public BuildingState GetBuildingState()
    {
        return buildingState;
    }

    public void SetBuildingState(BuildingState state)
    {
        // Clean last frame and selected highlighted hex when going to buildingState
        if (state == BuildingState.BuildingOperation)
        {
            MouseManager.instance.CleanLastFrameHex();
            MouseManager.instance.CleanSelectedHex();
        }

        //Debug.Log("Building State changed to " + state.ToString());
        buildingState = state;
    }

    public bool IsPreviewBuildingToRemove()
    {
        return isPreviewBuildingToRemove;
    }


    // Different buildings available
    [System.Serializable]
    public class BuildingType
    {
        public int id;
        public string name = "noName";
        public int health = 100;
        public int size;
        public GameObject prefab;
        public Sprite image = null;
        public Sprite imageWorkerSetting = null;

        // Maximum number of workers one building of this type can use
        public int maxWorkersPerBuildingNumber = 0;

        // Number of workers working in all buildings of this type
        public int totalWorkersNumber = 0;

        // Resources needed to build this building
        public List<ResourcesManager.ResourceCost> resourcesCost;

        public List<ResourcesManager.Production> productions;

        public List<Building> buildingList = new List<Building>();

        // Actions to do when creating or deleting a building
        public List<System.Action> creationActions = new List<System.Action>();
        public List<System.Action> deletionActions = new List<System.Action>();


        // Constructors
        public BuildingType(int id, string name, GameObject prefab, List<ResourcesManager.ResourceCost> resourcesCost, List<ResourcesManager.Production> productions, int maxWorkersNb, Sprite img, Sprite imgWorkerSetting, List<System.Action> cActions, List<System.Action> dActions, int size)
        {
            this.id = id;
            this.name = name;
            this.prefab = prefab;
            this.size = size;
            this.resourcesCost = resourcesCost;
            this.productions = productions;
            this.maxWorkersPerBuildingNumber = maxWorkersNb;
            this.totalWorkersNumber = 0;
            this.image = img;
            this.imageWorkerSetting = imgWorkerSetting;
            this.creationActions = cActions;
            this.deletionActions = dActions;
        }

        public void AddBuildingOfThisType(Building b)
        {
            buildingList.Add(b);

            WorkersManager.instance.AttributeWorkersNb();
        }

        public void RemoveBuildingOfThisType(Building b)
        {

            buildingList.Remove(b);

            //Debug.Log("There is now " + buildingList.Count + " buildings of this type.");

            WorkersManager.instance.AttributeWorkersNb();

        }

        // TODO : Remove
        public static void TestFunction()
        {
            Debug.Log("The test function worked !");
        }

    }



}
