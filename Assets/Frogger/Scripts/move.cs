using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour
{
    [SerializeField]
    private int cols = 10;
    public Transform camPos;

    public ButtonPressHandler bph;

    public float moveSpeed = 5f;
    public float hopLength = 1f;

    public bool onLog = false;
    private float logless = 0f;


    // Start is called before the first frame update
    void Start()
    {
        camPos = GameObject.Find("Main Camera").GetComponent<Transform>();
        bph = GameObject.Find("EventSystem").GetComponent<ButtonPressHandler>();
        // Spawn the player and center the camera on them.
        var cellWidth = Screen.width / cols;
        hopLength = cellWidth;
        transform.position = new Vector3(Screen.width/2f + cellWidth * .5f, cellWidth * .5f, -0.5f);
        transform.localScale = new Vector3(cellWidth * 0.15f, cellWidth * 0.15f, 1);

        camPos.position = transform.position + new Vector3(-cellWidth * .5f, 0, -1);
        float ratio = (float)Screen.height / Screen.width;
        GameObject.Find("Main Camera").GetComponent<Camera>().orthographicSize = ratio * Screen.width/2f;
    }

    // Update is called once per frame
    void Update()
    {
        // Legacy code.
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


        if((bph.playerRowPos-1)%10 > 5)
        {
            // On a water tile.
            //onLog = Physics.Raycast(transform.position, new Vector3(0f, 0f, 1f), 10f);
            StartCoroutine(CheckOnLog());

        }

    }

    IEnumerator CheckOnLog()
    {
        yield return new WaitForSeconds(0.1f);
        if(!onLog && (bph.playerRowPos - 1) % 10 > 5)
        {
            int score = CalculatePoints(bph.playerRowPos);
            PointsDatabase.SaveAdditively(PointsDatabase.Field.Frogger, score);
            SceneManager.GoToHub();
        }

    }
    void jump(Vector3 v)
    {
        transform.position += v;
    }

    private int CalculatePoints(int rows)
    {
        // Award the player based on how far they got.
        return (int)(1.1 * Mathf.Pow(1.24f, rows));
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.name == "TruckPrefab(Clone)")
        {
            int score = CalculatePoints(bph.playerRowPos);
            PointsDatabase.SaveAdditively(PointsDatabase.Field.Frogger, score);
            SceneManager.GoToHub();
        }
        else
        {
            onLog = true;
            print("on log");
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        print("out log");
        logless = Time.time;
        onLog = false;
    }
}
