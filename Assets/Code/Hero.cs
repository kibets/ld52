using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;

public class Hero : Singleton<Hero>
{
    [SerializeField] public float Speed;
    [SerializeField] public float JumpSpeed;
    [SerializeField] public float GravityForce;

    [SerializeField] private Transform arm;
    [SerializeField] private Bow bow;

    [SerializeField] private Transform leftArm;

    [SerializeField] private Transform legLeft;
    [SerializeField] private Transform legRight;

    [SerializeField] private Transform artGroup;

    [SerializeField] private Transform keyHolder;
    
    private Rigidbody _rig;

    private Vector3 _movement;

    private int _floorMask;
    private bool _grounded;

    private float JumpCooldown = 0.1f;

    private float _jumpTimer;
    private bool _doJump;
    private bool _animMoving;
    private bool _lookingRight;

    public Key Key { get; private set; }
    public bool ShootingDisabled { get; set; }

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

        UpdateLocomotion();
    }

    private void UpdateLocomotion()
    {
        const float VelocityTrigger = 1f;
        var angle = 25;
        
        if (_grounded && _rig.velocity.magnitude > VelocityTrigger && !_animMoving)
        {
            _animMoving = true;
            legLeft.DOKill();
            legRight.DOKill();


            legLeft.localPosition = new Vector3(legLeft.localPosition.x, 0f, 0f);
            legRight.localPosition = new Vector3(legRight.localPosition.x, 0f, 0f);
            
            legLeft.localRotation = Quaternion.Euler(0, 0, -angle);
            legRight.localRotation = Quaternion.Euler(0, 0, -angle);
            
            legLeft.DOLocalRotate(new Vector3(0, 0, angle), 0.23f).SetLoops(-1, LoopType.Yoyo).SetDelay(0.23f);
            legRight.DOLocalRotate(new Vector3(0, 0, angle), 0.23f).SetLoops(-1, LoopType.Yoyo);
            
            legLeft.DOLocalMoveY(0.5f,0.23f).SetLoops(-1, LoopType.Yoyo).SetDelay(0.23f);
            legRight.DOLocalMoveY(0.5f, 0.23f).SetLoops(-1, LoopType.Yoyo);
        }
        else if (_animMoving && (_rig.velocity.magnitude <= VelocityTrigger || !_grounded))
        {
            _animMoving = false;
            legLeft.DOKill();
            legRight.DOKill();

            legLeft.DOLocalRotate(Vector3.zero, 0.13f);
            legRight.DOLocalRotate(Vector3.zero, 0.13f);

            legLeft.DOLocalMoveY(0, 0.13f);
            legRight.DOLocalMoveY(0, 0.13f);
        }
    }

    private void UpdateFireInput()
    {
        if (ShootingDisabled && !_lookingRight) return;

        if (Input.GetButton("Fire1"))
        {
            var springPos = bow.HoldTrigger();

            if (springPos == Vector3.zero)
            {
                leftArm.transform.localPosition = Vector3.zero;
            }
            else
            {
                leftArm.transform.localPosition = springPos + Vector3.right * 0.2f;
            }
        }
        else
        {
            bow.ReleaseTrigger();
            
            leftArm.transform.localPosition = Vector3.zero;
        }
    }

    private void UpdateBowRotation()
    {
        var target = MousePointer.Instance.transform.position;
        
        var angleRad = Mathf.Atan2(target.y - transform.position.y, target.x - transform.position.x);
        var angleDeg = (180 / Mathf.PI) * angleRad;

        arm.rotation = Quaternion.Euler(0, 0, angleDeg);

        var shouldLookRight = Mathf.Abs(angleDeg) <= 90;

        if (shouldLookRight && !_lookingRight)
        {
            _lookingRight = true;
            
            artGroup.DOKill();
            artGroup.DOScaleX(1f, 0.13f);
        }

        if (!shouldLookRight && _lookingRight)
        {
            _lookingRight = false;

            artGroup.DOKill();
            artGroup.DOScaleX(-1f, 0.13f);
        }
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Apple") && collision.gameObject.TryGetComponent<Apple>(out var apple))
        {
            if (!apple.Collected)
            {
                Progress.Instance.CollectApple(apple);
                LevelUI.Instance.UpdateUI();

                apple.Collect();
            }
        }
    }

    public void AddKey(string keyCode)
    {
        Key = Prefabs.Instance.Produce<Key>(keyCode);
        Key.transform.SetParent(keyHolder);
        Key.transform.position = Trader.Instance.transform.position + Vector3.up * 3f;
        Key.transform.rotation = quaternion.identity;

        Key.transform.DOLocalMove(Vector3.zero, 1.13f);
    }


    public void Bite()
    {
        // transform.DOScale(Vector3.one * 0.1f, 0.23f);
    }
}
