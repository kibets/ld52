using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple : MonoBehaviour
{
    private Rigidbody _rig;

    private void Awake()
    {
        _rig = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        _rig.useGravity = true;

        if (collision.collider.CompareTag("ArrowTip") && collision.gameObject.TryGetComponent<Arrow>(out var arrow))
        {
            arrow.StickTo(transform);
        }
    }
}
