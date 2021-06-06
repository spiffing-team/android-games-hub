using Sirenix.OdinInspector;
using UnityEngine;

namespace Racer
{
    public class RacerManager : GameBehaviour
    {
        private ScreenOrientation previousScreenOrientation;

        private static float startTime;

        void Start()
        {
            previousScreenOrientation = Screen.orientation;
            Screen.orientation = ScreenOrientation.Portrait;
            SetCorrectCameraSize();
        }

        [Button]
        public void StartGame()
        {
            SpeedController.Instance.StartGame();
            ObstackleSpawner.Instance.StartSpawning();
            startTime = Time.time;
        }

        protected override void OnGoBack()
        {
            Screen.orientation = previousScreenOrientation;
            base.OnGoBack();
        }

        public static void EndGame()
        {
            Debug.Log(Time.time - startTime);
            int points = (int)((Time.time - startTime) * 0.6f);
            PointsDatabase.SaveAdditively(PointsDatabase.Field.Racer, points);

            Player.Instance.OnEnd();
            ObstackleSpawner.Instance.OnEnd();
            Road.Instance.OnEnd();
            EndGameScreen.Instance.OnEnd(points);
        }

        public static void Restart()
        {
            SceneManager.RestartScene();
        }

        public void ToMenu()
        {
            OnGoBack();
        }

        private void SetCorrectCameraSize()
        {
            float calculatedSize = 5 * ((float) Screen.height / Screen.width);
            if (calculatedSize > 10)
            {
                Camera cam = PlayerCamera.Instance.gameObject.GetComponent<Camera>();
                cam.orthographicSize = calculatedSize;
            }
        }
    }
}
