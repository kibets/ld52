using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Logo : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(LogoRoutine());
    }

    private IEnumerator LogoRoutine()
    {
        yield return new WaitForSeconds(5f);
        
        transform.DOLocalMoveY(20f, 0.79f).SetEase(Ease.InBack);
        Sounds.Instance.PlayExact("ui_swing_1");

    }
}
