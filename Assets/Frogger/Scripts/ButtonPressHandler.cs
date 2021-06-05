using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonPressHandler : GameBehaviour
{
    private int cols = 10;
    public Transform camPos;
    public Transform playerPos;
    public float hopLength;
    // Start is called before the first frame update
    void Start()
    {
        hopLength = Screen.width / cols;
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
                hop.y = hopLength;
                camPos.position = new Vector3(camPos.position.x, camPos.position.y + hopLength, camPos.position.z);
                break;
            case "DownButton":
                hop.y = -hopLength;
                camPos.position = new Vector3(camPos.position.x, camPos.position.y - hopLength, camPos.position.z);
                break;
            case "LeftButton":
                hop.x = -hopLength;
                break;
            case "RightButton":
                hop.x = hopLength;
                break;
            default:
                break;
        }
        playerPos.position += hop;
    }
}
