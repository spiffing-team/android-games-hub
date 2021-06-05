using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    [SerializeField]
    private TileBase grassTile;
    [SerializeField]
    private TileBase asphaltTile;
    [SerializeField]
    private TileBase waterTile;

    private int rows = 21;
    private int cols = 10;

    // Start is called before the first frame update
    void Start()
    {
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        // Generate the grid and tilemap game objects.
        var grid = new GameObject("Grid").AddComponent<Grid>();
        var tilemap = new GameObject("Tilemap").AddComponent<Tilemap>();
        tilemap.gameObject.AddComponent<TilemapRenderer>();
        tilemap.transform.SetParent(grid.transform);
        // Ensure the map fits the camera view: it should have 10 columns, so adjust the tile size appropriately.
        var cellWidth = Screen.width / cols;
        grid.GetComponent<Transform>().localScale = new Vector3(cellWidth, cellWidth, 1);

        TileBase t;
        for(int row=0; row<rows; row++)
        {
            t = DetermineCurrentTile(row);

            for(int col=0; col<cols; col++)
            {
                tilemap.SetTile(new Vector3Int(col, row, 0), t);
            }
        }
        

    }

    // Check the row number and return the tile it should be made of.
    private TileBase DetermineCurrentTile(int row)
    {
        if ((row % 10 >= 0 && row % 10 < 2) || (row % 10 == 5))
        {
            return grassTile;
        }
        else if (row % 10 >= 2 && row % 10 < 5)
        {
            return asphaltTile;
        }
        else
        {
            return waterTile;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
