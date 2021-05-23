using UnityEngine;

namespace Racer
{
    public class RacerManager : MonoBehaviour
    {
        void Start()
        {
            Screen.orientation = ScreenOrientation.Portrait;
            SetCorrectCameraSize();
        }

        public void Close()
        {
            Screen.orientation = ScreenOrientation.AutoRotation;
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
