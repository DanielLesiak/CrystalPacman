using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class LevelController : MonoBehaviour
{
    public TextMeshProUGUI scoreText, livesText, highScoreText;
    public GameObject ghostA, ghostB, ghostC, ghostD, pellets;
    public AudioClip a_ghostNormal, a_ghostScared, a_PacmanHit;

    bool stageStarted, introSong, deathAnim;
    PlayerController playerController;
    int score, lives, highScore;
    AudioSource aSource;
    float timer;
    GameController gameController;

    private void Start()
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        gameController.GetController(GetComponent<LevelController>());
        gameController.ResetLivesScore();
        scoreText.text = score.ToString();
        livesText.text = lives.ToString();
        highScoreText.text = highScore.ToString();
        timer = 0f;
        aSource = GetComponent<AudioSource>();
        introSong = true;
        stageStarted = false;
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        deathAnim = false;
    }

    private void Update()
    {
        if (introSong)
        {
            if (aSource.isPlaying == false)
            {
                introSong = false;
            }
            {
                return;
            }
        }

        if (Input.GetAxisRaw("Left") > 0.8f && !stageStarted && !deathAnim)
        {
            stageStarted = true;
            playerController.stopMovement = false;
            ghostA.GetComponent<Ghost>().ReleaseGhost();
            aSource.clip = a_ghostNormal;
            aSource.Play();
            aSource.loop = true;
        }
        else if (Input.GetAxisRaw("Right") > 0.8f && !stageStarted && !deathAnim)
        {

            stageStarted = true;
            playerController.stopMovement = false;
            ghostA.GetComponent<Ghost>().ReleaseGhost();
            aSource.clip = a_ghostNormal;
            aSource.Play();
            aSource.loop = true;

        }
        else if (Input.GetAxisRaw("Down") > 0.8f && !stageStarted && !deathAnim)
        { playerController.stopMovement = false; }
        scoreText.text = score.ToString();
        livesText.text = lives.ToString();
        highScoreText.text = highScore.ToString();
        CheckHighScore();

        ReleaseGhostTimer();
        StageComplete();

    }

    public void AddScore(int scoreToAdd)
    {
        score += scoreToAdd;
    }

    void ReleaseGhostTimer()
    {
        if (stageStarted && ghostA != null)
        {
            timer += Time.deltaTime;
            if (timer > 5f && !ghostB.GetComponent<Ghost>().released)
            {
                ghostB.GetComponent<Ghost>().ReleaseGhost();
            }
            else if (timer > 10f && !ghostC.GetComponent<Ghost>().released)
            {
                ghostC.GetComponent<Ghost>().ReleaseGhost();
            }
            else if (timer > 15f && !ghostD.GetComponent<Ghost>().released)
            {
                ghostD.GetComponent<Ghost>().ReleaseGhost();
            }

        }
    }

    public void ResetStage()
    {
        timer = 0f;
    }

    public void PowerPac()
    {
        ghostA.GetComponent<Ghost>().Scared();
        ghostB.GetComponent<Ghost>().Scared();
        ghostC.GetComponent<Ghost>().Scared();
        ghostD.GetComponent<Ghost>().Scared();
        playerController.powerPac = 8f;
        aSource.clip = a_ghostScared;
        aSource.Play();

    }

    public void PowerPacOff()
    {
        if (aSource.clip != a_ghostNormal)
        {
            aSource.clip = a_ghostNormal;
            aSource.Play();
            ghostA.GetComponent<Ghost>().BackToNormal();
            ghostB.GetComponent<Ghost>().BackToNormal();
            ghostC.GetComponent<Ghost>().BackToNormal();
            ghostD.GetComponent<Ghost>().BackToNormal();
        }

    }

    public void GetHurt()
    {
        if(lives >1)
        {
            lives--;
            deathAnim = true;
            aSource.clip = a_PacmanHit;
            aSource.Play();
            aSource.loop = false;
            playerController.stopMovement = true;
            stageStarted = false;

            ghostA.GetComponent<Ghost>().PacGotHit();
            ghostB.GetComponent<Ghost>().PacGotHit();
            ghostC.GetComponent<Ghost>().PacGotHit();
            ghostD.GetComponent<Ghost>().PacGotHit();
        }
        else
        {
            deathAnim = true;
            aSource.clip = a_PacmanHit;
            aSource.Play();
            aSource.loop = false;
            playerController.stopMovement = true;
            stageStarted = false;

            ghostA.GetComponent<Ghost>().PacGotHit();
            ghostB.GetComponent<Ghost>().PacGotHit();
            ghostC.GetComponent<Ghost>().PacGotHit();
            ghostD.GetComponent<Ghost>().PacGotHit();
            StartCoroutine(GameOver());
        }

    }

    public void DeathAnimeOver()
    {
        deathAnim = false;
        playerController.StopAnim();
        playerController.RepositionPlayer();
        timer = 0f;
        ghostA.GetComponent<Ghost>().RepositionGhosts();
        ghostB.GetComponent<Ghost>().RepositionGhosts();
        ghostC.GetComponent<Ghost>().RepositionGhosts();
        ghostD.GetComponent<Ghost>().RepositionGhosts();
    }

    void StageComplete()
    {
        if (pellets.transform.childCount < 1)
        {
            gameController.SetPrefs(score, lives);
            aSource.loop = false;
            playerController.stopMovement = true;
            stageStarted = false;

            ghostA.GetComponent<Ghost>().PacGotHit();
            ghostB.GetComponent<Ghost>().PacGotHit();
            ghostC.GetComponent<Ghost>().PacGotHit();
            ghostD.GetComponent<Ghost>().PacGotHit();
            gameController.ReloadScene();
        }
    }
    public void GetPrefs(int _score, int _lives, int _highScore)
    {
        score = _score;
        lives = _lives;
        highScore = _highScore;
    }
    void CheckHighScore()
    { 
        if(score > highScore)
        {
            highScore = score;
            gameController.SetHighScore(highScore);
        }
    }

    IEnumerator GameOver()
    {
        yield return new WaitForSeconds(1.8f);
        gameController.GameOver();
    }


}
