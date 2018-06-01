using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingPanel : MonoBehaviour {

    public static LoadingPanel instance;

    void Awake()
    {   
        if (instance != null)
        {
            Debug.LogError("More than one LoadingPanel in scene !");
            return;
        }
        instance = this;
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void DisplayGameWindow()
    {
        Image hiddingImage = this.GetComponent<Image>();
        Color tempColor = hiddingImage.color;
        tempColor.a = 0f;
        hiddingImage.color = tempColor;

        this.gameObject.SetActive(false);
    }
}
