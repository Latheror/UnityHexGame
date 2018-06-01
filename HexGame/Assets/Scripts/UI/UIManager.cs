using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    // Make sure there is only one UIManager
    public static UIManager instance;

    void Awake()
    {   
        if(instance != null)
        {
            Debug.LogError("More than one UIManager in scene !");
            return;
        }
        instance = this;
    }


    // Popup UI spawned when ressource is produced
    public GameObject popupUI;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    
    public void PopupText(Building b, ResourcesManager.Resource r, int amount)
    {
        GameObject instantiatedPopupGO = Instantiate(popupUI, b.instantiatedGO.transform.position, Quaternion.identity);

        GameObject popupPanel = instantiatedPopupGO.GetComponent<PopupUI>().subPanel;
        GameObject popupText = instantiatedPopupGO.GetComponent<PopupUI>().popupText;
        Image popupImage = instantiatedPopupGO.GetComponent<PopupUI>().popupResourceImage;


        popupText.GetComponent<Text>().text = ("+ " + amount.ToString());
        popupImage.GetComponent<Image>().sprite = r.image;

        Animator popupAnim = popupPanel.GetComponent<Animator>();

        popupAnim.Play("PopupResource");

        Destroy(instantiatedPopupGO, popupAnim.GetCurrentAnimatorClipInfo(0)[0].clip.length);
    }



    public void DoPlusAnimation(Building b, ResourcesManager.Resource r, int amount)
    {
        // Debug.Log("Production: " + p.GetProductionResource().name);

        PopupText(b, r, amount);
    }
}
