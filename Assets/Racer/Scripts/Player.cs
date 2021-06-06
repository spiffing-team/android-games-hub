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

            float tilt = GetGyroTilt();

            rotation.z = -tilt * playerMaxAngle;
            transform.rotation = Quaternion.Euler(rotation);

            position = Mathf.Clamp(position + tilt * sideSpeed * Time.deltaTime, -roadWidth, roadWidth);

            transform.position = new Vector3(position, 0);
        }

        private float GetGyroTilt()
        {
            return useGyro ?
                Mathf.Clamp(Input.gyro.gravity.x * (90f / inputMaxAngle), -1, 1) :
                Input.GetAxis("Horizontal");
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
            Vector2 velocity = ((Vector2)transform.up + (Vector2.right * GetGyroTilt())) * speed;
            rb.velocity = velocity;
        }
    }
}
