using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public static MainMenu instance;

    void Awake()
    {   
        if (instance != null)
        {
            Debug.LogError("More than one MainMenu in scene !");
            return;
        }
        instance = this;
    }

    public GameObject playButton;
    public GameObject optionsButton;
    public GameObject quitButton;


	public void PlayGame() 
    {
         FadeSceneManager.instance.FadeToGame();
	}

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ShowOptionMenu()
    {
        
    }

    public void LoadGameScene()
    {
        SceneManager.LoadScene(1);
    }


}
