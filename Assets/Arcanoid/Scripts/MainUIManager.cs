using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainUIManager : MonoBehaviour, IView
{
    public event EventHandler OnEnd;
        
    public  void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {    
        gameObject.SetActive(false);
        OnEnd?.Invoke();
    }

}
