using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public enum EnemyState
{
    Idle,
    Agro
}

public class Enemy : MonoBehaviour
{
    [SerializeField] public float Speed;
    [SerializeField] public float Health;
    [SerializeField] public float ImpulseThreshold;

    [SerializeField] public float AgroRadius;
    [SerializeField] public float DeAgroRadius;

    private Rigidbody _rig;

    private bool _dead;

    private EnemyState _state;

    private Vector3 _myPos;
    private Vector3 _heroPos;
    private float _targetDist;
    private Vector3 _targetDir;
    
    private void Awake()
    {
        _rig = GetComponent<Rigidbody>();

        _state = EnemyState.Idle;
    }

    private void Update()
    {
        _myPos = transform.position;
        _heroPos = Hero.Instance.transform.position;
        
        _targetDist = Vector3.Distance(_heroPos, _myPos);
        _targetDir = _heroPos - _myPos;

    }


    private void FixedUpdate()
    {
        if (_dead) return;
        
        if (_state == EnemyState.Idle) FixedIdle();
        if (_state == EnemyState.Agro) FixedAgro();
    }
    
    private void FixedIdle()
    {
        if (_targetDist < AgroRadius) _state = EnemyState.Agro;
    }

    private void FixedAgro()
    {
        if (_targetDist > DeAgroRadius) _state = EnemyState.Idle;
        
        _rig.AddForce(_targetDir.normalized * (Speed * Time.fixedDeltaTime), ForceMode.VelocityChange);
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("ArrowTip") && collision.gameObject.TryGetComponent<Arrow>(out var arrow))
        {
            if (collision.contactCount > 0 && !arrow.Reflected && collision.impulse.magnitude > ImpulseThreshold)
            {
                var point = collision.GetContact(0);

                var effect = Prefabs.Instance.Produce("EnemyHitFx");
                effect.transform.SetParent(transform);
                effect.transform.position = point.point;
                effect.transform.rotation = arrow.transform.rotation;
            
                arrow.StickTo(_rig);

                Health -= 1;

                if (Health <= 0)
                {
                    Die();
                }
            }
        }
    }

    private void Die()
    {
        if (_dead) return;
        
        _dead = true;

        _rig.useGravity = true;
        _rig.drag = 0.1f;
        
        var mat = GetComponentInChildren<MeshRenderer>().materials[0];
        mat.DOColor(Color.gray, "_EmissionColor", 0.34f);
        mat.DOColor(Color.gray, "_BaseColor", 0.34f);
        mat.DOColor(Color.gray, "_Color", 0.34f);

        transform.DOScale(Vector3.one * 0.1f, 0.35f).OnComplete(() => Destroy(gameObject)).SetDelay(2.5f);
    }
}
