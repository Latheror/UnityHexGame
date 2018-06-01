using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexGO : MonoBehaviour {

	public Hex ownerHex;
    public int x;
    public int y;

    public GameObject topOfHex;

    public GameObject GetTopOfHex(){ return this.topOfHex; }
}
