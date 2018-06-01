using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseManager : MonoBehaviour
{

    // Make sure there is only one MouseManager
    public static MouseManager instance;

    void Awake()
    {   
        if (instance != null)
        {
            Debug.LogError("More than one MouseManager in scene !");
            return;
        }
        instance = this;
    }

    public DetailsPanel detailsPanel;


    public BuildManager buildManager;

    public Hex targetedHex = new Hex(0, 0);
    public Hex selectedHex = new Hex(0, 0);
    public Hex previousHex = new Hex(-1, -1);
    public Hex lastFrameHex = new Hex(-1, -1);

    public Hex nullHex = new Hex(-1, -1);

    public bool previousHexNeighboursCleaned = true;

    public Hex GetTargetedHex()
    {
        return targetedHex;
    }

    // Use this for initialization
    void Start()
    {

        nullHex = new Hex(-1, -1);
        previousHex = nullHex;
        lastFrameHex = nullHex;
        targetedHex = nullHex;
        selectedHex = nullHex;

        //Debug.Log("START FUNCTION - lastFrameHex: " + lastFrameHex.toString());
    }
	
    // Update is called once per frame
    void Update()
    {

        // GameObjects that are part of the EventSystem (like UI)
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        // Also check is game is paused
        // If menu is opened ?

        // Raycast
        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        if (Physics.Raycast(cameraRay, out hitInfo))
        {
            Transform hitObject = hitInfo.collider.transform;
            //Debug.Log("MouseM | We hit an object with the tag : " + hitObject.tag);

            // Detection of a Hex
            if (hitObject.gameObject.name == "newHex")
            {
                //Debug.Log("Mousing over a hex");

                // Get the hexGO and the hex
                HexGO hexGO = hitObject.parent.gameObject.GetComponent<HexGO>();
                targetedHex = hexGO.ownerHex;

                // If the hex is correct
                if (!targetedHex.Equals(nullHex))
                {
                    //Debug.Log("MouseOverHex - Hex: " + targetedHex.ToString());
                    MouseOverHex(targetedHex);
                }
            }
            // Detection of a Villager
            else if (hitObject.gameObject.tag == "Villager")
            {
                //Debug.Log("We are mousing over a villager !");
                Villager villager = hitObject.gameObject.GetComponent<VillagerGO>().associatedVillager;
                MouseOverVillager(villager);
            }
        }
    }


    // We are mousing over a hex
    void MouseOverHex(Hex tHex)
    {

        // Get its mesh renderer
        MeshRenderer mr = tHex.associatedGO.GetComponentInChildren<MeshRenderer>();

        // Initialize lastFrame & previous hexes (only the first time)
        if (lastFrameHex.Equals(nullHex))
        {
            //Debug.Log("Initializing lastFrameHex into: " + tHex.ToString());
            lastFrameHex = tHex;
        }
        if (previousHex.Equals(nullHex))
        {
            //Debug.Log("Initializing previousHex into: " + tHex.ToString());
            previousHex = tHex;
        }

        // If the current hex is different from the last frame one, update previous hex
        if (!lastFrameHex.Equals(tHex))
        {
            previousHex = lastFrameHex;
        }

        // ----------------------------------------- MODES -------------------------------------------------- //
        // If we are in building mode
        if (BuildManager.instance.GetBuildingState() == BuildManager.BuildingState.BuildingOperation)
        {

            // We are clicking
            if (Input.GetMouseButtonDown(0))
            {
                // The selected hex is now the one we hover on
                selectedHex = tHex;
                // Put it in red
                selectedHex.SetSelectedColor();

                // Apply the selectedHex of Buildmanager
                BuildManager.instance.SetSelectedHex(selectedHex);

                // Build a building there if we are in BuildingState
                if (BuildManager.instance.GetBuildingState() == BuildManager.BuildingState.BuildingOperation)
                {
                    BuildManager.instance.BuildBuilding();
                    MapManager.instance.CleanSuroundingHexes(tHex);
                }
            }
            else
            // We are not clicking
            {
                // Is a building selected ?
                DisplayBuildingPreview(tHex);
            }
        }
        else
        // We are in normal mode
        {
            // We are clicking
            if (Input.GetMouseButtonDown(0))
            {
                // Clean previously selected hex if different and correct
                if (!selectedHex.Equals(tHex) && !selectedHex.Equals(nullHex))
                {
                    //Debug.Log("Selecting a new hex");
                    CleanSelectedHex();
                }

                // The selected hex is now the one we hover on
                selectedHex = tHex;
                // Put it in red
                selectedHex.SetSelectedColor();

                // Apply the selectedHex of Buildmanager
                BuildManager.instance.SetSelectedHex(selectedHex);

                ClickOnHex(selectedHex);             

            }
            else
            // We are not clicking
            {
                // We are not hovering over the selected hex
                {

                    // We are hovering over the same hex as last frame
                    if (lastFrameHex.Equals(tHex))
                    {
                        
                    }
                    else
                    // We are hovering over a different hex than last frame
                    {
                        // First clean last frame hex
                        CleanLastFrameHex();

                        // We are hovering over the selected hex
                        if (tHex.Equals(selectedHex))
                        {
                            // TODO - Atm: do nothing

                        }
                        else
                        {
                            // Set hover color
                            tHex.SetHoverColor();
                        }
                    }
                }
            }
        }

        // Update lastFrameHex
        lastFrameHex = tHex;
    }

    public void MouseOverVillager(Villager villager)
    {
        if (Input.GetMouseButtonDown(0))
        {
            ClickOnVillager(villager);
        }
    }


    // TODO : deal with can build or not
    void DisplayBuildingPreview(Hex tHex)
    {
        // Remove previous building preview
        if (BuildManager.instance.IsPreviewBuildingToRemove())
        {
            BuildManager.instance.RemovePreviewBuilding();
        }

        // Display the preview of the building
        BuildManager.instance.putPreviewBuilding(tHex);

        // Get a list of hexes needed to build
        List<Hex> neededHexes = BuildManager.instance.hexNeededToBuild(BuildManager.instance.GetSelectedBuildingType(), targetedHex);

        // If we hover over a new hex, clean previous hexes indications
        if (!tHex.Equals(lastFrameHex))
        {
            // First, clean previous hex building indications
            MapManager.instance.CleanSuroundingHexes(previousHex);
            CleanPreviouseHex();

            foreach (var hex in neededHexes)
            {   
                if((! hex.Equals(nullHex)) && !( hex == null))
                {
                    // Can we build here ?
                    bool canBuildOnHex = BuildManager.instance.CanBuildOnThisHex(BuildManager.instance.GetSelectedBuildingType(), hex);

                    // We can build here
                    if (canBuildOnHex)
                    {
                        MapManager.instance.HighlightCanBuildHex(hex);
                    }
                    else
                    // We can't build here
                    {
                        MapManager.instance.HighlightCantBuildHex(hex);
                    }
                }
            }
        }
        else
        // This is the same hex
        {

        }
    }


    // Clean lastFrame hex colors
    public void CleanLastFrameHex()
    {
        // lastFrameHex is correct & is not the selected hex
        if (!lastFrameHex.Equals(nullHex) && !lastFrameHex.Equals(selectedHex))
        {
            Color defaultColor = lastFrameHex.terrainType.associatedDefaultColor;
            MeshRenderer mr = lastFrameHex.associatedGO.GetComponentInChildren<MeshRenderer>();
            mr.materials[1].color = ColorManager.instance.seasonColor1.GetColorFromCurrentSeason();
            mr.materials[0].color = ColorManager.instance.hexBorderColor;
        }
    }

    // Clean lastFrame hex colors
    public void CleanPreviouseHex()
    {
        // lastFrameHex is correct & is not the selected hex
        if (!previousHex.Equals(nullHex))
        {
            Color defaultColor = previousHex.terrainType.associatedDefaultColor;
            MeshRenderer mr = previousHex.associatedGO.GetComponentInChildren<MeshRenderer>();
            mr.materials[1].color = ColorManager.instance.seasonColor1.GetColorFromCurrentSeason();
            mr.materials[0].color = ColorManager.instance.hexBorderColor;
        }
    }


    public void CleanTargetedHex()
    {
        if (!targetedHex.Equals(nullHex))
        {
            if (targetedHex.associatedGO == null)
            {
                Debug.Log("The targetedHex has no associatedGO...");
            }
                
            Color defaultColor = targetedHex.terrainType.associatedDefaultColor;

            GameObject associatedGO = targetedHex.associatedGO;

            MeshRenderer mr = associatedGO.GetComponentInChildren<MeshRenderer>();
            mr.materials[1].color = ColorManager.instance.seasonColor1.GetColorFromCurrentSeason();
            mr.materials[0].color = ColorManager.instance.hexBorderColor;
        }
        else
        {
            Debug.Log("ERROR - CleanTargetedHex - No targeted hex !");
        }

        // Remove preview building if we are in BuildingState
        if (BuildManager.instance.GetBuildingState() == BuildManager.BuildingState.BuildingOperation)
        {
            //Debug.Log("Removing building preview from a hex.");
            if (BuildManager.instance.IsPreviewBuildingToRemove())
            {
                BuildManager.instance.RemovePreviewBuilding();
            }
        }


    }

    public void CleanSelectedHex()
    {
        if(!selectedHex.Equals(nullHex))
        {
            Color defaultColor = selectedHex.terrainType.associatedDefaultColor;

            MeshRenderer mr = selectedHex.associatedGO.GetComponentInChildren<MeshRenderer>();
            mr.materials[1].color = ColorManager.instance.seasonColor1.GetColorFromCurrentSeason();
            mr.materials[0].color = ColorManager.instance.hexBorderColor;
        }
    }

    public void ClickOnHex(Hex hex)
    {
        //Debug.Log("Clicking on hex : " + hex.ToString());
        // Display details of building in there is one on this hex
        if(hex.HasBuilding())
        {
            //Debug.Log("Hex has a building on it, which is : " + hex.getCurrentBuilding().GetBuildingType().name);
            detailsPanel.HexWithBuildingSelected(hex);
        }
    }

    public void ClickOnVillager(Villager villager)
    {
        detailsPanel.VillagerSelected(villager);
    }


}
