using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class Apple : MonoBehaviour
{
    [SerializeField] private AppleJoint parentJoint;

    [SerializeField] private Transform greenArt;
    [SerializeField] private Transform redArt;
    [SerializeField] private Transform badArt;
    
    [SerializeField] private float greenTime;
    [SerializeField] private float redTime;
    [SerializeField] private float badTime;
    
    private Rigidbody _rig;

    private float _ripenTimer;

    private bool _stageRed;
    private bool _stageBad;

    private bool _damaged;
    private bool _jointBreak;
    
    private void Awake()
    {
        greenTime += greenTime * Random.value * 0.2f;
        redTime += redTime * Random.value * 0.2f;
        
        _rig = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        badArt.localScale = Vector3.one * 0.1f;
        redArt.localScale = Vector3.one * 0.1f;
    }

    private void Update()
    {
        UpdateJointStatus();

        UpdateRipeProcess();
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
        }

        if (_ripenTimer > greenTime + redTime && !_stageBad)
        {
            _stageBad = true;
            
            badArt.DOScale(1f, 0.17f);
            redArt.DOScale(0.1f, 0.17f);
            greenArt.DOScale(0.1f, 0.17f);
        }

        if (_ripenTimer > greenTime + redTime + badTime)
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
        if (collision.collider.CompareTag("ArrowTip") && collision.gameObject.TryGetComponent<Arrow>(out var arrow))
        {
            _damaged = true;
            
            arrow.StickTo(_rig);
        }
    }
}
