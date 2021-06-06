using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PaddleController : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform firstPlayer;

    public Transform secondPlayer;
    private Camera _camera;

    void Start()
    {
        _camera =   Camera.main;

    }

    // Update is called once per frame
    void Update()
    {
        PaddleMovement();
    }


    private void PaddleMovement()
    {

        foreach (var touch in Input.touches)
        {
            var pos = touch.position;

            if (pos.y>0)
            {
                firstPlayer.DOMoveX(pos.x, 0.1f);
            }
            else
            {
                secondPlayer.DOMoveX(pos.x, 0.1f);
            }
        }

    #if UNITY_EDITOR
            if (Input.GetMouseButton(0))
            {
                var pos = _camera.ScreenToWorldPoint(Input.mousePosition);;


                if (pos.y>0)
                {
                    firstPlayer.DOMoveX(pos.x, 0.1f);
                }
                else
                {
                    secondPlayer.DOMoveX(pos.x, 0.1f);
                }
            }

    #endif
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag("Ball"))
        {
            var moveDir = other.transform.GetComponent<Ball>().move;

            if (other.contacts[0].point.x < transform.position.x)
            {
                moveDir.x = 1;
            }
            else
            {
                moveDir.x = -1;
            }

            moveDir *= -1;

            other.transform.GetComponent<Ball>().move = moveDir;

            BlockGenerator.instance.ActivateBlock();
            BlockGenerator.instance.ActivateBlock();

        }


    }
}
