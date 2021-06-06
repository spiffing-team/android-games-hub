using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SpawnerScript : MonoBehaviour
{
    public GameObject tilemap;

    [SerializeField]
    private GameObject truckPrefab;
    [SerializeField]
    private GameObject logPrefab;
    [SerializeField]
    private float spawnDelay = 6;   // Measured in seconds.
    private float nextSpawnTime = 0;
    private float entitySpeed = 1.5f;

    private GameObject eventSystemRef;   // Track player's position to spawn entities nearby.
    private ButtonPressHandler bph;

    private float cellWidth;
    private int currRow = 1;
    private float rows = 21;

    public struct Row
    {
        public int number;
        public char type;  // [a]sphalt/[w]ater

        public Row(int number, char type)
        {
            this.number = number;
            this.type = type;
        }
    }

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
        
        if ( ShouldSpawn() )
        {
            Spawn();
        }
    }

    private void Spawn()
    {
        // Reset the spawn timer.
        nextSpawnTime = Time.time + Random.Range(5, spawnDelay);
        // Select eligible rows from the nearest ones (road or water).
        List<Row> goodRows = new List<Row>();
        for (int r = currRow; r < currRow + 3 && r < rows; r++)
        {
            if ((r % 10 >= 0 && r % 10 < 2) || (r % 10 == 5))
            {
                continue;   // Grass row.
            }
            else if (r % 10 >= 2 && r % 10 < 5)
            {
                goodRows.Add(new Row(r, 'a'));
            }
            else
            {
                goodRows.Add(new Row(r, 'w'));
            }
        }
        if(goodRows.Count == 0)
        {
            return;
        }
        // Randomly choose one of them.
        int randInt = Random.Range(0, goodRows.Count - 1);
        Row randomRow = goodRows[randInt];
        goodRows.RemoveAt(randInt);
        // For variety, trucks will come from the left and logs from the right.
        var spawnPos = new Vector3(0, cellWidth * .5f + cellWidth * randomRow.number, -.4f);
        var displacement = entitySpeed * cellWidth;
        GameObject entity, entity2 = null;
        if (randomRow.type == 'w')
        {
            spawnPos.x = Screen.width;
            displacement = displacement * (-1);
            // Spawn the entity and adjust its properties.
            entity = Instantiate(logPrefab, this.transform);
            entity.transform.position = spawnPos;
            if (goodRows.Count > 0)
            {
                randomRow = goodRows[Random.Range(0, goodRows.Count - 1)];
                spawnPos = new Vector3(Screen.width, cellWidth * .5f + cellWidth * randomRow.number, -.4f);
                entity2 = Instantiate(logPrefab, this.transform);
                entity2.transform.position = spawnPos;
            }
        }
        else
        {
            entity = Instantiate(truckPrefab, this.transform);
            entity.transform.position = spawnPos;
        }

   
        entity.transform.localScale = new Vector3(cellWidth, cellWidth, 1);
        EntityMove em = entity.AddComponent<EntityMove>();
        em.velocity = displacement;
        if(entity2 != null)
        {
            entity2.transform.localScale = new Vector3(cellWidth, cellWidth, 1);
            EntityMove em2 = entity2.AddComponent<EntityMove>();
            em2.velocity = displacement;
        }

    }

    private bool ShouldSpawn()
    {
        return Time.time >= nextSpawnTime;
    }
}
