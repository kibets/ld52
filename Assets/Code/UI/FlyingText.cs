using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class FlyingText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI label;
    
    public void SetText(string text)
    {
        label.SetText(text);
    }

    public void FlyAnimation(string text, Color color, float duration = 0.6f)
    {
        label.color = color;

        label.SetText(text);

        label.rectTransform.DOAnchorPos(Vector2.up * 1, duration).SetEase(Ease.OutQuart);
        label.DOColor(Color.clear, duration * 0.5f).SetDelay(duration).OnComplete(() =>
        {
            Destroy(gameObject);
        });
    }
}
