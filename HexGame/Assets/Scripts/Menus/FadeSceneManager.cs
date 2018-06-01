using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeSceneManager : MonoBehaviour {

    public static FadeSceneManager instance;

    void Awake()
    {   
        if (instance != null)
        {
            Debug.LogError("More than one FadeSceneManager in scene !");
            return;
        }
        instance = this;
    }

    public Animator fadeSceneAnimator;

    public void FadeToGame()
    {
        fadeSceneAnimator.SetTrigger("FadeOut");
    }

    public void OnFadeComplete()
    {
        MainMenu.instance.LoadGameScene();
    }
}
