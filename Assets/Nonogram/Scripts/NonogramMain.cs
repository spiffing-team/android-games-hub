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

public class NonogramMain : GameBehaviour
{
    public GameObject Canvas;
    public GameObject NonoblockPrefab;
    public GameObject TilesCountTextPrefab;
    public GameObject EndGameTextPrefab;

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

    private List<GameObject> textObjects = new List<GameObject>();

    private int score = 0;
    private bool alreadyWon = false;

    // Start is called before the first frame update
    void Start()
    {
        this.updateCanvasSize();
        this.selectImageMap();
        this.instatiateGrid();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
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

        // instantiate the tiles counters
        // vertical ones
        origin = new Vector3(
            size * -0.5f,
            size * 0.5f,
            -1
        );
        this.instantiateTilesCounters(
            0, // outer loop: rows
            1, // inner loop: columns
            map,
            origin,
            (int i) => new Vector3(0, -i * cellSize - 0.75f * cellSize, 0) // move vertically
        );
        // horizontal ones
        origin += new Vector3(1.05f * cellSize, 0, 0);
        this.instantiateTilesCounters(
            1, // outer loop: columns
            0, // inner loop: rows
            map,
            origin,
            (int i) => new Vector3(i * cellSize - 0.25f * cellSize, 0, 0) // move horizontally
        );
    }

    // instantiating a row/column of tile counters
    private delegate Vector3 OriginOffset(int i);
    private void instantiateTilesCounters(
        int outerRow,
        int innerRow,
        bool[,] map,
        Vector3 origin,
        OriginOffset originOffset
    )
    {
        for (int i = 0; i < map.GetLength(outerRow); i++)
        {
            var prefab = Instantiate(
                this.TilesCountTextPrefab,
                origin + originOffset(i),
                Quaternion.identity
            );
            this.textObjects.Add(prefab);
            var textObj = prefab.GetComponentInChildren<TilesCountText>();
            // calc
            var vals = new List<int>();
            vals.Add(0);
            var index = 0;
            for (int j = 0; j < map.GetLength(innerRow); j++)
            {
                var t = outerRow < innerRow ? map[i, j] : map[j, i];
                if (t)
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
                if (val != 0 || output == "")
                    output += val.ToString();
            }
            output = output.Trim();
            textObj.SetText(output);
            var textSize = 0.4f * this.canvasSize;
            if (output.Length > 1)
            {
                var modifier = output.Length * 0.55f;
                textObj.SetSize(textSize * (1f / modifier));
            }
            else
            {
                textObj.SetSize(textSize);
            }
        }
    }

    private void removeTextObjects()
    {
        foreach (var obj in this.textObjects)
        {
            Destroy(obj);
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

        cube.AnimateClick(Vector3.one * this.canvasSize * 0.08f);

        if (this.checkIfWon() && !this.alreadyWon)
        {
            this.alreadyWon = true;
            Debug.Log("you’ve won!");
            this.handleWin();
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

    private void handleWin()
    {
        // remove now unnecessary tiles counters
        this.removeTextObjects();
        // hide the grid behind the canvas
        this.Canvas.transform.position += new Vector3(0, 0, -2);
        // instantiate the game over text
        var textObj = Instantiate(
            this.EndGameTextPrefab,
            new Vector3(0, 0, -3),
            Quaternion.identity
        );
        this.score += 100;

        // display the score
        var text = textObj.GetComponentInChildren<TilesCountText>();
        text.SetText("You’ve won\nwith a score of:\n" + this.score);
        text.SetSize(this.canvasSize * 0.5f);

        // save the score
        PointsDatabase.SaveAdditively(PointsDatabase.Field.Nonogram, this.score);
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
