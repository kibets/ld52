using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class Apple : MonoBehaviour
{
    [SerializeField] private AppleJoint parentJoint;
    [SerializeField] private SphereCollider mainCollider;
    
    [SerializeField] private Transform greenArt;
    [SerializeField] private Transform redArt;
    [SerializeField] private Transform ripeArt;
    
    [SerializeField] private float greenTime;
    [SerializeField] private float redTime;
    [SerializeField] private float ripeTime;

    [SerializeField] private float greenColliderRad;
    [SerializeField] private float redColliderRad;
    [SerializeField] private float ripeColliderRad;
    
    [SerializeField] private float floorTime;
    
    private Rigidbody _rig;

    private float _ripenTimer;

    private bool _stageRed;
    private bool _stageBad;

    private bool _damaged;
    private bool _jointBreak;

    private bool _touchedFloor;
    
    private float _floorTimer;
    private Coroutine _blinkRoutine;

    private void Awake()
    {
        greenTime += greenTime * Random.value * 0.2f;
        redTime += redTime * Random.value * 0.2f;
        
        _rig = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        ripeArt.localScale = Vector3.one * 0.1f;
        redArt.localScale = Vector3.one * 0.1f;
        mainCollider.radius = greenColliderRad;
    }

    private void Update()
    {
        UpdateJointStatus();

        UpdateRipeProcess();

        UpdateFloorTime();
    }

    private void UpdateFloorTime()
    {
        if (_floorTimer > 0)
        {
            _floorTimer -= Time.deltaTime;

            if (_floorTimer <= 0)
            {
                _blinkRoutine = StartCoroutine(DoBlinkRoutine());
            }
        }
    }

    private IEnumerator DoBlinkRoutine()
    {
        var renderers = GetComponentsInChildren<MeshRenderer>();

        for (int i = 0; i < 20; i++)
        {
            foreach (var rend in renderers)
            {
                if(rend != null) rend.enabled = false;
            }
            yield return new WaitForSeconds(0.07f);
            
            foreach (var rend in renderers)
                if (rend != null)
                {
                    rend.enabled = true;
                }
            yield return new WaitForSeconds(0.07f);
        }
        Destroy(gameObject);
    }

    private void UpdateJointStatus()
    {
        if (_jointBreak) return;
        if (parentJoint == null)
        {
            _jointBreak = true;
        }
        
        if (parentJoint.MainJoint == null || parentJoint.MainJoint.connectedBody == null)
        {
            _jointBreak = true;

            transform.SetParent(null);
            
            Destroy(parentJoint.gameObject);
            parentJoint = null;
        }
    }
    
    private void UpdateRipeProcess()
    {
        if (_jointBreak || _damaged) return;
        
        _ripenTimer += Time.deltaTime;

        if (_ripenTimer > greenTime && !_stageRed)
        {
            _stageRed = true;
            redArt.DOScale(1f, 0.17f);
            greenArt.DOScale(0.1f, 0.17f);
            mainCollider.radius = redColliderRad;
        }

        if (_ripenTimer > greenTime + redTime && !_stageBad)
        {
            _stageBad = true;
            
            ripeArt.DOScale(1f, 0.17f);
            redArt.DOScale(0.1f, 0.17f);
            greenArt.DOScale(0.1f, 0.17f);
            mainCollider.radius = ripeColliderRad;
        }

        if (_ripenTimer > greenTime + redTime + ripeTime)
        {
            if (parentJoint != null)
            {
                Destroy(parentJoint.gameObject);
            }
            Destroy(transform.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Floor") && !_touchedFloor)
        {
            _touchedFloor = true;
            _floorTimer = floorTime;
        }
        
        if (collision.collider.CompareTag("ArrowTip") && collision.gameObject.TryGetComponent<Arrow>(out var arrow))
        {
            if (collision.contactCount > 0)
            {
                var point = collision.GetContact(0);

                var effect = Prefabs.Instance.Produce("AppleHitFx");
                effect.transform.SetParent(transform);
                effect.transform.position = point.point;
                effect.transform.rotation = arrow.transform.rotation;
            }
            
            _damaged = true;
            
            arrow.StickTo(_rig);
        }
    }
}
