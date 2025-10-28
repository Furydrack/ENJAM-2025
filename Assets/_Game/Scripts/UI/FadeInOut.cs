using DG.Tweening;
using System.Collections;
using TriInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class FadeInOut : MonoBehaviour
{
    [Title("Refs")]
    [SerializeField]
    private Image _image;

    [Title("Settings")]
    [SerializeField]
    private float _inDuration;
    [SerializeField]
    private float _outDuration;
    [SerializeField]
    Ease _ease;

    [HideInInspector]
    public UnityEvent onFadeLaunched, onFadeInFinished, onFadeFinished;

    public void LaunchFade(bool fadeIn = true, bool fadeOut = true)
    {
        StartCoroutine(Fade(fadeIn, fadeOut));
    }

    IEnumerator Fade(bool fadeIn, bool fadeOut)
    {
        Color imageColor = _image.color;
        imageColor.a = 1f;
        Color invisibleImageColor = _image.color;
        invisibleImageColor.a = 0f;

        onFadeLaunched.Invoke();

        if(fadeIn)
        {
            _image.color = invisibleImageColor;
            _image.DOColor(imageColor, _inDuration).SetEase(_ease);
            yield return new WaitForSeconds(_inDuration);
        }

        onFadeInFinished.Invoke();

        if(fadeOut)
        {
            _image.color = imageColor;
            _image.DOColor(invisibleImageColor, _outDuration).SetEase(_ease);
            yield return new WaitForSeconds(_outDuration);
        }

        onFadeFinished.Invoke();
    }
}
