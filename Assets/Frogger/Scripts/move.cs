using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour
{
    [SerializeField]
    private int cols = 10;
    public Transform camPos;

    public float moveSpeed = 5f;
    public float hopLength = 1f;


    // Start is called before the first frame update
    void Start()
    {
        camPos = GameObject.Find("Main Camera").GetComponent<Transform>();
        // Spawn the player and center the camera on them.
        var cellWidth = Screen.width / cols;
        hopLength = cellWidth;
        transform.position = new Vector3(Screen.width/2f + cellWidth * .5f, cellWidth * .5f, -0.5f);
        transform.localScale = new Vector3(cellWidth * 0.15f, cellWidth * 0.15f, 1);

        camPos.position = transform.position + new Vector3(-cellWidth * .5f, 0, -1);
        GameObject.Find("Main Camera").GetComponent<Camera>().orthographicSize = Screen.width/3f;
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
