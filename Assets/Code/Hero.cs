using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    [SerializeField] public float Speed;

    [SerializeField] private Transform arm;
    [SerializeField] private Bow bow;
    
    private Rigidbody _rig;

    private Vector3 _movement;
    
    private void Awake()
    {
        _rig = GetComponent<Rigidbody>();
    }

    void Update()
    {
        _movement = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"),0f);

        if (Input.GetButton("Fire1"))
        {
            bow.HoldTrigger();
        }
        else
        {
            bow.ReleaseTrigger();
        }

        var target = MousePointer.Instance.transform.position;
        
        // Get Angle in Radians
        float AngleRad = Mathf.Atan2(target.y - transform.position.y, target.x - transform.position.x);
        // Get Angle in Degrees
        float AngleDeg = (180 / Mathf.PI) * AngleRad;
        // Rotate Object
        arm.rotation = Quaternion.Euler(0, 0, AngleDeg);
        
        //
        // var dir = (MousePointer.Instance.transform.position - transform.position).normalized;
        // var rotation = Quaternion.LookRotation(dir);
        //
        // arm.rotation = rotation;
    }

    private void FixedUpdate()
    {
        // _rig.MovePosition(_rig.position += _movement* (Speed * Time.fixedDeltaTime));
        _rig.AddForce(_movement * (Speed * Time.fixedDeltaTime), ForceMode.VelocityChange);
    }
}
