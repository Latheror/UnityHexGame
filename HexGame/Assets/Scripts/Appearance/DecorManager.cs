using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecorManager : MonoBehaviour {

    // Make sure there is only one DecorManager
    public static DecorManager instance;

    void Awake()
    {   
        if(instance != null)
        {
            Debug.LogError("More than one DecorManager in scene !");
            return;
        }
        instance = this;
    }


    public GameObject forestGO;
    public GameObject plainGO;
    public GameObject lakeGO;
    public GameObject mountainGO;
    public GameObject connectedMountainGO;
    public GameObject connectedLakeGO;

    public GameObject connectedLake20GO;
    public GameObject connectedLake21GO;
    public GameObject connectedLake22GO;
    public GameObject connectedLake30GO;
    public GameObject connectedLake31GO;
    public GameObject connectedLake32GO;
    public GameObject connectedLake33GO;
    public GameObject connectedLake40GO;
    public GameObject connectedLake41GO;
    public GameObject connectedLake42GO;
    public GameObject connectedLake5GO;
    public GameObject connectedLake6GO;

    public GameObject connectedMountain20GO;
    public GameObject connectedMountain21GO;
    public GameObject connectedMountain22GO;
    public GameObject connectedMountain30GO;
    public GameObject connectedMountain31GO;
    public GameObject connectedMountain32GO;
    public GameObject connectedMountain33GO;
    public GameObject connectedMountain40GO;
    public GameObject connectedMountain41GO;
    public GameObject connectedMountain42GO;
    public GameObject connectedMountain5GO;
    public GameObject connectedMountain6GO;

    public Hex nullHex = new Hex(-1,-1);

    void Start()
    {
        nullHex = new Hex(-1,-1);
    }

    // Lakes shapes
    public enum Bonds {Alone, Left, Right, Topleft, Topright, Bottomleft, Bottomright,
                           // 2 bonds
                           LeftTopleft, LeftTopright, LeftRight, LeftBottomright, LeftBottomleft,
                           RightBottomright, RightBottomleft, RightTopleft, RightTopright,
                           TopleftTopright, TopleftRight, TopleftBottomright, TopleftBottomleft,
                           ToprightRight, ToprightBottomright, ToprightBottomleft,
                           BottomrightBottomleft,
                           // 3 bonds
             /* Left */    LeftTopleftTopright, LeftTopleftRight, LeftTopleftBottomright, LeftTopleftBottomleft,
                           LeftToprightRight, LeftToprightBottomright, LeftToprightBottomleft,
                           LeftRightBottomright, LeftRightBottomleft,
                           LeftBottomrightBottomleft,
          /* Topleft */    TopleftToprightRight, TopleftToprightBottomright, TopleftToprightBottomleft,
                           TopleftRightBottomright, TopleftRightBottomleft,
                           TopleftBottomrightBottomleft,
         /* Topright */    ToprightRightBottomright, ToprightRightBottomleft,
                           ToprightBottomrightBottomleft,
            /* Right */    RightBottomrightBottomleft,
                           // 4 bonds
             /* Left */    LeftTopleftToprightRight, LeftTopleftToprightBottomright, LeftTopleftToprightBottomleft,
                           LeftTopleftRightBottomright, LeftTopleftRightBottomleft,
                           LeftTopleftBottomrightBottomleft,
                           LeftToprightRightBottomright, LeftToprightRightBottomleft,
                           LeftToprightBottomrightBottomleft,
                           LeftRightBottomrightBottomleft,
          /* Topleft */    TopleftToprightRightBottomright, TopleftToprightRightBottomleft,
                           TopleftToprightBottomrightBottomleft,
                           TopleftRightBottomrightBottomleft,
         /* Topright */    ToprightRightBottomrightBottomleft,
                           // 5 bonds
                           LeftTopleftToprightRightBottomright,
                           LeftToprightRightBottomrightBottomleft,
                           LeftTopleftRightBottomrightBottomleft,
                           LeftTopleftToprightBottomrightBottomleft,
                           LeftTopleftToprightRightBottomleft,
                           TopleftToprightRightBottomrightBottomleft,
                           // 6 bonds
                           All

                           };


    public void BuildDecor(Hex hex)
    {
        // Get the decor to instantiate on the hex
        GameObject decorToInstantiate = hex.terrainType.decorGO;

        if(decorToInstantiate != null)
        {
            //Debug.Log("Instantiating hex decor for " + hex.ToString() + ". Its type is : " + hex.terrainType.name);
            // Handle water and mountains separatly
            if(! hex.terrainType.name.Equals("water") && ! hex.terrainType.name.Equals("mountain"))
            {
                GameObject instantiatedDecor = Instantiate(decorToInstantiate, hex.associatedGO.transform.position, Quaternion.Euler(0, BuildManager.instance.getRandomHexAngle(), 0));
                instantiatedDecor.transform.SetParent(DecorManager.instance.transform,false);
                hex.decor = instantiatedDecor;
            }
            else
            {
                // Hex is water
                // Debug.Log("Hex is water - Putting decor after the other terrains.");

            }

        }

    }

    public void DestroyDecor(Hex hex)
    {
        Destroy(hex.decor.gameObject);
    }



    public void ArrangeLakes()
    {
        foreach (var hexInList in HexManager.instance.hexList)
        {
            
            // If hex is a water tile
            if(hexInList.terrainType.name.Equals("water"))
            {
                //Debug.Log("ArangeLake - Hex(" + hexInList.x + "," + hexInList.y + ")");
                
                LakeGOAngle lakeGOAngle = findLakeBonds(hexInList);

                GameObject lakeGoType = lakeGOAngle.go;
                int lakeAngle = lakeGOAngle.angle;
                // Debug.Log("Lake positioning : angle=" + lakeAngle);

                GameObject instantiatedDecor = Instantiate(lakeGoType, hexInList.associatedGO.transform.position, Quaternion.Euler(0, lakeAngle, 0));
                instantiatedDecor.transform.SetParent(DecorManager.instance.transform,false);
                hexInList.decor = instantiatedDecor;



            }

        }
    }

    public void ArrangeMountains()
    {
        foreach (var hexInList in HexManager.instance.hexList)
        {
            // If hex is a water tile
            if(hexInList.terrainType.name.Equals("mountain"))
            {
                //Debug.Log("ArangeMountains - Hex(" + hexInList.x + "," + hexInList.y + ")");

                MountainGOAngle mountainGOAngle = findMountainBonds(hexInList);

                GameObject mountainGoType = mountainGOAngle.go;
                int mountainAngle = mountainGOAngle.angle;
                // Debug.Log("Lake positioning : angle=" + lakeAngle);

                GameObject instantiatedDecor = Instantiate(mountainGoType, hexInList.associatedGO.transform.position, Quaternion.Euler(0, mountainAngle, 0));
                instantiatedDecor.transform.SetParent(DecorManager.instance.transform,false);
                hexInList.decor = instantiatedDecor;



            }

        }
    }

    public bool IsLeftType(Hex hex, string type)
    {
        bool isLeftType = false;
        if(! MapManager.instance.GetLeftNeighbour(hex).Equals(nullHex))
        {
            if(MapManager.instance.GetLeftNeighbour(hex).terrainType.name.Equals(type))
            {
                isLeftType = true;
            }
            else
            {
                // Debug.Log("Left neighbour is not a lake");
            }
        }
        else
        {
            // Debug.Log("Left neighbour is null");
        }
        return isLeftType;
    }

    public bool IsTopleftType(Hex hex, string type)
    {
        bool isTopleftType = false;
        if(! MapManager.instance.GetTopleftNeighbour(hex).Equals(nullHex))
        {
            if(MapManager.instance.GetTopleftNeighbour(hex).terrainType.name.Equals(type))
            {
                isTopleftType = true;
            }
        }
        return isTopleftType;
    }

    public bool IsToprightType(Hex hex, string type)
    {
        bool isToprightType = false;
        if(! MapManager.instance.GetToprightNeighbour(hex).Equals(nullHex))
        {
            if(MapManager.instance.GetToprightNeighbour(hex).terrainType.name.Equals(type))
            {
                isToprightType = true;
            }
        }
        return isToprightType;
    }

    public bool IsRightType(Hex hex, string type)
    {
        bool isRightType = false;
        if(! MapManager.instance.GetRightNeighbour(hex).Equals(nullHex))
        {
            if(MapManager.instance.GetRightNeighbour(hex).terrainType.name.Equals(type))
            {
                isRightType = true;
            }
        }
        return isRightType;
    }

    public bool IsBottomrightType(Hex hex, string type)
    {
        bool isBottomrightType = false;
        if(! MapManager.instance.GetBottomrightNeighbour(hex).Equals(nullHex))
        {
            if(MapManager.instance.GetBottomrightNeighbour(hex).terrainType.name.Equals(type))
            {
                isBottomrightType = true;
            }
        }
        return isBottomrightType;
    }

    public bool IsBottomleftType(Hex hex, string type)
    {
        bool isBottomleftType = false;
        if(! MapManager.instance.GetBottomleftNeighbour(hex).Equals(nullHex))
        {
            if(MapManager.instance.GetBottomleftNeighbour(hex).terrainType.name.Equals(type))
            {
                isBottomleftType = true;
            }
        }
        return isBottomleftType;
    }

    // Returns the lake neigbourhood
    public LakeGOAngle findLakeBonds(Hex hex)
    {   
        string typeName = "water";
        GameObject lakeShape = lakeGO;
        int shapeAngle = 0;
        int nbLakeNeighbours = 0;

        Bonds lakeBonds = Bonds.Alone;
        // Binary code for bonds
        int lakeBondsID = 1000000;

        if(IsLeftType(hex,typeName)) { lakeBondsID += 100000;  nbLakeNeighbours += 1;}
        if(IsTopleftType(hex,typeName)) { lakeBondsID += 10000; nbLakeNeighbours += 1;}
        if(IsToprightType(hex,typeName)) { lakeBondsID += 1000; nbLakeNeighbours += 1;}
        if(IsRightType(hex,typeName)) { lakeBondsID += 100; nbLakeNeighbours += 1;}
        if(IsBottomrightType(hex,typeName)) { lakeBondsID += 10; nbLakeNeighbours += 1;}
        if(IsBottomleftType(hex,typeName)) { lakeBondsID += 1; nbLakeNeighbours += 1;}

        // Debug.Log("Water hex (" + hex.x + ";" + hex.y + ") has " + nbLakeNeighbours + " neighbours");

        // Debug.Log("Its situation is : " + lakeBondsID);

        switch(lakeBondsID)
        {
            case(1000000) :
            {
                    lakeBonds = Bonds.Alone;
                    lakeShape = lakeGO;
                    shapeAngle = BuildManager.instance.getRandomHexAngle();
                    break;
            }
            case(1000001) :
            {
                    lakeBonds = Bonds.Bottomleft;
                    lakeShape = connectedLakeGO;
                    shapeAngle = 300;
                    break;
            }
            case(1000010) :
            {
                    lakeBonds = Bonds.Bottomright;
                    lakeShape = connectedLakeGO;
                    shapeAngle = 240;
                    break;
            }
            case(1000011) :
            {
                    lakeBonds = Bonds.BottomrightBottomleft;
                    lakeShape = connectedLake20GO;
                    shapeAngle = 240;
                    break;
            }
            case(1000100) :
            {
                    lakeBonds = Bonds.Right;
                    lakeShape = connectedLakeGO;
                    shapeAngle = 180;
                    break;
            }
            case(1000101) :
            {
                    lakeBonds = Bonds.RightBottomleft;
                    lakeShape = connectedLake21GO;
                    shapeAngle = 180;
                    break;
            }
            case(1000110) :
            {
                    lakeBonds = Bonds.RightBottomright;
                    lakeShape = connectedLake20GO;
                    shapeAngle = 180;
                    break;
            }
            case(1000111) :
            {
                    lakeBonds = Bonds.RightBottomrightBottomleft;
                    lakeShape = connectedLake30GO;
                    shapeAngle = 180;
                    break;
            }
            case(1001000) :
            {
                    lakeBonds = Bonds.Topright;
                    lakeShape = connectedLakeGO;
                    shapeAngle = 120;
                    break;
            }
            case(1001001) :
            {
                    lakeBonds = Bonds.ToprightBottomleft;
                    lakeShape = connectedLake22GO;
                    shapeAngle = 120;
                    break;
            }
            case(1001010) :
            {
                    lakeBonds = Bonds.ToprightBottomright;
                    lakeShape = connectedLake21GO;
                    shapeAngle = 120;
                    break;
            }
            case(1001011) :
            {
                    lakeBonds = Bonds.ToprightBottomrightBottomleft;
                    lakeShape = connectedLake32GO;
                    shapeAngle = 240;
                    break;
            }
            case(1001100) :
            {
                    lakeBonds = Bonds.ToprightRight;
                    lakeShape = connectedLake20GO;
                    shapeAngle = 120;
                    break;
            }
            case(1001101) :
            {
                    lakeBonds = Bonds.ToprightRightBottomleft;
                    lakeShape = connectedLake31GO;
                    shapeAngle = 120;
                    break;
            }
            case(1001110) :
            {
                    lakeBonds = Bonds.ToprightRightBottomright;
                    lakeShape = connectedLake30GO;
                    shapeAngle = 120;
                    break;
            }
            case(1001111) :
            {
                    lakeBonds = Bonds.ToprightRightBottomrightBottomleft;
                    lakeShape = connectedLake40GO;
                    shapeAngle = 120;
                    break;
            }
            case(1010000) :
            {
                    lakeBonds = Bonds.Topleft;
                    lakeShape = connectedLakeGO;
                    shapeAngle = 60;
                    break;
            }
            case(1010001) :
            {
                    lakeBonds = Bonds.TopleftBottomleft;
                    lakeShape = connectedLake21GO;
                    shapeAngle = 300;
                    break;
            }
            case(1010010) :
            {
                    lakeBonds = Bonds.TopleftBottomright;
                    lakeShape = connectedLake22GO;
                    shapeAngle = 60;
                    break;
            }
            case(1010011) :
            {
                    lakeBonds = Bonds.TopleftBottomrightBottomleft;
                    lakeShape = connectedLake31GO;
                    shapeAngle = 240;
                    break;
            }
            case(1010100) :
            {
                    lakeBonds = Bonds.TopleftRight;
                    lakeShape = connectedLake21GO;
                    shapeAngle = 60;
                    break;
            }
            case(1010101) :
            {
                    lakeBonds = Bonds.TopleftRightBottomleft;
                    lakeShape = connectedLake33GO;
                    shapeAngle = 60;
                    break;
            }
            case(1010110) :
            {
                    lakeBonds = Bonds.TopleftRightBottomright;
                    lakeShape = connectedLake32GO;
                    shapeAngle = 180;
                    break;
            }
            case(1010111) :
            {
                    lakeBonds = Bonds.TopleftRightBottomrightBottomleft;
                    lakeShape = connectedLake42GO;
                    shapeAngle = 180;
                    break;
            }
            case(1011000) :
            {
                    lakeBonds = Bonds.TopleftTopright;
                    lakeShape = connectedLake20GO;
                    shapeAngle = 60;
                    break;
            }
            case(1011001) :
            {
                    lakeBonds = Bonds.TopleftToprightBottomleft;
                    lakeShape = connectedLake32GO;
                    shapeAngle = 60;
                    break;
            }
            case(1011010) :
            {
                    lakeBonds = Bonds.TopleftToprightBottomright;
                    lakeShape = connectedLake31GO;
                    shapeAngle = 60;
                    break;
            }
            case(1011011) :
            {
                    lakeBonds = Bonds.TopleftToprightBottomrightBottomleft;
                    lakeShape = connectedLake41GO;
                    shapeAngle = 60;
                    break;
            }
            case(1011100) :
            {
                    lakeBonds = Bonds.TopleftToprightRight;
                    lakeShape = connectedLake30GO;
                    shapeAngle = 60;
                    break;
            }
            case(1011101) :
            {
                    lakeBonds = Bonds.TopleftToprightRightBottomleft;
                    lakeShape = connectedLake42GO;
                    shapeAngle = 60;
                    break;
            }
            case(1011110) :
            {
                    lakeBonds = Bonds.TopleftToprightRightBottomright;
                    lakeShape = connectedLake40GO;
                    shapeAngle = 60;
                    break;
            }
            case(1011111) :
            {
                    lakeBonds = Bonds.TopleftToprightRightBottomrightBottomleft;
                    lakeShape = connectedLake5GO;
                    shapeAngle = 0;
                    break;
            }
            case(1100000) :
            {
                    lakeBonds = Bonds.Left;
                    lakeShape = connectedLakeGO;
                    shapeAngle = 0;
                    break;
            }
            case(1100001) :
            {
                    lakeBonds = Bonds.LeftBottomleft;
                    lakeShape = connectedLake20GO;
                    shapeAngle = 300;
                    break;
            }
            case(1100010) :
            {
                    lakeBonds = Bonds.LeftBottomright;
                    lakeShape = connectedLake21GO;
                    shapeAngle = 240;
                    break;
            }
            case(1100011) :
            {
                    lakeBonds = Bonds.LeftBottomrightBottomleft;
                    lakeShape = connectedLake30GO;
                    shapeAngle = 240;
                    break;
            }
            case(1100100) :
            {
                    lakeBonds = Bonds.LeftRight;
                    lakeShape = connectedLake22GO;
                    shapeAngle = 0;
                    break;
            }
            case(1100101) :
            {
                    lakeBonds = Bonds.LeftRightBottomleft;
                    lakeShape = connectedLake32GO;
                    shapeAngle = 300;
                    break;
            }
            case(1100110) :
            {
                    lakeBonds = Bonds.LeftRightBottomright;
                    lakeShape = connectedLake31GO;
                    shapeAngle = 180;
                    break;
            }
            case(1100111) :
            {
                    lakeBonds = Bonds.LeftRightBottomrightBottomleft;
                    lakeShape = connectedLake40GO;
                    shapeAngle = 180;
                    break;
            }
            case(1101000) :
            {
                    lakeBonds = Bonds.LeftTopright;
                    lakeShape = connectedLake21GO;
                    shapeAngle = 0;
                    break;
            }
            case(1101001) :
            {
                    lakeBonds = Bonds.LeftToprightBottomleft;
                    lakeShape = connectedLake31GO;
                    shapeAngle = 300;
                    break;
            }
            case(1101010) :
            {
                    lakeBonds = Bonds.LeftToprightBottomright;
                    lakeShape = connectedLake33GO;
                    shapeAngle = 0;
                    break;
            }
            case(1101011) :
            {
                    lakeBonds = Bonds.LeftToprightBottomrightBottomleft;
                    lakeShape = connectedLake42GO;
                    shapeAngle = 240;
                    break;
            }
            case(1101100) :
            {
                    lakeBonds = Bonds.LeftToprightRight;
                    lakeShape = connectedLake32GO;
                    shapeAngle = 120;
                    break;
            }
            case(1101101) :
            {
                    lakeBonds = Bonds.LeftToprightRightBottomleft;
                    lakeShape = connectedLake41GO;
                    shapeAngle = 120;
                    break;
            }
            case(1101110) :
            {
                    lakeBonds = Bonds.LeftToprightRightBottomright;
                    lakeShape = connectedLake42GO;
                    shapeAngle = 120;
                    break;
            }
            case(1101111) :
            {
                    lakeBonds = Bonds.LeftToprightRightBottomrightBottomleft;
                    lakeShape = connectedLake5GO;
                    shapeAngle = 60;
                    break;
            }
            case(1110000) :
            {
                    lakeBonds = Bonds.LeftTopleft;
                    lakeShape = connectedLake20GO;
                    shapeAngle = 0;
                    break;
            }
            case(1110001) :
            {
                    lakeBonds = Bonds.LeftTopleftBottomleft;
                    lakeShape = connectedLake30GO;
                    shapeAngle = 300;
                    break;
            }
            case(1110010) :
            {
                    lakeBonds = Bonds.LeftTopleftBottomright;
                    lakeShape = connectedLake32GO;
                    shapeAngle = 0;
                    break;
            }
            case(1110011) :
            {
                    lakeBonds = Bonds.LeftTopleftBottomrightBottomleft;
                    lakeShape = connectedLake40GO;
                    shapeAngle = 240;
                    break;
            }
            case(1110100) :
            {
                    lakeBonds = Bonds.LeftTopleftRight;
                    lakeShape = connectedLake31GO;
                    shapeAngle = 0;
                    break;
            }
            case(1110101) :
            {
                    lakeBonds = Bonds.LeftTopleftRightBottomleft;
                    lakeShape = connectedLake42GO;
                    shapeAngle = 300;
                    break;
            }
            case(1110110) :
            {
                    lakeBonds = Bonds.LeftTopleftRightBottomright;
                    lakeShape = connectedLake41GO;
                    shapeAngle = 0;
                    break;
            }
            case(1110111) :
            {
                    lakeBonds = Bonds.LeftTopleftRightBottomrightBottomleft;
                    lakeShape = connectedLake5GO;
                    shapeAngle = 120;
                    break;
            }
            case(1111000) :
            {
                    lakeBonds = Bonds.LeftTopleftTopright;
                    lakeShape = connectedLake30GO;
                    shapeAngle = 0;
                    break;
            }
            case(1111001) :
            {
                    lakeBonds = Bonds.LeftTopleftToprightBottomleft;
                    lakeShape = connectedLake40GO;
                    shapeAngle = 300;
                    break;
            }
            case(1111010) :
            {
                    lakeBonds = Bonds.LeftTopleftToprightBottomright;
                    lakeShape = connectedLake42GO;
                    shapeAngle = 0;
                    break;
            }
            case(1111011) :
            {
                    lakeBonds = Bonds.LeftTopleftToprightBottomrightBottomleft;
                    lakeShape = connectedLake5GO;
                    shapeAngle = 180;
                    break;
            }
            case(1111100) :
            {
                    lakeBonds = Bonds.LeftTopleftToprightRight;
                    lakeShape = connectedLake40GO;
                    shapeAngle = 0;
                    break;
            }
            case(1111101) :
            {
                    lakeBonds = Bonds.LeftTopleftToprightRightBottomleft;
                    lakeShape = connectedLake5GO;
                    shapeAngle = 240;
                    break;
            }
            case(1111110) :
            {
                    lakeBonds = Bonds.LeftTopleftToprightRightBottomright;
                    lakeShape = connectedLake5GO;
                    shapeAngle = 300;
                    break;
            }
            case(1111111) :
            {
                    lakeBonds = Bonds.All;
                    lakeShape = connectedLake6GO;
                    shapeAngle = 0;
                    break;
            }



        }


        return new LakeGOAngle(lakeShape, shapeAngle);
    }


    // Returns the lake neigbourhood
    public MountainGOAngle findMountainBonds(Hex hex)
    {   
        string typeName = "mountain";
        GameObject mountainShape = mountainGO;
        int shapeAngle = 0;
        int nbMountainNeighbours = 0;

        Bonds mountainBonds = Bonds.Alone;
        // Binary code for bonds
        int mountainBondsID = 1000000;

        if(IsLeftType(hex,typeName)) { mountainBondsID += 100000;  nbMountainNeighbours += 1;}
        if(IsTopleftType(hex,typeName)) { mountainBondsID += 10000; nbMountainNeighbours += 1;}
        if(IsToprightType(hex,typeName)) { mountainBondsID += 1000; nbMountainNeighbours += 1;}
        if(IsRightType(hex,typeName)) { mountainBondsID += 100; nbMountainNeighbours += 1;}
        if(IsBottomrightType(hex,typeName)) { mountainBondsID += 10; nbMountainNeighbours += 1;}
        if(IsBottomleftType(hex,typeName)) { mountainBondsID += 1; nbMountainNeighbours += 1;}

        //Debug.Log("Mountain hex (" + hex.x + ";" + hex.y + ") has " + nbMountainNeighbours + " neighbours");
        //Debug.Log("Its situation is : " + mountainBondsID);

        switch(mountainBondsID)
        {
            case(1000000) :
            {
                    mountainBonds = Bonds.Alone;
                    mountainShape = mountainGO;
                    shapeAngle = BuildManager.instance.getRandomHexAngle();
                    break;
            }
            case(1000001) :
            {
                    mountainBonds = Bonds.Bottomleft;
                    mountainShape = connectedMountainGO;
                    shapeAngle = 300;
                    break;
            }
            case(1000010) :
            {
                    mountainBonds = Bonds.Bottomright;
                    mountainShape = connectedMountainGO;
                    shapeAngle = 240;
                    break;
            }
            case(1000011) :
            {
                    mountainBonds = Bonds.BottomrightBottomleft;
                    mountainShape = connectedMountain20GO;
                    shapeAngle = 240;
                    break;
            }
            case(1000100) :
            {
                    mountainBonds = Bonds.Right;
                    mountainShape = connectedMountainGO;
                    shapeAngle = 180;
                    break;
            }
            case(1000101) :
            {
                    mountainBonds = Bonds.RightBottomleft;
                    mountainShape = connectedMountain21GO;
                    shapeAngle = 180;
                    break;
            }
            case(1000110) :
            {
                    mountainBonds = Bonds.RightBottomright;
                    mountainShape = connectedMountain20GO;
                    shapeAngle = 180;
                    break;
            }
            case(1000111) :
            {
                    mountainBonds = Bonds.RightBottomrightBottomleft;
                    mountainShape = connectedMountain30GO;
                    shapeAngle = 180;
                    break;
            }
            case(1001000) :
            {
                    mountainBonds = Bonds.Topright;
                    mountainShape = connectedMountainGO;
                    shapeAngle = 120;
                    break;
            }
            case(1001001) :
            {
                    mountainBonds = Bonds.ToprightBottomleft;
                    mountainShape = connectedMountain22GO;
                    shapeAngle = 120;
                    break;
            }
            case(1001010) :
            {
                    mountainBonds = Bonds.ToprightBottomright;
                    mountainShape = connectedMountain21GO;
                    shapeAngle = 120;
                    break;
            }
            case(1001011) :
            {
                    mountainBonds = Bonds.ToprightBottomrightBottomleft;
                    mountainShape = connectedMountain32GO;
                    shapeAngle = 240;
                    break;
            }
            case(1001100) :
            {
                    mountainBonds = Bonds.ToprightRight;
                    mountainShape = connectedMountain20GO;
                    shapeAngle = 120;
                    break;
            }
            case(1001101) :
            {
                    mountainBonds = Bonds.ToprightRightBottomleft;
                    mountainShape = connectedMountain31GO;
                    shapeAngle = 120;
                    break;
            }
            case(1001110) :
            {
                    mountainBonds = Bonds.ToprightRightBottomright;
                    mountainShape = connectedMountain30GO;
                    shapeAngle = 120;
                    break;
            }
            case(1001111) :
            {
                    mountainBonds = Bonds.ToprightRightBottomrightBottomleft;
                    mountainShape = connectedMountain40GO;
                    shapeAngle = 120;
                    break;
            }
            case(1010000) :
            {
                    mountainBonds = Bonds.Topleft;
                    mountainShape = connectedMountainGO;
                    shapeAngle = 60;
                    break;
            }
            case(1010001) :
            {
                    mountainBonds = Bonds.TopleftBottomleft;
                    mountainShape = connectedMountain21GO;
                    shapeAngle = 300;
                    break;
            }
            case(1010010) :
            {
                    mountainBonds = Bonds.TopleftBottomright;
                    mountainShape = connectedMountain22GO;
                    shapeAngle = 60;
                    break;
            }
            case(1010011) :
            {
                    mountainBonds = Bonds.TopleftBottomrightBottomleft;
                    mountainShape = connectedMountain31GO;
                    shapeAngle = 240;
                    break;
            }
            case(1010100) :
            {
                    mountainBonds = Bonds.TopleftRight;
                    mountainShape = connectedMountain21GO;
                    shapeAngle = 60;
                    break;
            }
            case(1010101) :
            {
                    mountainBonds = Bonds.TopleftRightBottomleft;
                    mountainShape = connectedMountain33GO;
                    shapeAngle = 60;
                    break;
            }
            case(1010110) :
            {
                    mountainBonds = Bonds.TopleftRightBottomright;
                    mountainShape = connectedMountain32GO;
                    shapeAngle = 180;
                    break;
            }
            case(1010111) :
            {
                    mountainBonds = Bonds.TopleftRightBottomrightBottomleft;
                    mountainShape = connectedMountain42GO;
                    shapeAngle = 180;
                    break;
            }
            case(1011000) :
            {
                    mountainBonds = Bonds.TopleftTopright;
                    mountainShape = connectedMountain20GO;
                    shapeAngle = 60;
                    break;
            }
            case(1011001) :
            {
                    mountainBonds = Bonds.TopleftToprightBottomleft;
                    mountainShape = connectedMountain32GO;
                    shapeAngle = 60;
                    break;
            }
            case(1011010) :
            {
                    mountainBonds = Bonds.TopleftToprightBottomright;
                    mountainShape = connectedMountain31GO;
                    shapeAngle = 60;
                    break;
            }
            case(1011011) :
            {
                    mountainBonds = Bonds.TopleftToprightBottomrightBottomleft;
                    mountainShape = connectedMountain41GO;
                    shapeAngle = 60;
                    break;
            }
            case(1011100) :
            {
                    mountainBonds = Bonds.TopleftToprightRight;
                    mountainShape = connectedMountain30GO;
                    shapeAngle = 60;
                    break;
            }
            case(1011101) :
            {
                    mountainBonds = Bonds.TopleftToprightRightBottomleft;
                    mountainShape = connectedMountain42GO;
                    shapeAngle = 60;
                    break;
            }
            case(1011110) :
            {
                    mountainBonds = Bonds.TopleftToprightRightBottomright;
                    mountainShape = connectedMountain40GO;
                    shapeAngle = 60;
                    break;
            }
            case(1011111) :
            {
                    mountainBonds = Bonds.TopleftToprightRightBottomrightBottomleft;
                    mountainShape = connectedMountain5GO;
                    shapeAngle = 0;
                    break;
            }
            case(1100000) :
            {
                    mountainBonds = Bonds.Left;
                    mountainShape = connectedMountainGO;
                    shapeAngle = 0;
                    break;
            }
            case(1100001) :
            {
                    mountainBonds = Bonds.LeftBottomleft;
                    mountainShape = connectedMountain20GO;
                    shapeAngle = 300;
                    break;
            }
            case(1100010) :
            {
                    mountainBonds = Bonds.LeftBottomright;
                    mountainShape = connectedMountain21GO;
                    shapeAngle = 240;
                    break;
            }
            case(1100011) :
            {
                    mountainBonds = Bonds.LeftBottomrightBottomleft;
                    mountainShape = connectedMountain30GO;
                    shapeAngle = 240;
                    break;
            }
            case(1100100) :
            {
                    mountainBonds = Bonds.LeftRight;
                    mountainShape = connectedMountain22GO;
                    shapeAngle = 0;
                    break;
            }
            case(1100101) :
            {
                    mountainBonds = Bonds.LeftRightBottomleft;
                    mountainShape = connectedMountain32GO;
                    shapeAngle = 300;
                    break;
            }
            case(1100110) :
            {
                    mountainBonds = Bonds.LeftRightBottomright;
                    mountainShape = connectedMountain31GO;
                    shapeAngle = 180;
                    break;
            }
            case(1100111) :
            {
                    mountainBonds = Bonds.LeftRightBottomrightBottomleft;
                    mountainShape = connectedMountain40GO;
                    shapeAngle = 180;
                    break;
            }
            case(1101000) :
            {
                    mountainBonds = Bonds.LeftTopright;
                    mountainShape = connectedMountain21GO;
                    shapeAngle = 0;
                    break;
            }
            case(1101001) :
            {
                    mountainBonds = Bonds.LeftToprightBottomleft;
                    mountainShape = connectedMountain31GO;
                    shapeAngle = 300;
                    break;
            }
            case(1101010) :
            {
                    mountainBonds = Bonds.LeftToprightBottomright;
                    mountainShape = connectedMountain33GO;
                    shapeAngle = 0;
                    break;
            }
            case(1101011) :
            {
                    mountainBonds = Bonds.LeftToprightBottomrightBottomleft;
                    mountainShape = connectedMountain42GO;
                    shapeAngle = 240;
                    break;
            }
            case(1101100) :
            {
                    mountainBonds = Bonds.LeftToprightRight;
                    mountainShape = connectedMountain32GO;
                    shapeAngle = 120;
                    break;
            }
            case(1101101) :
            {
                    mountainBonds = Bonds.LeftToprightRightBottomleft;
                    mountainShape = connectedMountain41GO;
                    shapeAngle = 120;
                    break;
            }
            case(1101110) :
            {
                    mountainBonds = Bonds.LeftToprightRightBottomright;
                    mountainShape = connectedMountain42GO;
                    shapeAngle = 120;
                    break;
            }
            case(1101111) :
            {
                    mountainBonds = Bonds.LeftToprightRightBottomrightBottomleft;
                    mountainShape = connectedMountain5GO;
                    shapeAngle = 60;
                    break;
            }
            case(1110000) :
            {
                    mountainBonds = Bonds.LeftTopleft;
                    mountainShape = connectedMountain20GO;
                    shapeAngle = 0;
                    break;
            }
            case(1110001) :
            {
                    mountainBonds = Bonds.LeftTopleftBottomleft;
                    mountainShape = connectedMountain30GO;
                    shapeAngle = 300;
                    break;
            }
            case(1110010) :
            {
                    mountainBonds = Bonds.LeftTopleftBottomright;
                    mountainShape = connectedMountain32GO;
                    shapeAngle = 0;
                    break;
            }
            case(1110011) :
            {
                    mountainBonds = Bonds.LeftTopleftBottomrightBottomleft;
                    mountainShape = connectedMountain40GO;
                    shapeAngle = 240;
                    break;
            }
            case(1110100) :
            {
                    mountainBonds = Bonds.LeftTopleftRight;
                    mountainShape = connectedMountain31GO;
                    shapeAngle = 0;
                    break;
            }
            case(1110101) :
            {
                    mountainBonds = Bonds.LeftTopleftRightBottomleft;
                    mountainShape = connectedMountain42GO;
                    shapeAngle = 300;
                    break;
            }
            case(1110110) :
            {
                    mountainBonds = Bonds.LeftTopleftRightBottomright;
                    mountainShape = connectedMountain41GO;
                    shapeAngle = 0;
                    break;
            }
            case(1110111) :
            {
                    mountainBonds = Bonds.LeftTopleftRightBottomrightBottomleft;
                    mountainShape = connectedMountain5GO;
                    shapeAngle = 120;
                    break;
            }
            case(1111000) :
            {
                    mountainBonds = Bonds.LeftTopleftTopright;
                    mountainShape = connectedMountain30GO;
                    shapeAngle = 0;
                    break;
            }
            case(1111001) :
            {
                    mountainBonds = Bonds.LeftTopleftToprightBottomleft;
                    mountainShape = connectedMountain40GO;
                    shapeAngle = 300;
                    break;
            }
            case(1111010) :
            {
                    mountainBonds = Bonds.LeftTopleftToprightBottomright;
                    mountainShape = connectedMountain42GO;
                    shapeAngle = 0;
                    break;
            }
            case(1111011) :
            {
                    mountainBonds = Bonds.LeftTopleftToprightBottomrightBottomleft;
                    mountainShape = connectedMountain5GO;
                    shapeAngle = 180;
                    break;
            }
            case(1111100) :
            {
                    mountainBonds = Bonds.LeftTopleftToprightRight;
                    mountainShape = connectedMountain40GO;
                    shapeAngle = 0;
                    break;
            }
            case(1111101) :
            {
                    mountainBonds = Bonds.LeftTopleftToprightRightBottomleft;
                    mountainShape = connectedMountain5GO;
                    shapeAngle = 240;
                    break;
            }
            case(1111110) :
            {
                    mountainBonds = Bonds.LeftTopleftToprightRightBottomright;
                    mountainShape = connectedMountain5GO;
                    shapeAngle = 300;
                    break;
            }
            case(1111111) :
            {
                    mountainBonds = Bonds.All;
                    mountainShape = connectedMountain6GO;
                    shapeAngle = 0;
                    break;
            }
        }

        return new MountainGOAngle(mountainShape, shapeAngle);
    }


    public class LakeGOAngle
    {
        public GameObject go;
        public int angle;

        public LakeGOAngle(GameObject go, int angle)
        {
            this.go = go;
            this.angle = angle;
        }
    }

    public class MountainGOAngle
    {
        public GameObject go;
        public int angle;

        public MountainGOAngle(GameObject go, int angle)
        {
            this.go = go;
            this.angle = angle;
        }
    }


}
