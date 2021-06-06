using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PaddleController : MonoBehaviour
{
    // Start is called before the first frame update
    public int player=0;
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
        if (player == 0)
        {
            if (Input.touchCount > 0 || Input.GetMouseButton(0))
            {
                var pos = _camera.ScreenToWorldPoint(Input.mousePosition).x;;
                Debug.Log("MYSZKa");
                //Touch touch = Input.GetTouch(0);
            
                transform.DOMoveX(pos, 0.1f);

                //transform.DOMoveX(touch.position.x, 0.1f);
            }
        }
        else
        {
            if (Input.touchCount > 0 || Input.GetMouseButton(1))
            {
            
                var pos = Camera.current.ScreenToWorldPoint(Input.mousePosition).x;;
                Debug.Log("MYSZKa");
                //Touch touch = Input.GetTouch(0);
            
                transform.DOMoveX(pos, 0.1f);

                //transform.DOMoveX(touch.position.x, 0.1f);
            }
        }
       
      
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
