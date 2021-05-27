using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class GameBehaviour : MonoBehaviour
{
    protected virtual void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
            OnGoBack();
    }

    protected virtual void OnGoBack()
    {
        SceneManager.GoToHub();
    }
}
