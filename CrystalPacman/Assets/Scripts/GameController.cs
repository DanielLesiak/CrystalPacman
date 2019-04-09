using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    LevelController levelcontroller;

    int currentScore;
    int highScore;
    int lives;
    Scene currentScene;

    private static GameController instance;

    private void Awake()
    {
        if(instance != null)
        {
            return;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    public void GetController(LevelController cont)
    {
        levelcontroller = cont;
        levelcontroller.GetPrefs(currentScore, lives, highScore);
    }

    private void Start()
    {
        if(SceneManager.GetActiveScene().name == "TitleScreen")
        {
            ResetLivesScore();
        }
    }
    private void Update()
    {
        PauseMenu();
    }

    public void ResetLivesScore()
    {
        lives = 3;
        currentScore = 0;
        highScore = PlayerPrefs.GetInt("High Score");
    }
    public void SetPrefs(int _score, int _lives)
    {
        currentScore = _score;
        lives = _lives;
    }

    public void ReloadScene()
    {
        currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    public void SetHighScore(int _highScore)
    {
        highScore = _highScore;
        PlayerPrefs.SetInt("High Score", highScore);
    }

    public void GameOver()
    {
        SceneManager.LoadScene("TitleScreen");

    }

    void PauseMenu()
    { 
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (SceneManager.GetActiveScene().name == "TitleScreen")
            {
                Debug.Log("Application Quit");
                Application.Quit();
            }
            else
            {
                Time.timeScale = 0;
                SceneManager.LoadScene("Pause", LoadSceneMode.Additive);
            }
        }   
    }

}
