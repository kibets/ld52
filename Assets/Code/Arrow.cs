using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private Rigidbody _rig;
    private List<Collider> _colliders;

    private bool _collided;
    private bool _sticked;
    public float Length => 1.5f;

    private bool _touchedFloor;

    private void Awake()
    {
        _rig = GetComponent<Rigidbody>();
        _colliders = GetComponentsInChildren<Collider>().ToList();
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

        if (collision.collider.CompareTag("Floor") && !_touchedFloor && !_sticked)
        {
            _touchedFloor = true;
            
            Destroy(gameObject, 3f);
        }
    }

    public void StickTo(Rigidbody target)
    {
        _collided = true;
        _sticked = true;
        
        target.AddForce(_rig.velocity, ForceMode.Impulse);
        
        Destroy(_rig);

        var dir = target.position - transform.position;

        transform.position += dir * 0.5f;
        
        transform.position += transform.right * 0.3f;

        transform.SetParent(target.transform);
    }

    public void DisableColliders()
    {
        foreach (var col in _colliders)
        {
            col.enabled = false;
        }

        _rig.isKinematic = true;
    }
}
