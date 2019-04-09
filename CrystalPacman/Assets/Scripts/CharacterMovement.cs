using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float speed;

    Rigidbody rb;
    public enum Directions { Up, Down, Left, Right, Idle };
    Directions direction;

    public void Awake()
    {
        direction = Directions.Idle;
        rb = this.GetComponent<Rigidbody>();
    }

    public void MoveForward()
    {
        rb.velocity = this.transform.forward * speed;
    }
}
