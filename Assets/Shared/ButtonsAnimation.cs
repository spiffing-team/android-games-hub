using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ButtonsAnimation : MonoBehaviour
{
    [SerializeField] private Transform[] buttons;

    // Start is called before the first frame update
    void Start()
    {
        var sequence = DOTween.Sequence();
        foreach (var button in buttons)
        {
            sequence.Append(button.DOScale(1.1f, 1.0f));
            sequence.Append(button.DOScale(1.0f, 1.0f));
        }

        sequence.SetLoops(-1);
    }
}
