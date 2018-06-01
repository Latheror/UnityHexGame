using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour {


    // Make sure there is only one GameManager
    public static GameManager instance;

    void Awake()
    {   
        if(instance != null)
        {
            Debug.LogError("More than one GameManager in scene !");
            return;
        }
        instance = this;
    }

    public enum GameState { Play, Pause};
    public enum GameSpeed { NormalSpeed, FastSpeed, VeryFastSpeed};

    public GameState gameState;
    public GameSpeed gameSpeed;

    public GameObject menuButton;
    public GameObject pauseButton;
    public GameObject normalSpeedButton;
    public GameObject fastSpeedButton;

    // Canvas displayed when game paused
    public GameObject pauseCanvas;

    public Color defaultPauseButtonColor = Color.white;
    public Color pausedButtonColor = Color.red;


	// Use this for initialization
	void Start () {
        gameState = GameState.Play;
        gameSpeed = GameSpeed.NormalSpeed;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    

    public void PauseGame()
    {
        gameState = GameState.Pause;
        Debug.Log("Game Paused.");
        // Set the pause button background red
        pauseButton.GetComponent<Image>().color = pausedButtonColor;
    }

    public void ResumeGame()
    {
        gameState = GameState.Play;
        Debug.Log("Game Resumed.");
        // Set the pause button background back to white
        pauseButton.GetComponent<Image>().color = defaultPauseButtonColor;
    }

    public void SetNormalSpeed()
    {
        gameSpeed = GameSpeed.NormalSpeed;
        Debug.Log("Normal Speed.");

    }

    public void SetFastSpeed()
    {
        gameSpeed = GameSpeed.FastSpeed;
        Debug.Log("Fast Speed.");
    }

    public void IncreaseSpeed()
    {
        if(gameSpeed == GameSpeed.NormalSpeed)
        {
            gameSpeed = GameSpeed.FastSpeed;
        }
        else if(gameSpeed == GameSpeed.FastSpeed)
        {
            gameSpeed = GameSpeed.VeryFastSpeed;
        }
    }

    public void DecreaseSpeed()
    {
        if(gameSpeed == GameSpeed.FastSpeed)
        {
            gameSpeed = GameSpeed.NormalSpeed;
        }
        else if(gameSpeed == GameSpeed.VeryFastSpeed)
        {
            gameSpeed = GameSpeed.FastSpeed;
        }
    }


    public void PauseActions()
    {   
        // Playing state
        if(gameState == GameState.Play)
        {
            PauseGame();
        }
        // Pause state
        else if(gameState == GameState.Pause)
        {
            ResumeGame();
        }
    }

    public void MenuActions()
    {
        pauseCanvas.SetActive(true);
        PauseActions();

    }

    public void ResumeActions()
    {
        pauseCanvas.SetActive(false);
        PauseActions();
    }

    public void GoToMainMenuActions()
    {
        SceneManager.LoadScene(0);
    }
}
