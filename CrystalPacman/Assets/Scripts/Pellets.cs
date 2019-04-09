using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pellets : MonoBehaviour
{
    PelletsSource pelletSource;

    private void Awake()
    {
        pelletSource = GetComponentInParent<PelletsSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            pelletSource.NormalPellet();
            Destroy(this.gameObject);
        }
    }
}
