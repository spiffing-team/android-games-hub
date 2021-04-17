using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class IntroManager : MonoBehaviour
{
    private AudioSource audio;
    [SerializeField] private TextMeshProUGUI intro;

    private void Awake()
    {
        audio = transform.GetComponent<AudioSource>();
    }

    internal void PlayIntro()
    {
        audio.Play();
        intro.DOFade(0.0f, 0f);
        
        var sequence = DOTween.Sequence();
        sequence.Append(intro.DOFade(1.0f, 1f));
        sequence.AppendInterval(3f);
        sequence.Append(intro.DOFade(0.0f, 1f));
        sequence.AppendInterval(0.7f);
        sequence.Append(intro.DOFade(1.0f, 1f));
        sequence.AppendInterval(3f);
        sequence.Append(intro.DOFade(0.0f, 1f));

    }
}
