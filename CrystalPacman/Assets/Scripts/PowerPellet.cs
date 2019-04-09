using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerPellet : MonoBehaviour
{
    AudioSource source;
    LevelController levelController;

    private void Start()
    {
        source = GetComponent<AudioSource>();
        levelController = GameObject.Find("Level Controller").GetComponent<LevelController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            levelController.PowerPac();
            source.Play();
            StartCoroutine(DestoyAfterStopPlaying());
        }
    }

    IEnumerator DestoyAfterStopPlaying()
    {
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
        yield return new WaitForSeconds(1f);
        Destroy(this.gameObject);

    }
}
