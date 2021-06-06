using UnityEngine;

namespace Racer
{
    public class ObstackleDestroyer : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            Obstacle o = other.GetComponent<Obstacle>();
            ObstackleSpawner.Instance.DestroyObstacle(o);
        }
    }
}
