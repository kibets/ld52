using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private Rigidbody _rig;

    private bool _collided;
    private bool _sticked;
    
    private void Awake()
    {
        _rig = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (_sticked) return;

        if (_rig.velocity.magnitude > 3f && !_collided)
        {
            transform.right = Vector3.Slerp(transform.right, _rig.velocity.normalized, Time.fixedDeltaTime * 10f);
        }
    }

    public void ShootForward(float force)
    {
        _rig.AddForce(transform.right * force, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        _collided = true;
    }

    public void StickTo(Transform target)
    {
        _collided = true;
        _sticked = true;
        
        Destroy(_rig);


        var dir = target.position - transform.position;

        transform.position += dir * 0.5f;
        
        transform.position += transform.right * 0.3f;

        transform.SetParent(target);
    }
}
