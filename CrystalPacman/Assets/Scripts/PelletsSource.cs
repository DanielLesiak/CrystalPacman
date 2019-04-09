using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PelletsSource : MonoBehaviour
{
    AudioSource source;
    LevelController levelController;

    private void Start()
    {
        source = GetComponent<AudioSource>();
        levelController = GameObject.Find("Level Controller").GetComponent<LevelController>();
    }

    public void NormalPellet()
    {
        source.Play();
        levelController.AddScore(10);
        
    }
}
