using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FitScreen : MonoBehaviour
{
    public GameObject Canvas;

    // Start is called before the first frame update
    void Start()
    {
        this.UpdateCanvasSize();
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    /// <summary>
    /// Updates the canvas size according to the current screen size.
    /// The size is relative, so the method calculates the screen ratio,
    /// thus it supports all screen resolutions.
    /// </summary>
    void UpdateCanvasSize() {
        if (this.Canvas == null) {
            throw new System.Exception("Canvas object needed");
        }
        
        // get the screen size
        int width = Screen.width;
        int height = Screen.height;
        
        // calculate the width/height ratio
        float canvasSize = 1f;
        if (width > height) {
            canvasSize = (float)height / width;
        } else {
            canvasSize = (float)width / height;
        }
        
        // resize the canvase to fit the screen
        // (a square that perfectly fits the screen)
        this.Canvas.gameObject.transform.localScale = new Vector3(canvasSize, 1, canvasSize);
    }
}
