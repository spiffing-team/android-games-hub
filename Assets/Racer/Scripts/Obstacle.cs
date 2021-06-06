using UnityEngine;

namespace Racer
{
    public class Obstacle : MonoBehaviour
    {
        private Rigidbody2D rb;

        public void SetSpeed(float speed)
        {
            if (!rb)
                rb = GetComponent<Rigidbody2D>();
            rb.velocity = Vector2.down * speed;
        }

        public void OnEnd(float speed)
        {
            rb.velocity = Vector2.up * speed;
        }
    }
}
