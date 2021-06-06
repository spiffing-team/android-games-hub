using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] private Boolean isTop;
    [SerializeField] private GameplayManager _gameplayManager;
    private void OnTriggerEnter(Collider other)
    {
        _gameplayManager.AddScore(isTop);
        _gameplayManager.CreateNewBall();
    }
}
