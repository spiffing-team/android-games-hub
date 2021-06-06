using Sirenix.OdinInspector;
using UnityEngine;

namespace Racer
{
    public class SpeedController : Singleton<SpeedController>
    {
        [SerializeField] private float initialSpeed = 2f;
        [SerializeField] private float startSpeed = 4f;

        private static float currentSpeed;

        private void Start()
        {
            SetNewSpeed(initialSpeed);
        }

        public void StartGame()
        {
            SetNewSpeed(startSpeed, false);
        }

        [Button]

        public static void ChangeSpeed(float amount)
        {
            float newSpeed = currentSpeed + amount;
            Player.Instance.ChangeSpeed(newSpeed);
            ObstackleSpawner.Instance.ChangeSpeed(newSpeed);
            Road.Instance.RecalculateSpeed(newSpeed);
        }
        public static void SetNewSpeed(float newSpeed, bool affectCamera = true)
        {
            Player.Instance.ChangeSpeed(newSpeed, affectCamera);
            ObstackleSpawner.Instance.ChangeSpeed(newSpeed);
            Road.Instance.RecalculateSpeed(newSpeed);
            currentSpeed = newSpeed;
        }
    }
}
