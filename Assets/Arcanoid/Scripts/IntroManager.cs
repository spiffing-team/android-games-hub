using DG.Tweening;
using TMPro;
using UnityEngine;

public class IntroManager :  MonoBehaviour, IView
{
    private AudioSource audio;
    [SerializeField] private TextMeshProUGUI intro;
    [SerializeField] private GameObject mainUI;
    
    public event EventHandler OnEnd;

    public void Show()
    {
        gameObject.SetActive(true);
        audio = transform.GetComponent<AudioSource>();
        PlayIntro();
    }
    

    private void PlayIntro()
    {
        intro.text = "Arkanoid";
        
        audio.Play();
        intro.DOFade(0.0f, 0f);
        
        var sequence = DOTween.Sequence();
        sequence.Append(intro.DOFade(1.0f, 1f));
        sequence.AppendInterval(3f);
        sequence.Append(intro.DOFade(0.0f, 1f));

        sequence.AppendCallback(() => { intro.text = "Pong";});
        
        sequence.AppendInterval(0.7f);
        sequence.Append(intro.DOFade(1.0f, 1f));
        sequence.AppendInterval(3f);
        sequence.Append(intro.DOFade(0.0f, 1f));
        sequence.AppendCallback(Hide);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        OnEnd?.Invoke();
    }
}
