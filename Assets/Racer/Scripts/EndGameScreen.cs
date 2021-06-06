using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Racer
{
    public class EndGameScreen : Singleton<EndGameScreen>
    {
        [SerializeField] private TMP_Text earnedPointsText;
        [SerializeField] private TMP_Text totalPointsText;

        public void OnEnd(int points)
        {
            transform.DOMove(Vector3.zero, 4f).SetEase(Ease.InQuint).Play();
            earnedPointsText.text = points.ToString();
            totalPointsText.text = PointsDatabase.Load(PointsDatabase.Field.Racer).ToString();
        }
    }
}
