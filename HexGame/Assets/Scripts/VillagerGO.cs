using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillagerGO : MonoBehaviour {

    public Villager associatedVillager;
    public Animator villagerAnimator;

	// Use this for initialization
	void Start () {
        villagerAnimator = this.gameObject.transform.GetChild(0).gameObject.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public Villager GetAssociatedVillager(){ return this.associatedVillager; }
    public void SetAssociatedVillager(Villager villager){ this.associatedVillager = villager; }

    public void StartWalking()
    {
        villagerAnimator.SetTrigger("WalkTrigger");
    }

    public void StopWalking()
    {
        villagerAnimator.SetTrigger("StopTrigger");
    }
}
