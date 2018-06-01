using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexManager : MonoBehaviour {

    // Make sure there is only one HexManager
    public static HexManager instance;

    void Awake()
    {   
        if(instance != null)
        {
            Debug.LogError("More than one HexManager in scene !");
            return;
        }
        instance = this;
    }

    public List<Hex> hexList = new List<Hex>();

    public static Hex nullHex = new Hex(-1,-1);

    public List<Hex> GetHexList(){ return hexList; }

	void Start () { }    

    public void AddHexToList(Hex hex)
    {
        hexList.Add(hex);
    }

    public void UpdateHexesSeasonalColor(TimeManager.Season season)
    {
        if(hexList != null)
        {
            Color colorToApply = ColorManager.instance.seasonColor1.GetColorFromSeason(season);
            foreach (var hex in hexList)
            {
                hex.SetCenterColor(colorToApply);
            }
        }
    }

    public Hex SelectHex(int x, int y)
    {
        if ((x > MapBuilder.instance.mapSize - 1) || (y > MapBuilder.instance.mapSize - 1))
        {
            Debug.Log("WARNING ! Trying to select a Hex with out of limits coordinates");
            return null;
        }
        else
        {
            Hex hexToReturn = new Hex(0,0);
            foreach (var hex in hexList)
            {
                if(hex.x == x && hex.y == y)
                {
                    hexToReturn = hex;
                    break;
                }
            }     

            return hexToReturn;
        }
                              
    }

    public Hex SelectRandomHex()
    {
        int mapSize = MapBuilder.instance.mapSize;
        int randomX = Random.Range(0, mapSize);
        int randomY = Random.Range(0, mapSize);

        return SelectHex(randomX, randomY);
    }

    public Hex GetRandomAdjacentHex(Hex hex)
    {
        Hex randomAdjacentHex = new Hex(0, 0);
        List<Hex> hexNeighboursList = new List<Hex>();
        int nbNeighbours = 0;

        // If the hex has a left neighbour, add it to the list, etc
        if(! hex.GetLeftNeighbour().Equals(nullHex))
        {
            hexNeighboursList.Add(hex.GetLeftNeighbour());
        }
        if(! hex.GetTopleftNeighbour().Equals(nullHex))
        {
            hexNeighboursList.Add(hex.GetTopleftNeighbour());
        }
        if(! hex.GetToprightNeighbour().Equals(nullHex))
        {
            hexNeighboursList.Add(hex.GetToprightNeighbour());
        }
        if(! hex.GetRightNeighbour().Equals(nullHex))
        {
            hexNeighboursList.Add(hex.GetRightNeighbour());
        }
        if(! hex.GetBottomrightNeighbour().Equals(nullHex))
        {
            hexNeighboursList.Add(hex.GetBottomrightNeighbour());
        }
        if(! hex.GetBottomleftNeighbour().Equals(nullHex))
        {
            hexNeighboursList.Add(hex.GetBottomleftNeighbour());
        }

        nbNeighbours = hexNeighboursList.Count;

        randomAdjacentHex = hexNeighboursList[Random.Range(0, nbNeighbours)];

        return randomAdjacentHex;
    }


    public List<Hex> GetNeighboursList(Hex hex)
    {
        List<Hex> neighbourHexes = new List<Hex>();

        if(! (hex.GetLeftNeighbour().Equals(nullHex))){ neighbourHexes.Add(hex.GetLeftNeighbour()); }
        if(! (hex.GetTopleftNeighbour().Equals(nullHex))){ neighbourHexes.Add(hex.GetTopleftNeighbour()); }
        if(! (hex.GetToprightNeighbour().Equals(nullHex))){ neighbourHexes.Add(hex.GetToprightNeighbour()); }
        if(! (hex.GetRightNeighbour().Equals(nullHex))){ neighbourHexes.Add(hex.GetRightNeighbour()); }
        if(! (hex.GetBottomrightNeighbour().Equals(nullHex))){ neighbourHexes.Add(hex.GetBottomrightNeighbour()); }
        if(! (hex.GetBottomleftNeighbour().Equals(nullHex))){ neighbourHexes.Add(hex.GetBottomleftNeighbour()); }

        return neighbourHexes;
    }

    // Return a list of connected hexes with the same terrain type
    public List<Hex> GetGroupOfSameTerrainHexes(Hex hex)
    {
        //Debug.Log("Getting Hexes connected to " + hex.ToString() + " and having its TerrainType : " + hex.GetTerrainType().name);

        // Final list
        List<Hex> hexGroup = new List<Hex>();

        List<Hex> alreadyKnownHexes = new List<Hex>();
        alreadyKnownHexes.Add(hex);

        hexGroup = IterateThroughConnectedHexesSameTerrain(hex, alreadyKnownHexes);
        hexGroup.Add(hex);

        //Debug.Log("Hexes found to be connected and of same terrain : " + HexManager.instance.PrintListOfHexes(hexGroup));

        return hexGroup;
    }

    public List<Hex> IterateThroughConnectedHexesSameTerrain(Hex hex, List<Hex> alreadyKnownHexes)
    {
        List<Hex> hexesToReturn = new List<Hex>();
        List<Hex> neighbours = GetNeighboursList(hex);
        List<Hex> hexesToVisit = new List<Hex>();

        // Add the direct neighbours with the same terrain to the list to return, and the list to visit
        foreach (var neighbourHex in neighbours)
        {
            if(hex.HasHexSameTerrain(neighbourHex) && ! (neighbourHex.IsHexInList(alreadyKnownHexes)))
            {
                hexesToReturn.Add(neighbourHex);
                hexesToVisit.Add(neighbourHex);
            }
        }

        // Update AlreadyKnownHexes
        foreach (var hexToReturn in hexesToReturn)
        {
            if(! hexToReturn.IsHexInList(alreadyKnownHexes))
            {
                alreadyKnownHexes.Add(hexToReturn);
            }
        }

        foreach (var hexToVisit in hexesToVisit)
        {
            List<Hex> foundHexes = IterateThroughConnectedHexesSameTerrain (hexToVisit, alreadyKnownHexes) ;
            foreach (var foundHex in foundHexes)
            {
                if(! foundHex.IsHexInList(hexesToReturn))
                {
                    hexesToReturn.Add(foundHex);
                }
            }  
        }

        return hexesToReturn;
    }


    public string PrintListOfHexes(List<Hex> list)
    {
        string text = "List of hexes : ";
        foreach (var hex in list)
        {
            text += " | ";
            text += hex.ToString();
        }
        return text;
    }

    public string PrintListOfMapHexes()
    {
        return PrintListOfHexes(GetHexList());
    }


    public void AddListToList(List<Hex> source, List<Hex> destination)
    {
        foreach (var hex in source)
        {
            if(! hex.IsHexInList(destination))
            {
                destination.Add(hex);
            }
        }
    }

    public TerrainsManager.TerrainType GetTerrainTypeFromLocalGroup(List<Hex> localGroup)
    {
        return localGroup[0].GetTerrainType();
    }

    public void FlattenHexGroup(List<Hex> group, Hex reference)
    {
        Vector3 referencePos = reference.GetAssociatedGO().transform.position;
        Vector3 referenceScale = reference.GetAssociatedGO().transform.localScale;

        foreach (var hex in group)
        {
            float previousHexX = hex.GetAssociatedGO().transform.position.x;
            float previousHexZ = hex.GetAssociatedGO().transform.position.z;

            hex.GetAssociatedGO().transform.position = new Vector3(previousHexX, referencePos.y, previousHexZ);
            hex.GetAssociatedGO().transform.localScale = referenceScale;
        }
    }

    public Hex GetRandomHex()
    {
        return hexList[Random.Range(0, hexList.Count)];
    }

    public Hex GetHexFromCoordinates(int hexX, int hexY)
    {
        Hex hexToReturn = nullHex;
        foreach (var hex in hexList)
        {
            if(hex.x == hexX && hex.y == hexY)
            {
                hexToReturn = hex;
                break;
            }
        }
        return hexToReturn;
    }


}
