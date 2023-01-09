using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private Rigidbody _rig;
    private List<Collider> _colliders;

    private bool _collided;
    private bool _sticked;
    public float Length => 1.5f;
    public bool Reflected => _reflectedTimer >= 0;

    private bool _touchedFloor;
    private float _reflectedTimer;

    public bool Fake { get; set; }
    
    private void Awake()
    {
        _rig = GetComponent<Rigidbody>();
        _colliders = GetComponentsInChildren<Collider>().ToList();
    }

    private void Start()
    {
        if (Registry.Instance != null) Registry.Instance.Add(this);
    }

    private void Update()
    {
        _reflectedTimer -= Time.deltaTime;
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

        if (Checks.IsGround(collision.collider) && !_touchedFloor && !_sticked)
        {
            _touchedFloor = true;
            
            Destroy(gameObject, 3f);
        }
    }

    public void StickTo(Rigidbody target, float adjustMod = 1f)
    {
        _collided = true;
        _sticked = true;
        
        target.AddForce(_rig.velocity, ForceMode.Impulse);

        Destroy(_rig);
        
        foreach (var col in _colliders)
        {
            col.enabled = false;
        }

        var dir = target.position - transform.position;

        transform.position += dir * (0.5f * adjustMod);
        
        transform.position += transform.right * 0.3f;

        transform.SetParent(target.transform);
    }

    public void ShrinkDestroy(float delay)
    {
        transform.DOScale(Vector3.one * 0.1f, 0.35f).OnComplete(() => Destroy(gameObject)).SetDelay(delay);
    }
    
    public void DisableColliders()
    {
        Fake = true;
        foreach (var col in _colliders)
        {
            col.enabled = false;
        }

        _rig.isKinematic = true;
    }

    public void Reflect()
    {
        if (_rig != null)
        {
            _reflectedTimer = 1f;
            _rig.velocity = Vector3.zero;
        }
    }

    public void Discharge()
    {
        _collided = true;
        _touchedFloor = true;
        _rig.collisionDetectionMode = CollisionDetectionMode.Discrete;
            
        Destroy(gameObject, 3f);
    }
    
    private void OnDestroy()
    {
        if (Registry.Instance != null) Registry.Instance.Remove(this);
    }
}
