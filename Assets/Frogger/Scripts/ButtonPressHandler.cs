using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonPressHandler : GameBehaviour
{
    private int cols = 10;
    private int rows = 21;
    public int playerRowPos = 1;   // For checking victory condition.
    private float upperLimit;
    public Transform camPos;
    public Transform playerPos;
    public float hopLength;
    // Start is called before the first frame update
    void Start()
    {
        hopLength = Screen.width / cols;
        upperLimit = hopLength * rows;
        camPos = GameObject.Find("Main Camera").GetComponent<Transform>();
        playerPos = GameObject.Find("Frog").GetComponent<Transform>();

        var allInteractables = GameObject.FindObjectsOfType<Button>();
        foreach (var i in allInteractables)
        {
            i.onClick.AddListener(() => MoveOnButtonPress(i.gameObject.name));
        }
    }

    private void MoveOnButtonPress(string buttonName)
    {
        Vector3 hop = new Vector3(0f, 0f, 0f);
        switch (buttonName)
        {
            case "UpButton":
                if (playerPos.position.y + hopLength < upperLimit)
                {
                    hop.y = hopLength;
                    camPos.position = new Vector3(camPos.position.x, camPos.position.y + hopLength, camPos.position.z);
                    playerPos.localRotation = Quaternion.Euler(0, 0, 0);
                    playerRowPos++;
                }
                break;
            case "DownButton":
                if (playerPos.position.y - hopLength > 0)
                {
                    hop.y = -hopLength;
                    camPos.position = new Vector3(camPos.position.x, camPos.position.y - hopLength, camPos.position.z);
                    playerPos.localRotation = Quaternion.Euler(0, 0, 180);
                    playerRowPos--;
                }
                break;
            case "LeftButton":
                if (playerPos.position.x - hopLength > 0)
                {
                    hop.x = -hopLength;
                    playerPos.localRotation = Quaternion.Euler(0, 0, 90);
                }
                break;
            case "RightButton":
                if( playerPos.position.x + hopLength < Screen.width )
                {
                    hop.x = hopLength;
                    playerPos.localRotation = Quaternion.Euler(0, 0, 270);
                }
                break;
            default:
                break;
        }
        playerPos.position += hop;
        if(playerRowPos == rows)
        {
            // Give the player a little treat for finishing the level.
            var score = 1.1 * Mathf.Pow(1.24f, playerRowPos); // Just happens to be 100 if playerRowPos = 21;
            Debug.Log("Victory screech!");
            PointsDatabase.SaveAdditively(PointsDatabase.Field.Frogger, (int)score);
            SceneManager.GoToHub();
        }
    }
}
