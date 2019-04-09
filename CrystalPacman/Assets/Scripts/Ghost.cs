using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Ghost : CharacterMovement
{
    public LayerMask characterLayer;
    public Transform firstPos;
    public Transform ghostBase;
    public Renderer materialObject;
    public Material normalMaterial, scaredMaterial, eatenMaterial;
    public Transform triggers;

    List<Transform> possibleTurns = new List<Transform>();
    RaycastHit hit;
    Transform posToGo;
    Vector3 startPos;
    PlayerController playerController;
    float timerEaten;
    AIDestinationSetter destination;
    AIPath aIPath;
    AudioSource source;

    [HideInInspector]
    public bool released, exiting;
    [HideInInspector]
    public bool hasBeenEaten;
    [HideInInspector]
    public bool blue;

    public enum EnemyTypes { GhostA, GhostB, GhostC, GhostD };
    public EnemyTypes enemyType;



    private void Start()
    {
        exiting = false;
        hasBeenEaten = false;
        startPos = transform.position;
        posToGo = null;
        released = false;
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        destination = GetComponent<AIDestinationSetter>();
        aIPath = GetComponent<AIPath>();
        source = GetComponent<AudioSource>();
        if (aIPath != null)
        {
            aIPath.enabled = false;
        }
    }
    private void Update()
    {
        switch (enemyType)
        {
            case EnemyTypes.GhostA:
                GhostA();
                break;
            case EnemyTypes.GhostB:
                GhostB();
                break;
            case EnemyTypes.GhostC:
                GhostC();
                break;
            case EnemyTypes.GhostD:
                GhostD();
                break;
        }

        LookAtPlayer();
        CountdownToRevive();
    }

    public void Avoid()
    {
        if (posToGo != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, posToGo.position, speed * Time.deltaTime);
            if (transform.position == posToGo.position)
            {
                FindTurnsEscapePlayer();
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, startPos, speed * 2 * Time.deltaTime);
        }
    }

    public void Scatter()
    {
        if (posToGo != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, posToGo.position, speed * Time.deltaTime);
            if (transform.position == posToGo.position)
            {
                FindTurns();
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, startPos, speed * 2 * Time.deltaTime);
        }
    }

    void MakeTurn()
    {
        posToGo = possibleTurns[Random.Range(0, possibleTurns.Count)];
    }
    void FindTurns()
    {
        possibleTurns.Clear();
        if (Physics.Raycast(transform.position, Vector3.forward, out hit, Mathf.Infinity, characterLayer))
        {
            if (hit.transform.CompareTag("TurnPoints"))
            {
                possibleTurns.Add(hit.transform);
            }
        }
        if (Physics.Raycast(transform.position, Vector3.right, out hit, Mathf.Infinity, characterLayer))
        {
            if (hit.transform.CompareTag("TurnPoints"))
            {
                possibleTurns.Add(hit.transform);
            }
        }
        if (Physics.Raycast(transform.position, Vector3.back, out hit, Mathf.Infinity, characterLayer))
        {
            if (hit.transform.CompareTag("TurnPoints"))
            {
                possibleTurns.Add(hit.transform);
            }
        }
        if (Physics.Raycast(transform.position, Vector3.left, out hit, Mathf.Infinity, characterLayer))
        {
            if (hit.transform.CompareTag("TurnPoints"))
            {
                possibleTurns.Add(hit.transform);
            }
        }

        MakeTurn();
    }

    void FindTurnsEscapePlayer()
    {
        possibleTurns.Clear();
        if (Physics.Raycast(transform.position, Vector3.forward, out hit, Mathf.Infinity))
        {
            if (hit.transform.CompareTag("TurnPoints"))
            {
                possibleTurns.Add(hit.transform);
            }
        }
        if (Physics.Raycast(transform.position, Vector3.right, out hit, Mathf.Infinity))
        {
            if (hit.transform.CompareTag("TurnPoints"))
            {
                possibleTurns.Add(hit.transform);
            }
        }
        if (Physics.Raycast(transform.position, Vector3.back, out hit, Mathf.Infinity))
        {
            if (hit.transform.CompareTag("TurnPoints"))
            {
                possibleTurns.Add(hit.transform);
            }
        }
        if (Physics.Raycast(transform.position, Vector3.left, out hit, Mathf.Infinity))
        {
            if (hit.transform.CompareTag("TurnPoints"))
            {
                possibleTurns.Add(hit.transform);
            }
        }

        MakeTurn();
    }

    public void RepositionEnemy()
    {
        transform.position = startPos;
        posToGo = firstPos;
    }
    public void StopEnemies()
    {
        posToGo = firstPos;
    }

    public void ReleaseGhost()
    {
        posToGo = firstPos;
        released = true;
        if (aIPath != null)
        {
            aIPath.enabled = true;
        }
    }

    public Transform CheckOnePosition(Directions direction)
    {
        Vector3 dirToLook = Vector3.forward;
        switch (direction)
        {
            case Directions.Up:
                dirToLook = Vector3.forward;
                break;
            case Directions.Right:
                dirToLook = Vector3.right;
                break;
            case Directions.Down:
                dirToLook = Vector3.back;
                break;
            case Directions.Left:
                dirToLook = Vector3.left;
                break;
        }

        if (Physics.Raycast(transform.position, dirToLook, out hit, Mathf.Infinity, characterLayer))
        {
            if (hit.transform.CompareTag("TurnPoints"))
            {
                return hit.transform;
            }
        }
        return null;
    }

    public void LookAtPlayer()
    {
        transform.LookAt(playerController.transform.position);
    }
    public void RepositionGhosts()
    {
        released = false;
        if(aIPath != null) aIPath.enabled = false;
        posToGo = null;
        transform.position = ghostBase.position;
    }
    public void PacGotHit()
    {
        released = false;
        if (aIPath != null) aIPath.enabled = false;
        posToGo = null;
    }

    public void BaseExit()
    {
        transform.position = Vector3.MoveTowards(transform.position, firstPos.position, Time.deltaTime);
    }
    public void Scared()
    {
        materialObject.material = scaredMaterial;
        blue = true;
    }
    public void BackToNormal()
    {
        materialObject.material = normalMaterial;
        blue = false;
        if (aIPath != null)
        {
            aIPath.enabled = true;
        }
    }
    public void EnemyEaten()
    {
    
        StartCoroutine(SlowDown());
        source.Play();
        posToGo = null;
        timerEaten = 3f;
        materialObject.material = eatenMaterial;
        hasBeenEaten = true;

    }
    public void CountdownToRevive()
    {
        if (hasBeenEaten)
        {
            if (timerEaten > 0f)
            {
                timerEaten -= Time.deltaTime;

            }
            else
            {
                hasBeenEaten = false;
                BackToNormal();
                RepositionEnemy();
                released = true;
                exiting = true;
            }
        }
    }

    void GhostA()
    {
        if (released)
        {
            Scatter();
        }
    }
    void GhostB()
    {
        if (released)
        {
            if (posToGo != null)
            {

                if (Vector3.Distance(transform.position, posToGo.position) < 1f)
                {
                    int r = triggers.childCount - 1;
                    posToGo = triggers.GetChild(Random.Range(0, r));
                }
                destination.target = posToGo;
            }
            else
            {
                aIPath.enabled = false;
                transform.position = Vector3.MoveTowards(transform.position, ghostBase.position, 2 * Time.deltaTime);
            }
        }
    }
    void GhostC()
    {
        if (released)
        {
            if (posToGo != null)
            {

                destination.target = playerController.transform;
            }
            else
            {
                aIPath.enabled = false;
                transform.position = Vector3.MoveTowards(transform.position, ghostBase.position, 2 * Time.deltaTime);
            }
        }
    }

    void GhostD()
    {
        if (released)
        {
            if (posToGo != null)
            {
            }
            else
            {
                aIPath.enabled = false;
                transform.position = Vector3.MoveTowards(transform.position, ghostBase.position, 2 * Time.deltaTime);
            }
        }
    }

    public void StopAllMovement()
    {
        aIPath.enabled = false;
    }

    IEnumerator SlowDown()
    {
        Time.timeScale = 0.2f;
        yield return new WaitForSeconds(0.1f);
        Time.timeScale = 1f;
    }
}
