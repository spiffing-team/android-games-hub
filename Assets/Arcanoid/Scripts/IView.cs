using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D;
using UnityEngine;

public interface IView 
{
    void Show();
    EventHandler GetHandler();
}
