using UnityEngine;

namespace Racer
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Player : Singleton<Player>
    {
        private float speed = 5f;
        [SerializeField] private int inputMaxAngle = 45;
        [SerializeField] private int playerMaxAngle = 30;
        [SerializeField] private float sideSpeed = 5f;
        [SerializeField] private float roadWidth = 4f;

        private new Transform transform;
        private Vector3 rotation;
        private float position;
        [SerializeField] private bool useGyro = true;
        private bool moving = true;

        private void Start()
        {
            transform = GetComponent<Transform>();
            Input.gyro.enabled = true;
            rotation = transform.rotation.eulerAngles;
            position = transform.position.x;
        }

        private void Update()
        {
            if (!moving) return;

            if (useGyro)
            {
                float tilt = GetGyroTilt();

                rotation.z = -tilt * Mathf.PI * (playerMaxAngle / 180f);
                transform.rotation = Quaternion.Euler(rotation);

                position = Mathf.Clamp(position + tilt * sideSpeed * Time.deltaTime, -roadWidth, roadWidth);
            }
            else
            {
                float input = Input.GetAxis("Horizontal");

                position = Mathf.Clamp(position + input * sideSpeed * Time.deltaTime, -roadWidth, roadWidth);
            }

            transform.position = new Vector3(position, 0);
        }

        private float GetGyroTilt()
        {
            return Mathf.Clamp(Input.gyro.gravity.x * (90f / inputMaxAngle), -1, 1);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            RacerManager.EndGame();
        }

        public void ChangeSpeed(float amount, bool affectCamera = true)
        {
            float dif = amount - speed;
            speed = amount;
            if (affectCamera)
                PlayerCamera.Instance.SimulateSpeed(dif);
        }

        public void OnEnd()
        {
            moving = false;
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Dynamic;
            Vector2 velocity = transform.up * speed;
            rb.velocity = velocity;
        }
    }
}
