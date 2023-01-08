using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Bow : MonoBehaviour
{
    [SerializeField] private Transform muzzle;
    [SerializeField] private float bowForceBase;
    [SerializeField] private float bowForceSpring;
    [SerializeField] private float springTime;

    [SerializeField] private ParticleSystem bowChargeFx;
    
    private bool _arming;
    private bool _armedMax;

    private float _springTimer;


    private Arrow _arrow;
    
    private void Update()
    {
        if (_arming)
        {
            _springTimer += Time.deltaTime;

            if (_springTimer > springTime)
            {
                _springTimer = springTime;

                if (!_armedMax)
                {
                    _armedMax = true;
                    bowChargeFx.Play();

                    // MousePointer.Instance.PlayBowMaxCharge();
                }
            }
        }
    }

    public Vector3 HoldTrigger()
    {
        if (!_arming)
        {
            LoadArrow();
        }

        if (_arming)
        {
            var springMod = _springTimer / springTime;
            var springPos = new Vector3(-springMod * _arrow.Length * 0.5f, 0, 0);

            _arrow.transform.localPosition = springPos;

            return springPos;
        }
        
        return Vector3.zero;
    }

    private void LoadArrow()
    {
        _arming = true;
        
        _arrow = Prefabs.Instance.Produce<Arrow>();
        _arrow.DisableColliders();

        _arrow.transform.SetParent(muzzle);
        _arrow.transform.localPosition = Vector3.zero;
        _arrow.transform.localRotation = Quaternion.identity;
    }

    public void ReleaseTrigger()
    {
        if (_arming)
        {
            Destroy(_arrow.gameObject);

            var arrow = Prefabs.Instance.Produce<Arrow>();

            arrow.transform.position = muzzle.position;
            arrow.transform.rotation = muzzle.rotation;

            var springMod = _springTimer / springTime;
            
            arrow.ShootForward(bowForceBase + bowForceSpring * springMod);
            
            _springTimer = 0;
            _arming = false;
            _armedMax = false;
        }
    }

}
