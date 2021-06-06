using System;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public static void LoadScene(int index)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(index);
    }

    public static void GoToHub()
    {
        LoadScene(0);
    }

    public static void RestartScene()
    {
        LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
}
