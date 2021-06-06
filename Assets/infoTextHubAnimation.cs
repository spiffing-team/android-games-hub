using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class infoTextHubAnimation : MonoBehaviour
{
    [SerializeField] private  float animationTime = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        var target = GetComponent<TextMeshProUGUI>();
        DOTweenTMPAnimator animator = new DOTweenTMPAnimator(target);
        Sequence sequence = DOTween.Sequence();
        for (int i = 0; i < animator.textInfo.characterCount; ++i) {
            if (!animator.textInfo.characterInfo[i].isVisible) continue;
            Vector3 currCharOffset = animator.GetCharOffset(i);
            sequence.Append(animator.DOOffsetChar(i, currCharOffset + new Vector3(0, 5, 0), animationTime));
            sequence.Join(animator.DOColorChar(i, Color.yellow, animationTime));
            sequence.Append(animator.DOOffsetChar(i, currCharOffset + new Vector3(0, -5, 0), animationTime));
            sequence.Join(animator.DOColorChar(i, Color.white, animationTime));



        }

        sequence.SetLoops(-1,LoopType.Yoyo);
    }


}
