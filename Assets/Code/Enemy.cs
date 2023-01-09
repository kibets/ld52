using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public enum EnemyState
{
    Idle,
    Agro,
    BiteCooldown
}

public class Enemy : MonoBehaviour
{
    [SerializeField] public float Speed;
    [SerializeField] public float Health;
    [SerializeField] public float ImpulseThreshold;

    [SerializeField] public float AgroRadius;
    [SerializeField] public float DeAgroRadius;

    private float BiteCooldownTime = 1.7f;
    
    private Rigidbody _rig;

    private bool _dead;

    private EnemyState _state;

    private Vector3 _myPos;
    private Vector3 _heroPos;
    private float _targetDist;
    private Vector3 _targetDir;

    private float _biteTimer;

    private List<Transform> _arrows = new();
    
    private void Awake()
    {
        _rig = GetComponent<Rigidbody>();

        _state = EnemyState.Idle;
    }

    private void Start()
    {
        if (Registry.Instance != null) Registry.Instance.Add(this);
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
        if (_state == EnemyState.BiteCooldown) FixedBiteCooldown();
    }

    private void FixedBiteCooldown()
    {
        _biteTimer -= Time.fixedDeltaTime;

        if (_biteTimer <= 0) _state = EnemyState.Idle;
    }

    private void FixedIdle()
    {
        if (_targetDist < AgroRadius && !Hero.Instance.Dead) _state = EnemyState.Agro;
    }

    private void FixedAgro()
    {
        if (_targetDist > DeAgroRadius || Hero.Instance.Dead) _state = EnemyState.Idle;
        
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

                _arrows.Add(arrow.transform);
                
                Health -= 1;

                if (Health <= 0)
                {
                    Die();
                }
                else
                {
                    Sounds.Instance.PlayRandom(transform.position, "hit_a");
                }
            }
        }
        
        if (collision.gameObject.CompareTag("Hero") && collision.gameObject.TryGetComponent<Hero>(out var hero))
        {
            _state = EnemyState.BiteCooldown;
            _biteTimer = BiteCooldownTime;

            hero.Bite();
        }
    }

    public void Die()
    {
        if (_dead) return;
        
        _dead = true;
        Health = 0;
        
        _rig.useGravity = true;
        _rig.drag = 0.1f;
        

        var fractured = Prefabs.Instance.Produce<FracturedSphere>();
        fractured.transform.position = transform.position;
        
        fractured.Setup(_rig.velocity);

        foreach (var oldArrow in _arrows)
        {
            var arrow = Prefabs.Instance.Produce<Arrow>();
            arrow.transform.SetPositionAndRotation(oldArrow.position, oldArrow.rotation);
            arrow.Discharge();
        }

        var core = Prefabs.Instance.Produce("ApplePurple");
        core.transform.position = transform.position;
        
        
        Sounds.Instance.PlayExact("snap_a_1");
        
        Destroy(gameObject);
        
    }

    private void OnDestroy()
    {
        if (Registry.Instance != null) Registry.Instance.Remove(this);
    }
}
