using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SpawnerScript : MonoBehaviour
{
    private float nextSpawnTime;

    public GameObject tilemap;

    [SerializeField]
    private GameObject truckPrefab;
    [SerializeField]
    private float spawnDelay = 10;
    private GameObject eventSystemRef;   // Track player's position to spawn entities nearby.
    private ButtonPressHandler bph;

    private float cellWidth;
    private float currRow;

    // Start is called before the first frame update
    void Start()
    {
        tilemap = GameObject.Find("Tilemap_Base");
        eventSystemRef = GameObject.Find("EventSystem");
        bph = GameObject.Find("EventSystem").GetComponent<ButtonPressHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        currRow = bph.playerRowPos;
        cellWidth = bph.hopLength;
        
        if( ShouldSpawn() )
        {
            Spawn();
        }
    }

    private void Spawn()
    {
        var spawnPos = new Vector3(0, cellWidth * .5f + cellWidth * currRow, -.5f);
        nextSpawnTime = Time.time + spawnDelay;
        GameObject truck = Instantiate(truckPrefab, this.transform);
        truck.transform.position = spawnPos;
        truck.transform.localScale = new Vector3(cellWidth, cellWidth, 1);
        truck.AddComponent<EntityMove>();
    }

    private bool ShouldSpawn()
    {
        return Time.time >= nextSpawnTime;
    }
}
