using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour {

    // Make sure there is only one MapManager
    public static MapManager instance;

    void Awake()
    {   
        if(instance != null)
        {
            Debug.LogError("More than one MapManager in scene !");
            return;
        }
        instance = this;
    }

    // A default hex with -1;-1 coordinates
    public Hex nullHex = new Hex(-1,-1);



	// Use this for initialization
	void Start () {
        nullHex = new Hex(-1,-1);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // OBSOLETE - TODO : Use GetLeftNeighbour(), etc
    public HexDirectNeighbourood GetHexNeighbours(Hex hex)
    {
        int hexX = hex.x;
        int hexY = hex.y;

        HexDirectNeighbourood hexNeighbourood = new HexDirectNeighbourood();
        hexNeighbourood.leftNeighbour = GameObject.Find("Hex(" + (hexX-1) + ";" + (hexY) + ")").GetComponent<Hex>();
        hexNeighbourood.rightNeighbour = GameObject.Find("Hex(" + (hexX+1) + ";" + (hexY) + ")").GetComponent<Hex>();
        if(hexY%2 == 0)
        {
            hexNeighbourood.bottomLeftNeighbour = GameObject.Find("Hex(" + (hexX-1) + ";" + (hexY-1) + ")").GetComponent<Hex>();
            hexNeighbourood.bottomRightNeighbour = GameObject.Find("Hex(" + (hexX) + ";" + (hexY-1) + ")").GetComponent<Hex>();
            hexNeighbourood.topLeftNeighbour = GameObject.Find("Hex(" + (hexX-1) + ";" + (hexY+1) + ")").GetComponent<Hex>();
            hexNeighbourood.topRightNeighbour = GameObject.Find("Hex(" + (hexX) + ";" + (hexY+1) + ")").GetComponent<Hex>();
        }
        else
        {
            hexNeighbourood.bottomLeftNeighbour = GameObject.Find("Hex(" + (hexX) + ";" + (hexY-1) + ")").GetComponent<Hex>();
            hexNeighbourood.bottomRightNeighbour = GameObject.Find("Hex(" + (hexX+1) + ";" + (hexY-1) + ")").GetComponent<Hex>();
            hexNeighbourood.topLeftNeighbour = GameObject.Find("Hex(" + (hexX) + ";" + (hexY+1) + ")").GetComponent<Hex>();
            hexNeighbourood.topRightNeighbour = GameObject.Find("Hex(" + (hexX+1) + ";" + (hexY+1) + ")").GetComponent<Hex>();
        }
           
        return hexNeighbourood;
    }


    public struct HexDirectNeighbourood 
    {
        public Hex leftNeighbour;
        public Hex rightNeighbour;
        public Hex bottomLeftNeighbour;
        public Hex bottomRightNeighbour;
        public Hex topLeftNeighbour;
        public Hex topRightNeighbour;

        public HexDirectNeighbourood(Hex l, Hex r, Hex bl, Hex br, Hex tl, Hex tr)
        {
            this.leftNeighbour = l;
            this.rightNeighbour = r;
            this.bottomLeftNeighbour = bl;
            this.bottomRightNeighbour = br;
            this.topLeftNeighbour = tl;
            this.topRightNeighbour = tr;
        }
    }

    public void PrintHexNeighbourood(Hex hex)
    {   
        HexDirectNeighbourood neighbourood = GetHexNeighbours(hex);

        //Debug.Log("HEX NEIGHBOUROOD : \n" +
        //"Left neighbour: " + neighbourood.leftNeighbour.name + "\n" + "Right neighbour: " + neighbourood.rightNeighbour.name + "\n" +
        //"Bottom Left neighbour: " + neighbourood.bottomLeftNeighbour.name + "\n" + "Bottom Right neighbour: " + neighbourood.bottomRightNeighbour.name + "\n" +
        //"Top Left neighbour: " + neighbourood.topLeftNeighbour.name + "\n" + "Top Right neighbour: " + neighbourood.topRightNeighbour.name + "\n"
        //);

    }

    public void PrintHexNeighbouroodButton()
    {
        if(BuildManager.instance.GetSelectedHex() != null)
        {
            PrintHexNeighbourood(BuildManager.instance.GetSelectedHex());
        }
        else
        {
            Debug.Log("No Hex selected !");
        }
    }


    public void HighlightHex(Hex hex)
    {
        hex.associatedGO.GetComponentInChildren<MeshRenderer>().materials[0].color = ColorManager.instance.highlightedColor;
    }

    public void HighlightCanBuildHex(Hex hex)
    {
        hex.associatedGO.GetComponentInChildren<MeshRenderer>().materials[0].color = ColorManager.instance.canBuildColor;
    }

    public void HighlightCantBuildHex(Hex hex)
    {
        hex.associatedGO.GetComponentInChildren<MeshRenderer>().materials[0].color = ColorManager.instance.cantBuildColor;
    }

    public void HighlighHexError(Hex hex)
    {
        hex.associatedGO.GetComponentInChildren<MeshRenderer>().materials[0].color = ColorManager.instance.errorColor;
    }


    public void HighlightNeighbours(Hex hex)
    {
        HexDirectNeighbourood neighbourood = GetHexNeighbours(hex);
        neighbourood.leftNeighbour.associatedGO.GetComponentInChildren<MeshRenderer>().materials[0].color = ColorManager.instance.highlightedColor;
        neighbourood.rightNeighbour.associatedGO.GetComponentInChildren<MeshRenderer>().materials[0].color = ColorManager.instance.highlightedColor;
        neighbourood.bottomLeftNeighbour.associatedGO.GetComponentInChildren<MeshRenderer>().materials[0].color = ColorManager.instance.highlightedColor;
        neighbourood.bottomRightNeighbour.associatedGO.GetComponentInChildren<MeshRenderer>().materials[0].color = ColorManager.instance.highlightedColor;
        neighbourood.topLeftNeighbour.associatedGO.GetComponentInChildren<MeshRenderer>().materials[0].color = ColorManager.instance.highlightedColor;
        neighbourood.topRightNeighbour.associatedGO.GetComponentInChildren<MeshRenderer>().materials[0].color = ColorManager.instance.highlightedColor;
    }

    public void HighlightNeighboursButton()
    {
        if(BuildManager.instance.GetSelectedHex() != null)
        {
            HighlightNeighbours(BuildManager.instance.GetSelectedHex());
        }
        else
        {
            Debug.Log("No Hex selected !");
        }
    }

    public void CleanSuroundingHexes(Hex hex)
    {
        Debug.Log("Cleaning surounding hexes of " + hex.ToString());
        if (!GetLeftNeighbour(hex).Equals(nullHex))
        {
            Debug.Log("hex " + hex + " had a left neighbour: " + GetLeftNeighbour(hex).ToString());
            GetLeftNeighbour(hex).associatedGO.GetComponentInChildren<MeshRenderer>().materials[0].color = ColorManager.instance.hexBorderColor;
        }
        if (!GetTopleftNeighbour(hex).Equals(nullHex))
        {
            GetTopleftNeighbour(hex).associatedGO.GetComponentInChildren<MeshRenderer>().materials[0].color = ColorManager.instance.hexBorderColor;
        }
        if (!GetToprightNeighbour(hex).Equals(nullHex))
        {
            GetToprightNeighbour(hex).associatedGO.GetComponentInChildren<MeshRenderer>().materials[0].color = ColorManager.instance.hexBorderColor;
        }
        if (!GetRightNeighbour(hex).Equals(nullHex))
        {
            GetRightNeighbour(hex).associatedGO.GetComponentInChildren<MeshRenderer>().materials[0].color = ColorManager.instance.hexBorderColor;
        }
        if (!GetBottomrightNeighbour(hex).Equals(nullHex))
        {
            GetBottomrightNeighbour(hex).associatedGO.GetComponentInChildren<MeshRenderer>().materials[0].color = ColorManager.instance.hexBorderColor;
        }
        if (!GetBottomleftNeighbour(hex).Equals(nullHex))
        {
            GetBottomleftNeighbour(hex).associatedGO.GetComponentInChildren<MeshRenderer>().materials[0].color = ColorManager.instance.hexBorderColor;
        }

    }



    public Hex GetLeftNeighbour(Hex hex)
    {
        Hex hexToReturn = this.nullHex;
        int x = hex.x;
        int y = hex.y;

        // Debug.Log("Looking for the left neighbour of Hex(" + x + "," + y + ")");

        // If hex in on the left border of the map, it has no left neighbour
        if(x != 0)
        {
            foreach (var hexInList in HexManager.instance.hexList)
            {
                if((hexInList.x == (x-1)) && (hexInList.y == (y)))
                {
                    hexToReturn = hexInList;
                    // Debug.Log("Left neighbour found at X=" + hexInList.x + ", Y=" + hexInList.y);
                    break;
                }
            }
        }
        else
        {
            // Debug.Log("Hex in on the left border of the map.");
        }

        // Debug.Log("GETLEFTNEIGHBOUR - Returning Hex (" + hexToReturn.x + ";" + hexToReturn.y + ")");
        return hexToReturn;
    }


    public Hex GetTopleftNeighbour(Hex hex)
    {
        Hex hexToReturn = this.nullHex;
        int x = hex.x;
        int y = hex.y;

        // Debug.Log("Looking for the topleft neighbour of Hex(" + x + "," + y + ")");

        // If hex in on the top or left border of the map, it has no topleft neighbour
        if(y != MapBuilder.instance.mapSize)
        {
            foreach (var hexInList in HexManager.instance.hexList)
            {
                // If our hex has an even Y
                if((hex.y % 2) == 0)
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



    public Hex GetToprightNeighbour(Hex hex)
    {
        Hex hexToReturn = nullHex = new Hex(-1,-1);
        int x = hex.x;
        int y = hex.y;

        // Debug.Log("Looking for the topright neighbour of Hex(" + x + "," + y + ")");

        // If hex in on the top or right border of the map, it has no topright neighbour
        if(y != MapBuilder.instance.mapSize && x != MapBuilder.instance.mapSize)
        {
            foreach (var hexInList in HexManager.instance.hexList)
            {
                // If our hex has an even Y
                if((hex.y % 2) == 0)
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

    public Hex GetRightNeighbour(Hex hex)
    {
        Hex hexToReturn = nullHex = new Hex(-1,-1);
        int x = hex.x;
        int y = hex.y;

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


    public Hex GetBottomrightNeighbour(Hex hex)
    {
        Hex hexToReturn = nullHex = new Hex(-1,-1);
        int x = hex.x;
        int y = hex.y;

        // Debug.Log("Looking for the bottomright neighbour of Hex(" + x + "," + y + ")");

        // If hex in on the top or right border of the map, it has no topright neighbour
        if(y != 0 && (( x != MapBuilder.instance.mapSize && y%2 !=0 ) || y%2 == 0) )
        {
            foreach (var hexInList in HexManager.instance.hexList)
            {
                // If our hex has an even Y
                if((hex.y % 2) == 0)
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


    public Hex GetBottomleftNeighbour(Hex hex)
    {
        Hex hexToReturn = nullHex = new Hex(-1,-1);
        int x = hex.x;
        int y = hex.y;

        // Debug.Log("Looking for the bottomleft neighbour of Hex(" + x + "," + y + ")");

        // If hex in on the right border of the map, it has no right neighbour
        if(y != 0 && ( (x != 0 && y%2==0 ) || (y%2 != 0) ))
        {
            foreach (var hexInList in HexManager.instance.hexList)
            {
                // If our hex has an even Y
                if((hex.y % 2) == 0)
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



    public string HexListToString(List<Hex> list)
    {
        string text = "HexList:";
        foreach (var hex in list)
        {
            text += (" | (" + hex.x + ";" + hex.y + ")");
        }
        return text;
    }


}
