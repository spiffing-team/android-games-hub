using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Racer
{
    public class ObstackleSpawner : Singleton<ObstackleSpawner>
    {
        [Required]
        [SerializeField] private Obstacle obstacklePrefab = default;

        [BoxGroup("Distances")]
        [SerializeField] private float obstackleLength = 1f;
        [BoxGroup("Distances")]
        [SerializeField] private float closeDistance = 3f;
        [BoxGroup("Distances")]
        [SerializeField] private float largeDistance = 6f;

        [BoxGroup("Columns")]
        [MinValue(1)]
        [SerializeField] private int columns = 4;
        [BoxGroup("Columns")]
        [MinValue(0.01f)]
        [SerializeField] private float columnWidth = 3f;

        private float speed;

        private float nextObstackleHeight;
        private float[] columnPositionsX;
        private float initialPosY;

        private bool spawning = false;
        private Transform obstaclePoolTransform;
        private readonly List<Obstacle> obstaclePool = new List<Obstacle>();
        private readonly List<Obstacle> activeObstacles = new List<Obstacle>();

        private void Start()
        {
            Vector3 position = transform.position;
            initialPosY = position.y;
            nextObstackleHeight = position.y;

            CreateLines();
            InitializePool();
        }

        public void StartSpawning()
        {
            spawning = true;
        }

        private void Update()
        {
            if (!spawning) return;

            nextObstackleHeight -= speed * Time.deltaTime;
            if (nextObstackleHeight < initialPosY)
                SelectPatternToSpawn();
        }

        private void CreateLines()
        {
            columnPositionsX = new float[columns];
            float firstColumnX = columnWidth * (columns - 1) / -2;
            for (int i = 0; i < columns; i++)
            {
                float xPos = firstColumnX + i * columnWidth;
                columnPositionsX[i] = xPos;
            }
        }

        private void InitializePool()
        {
            if (!obstaclePoolTransform)
            {
                obstaclePoolTransform = new GameObject("Obstacles Pool").transform;
                obstaclePoolTransform.parent = transform;
            }

            int spawnCount = 10 - obstaclePool.Count;
            for (int i = 0; i < spawnCount; i++)
            {
                Obstacle obstacle = Instantiate(obstacklePrefab, obstaclePoolTransform);
                obstacle.gameObject.SetActive(false);
                obstacle.transform.parent = obstaclePoolTransform;
                obstaclePool.Add(obstacle);
            }
        }

        public void DestroyObstacle(Obstacle obstacle)
        {
            obstacle.gameObject.SetActive(false);
            activeObstacles.Remove(obstacle);
            obstaclePool.Add(obstacle);
            obstacle.transform.SetParent(obstaclePoolTransform);
        }

        private void CreateObstacle(int column)
        {
            Obstacle obstacle;
            if (obstaclePool.Count > 0)
            {
                obstacle = obstaclePool[0];
                obstaclePool.RemoveAt(0);
            }
            else
            {
                obstacle = Instantiate(obstacklePrefab);
            }
            activeObstacles.Add(obstacle);
            obstacle.gameObject.SetActive(true);
            var obstacleTransform = obstacle.transform;
            obstacleTransform.position = new Vector3(columnPositionsX[column], nextObstackleHeight);
            obstacleTransform.parent = transform;
            obstacle.SetSpeed(speed);
        }


        public void ChangeSpeed(float newSpeed)
        {
            speed = newSpeed;
            foreach (Obstacle obstacle in activeObstacles)
            {
                obstacle.SetSpeed(newSpeed);
            }
        }

        public void OnEnd()
        {
            foreach (Obstacle obstacle in activeObstacles)
            {
                obstacle.OnEnd(speed);
            }

            spawning = false;
        }

        private List<int> CreateLine(IList<int> availableColumns, int obstacleCount, float spacing = 0)
        {
            if (spacing <= 0)
                spacing = largeDistance;

            if (availableColumns.Count < obstacleCount)
            {
                //Debug.LogWarning("Requested obstackle count is lower than available columns");
                obstacleCount = availableColumns.Count;
            }


            var freeColumns = new bool[columns];
            for (int i = 0; i < columns; i++)
            {
                freeColumns[i] = true;
            }

            for (int i = 0; i < obstacleCount; i++)
            {
                int column = availableColumns[Random.Range(0, availableColumns.Count)];
                availableColumns.Remove(column);
                CreateObstacle(column);
                freeColumns[column] = false;
            }

            nextObstackleHeight += spacing;

            var nextAvailableColumns = new List<int>();
            for (int i = 0; i < columns; i++)
            {
                if ((i > 0 && freeColumns[i - 1]) ||
                    freeColumns[i] ||
                    (i < columns - 1 && freeColumns[i + 1]))
                    nextAvailableColumns.Add(i);
            }

            return nextAvailableColumns;
        }

        [NotNull]
        private List<int> GetAllColumns()
        {
            List<int> ret = new List<int>(columns);
            for (int i = 0; i < columns; i++)
            {
                ret.Add(i);
            }
            return ret;
        }


        private void SelectPatternToSpawn()
        {
            int rand = Random.Range(0, 5);
            if (rand == 0)
                SpawnSingleLine(Random.Range(3, 6));
            else if (rand == 1)
                SpawnSimplePath(Random.Range(3, 5));
            else
                CreateLine(GetAllColumns(), Random.Range(2, 4), largeDistance);
        }

        private void SpawnSimplePath(int length)
        {
            var availableColumns = GetAllColumns();
            int selectedLine = Random.Range(0, columns);
            var copy = availableColumns.ToList();
            copy.Remove(selectedLine);
            availableColumns = CreateLine(copy, 3, closeDistance);
            for (int i = 0; i < length; i++)
            {
                selectedLine = Random.Range(0, 2) == 0 ?
                    Mathf.Clamp(selectedLine - 1, 0, selectedLine) :
                    Mathf.Clamp(selectedLine + 1, selectedLine, columns - 1);

                copy = availableColumns.ToList();
                copy.Remove(selectedLine);
                availableColumns = CreateLine(copy, Random.Range(2, columns), closeDistance);
            }

            nextObstackleHeight += largeDistance - closeDistance;
        }

        private void SpawnSingleLine(int length)
        {
            int selectedFreeLine = Random.Range(0, columns);
            var otherColumns = GetAllColumns();
            otherColumns.Remove(selectedFreeLine);

            for (int i = 0; i < length; i++)
            {
                CreateLine(otherColumns.ToList(), Random.Range(2, columns), obstackleLength);
            }

            nextObstackleHeight += largeDistance - obstackleLength;
        }
    }
}
