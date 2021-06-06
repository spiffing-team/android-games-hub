using UnityEngine;

namespace Racer
{
    public class PlayerCamera : Singleton<PlayerCamera>
    {
        [SerializeField] private float smoothness = 5f;
        [SerializeField] private float speedBoostMagnifier = 2f;
        
        private new Transform transform;
        private Vector3 startPosition;
        private Vector3 velocity;

        protected override void OnAwake()
        {
            transform = GetComponent<Transform>();
            startPosition = transform.position;
        }

        private void LateUpdate()
        {
            transform.position = Vector3.SmoothDamp(transform.position, startPosition, ref velocity, smoothness);
        }
        
        public void SimulateSpeed(float amount)
        {
            velocity += amount * speedBoostMagnifier * Vector3.down;
        }
    }
}