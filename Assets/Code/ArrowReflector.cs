using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowReflector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if ((other.CompareTag("Arrow") || other.CompareTag("ArrowTip")) && other.TryGetComponent<Arrow>(out var arrow))
        {
            arrow.Reflect();
        }
    }
}
