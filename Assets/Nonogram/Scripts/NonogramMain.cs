using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class NonogramMain : GameBehaviour
{
    public GameObject Canvas;
    public GameObject NonoblockPrefab;
    public GameObject TilesCountTextPrefab;
    public GameObject EndGameTextPrefab;
    public GameObject EndGameImage;

    public int HintIfLessThan;
    public int ScorePerRound;
    public int BonusPoints;

    private ScreenOrientation previousScreenOrientation;

    private TextAsset[] imageMaps;
    private Sprite[] imageMapsImgs;
    private int chosenImageMap;

    private float canvasSize;
    /// <summary>
    /// The original size of the canvas.
    /// It serves the purpose of a modifier value used when aligining and resizing
    /// some other objects that should be on the canvas.
    /// </summary>
    private float canvasOriginalSize = 10;

    private bool[,] selectedImageMap;
    private bool[,] selectedImageMapOriginal;
    private (int, int) bonusPointsTile;

    private List<GameObject> textObjects = new List<GameObject>();
    private List<GameObject> tiles = new List<GameObject>();

    private bool alreadyWon = false;
    private bool bonusPointsAcquired = false;

    // Start is called before the first frame update
    void Start()
    {
        previousScreenOrientation = Screen.orientation;
        Screen.orientation = ScreenOrientation.Portrait;
        this.updateCanvasSize();
        this.loadImageMaps();
        this.selectImageMap();
        this.instatiateGrid();
    }

    protected override void OnGoBack()
    {
        Screen.orientation = previousScreenOrientation;
        base.OnGoBack();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        this.checkOnClick();
    }

    private void loadImageMaps()
    {
        this.imageMapsImgs = Resources.LoadAll("Nonogram/ImageMaps", typeof(Sprite)).Cast<Sprite>().ToArray();
        this.imageMaps = Resources.LoadAll("Nonogram/ImageMaps", typeof(TextAsset)).Cast<TextAsset>().ToArray();
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
                this.tiles.Add(obj);
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

        var scale = 1f;
        if (this.cellsLeft() < this.HintIfLessThan)
        {
            scale = 0.9f;
        }
        foreach (var tile in this.tiles)
        {
            var t = tile.GetComponentInChildren<Nonoblock>();
            if (this.selectedImageMapOriginal[t.y, t.x])
            {
                t.SetCubeSize(scale);
            }
        }

        var colour = false;
        if (x == this.bonusPointsTile.Item1 && y == this.bonusPointsTile.Item2)
        {
            this.bonusPointsAcquired = true;
            colour = true;
        }

        cube.AnimateClick(Vector3.one * this.canvasSize * 0.08f, colour);

        if (this.checkIfWon() && !this.alreadyWon)
        {
            this.alreadyWon = true;
            Debug.Log("you???ve won!");
            this.handleWin();
        }

    }

    private int cellsLeft()
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
        return sum;
    }

    private bool checkIfWon()
    {
        return this.cellsLeft() == 0;
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
        var score = this.ScorePerRound;

        // display the score
        var text = textObj.GetComponentInChildren<TilesCountText>();
        var t = "You???ve won\nwith a score of:\n";
        if (this.bonusPointsAcquired)
        {
            score += this.BonusPoints;
            t += score + "\n(You???ve found\n" + this.BonusPoints + " bonus points!)";
        }
        else
        {
            t += score;
        }
        text.SetText(t);
        text.SetSize(this.canvasSize * 0.5f);

        // save the score
        PointsDatabase.SaveAdditively(PointsDatabase.Field.Nonogram, score);

        // play the ending animation
        var endAnimation = this.EndGameImage.GetComponent<Animator>();
        endAnimation.Play("EndGameImage");
        Destroy(this.EndGameImage, endAnimation.runtimeAnimatorController.animationClips[0].length);
    }

    private void selectImageMap(string mapName = null)
    {
        bool[,] output = null;
        TextAsset asset = null;
        var size = 0;

        if (mapName == null)
        {
            // choose at random
            asset = this.imageMaps[Random.Range(0, this.imageMaps.Length)];
        }
        else
        {
            asset = this.imageMaps.Where((TextAsset ta) => ta.name == mapName).First();
        }

        if (asset != null)
        {
            var raw = asset.text;

            var rows = raw.Split('\n');
            size = rows.Length - 1; // do not include the last empty line
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
        this.selectedImageMapOriginal = (bool[,])output.Clone();

        this.chosenImageMap = System.Array.IndexOf(this.imageMaps, asset);

        var sprite = this.EndGameImage.GetComponent<SpriteRenderer>();
        sprite.sprite = this.imageMapsImgs[this.chosenImageMap];

        // choose a tile that grants bonus points
        this.bonusPointsTile = (Random.Range(0, size), Random.Range(0, size));
        var xt = this.bonusPointsTile.Item1;
        var yt = this.bonusPointsTile.Item2;
        // nullify the tile if it is outside the image
        if (!this.selectedImageMapOriginal[yt, xt])
        {
            this.bonusPointsTile = (-1, -1);
        }
    }
}
