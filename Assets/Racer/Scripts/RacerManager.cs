using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Racer
{
    public class RacerManager : GameBehaviour
    {
        [SerializeField] private RectTransform endGameScreen;

        private ScreenOrientation previousScreenOrientation;

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
        }

        protected override void OnGoBack()
        {
            Screen.orientation = previousScreenOrientation;
            base.OnGoBack();
        }

        public static void EndGame()
        {
            Player.Instance.OnEnd();
            ObstackleSpawner.Instance.OnEnd();
            Road.Instance.OnEnd();
            EndGameScreen.Instance.OnEnd();

            var points = PointsDatabase.Load(PointsDatabase.Field.Racer);
            PointsDatabase.Save(PointsDatabase.Field.Racer, points + 1);
        }

        public static void Restart()
        {
            SceneManager.RestartScene();
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
