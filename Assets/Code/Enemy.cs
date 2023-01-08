using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] public float Speed;
    
    private Rigidbody _rig;

    private void Awake()
    {
        _rig = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        var dir = Hero.Instance.transform.position - transform.position;
        
        _rig.AddForce(dir.normalized * (Speed * Time.fixedDeltaTime));
    }
}
