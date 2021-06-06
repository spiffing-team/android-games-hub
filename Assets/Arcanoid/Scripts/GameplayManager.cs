using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager instance;
    
    [HideInInspector] public int scoreTop = 0;

    [HideInInspector] public int scoreBot = 0;

    public GameObject Ball;
    public GameObject startPos;
    
    public TextMeshProUGUI scoreText;

    public GameOverScreen gameOverScreen;
    
    public GameObject gameUI;

    
    // Start is called before the first frame update


    public void StartGameplay()
    {
        gameObject.SetActive(true);
        gameUI.SetActive(true);
    }
    private void Start()
    {
        if (instance == null)
        {
            instance = this;

        }
    }

    public void AddScore(bool isTop)
    {
        if (isTop)
        {
            scoreTop++;
        }
        else
        {
            scoreBot++;
        }
        UpdateScore();

        if (scoreTop > 4 || scoreBot > 4)
        {
            gameUI.SetActive(false);
            gameObject.SetActive(false);
            gameOverScreen.GameEnded(scoreBot,scoreTop);
        }
    }
    
    public void SubScore(bool isTop)
    {
        if (isTop)
        {
            scoreTop--;
        }
        else
        {
            scoreBot--;
        }
        UpdateScore();
    }

    public void CreateNewBall()
    {
        Ball.transform.position = startPos.transform.position;
        Ball.GetComponent<Ball>().StartSpeed();
    }

    public void UpdateScore()
    {
        scoreText.text = scoreTop.ToString() + ":" + scoreBot.ToString();
    }
}
