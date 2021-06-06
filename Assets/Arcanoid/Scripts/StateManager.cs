using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    [SerializeField] private IntroManager _introManager;
    [SerializeField] private MainUIManager _mainUiManager;
    [SerializeField] private GameplayManager Gameplay;
    [SerializeField] private GameObject GameOver;
    
    
    void Awake()
    {
        _introManager.OnEnd += HideIntro;
        _mainUiManager.OnEnd += GameStart;
        ShowIntro();
    }

    #region Intro

    private void ShowIntro()
    {
        _introManager.Show();
    }
    
    private void HideIntro()
    {
        ShowMainMenu();
    }

    #endregion
    
    #region Menu

    private void ShowMainMenu()
    {
        _mainUiManager.Show();
    }
    

    #endregion

    #region Gameplay

    private void  GameStart()
    {
        Gameplay.StartGameplay();
    }
    
    

    #endregion
   
}
