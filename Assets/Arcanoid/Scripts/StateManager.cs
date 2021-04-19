using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    [SerializeField] private IntroManager _introManager;
    [SerializeField] private MainUIManager _mainUiManager;
    
    
    void Awake()
    {
        _introManager.EndOfIntro += HideIntro;
        _mainUiManager.HideUi += HideMainMenu;
        ShowIntro();
    }

    #region Intro

    private void ShowIntro()
    {
        _introManager.Show();
    }
    
    private void HideIntro()
    {
        _introManager.gameObject.SetActive(false);
        ShowMainMenu();
    }

    #endregion
    
    #region Menu

    private void ShowMainMenu()
    {
        _mainUiManager.Show();
    }

    private void HideMainMenu()
    {
        _mainUiManager.gameObject.SetActive(false);
    }
    

    #endregion

    #region Gameplay

    

    #endregion
   
}
