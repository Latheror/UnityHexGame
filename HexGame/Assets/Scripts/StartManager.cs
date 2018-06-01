using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartManager : MonoBehaviour {


    // Make sure there is only one StartManager
    public static StartManager instance;

    void Awake()
    {   
        if (instance != null)
        {
            Debug.LogError("More than one StartManager in scene !");
            return;
        }
        instance = this;

    }

    void Start()
    {

        MenuSettingsManager msm = MenuSettingsManager.instance;
        StartSettingsManager sm = StartSettingsManager.instance;
        TerrainsManager tm = TerrainsManager.instance;
        MapBuilder mb = MapBuilder.instance;
        ResourcesManager rm = ResourcesManager.instance;
        BiomeManager bim = BiomeManager.instance;
        BuildManager bm = BuildManager.instance;
        UIManager um = UIManager.instance;
        CityManager cm = CityManager.instance;
        WorkersManager wm = WorkersManager.instance;
        VillagersManager vm = VillagersManager.instance;
        BuildingShopManager bsm = BuildingShopManager.instance;
        LoadingPanel lp = LoadingPanel.instance;
        PathFindingManager pfm = PathFindingManager.instance;


        sm.GetWaterMapPercentageFromMenuSettings();
        sm.GetMapSizeFromMenuSettings();

        tm.BuildTerrainTypesDictionary();
        mb.mapSize = sm.mapSize;

        bim.InitialiseBiomeTypesList();

        // Resources
        rm.ImportResources();
        rm.FillResourcesDictionary();
        rm.BuildResourcesInformation();
        rm.BuildResourcesIndicators();
        rm.UpdateMaxResourceAmountsIndicators();
        rm.UpdateResourcesIndicators();

        bm.ImportBuildings();
        bm.FillBuildingDictionary();

        bsm.BuildBuildingShop();

        // Information to gather from the menu
        //mb.usingBiomes = true;
        mb.usingBiomes = false;
        mb.MapStartingOperations();

        // TODO | DEBUG - Change this
        pfm.GeneratePathFindingGraph();
        pfm.GeneratePathFromHexToHex(HexManager.instance.GetHexFromCoordinates(0,0), HexManager.instance.GetHexFromCoordinates(6,9));

        bm.SetStartingBuildingType();

        vm.InitializeStartVillagers();

        wm.InitializeStartingWorkers();
        wm.InitializeJobSettings();

        // Remove the panel hidding the game scene
        lp.DisplayGameWindow();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
