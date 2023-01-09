using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private string keyCode;
    [SerializeField] private Transform keyHole;
    private bool _opened;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Key") && other.TryGetComponent<Key>(out var key))
        {
            if (key.Code == keyCode && !_opened)
            {
                _opened = true;
                
                key.transform.SetParent(keyHole);
                key.transform.DOLocalMove(Vector3.zero, 0.9f).OnComplete(OpenDoor);
                
            }
        }
    }

    private void OpenDoor()
    {
        transform.DOMoveY(50f, 0.42f).SetEase(Ease.InOutSine).SetDelay(0.7f);
    }
}
