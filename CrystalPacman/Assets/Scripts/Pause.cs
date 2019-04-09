using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    void Update()
    { 
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 1;
            SceneManager.LoadScene("TitleScreen");
        }
        else if(Input.anyKeyDown)
        {
            Time.timeScale = 1;
            SceneManager.UnloadSceneAsync("Pause");
        }
    }
}
