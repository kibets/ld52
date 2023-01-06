using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Temp : MonoBehaviour
{
    void Start()
    {
        transform.DOScale(1.1f, 1f).SetLoops(-1, LoopType.Yoyo);
    }
}
