using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum ImageMap
{
    Bird,
    Apple,
    Car,
    Dog,
    Ship
}

public class NonogramMain : MonoBehaviour
{
    public GameObject Canvas;
    public GameObject NonoblockPrefab;
    public GameObject TilesCountTextPrefab;

    public TextAsset BirdImageMap;
    public TextAsset AppleImageMap;
    public TextAsset CarImageMap;
    public TextAsset DogImageMap;
    public TextAsset ShipImageMap;

    private float canvasSize;
    /// <summary>
    /// The original size of the canvas.
    /// It serves the purpose of a modifier value used when aligining and resizing
    /// some other objects that should be on the canvas.
    /// </summary>
    private float canvasOriginalSize = 10;

    private bool[,] selectedImageMap;

    // Start is called before the first frame update
    void Start()
    {
        this.updateCanvasSize();
        this.selectImageMap();
        this.instatiateGrid();
    }

    // Update is called once per frame
    void Update()
    {
        this.checkOnClick();
    }

    private void requireCanvas()
    {
        if (this.Canvas == null)
        {
            throw new System.Exception("Canvas game object needed");
        }
    }

    /// <summary>
    /// Updates the canvas size according to the current screen size.
    /// The size is relative, so the method calculates the screen ratio,
    /// thus it supports all screen resolutions.
    /// </summary>
    private void updateCanvasSize()
    {
        this.requireCanvas();
        // get the screen size
        int width = Screen.width;
        int height = Screen.height;

        // calculate the width/height ratio
        float canvasSize = 1f;
        if (width > height)
        {
            canvasSize = (float)height / width;
        }
        else
        {
            canvasSize = (float)width / height;
        }

        // resize the canvase to fit the screen
        // (a square that perfectly fits the screen)
        this.Canvas.gameObject.transform.localScale = new Vector3(canvasSize, 1, canvasSize);
        this.canvasSize = canvasSize;
    }

    private void instatiateGrid()
    {
        this.requireCanvas();

        // the left and top offset of the grid
        // (accomodating the margins containing nonogram numbers)
        var offset = this.canvasSize * this.canvasOriginalSize * 0.15f;
        // the size of the grid
        var size = this.canvasSize * this.canvasOriginalSize - offset * 0.5f;

        // get the map
        var map = this.selectedImageMap;

        // calculate both the size of a cell and the margin between cells
        var cellSize = size / map.GetLength(0);

        // the starting point of the grid
        // (the top-left corner)
        var origin = new Vector3(
            (size - offset * 0.5f) * -0.5f,
            (size - offset * 0.5f) * 0.5f,
            0
        );

        // instantiate the tiles
        for (int y = 0; y < map.GetLength(0); y++)
        {
            for (int x = 0; x < map.GetLength(1); x++)
            {
                var obj = Instantiate(
                    this.NonoblockPrefab,
                    origin + new Vector3(x * cellSize + 0.5f * cellSize, -y * cellSize - 0.5f * cellSize, 0),
                    Quaternion.identity
                );
                var tmp = cellSize * 0.9f;
                obj.transform.localScale = new Vector3(tmp, tmp, tmp);
                // save
                var cube = obj.GetComponentInChildren<Nonoblock>();
                cube.x = x;
                cube.y = y;
            }
        }

        origin = new Vector3(
            size * -0.5f,
            size * 0.5f,
            -1
        );

        // instantiate the tiles counters
        for (int y = 0; y < map.GetLength(0); y++)
        {
            var prefab = Instantiate(
                this.TilesCountTextPrefab,
                origin + new Vector3(0, -y * cellSize - 0.75f * cellSize, 0),
                Quaternion.identity
            );
            var textObj = prefab.GetComponentInChildren<TilesCountText>();
            // calc
            var vals = new List<int>();
            vals.Add(0);
            var index = 0;
            for (int x = 0; x < map.GetLength(1); x++)
            {
                if (map[y, x])
                {
                    vals[index]++;
                }
                else if (vals[index] != 0)
                {
                    vals.Add(0);
                    index++;
                }
            }
            var output = "";
            foreach (var val in vals)
            {
                if (val != 0)
                    output += val.ToString();
            }
            textObj.SetText(output);
            var textSize = 0.4f * this.canvasSize;
            if (output.Length > 1)
            {
                var modifier = output.Length * 0.75f;
                textObj.SetSize(textSize * (1f / modifier));
            }
            else
            {
                textObj.SetSize(textSize);
            }
        }
        origin += new Vector3(cellSize, 0, 0);
        for (int x = 0; x < map.GetLength(1); x++)
        {
            var prefab = Instantiate(
                this.TilesCountTextPrefab,
                origin + new Vector3(x * cellSize - 0.25f * cellSize, 0, 0),
                Quaternion.identity
            );
            var textObj = prefab.GetComponentInChildren<TilesCountText>();
            // calc
            var vals = new List<int>();
            vals.Add(0);
            var index = 0;
            for (int y = 0; y < map.GetLength(1); y++)
            {
                if (map[y, x])
                {
                    vals[index]++;
                }
                else if (vals[index] != 0)
                {
                    vals.Add(0);
                    index++;
                }
            }
            var output = "";
            foreach (var val in vals)
            {
                if (val != 0)
                    output += val.ToString() + " ";
            }
            output = output.Trim();
            textObj.SetText(output);
            var textSize = 0.4f * this.canvasSize;
            if (output.Length > 1)
            {
                var modifier = output.Length * 0.75f;
                textObj.SetSize(textSize * (1f / modifier));
            }
            else
            {
                textObj.SetSize(textSize);
            }
        }
    }

    private Vector3? getInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            return Input.mousePosition;
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// Runs the `onClick` method when a click is detected.
    /// </summary>
    private void checkOnClick()
    {
        var clickPosition = this.getInput();
        if (clickPosition != null)
        {
            RaycastHit raycastHit;
            Ray ray = Camera.main.ScreenPointToRay(clickPosition ?? default);
            if (Physics.Raycast(ray, out raycastHit, 100f))
            {
                if (raycastHit.transform != null)
                {
                    this.onClick(raycastHit.transform.gameObject);
                }
            }
        }
    }

    private void onClick(GameObject target)
    {
        this.handleGridClick(target);
    }

    private void handleGridClick(GameObject target)
    {
        // require the cube that is sat in the Nonoblock prefab
        Nonoblock cube = target.GetComponent<Nonoblock>();
        if (cube == null)
        {
            return;
        }

        // get the tile that was selected
        var x = cube.x;
        var y = cube.y;
        // get the current state of the tile
        var state = this.selectedImageMap[y, x];
        // invert the state
        this.selectedImageMap[y, x] = !this.selectedImageMap[y, x];
        cube.ToggleSelected();

        if (this.checkIfWon())
        {
            Debug.Log("you’ve won!");
        }

    }

    private bool checkIfWon()
    {
        int sum = 0;
        var map = this.selectedImageMap;
        for (int y = 0; y < map.GetLength(0); y++)
        {
            for (int x = 0; x < map.GetLength(1); x++)
            {
                if (map[y, x]) { sum++; }
            }
        }
        return sum == 0;
    }

    private void selectImageMap(ImageMap? mapName = null)
    {
        if (mapName == null)
        {
            // choose at random
            var values = ImageMap.GetValues(typeof(ImageMap));
            mapName = (ImageMap)values.GetValue(Random.Range(0, values.Length));
        }
        bool[,] output = null;
        string raw = null;
        // map enum values to text assets
        // (yes, I know)
        switch (mapName)
        {
            case ImageMap.Bird:
                raw = this.BirdImageMap.text;
                break;
            case ImageMap.Apple:
                raw = this.AppleImageMap.text;
                break;
            case ImageMap.Car:
                raw = this.CarImageMap.text;
                break;
            case ImageMap.Dog:
                raw = this.DogImageMap.text;
                break;
            case ImageMap.Ship:
                raw = this.DogImageMap.text;
                break;
        }

        if (raw != null)
        {
            var rows = raw.Split('\n');
            var size = rows.Length - 1; // do not include the last empty line
            output = new bool[size, size];
            for (int y = 0; y < size; y++)
            {
                var row = rows[y];
                for (int x = 0; x < size; x++)
                {
                    if (row[x] != '1' && row[x] != '0') { continue; }
                    output[y, x] = (row[x] == '1');
                }
            }
        }
        this.selectedImageMap = output;
    }
}
