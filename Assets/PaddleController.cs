using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PaddleController : MonoBehaviour
{
    // Start is called before the first frame update
    public int player=0;
    void Start()
    {
        
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
            
                var pos = Camera.current.ScreenToWorldPoint(Input.mousePosition).x;;
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
        Debug.Log(other.gameObject.name);
        BlockGenerator.instance.ActivateBlock();
        other.transform.GetComponent<Rigidbody>().velocity = new Vector3(0,10,0);
        Debug.Log(other.transform.GetComponent<Rigidbody>().velocity) ;
        
    }
}
