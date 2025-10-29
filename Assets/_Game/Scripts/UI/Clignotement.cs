using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Clignotement : MonoBehaviour
{
    [SerializeField]
    private float _duration = 0.5f;
    
    private Image _image;
    private Coroutine _winkCoroutine;

    private Color _startColor, _fadeColor;

    private void Start()
    {
        _image = GetComponent<Image>();
        _startColor = _image.color;
        _fadeColor = _image.color;
        _fadeColor.a = 0f;
    }

    public void OnValueChanged(bool enabled)
    {
        _image.DOKill();
        if (_winkCoroutine != null) StopCoroutine(_winkCoroutine);

        _winkCoroutine = StartCoroutine(Wink());
    }

    private IEnumerator Wink()
    {
        while(true)
        {
            Color targetColor = _image.color == _startColor ? _fadeColor : _startColor;
            _image.DOColor(targetColor, _duration);
            yield return new WaitForSeconds(_duration);
        }
    }
}
