using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : CharacterMovement
{
    public LayerMask characterLayer;
    public int powerPelletSpeed;

    Transform posToGo;
    RaycastHit hit;

    protected Directions playerDir;
    Transform turnPoint;
    Vector3 startPosition;
    LevelController levelController;
    Animator anim;
    PostProcessControl postProcessControl;
    [HideInInspector]
    public float powerPac;
    [HideInInspector]
    public bool stopMovement;

    int enemyCombo = 0;

    private void Start()
    {
        stopMovement = true;
        posToGo = transform;
        playerDir = Directions.Idle;
        startPosition = transform.position;
        levelController = GameObject.Find("Level Controller").GetComponent<LevelController>();
        anim = GetComponent<Animator>();
        anim.speed = 0;
        postProcessControl = GameObject.Find("PostProcessing").GetComponent<PostProcessControl>();
    }

    private void Update()
    {
        Debug.DrawRay(transform.position, transform.forward, Color.red);
        if(!stopMovement) Movement();
        RotatePlayer();
    }


    void Movement()
    {

        PowerPelletCollected();

        transform.position = Vector3.MoveTowards(transform.position, posToGo.position, speed * Time.deltaTime);
        anim.speed = 1;

        if(transform.position == posToGo.position && playerDir != Directions.Idle)
        {
            anim.speed = 0;
            ContinueForward();
        }

        if (Input.GetAxisRaw("Up") > 0.9f && Physics.Raycast(transform.position, Vector3.forward, out hit, Mathf.Infinity,characterLayer))
        {
            if (hit.transform.CompareTag("TurnPoints"))
            {
                posToGo = hit.transform;
                playerDir = Directions.Up;
                turnPoint = hit.transform;
            }
            return;
        }
        if (Input.GetAxisRaw("Right") > 0.9f && Physics.Raycast(transform.position, Vector3.right, out hit, Mathf.Infinity, characterLayer))
        {
            if (hit.transform.CompareTag("TurnPoints"))
            {
                posToGo = hit.transform;
                playerDir = Directions.Right;
                turnPoint = hit.transform;
            }
            return;
        }
        if (Input.GetAxisRaw("Down") > 0.9f && Physics.Raycast(transform.position, Vector3.back, out hit, Mathf.Infinity, characterLayer))
        {
            if (hit.transform.CompareTag("TurnPoints"))
            {
                posToGo = hit.transform;
                playerDir = Directions.Down;
                turnPoint = hit.transform;
            }
            return;
        }
        if (Input.GetAxisRaw("Left") > 0.9f && Physics.Raycast(transform.position, Vector3.left, out hit, Mathf.Infinity, characterLayer))
        {
            if (hit.transform.CompareTag("TurnPoints"))
            {
                posToGo = hit.transform;
                playerDir = Directions.Left;
                turnPoint = hit.transform;
            }
            return;
        }
    }

   public void ContinueForward()
    { 
        if(Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity, characterLayer))
        {
            if (hit.transform.CompareTag("TurnPoints"))
            {
                posToGo = hit.transform;
                turnPoint = hit.transform;
            }
        }
    }

    void RotatePlayer()
    {
        switch (playerDir)
        {
            case Directions.Up:
                transform.rotation = Quaternion.identity;
                break;
            case Directions.Right:
                transform.rotation = Quaternion.Euler(0, 90, 0);
                break;
            case Directions.Down:
                transform.rotation = Quaternion.Euler(0, 180, 0);
                break;
            case Directions.Left:
                transform.rotation = Quaternion.Euler(0, 270, 0);
                break;
        }
    }

    public void RepositionPlayer()
    {
        transform.position = startPosition;
        posToGo = transform;
    }

    private void OnTriggerEnter(Collider trigger)
    {
        if(trigger.CompareTag("Enemy"))
        {
            if(powerPac > 0f && trigger.gameObject.GetComponent<Ghost>().blue)
            {
                enemyCombo++;
                trigger.gameObject.GetComponent<Ghost>().EnemyEaten();
                levelController.AddScore(100 * enemyCombo);

            }
            else
            {
                anim.SetTrigger("PacHit");
                levelController.GetHurt();
            }
        }
    }

    public void PowerPelletCollected()
    {
        if(powerPac > 0f)
        {
            postProcessControl.chromaValue = 1f;
            speed = powerPelletSpeed;
            powerPac -= Time.deltaTime;
        }
        else
        {
            speed = 2;
            postProcessControl.chromaValue = 0f;
            enemyCombo = 0;
            levelController.PowerPacOff();
        }
    }

    public void DeathAnimeDone()
    {
        levelController.DeathAnimeOver();
    }
    public void StopAnim()
    {
        anim.speed = 0;
    }
}
