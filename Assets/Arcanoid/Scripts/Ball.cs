using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.Utilities;
using UnityEngine;
using Random = UnityEngine.Random;

public class Ball : MonoBehaviour
{
    private Rigidbody rb;
    public Vector3 move;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        StartSpeed();
    }

    public void StartSpeed()
    {

        if (Random.Range(0f, 1f) > 0.5f)
            move = Vector3.up * 3f;
        else
            move = Vector3.up * -3f;


    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.velocity = move;
    }

    private void OnCollisionEnter(Collision other)
    {
       // move *= -1;
    }
}
