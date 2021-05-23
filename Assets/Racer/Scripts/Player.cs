using Sirenix.OdinInspector;
using UnityEngine;

namespace Racer
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Player : Singleton<Player>
    {
        [SerializeField] private float speed = 5f;
        [SerializeField] private int inputMaxAngle = 45;
        [SerializeField] private int playerMaxAngle = 30;
        [SerializeField] private float sideSpeed = 5f;
        [SerializeField] private float roadWidth = 4f;

        public float Speed
        {
            get => speed;
            private set
            {
                speed = value;
                Road.Instance.RecalculateSpeed(speed);
            }
        }

        private new Transform transform;
        private Vector3 rotation;
        private float position;
        private bool useGyro = true;

        private void Start()
        {
            transform = GetComponent<Transform>();
            Input.gyro.enabled = true;
            rotation = transform.rotation.eulerAngles;
            position = transform.position.x;
            Speed = speed;
        }

        private void Update()
        {
            Speed = speed;
            if (useGyro)
            {
                float tilt = GetGyroTilt();
            
                rotation.z = -tilt * Mathf.PI * (playerMaxAngle / 180f);
                transform.rotation = Quaternion.Euler(rotation);
            
                position = Mathf.Clamp(position + tilt * sideSpeed * Time.deltaTime, -roadWidth, roadWidth);
            }
            
            transform.position = new Vector3(position, 0);
        }

        private float GetGyroTilt()
        {
            return Mathf.Clamp(Input.gyro.gravity.x * (90f / inputMaxAngle), -1, 1);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            Input.gyro.enabled = false;
        }

        [Button]
        public void ChangeSpeed(float amount)
        {
            speed += amount;
            Road.Instance.RecalculateSpeed(speed);
            PlayerCamera.Instance.SimulateSpeed(amount);
        }
    }
}
