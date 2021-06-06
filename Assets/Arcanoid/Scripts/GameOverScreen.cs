using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI finalScore;
    public void GameEnded( int scoreBot, int scoreTop)
    {
        gameObject.SetActive(true);
        finalScore.text = scoreTop + ":" + scoreBot;

    }
}
