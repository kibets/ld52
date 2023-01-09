using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Transform container;
    [SerializeField] private string keyCode;
    [SerializeField] private Transform keyHole;
    private bool _opened;

    private void Start()
    {
        // OpenDoor();
    }

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
        container.transform.DOLocalMoveY(15f, 1.17f).SetEase(Ease.InOutSine).SetDelay(0.7f);
    }
}
