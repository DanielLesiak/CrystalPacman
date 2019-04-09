using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnTrigger : MonoBehaviour
{
    public enum Directions { Up, Down, Left, Right, Idle };

    [HideInInspector]
    public Transform up, left, down, right;


    //Function used for testing an older movement system.
    //Not used currently.
    //Script left in for potential future uses
    void CollectRefForTurns()
    {
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
        {
            if(hit.transform.CompareTag("TurnPoints"))
            {
                up = hit.transform;
            }
        }
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.back), out hit, Mathf.Infinity))
        {
            if (hit.transform.CompareTag("TurnPoints"))
            {
                down = hit.transform;
            }
        }
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.right), out hit, Mathf.Infinity))
        {
            if (hit.transform.CompareTag("TurnPoints"))
            {
                right = hit.transform;
            }
        }
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.left), out hit, Mathf.Infinity))
        {
            if (hit.transform.CompareTag("TurnPoints"))
            {
                left = hit.transform;
            }
        }
    }
}
