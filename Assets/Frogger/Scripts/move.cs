using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour
{
    public Transform camPos;

    public float moveSpeed = 5f;
    public float hopLength = 1f;

    [SerializeField]
    private int cols;
    // Start is called before the first frame update
    void Start()
    {
        camPos = GameObject.Find("Main Camera").GetComponent<Transform>();
        // Spawn the player and center the camera on them.
        transform.position = new Vector3(cols/2f + .5f, 0.5f, 0);
        camPos.position = transform.position + new Vector3(0, 0, -1);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 hop = new Vector3(0f, 0f, 0f);
        if (Input.GetKeyDown("w"))
        {
            hop.y = hopLength;
            camPos.position = new Vector3(camPos.position.x, camPos.position.y + hopLength, camPos.position.z);
            
        }
        if (Input.GetKeyDown("s"))
        {
            hop.y = -hopLength;
            camPos.position = new Vector3(camPos.position.x, camPos.position.y - hopLength, camPos.position.z);
        }
        if (Input.GetKeyDown("a"))
        {
            hop.x = -hopLength;
        }
        if (Input.GetKeyDown("d"))
        {
            hop.x = hopLength;
        }
        jump(hop);

    }

    void jump(Vector3 v)
    {
        transform.position += v;
    }
}
