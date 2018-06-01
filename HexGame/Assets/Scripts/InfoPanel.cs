using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InfoPanel : MonoBehaviour {

    // Make sure there is only one InfoPanel
    public static InfoPanel instance;

    void Awake()
    {   
        if(instance != null)
        {
            Debug.LogError("More than one InfoPanel in scene !");
            return;
        }
        instance = this;
    }

    public TextMeshProUGUI infoText;

	// Use this for initialization
	void Start () {
		
	}

    public void SetInfo(string text)
    {
        infoText.SetText(text);
    }

    public void ClearInfo()
    {
        infoText.SetText("");
    }
}
