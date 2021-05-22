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

    // Start is called before the first frame update
    void Start()
    {
        tilemap = GameObject.Find("Tilemap_Base");
    }

    // Update is called once per frame
    void Update()
    {
        if( ShouldSpawn() )
        {
            Spawn();
        }
    }

    private void Spawn()
    {
        tilemap.GetComponent<Tilemap>().CompressBounds();
        Vector3 size = tilemap.GetComponent<Tilemap>().size;
        nextSpawnTime = Time.time + spawnDelay;
        Instantiate(truckPrefab, tilemap.transform);
    }

    private bool ShouldSpawn()
    {
        return Time.time >= nextSpawnTime;
    }
}
