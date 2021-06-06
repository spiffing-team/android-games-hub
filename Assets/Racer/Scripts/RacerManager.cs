using UnityEngine;

namespace Racer
{
    public class RacerManager : GameBehaviour
    {
        private ScreenOrientation previousScreenOrientation;

        void Start()
        {
            previousScreenOrientation = Screen.orientation;
            Screen.orientation = ScreenOrientation.Portrait;
            SetCorrectCameraSize();
        }

        protected override void OnGoBack()
        {
            Screen.orientation = previousScreenOrientation;
            base.OnGoBack();
        }

        protected override void Update()
        {
            base.Update();
            var points = PointsDatabase.Load(PointsDatabase.Field.Racer);
            PointsDatabase.Save(PointsDatabase.Field.Racer, points + 1);
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
