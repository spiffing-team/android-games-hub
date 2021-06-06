using DG.Tweening;
using UnityEngine;

namespace Racer
{
    public class EndGameScreen : Singleton<EndGameScreen>
    {
        public void OnEnd()
        {
            transform.DOMove(Vector3.zero, 4f);
        }
    }
}
