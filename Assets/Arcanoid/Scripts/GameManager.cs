using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private IntroManager _introManager;
    // Start is called before the first frame update
    void Start()
    {
        _introManager.PlayIntro();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
