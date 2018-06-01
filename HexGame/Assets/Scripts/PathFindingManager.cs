using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PathFindingManager : MonoBehaviour {

    public static PathFindingManager instance;

    public List<Hex> pathFindingGraph = new List<Hex>();

    void Awake()
    {   
        if (instance != null){ Debug.LogError("More than one PathFindingManager in scene !"); return; } instance = this;
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

 
    public void GeneratePathFindingGraph()
    {
        // List of all Hexes on the map
        List<Hex> mapHexes = HexManager.instance.GetHexList();
        foreach (var hex in mapHexes)
        {
            hex.SetNeighboursHexes(HexManager.instance.GetNeighboursList(hex));
        }

        this.pathFindingGraph = mapHexes;
    }

    public List<Hex> GeneratePathFromHexToHex(Hex sourceHex, Hex destinationHex)
    {
        Debug.Log("GeneratePathFromHexToHex | From " + sourceHex.ToString() + " to " + destinationHex.ToString());
        List<Hex> pathHexes = new List<Hex>();
        List<Hex> graph = this.pathFindingGraph;

        Dictionary<Hex, float> distance = new Dictionary<Hex, float>();
        Dictionary<Hex, Hex> previous = new Dictionary<Hex, Hex>();

        // List of Hexes we haven't checked
        List<Hex> unvisited = new List<Hex>();

        // Initialize all node with infinity distance value
        foreach (var hex in graph)
        {
            if(hex != sourceHex)
            {
                distance[hex] = Mathf.Infinity;
                previous[hex] = null;
            }

            unvisited.Add(hex);
        }

        // The source of the path is at a distance of 0
        distance[sourceHex] = 0;
        previous[sourceHex] = null;

        Debug.Log("Unvisited Hexes are : " + HexManager.instance.PrintListOfHexes(unvisited));
        while(unvisited.Count > 0)
        {
            // Remove Hex with minimum distance
            Hex closestUnvisited = unvisited.OrderBy(n => distance[n]).First();
            Debug.Log("Removing Hex with shortest distance : " + closestUnvisited.ToString());
            unvisited.Remove(closestUnvisited);

            // 
            if(closestUnvisited.Equals(destinationHex))
            {
                pathHexes = GetShortestPath(closestUnvisited, previous);
                break;
            }

            foreach (var neighbourHex in closestUnvisited.neighboursHexes)
            {
                float alt = distance[closestUnvisited] + neighbourHex.GetTerrainType().pathFindingWalkingCost; // TODO : Take the terrain cost into account
                if(alt < distance[neighbourHex])
                {
                    distance[neighbourHex] = alt;
                    previous[neighbourHex] = closestUnvisited;
                }
            }
        }

        return pathHexes;
    }


    public List<Hex> GetShortestPath(Hex target, Dictionary<Hex,Hex> prev)
    {
        Debug.Log("GetShortestPath | From " + target.ToString() + " to " + prev.ToString());

        List<Hex> sequence = new List<Hex>();
        Hex u = target;
        int compteur = 0;
        while(prev[u] != null && compteur < 100)
        {
            Debug.Log("Loop | u : " + u.ToString());
            sequence.Insert(0, u);
            u = prev[u];
            compteur++;
        }
        sequence.Insert(0, u);

        Debug.Log("Shortest Path | " + sequence.Count() + " hexes | ");
        Debug.Log(HexManager.instance.PrintListOfHexes(sequence));
        return sequence;
    }


   
}
