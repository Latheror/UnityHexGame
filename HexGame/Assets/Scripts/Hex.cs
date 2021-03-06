using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Hex {

    public static Hex nullHex = new Hex(-1,-1);

    // Hex coordinates
	public int x;
    public int y;

    // Associated instantiated Game Object
    public GameObject associatedGO;

    // Type of terrain
    public TerrainsManager.TerrainType terrainType;

    // Building on the hex
    private Building currentBuilding = null;

    // If the hex has a building on it
    private bool hasBuilding = false;

    // Decor GO on the hex
    public GameObject decor;

    // List of neighbour Hexes
    public List<Hex> neighboursHexes = new List<Hex>();


    // Constructors
    public Hex(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public GameObject AssociatedGO { get; set; }

    public bool HasBuilding(){ return hasBuilding; }
    public void SetHasBuilding(bool has){ Debug.Log("SetHasBuilding false to Hex: " + this.ToString());    hasBuilding = has; }
    public Building getCurrentBuilding(){ return currentBuilding; }
    public GameObject GetAssociatedGO(){ return this.associatedGO; }
    public List<Hex> GetNeighboursHexes(){ return this.neighboursHexes; }
    public void SetNeighboursHexes(List<Hex> hexList){ this.neighboursHexes = hexList; } 

    public void SetBuilding(Building b)
    {
        if(!hasBuilding)
        {
            hasBuilding = true;
            currentBuilding = b;
        }else{
             Debug.Log("This hex already has a building on it !");
        }

    }


    public bool Equals(Hex hex2)
    {
        // Debug.Log("Trying to compare Hexes (" + this.x + ";" + this.y +") and (" + hex2.x + ";" + hex2.y + ")" );
        return((this.x == hex2.x) && (this.y == hex2.y));
    }

    override
    public string ToString()
    {
        return("Hex("+this.x+":"+this.y+")");
    }

    public TerrainsManager.TerrainType GetTerrainType()
    {
        return terrainType;
    }


    public void DestroyBuilding()
    {
        if(HasBuilding())
        {
            //Destroy(currentBuilding);
        }
        else
        {
            Debug.Log("No building to destroy here !");
        }
    }

    public void ApplyDefaultColor()
    {
        //Debug.Log("Applying default color to " + this.ToString() + ", which is a " + this.terrainType.name);
        associatedGO.GetComponentInChildren<MeshRenderer>().materials[1].color = terrainType.associatedDefaultColor;
    }

    public void SetSelectedColor()
    {
        associatedGO.GetComponentInChildren<MeshRenderer>().materials[0].color = ColorManager.instance.selectedColor;
    }

    public void SetHoverColor()
    {
        associatedGO.GetComponentInChildren<MeshRenderer>().materials[0].color = ColorManager.instance.hoverColor;
    }

    public void SetCenterColor(Color c)
    {
        associatedGO.GetComponentInChildren<MeshRenderer>().materials[1].color = c;
    }



    public Hex GetLeftNeighbour()
    {
        Hex hexToReturn = nullHex;
        int x = this.x;
        int y = this.y;
        // If hex in on the left border of the map, it has no left neighbour
        if(x != 0)
        {
            foreach (var hexInList in HexManager.instance.hexList)
            {
                if((hexInList.x == (x-1)) && (hexInList.y == (y)))
                {
                    hexToReturn = hexInList;
                    break;
                }
            }
        }
        else
        {
            // Debug.Log("Hex in on the left border of the map.");
        }
        return hexToReturn;
    }


    public Hex GetTopleftNeighbour()
    {
        Hex hexToReturn = nullHex;
        int x = this.x;
        int y = this.y;
        // If hex in on the top or left border of the map, it has no topleft neighbour
        if(y != MapBuilder.instance.mapSize)
        {
            foreach (var hexInList in HexManager.instance.hexList)
            {
                // If our hex has an even Y
                if((this.y % 2) == 0)
                {
                    if((hexInList.x == (x-1)) && (hexInList.y == (y+1)))
                    {
                        hexToReturn = hexInList;
                        // Debug.Log("Topleft neighbour found at X=" + hexInList.x + ", Y=" + hexInList.y);
                        break;
                    }
                }
                else
                {
                    if((hexInList.x == (x)) && (hexInList.y == (y+1)))
                    {
                        hexToReturn = hexInList;
                        // Debug.Log("Topleft neighbour found at X=" + hexInList.x + ", Y=" + hexInList.y);
                        break;
                    }
                }

            }
        }
        else
        {
            // Debug.Log("Hex in on the left or top border of the map.");
        }

        return hexToReturn;
    }



    public Hex GetToprightNeighbour()
    {
        Hex hexToReturn = nullHex;
        int x = this.x;
        int y = this.y;
        // If hex in on the top or right border of the map, it has no topright neighbour
        if(y != MapBuilder.instance.mapSize && x != MapBuilder.instance.mapSize)
        {
            foreach (var hexInList in HexManager.instance.hexList)
            {
                // If our hex has an even Y
                if((this.y % 2) == 0)
                {
                    if((hexInList.x == (x)) && (hexInList.y == (y+1)))
                    {
                        hexToReturn = hexInList;
                        // Debug.Log("Topright neighbour found at X=" + hexInList.x + ", Y=" + hexInList.y);
                        break;
                    }
                }
                else
                {
                    if((hexInList.x == (x+1)) && (hexInList.y == (y+1)))
                    {
                        hexToReturn = hexInList;
                        // Debug.Log("Topright neighbour found at X=" + hexInList.x + ", Y=" + hexInList.y);
                        break;
                    }
                }

            }
        }
        else
        {
            // Debug.Log("Hex in on the right or top border of the map.");
        }

        return hexToReturn;
    }

    public Hex GetRightNeighbour()
    {
        Hex hexToReturn = nullHex;
        int x = this.x;
        int y = this.y;

        // Debug.Log("Looking for the right neighbour of Hex(" + x + "," + y + ")");

        // If hex in on the right border of the map, it has no right neighbour
        if(x != MapBuilder.instance.mapSize)
        {
            foreach (var hexInList in HexManager.instance.hexList)
            {
                if((hexInList.x == (x+1)) && (hexInList.y == (y)))
                {
                    hexToReturn = hexInList;
                    // Debug.Log("Right neighbour found at X=" + hexInList.x + ", Y=" + hexInList.y);
                    break;
                }
            }
        }
        else
        {
            // Debug.Log("Hex in on the right border of the map.");
        }

        return hexToReturn;
    }


    public Hex GetBottomrightNeighbour()
    {
        Hex hexToReturn = nullHex;
        int x = this.x;
        int y = this.y;

        // Debug.Log("Looking for the bottomright neighbour of Hex(" + x + "," + y + ")");

        // If hex in on the top or right border of the map, it has no topright neighbour
        if(y != 0 && (( x != MapBuilder.instance.mapSize && y%2 !=0 ) || y%2 == 0) )
        {
            foreach (var hexInList in HexManager.instance.hexList)
            {
                // If our hex has an even Y
                if((this.y % 2) == 0)
                {
                    if((hexInList.x == (x)) && (hexInList.y == (y-1)))
                    {
                        hexToReturn = hexInList;
                        // Debug.Log("Bottomright neighbour found at X=" + hexInList.x + ", Y=" + hexInList.y);
                        break;
                    }
                }
                else
                {
                    if((hexInList.x == (x+1)) && (hexInList.y == (y-1)))
                    {
                        hexToReturn = hexInList;
                        // Debug.Log("Bottomright neighbour found at X=" + hexInList.x + ", Y=" + hexInList.y);
                        break;
                    }
                }

            }
        }
        else
        {
            // Debug.Log("Hex in on the right or bottom border of the map.");
        }

        return hexToReturn;
    }


    public Hex GetBottomleftNeighbour()
    {
        Hex hexToReturn = nullHex = new Hex(-1,-1);
        int x = this.x;
        int y = this.y;

        // Debug.Log("Looking for the bottomleft neighbour of Hex(" + x + "," + y + ")");

        // If hex in on the right border of the map, it has no right neighbour
        if(y != 0 && ( (x != 0 && y%2==0 ) || (y%2 != 0) ))
        {
            foreach (var hexInList in HexManager.instance.hexList)
            {
                // If our hex has an even Y
                if((this.y % 2) == 0)
                {
                    if((hexInList.x == (x-1)) && (hexInList.y == (y-1)))
                    {
                        hexToReturn = hexInList;
                        // Debug.Log("Bottomright neighbour found at X=" + hexInList.x + ", Y=" + hexInList.y);
                        break;
                    }
                }
                else
                {
                    if((hexInList.x == (x)) && (hexInList.y == (y-1)))
                    {
                        hexToReturn = hexInList;
                        // Debug.Log("Bottomright neighbour found at X=" + hexInList.x + ", Y=" + hexInList.y);
                        break;
                    }
                }
            }
        }
        else
        {
            // Debug.Log("Hex in on the left or bottom border of the map.");
        }

        return hexToReturn;
    }

    public bool HasHexSameTerrain(Hex hexToCompare)
    {
        return (this.GetTerrainType().Equals(hexToCompare.GetTerrainType()));
    }


    public bool IsHexInList(List<Hex> hexList)
    {
        bool isInList = false;

        foreach (var hex in hexList)
        {
            if(this.Equals(hex))
            {
                isInList = true;
                break;
            }
        }

        return isInList;
    }

    public void ModifyHeight(float height)
    {
        Vector3 newPos = new Vector3(this.associatedGO.transform.position.x, height, this.associatedGO.transform.position.z);
        this.GetAssociatedGO().transform.position = newPos;
        this.GetAssociatedGO().transform.localScale = new Vector3(1, 1 + 2 * height, 1);
    }

    public GameObject GetTopPointOfHex()
    {
        return this.GetAssociatedGO().GetComponent<HexGO>().GetTopOfHex();
    }

    public Transform HexToSurfaceTransform()
    {
        return this.GetAssociatedGO().GetComponent<HexGO>().GetTopOfHex().transform;
    }
   
}
