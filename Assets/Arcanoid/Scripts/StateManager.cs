using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class StateManager : GameBehaviour
{
    [SerializeField] private IntroManager _introManager;
    [SerializeField] private MainUIManager _mainUiManager;
    [SerializeField] private GameplayManager Gameplay;
    [SerializeField] private GameObject GameOver;

    [SerializeField] private  int scoreModifier = 100;


    void Awake()
    {
        _introManager.OnEnd += HideIntro;
        _mainUiManager.OnEnd += GameStart;
        Screen.orientation = ScreenOrientation.Portrait;
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


    public void EndArkanoid()
    {
        var sum = Gameplay.scoreBot + Gameplay.scoreTop;
        sum *= scoreModifier;
        PointsDatabase.SaveAdditively(PointsDatabase.Field.Arcanoid,sum);
        SceneManager.GoToHub();
    }

    #endregion

}
