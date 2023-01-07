using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour
{
    [SerializeField] private Transform muzzle;
    [SerializeField] private float bowForceBase;
    [SerializeField] private float bowForceSpring;
    [SerializeField] private float springTime;
    
    private bool _arming;

    private float _springTimer;


    private void Update()
    {
        if (_arming)
        {
            _springTimer += Time.deltaTime;

            if (_springTimer > springTime) _springTimer = springTime;
        }
    }

    public void HoldTrigger()
    {
        _arming = true;
    }
    
    public void ReleaseTrigger()
    {
        if (_arming)
        {
            _arming = false;

            var arrow = Prefabs.Instance.Produce<Arrow>();

            arrow.transform.position = muzzle.position;
            arrow.transform.rotation = muzzle.rotation;

            var springMod = _springTimer / springTime;
            
            arrow.ShootForward(bowForceBase + bowForceSpring * springMod);

            _springTimer = 0;
        }
    }

}
