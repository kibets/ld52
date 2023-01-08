using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    [SerializeField] public float Speed;
    [SerializeField] public float JumpSpeed;
    [SerializeField] public float GravityForce;

    [SerializeField] private Transform arm;
    [SerializeField] private Bow bow;
    
    private Rigidbody _rig;

    private Vector3 _movement;

    private int _floorMask;
    private bool _grounded;

    private float JumpCooldown = 0.1f;

    private float _jumpTimer;
    private bool _doJump;

    private void Awake()
    {
        _rig = GetComponent<Rigidbody>();
        
        _floorMask = LayerMask.GetMask("Floor", "Apple");
    }

    void Update()
    {

        UpdateFireInput();

        UpdateMovement();

        UpdateBowRotation();
        UpdateGroundCheck();
    }

    private void UpdateMovement()
    {
        _movement = new Vector3(Input.GetAxisRaw("Horizontal"),0f, 0f);
   
        _jumpTimer -= Time.deltaTime;
        
        if (Input.GetButtonDown("Jump"))
        {
            if (_grounded && _jumpTimer <= 0)
            {
                _jumpTimer = JumpCooldown;
                _doJump = true;
            }
        }
    }

    private void UpdateFireInput()
    {
        if (Input.GetButton("Fire1"))
        {
            bow.HoldTrigger();
        }
        else
        {
            bow.ReleaseTrigger();
        }
    }

    private void UpdateBowRotation()
    {
        var target = MousePointer.Instance.transform.position;
        
        var angleRad = Mathf.Atan2(target.y - transform.position.y, target.x - transform.position.x);
        var angleDeg = (180 / Mathf.PI) * angleRad;

        arm.rotation = Quaternion.Euler(0, 0, angleDeg);
    }

    private void UpdateGroundCheck()
    {
        var origin = transform.position + Vector3.up * 0.25f;

        var leftHit = Physics.Raycast(origin + Vector3.left * 0.5f, Vector3.down, 0.5f, _floorMask);
        var rightHit = Physics.Raycast(origin + Vector3.right * 0.5f, Vector3.down, 0.5f, _floorMask);
        var centerHit = Physics.Raycast(origin, Vector3.down, 0.5f, _floorMask);
        
        if (leftHit || rightHit || centerHit)
        {
            _grounded = true;
        }
        else
        {
            _grounded = false;
        }
    }
    
    private void FixedUpdate()
    {
        _rig.AddForce(_movement * (Speed * Time.fixedDeltaTime), ForceMode.VelocityChange);

        if (!_grounded)
        {
            _rig.AddForce(Vector3.down * GravityForce, ForceMode.Acceleration);
        }

        if (_doJump)
        {
            _doJump = false;
            _rig.AddForce(Vector3.up * JumpSpeed, ForceMode.Impulse);
        }
    }
}
