using System;
using UnityEngine;

public class Bow : MonoBehaviour
{
    [SerializeField] public string Title;
    [SerializeField] private Transform muzzle;
    [SerializeField] private float bowForceBase;
    [SerializeField] private float bowForceSpring;
    [SerializeField] private float springTime;
    [SerializeField] private float reloadingTime;
    
    [SerializeField] private ParticleSystem bowChargeFx;
    
    private bool _arming;
    private bool _armedMax;

    private float _springTimer;
    private float _reloadingTimer;

    private Arrow _arrow;

    private float _flyingTimer;
    private float _flyingDuration;
    private Vector3 _flyingOrigin;
    private Transform _flyingTarget;
    private Action _flyingCallback;

    private void Update()
    {
        if (_reloadingTimer > 0)
        {
            _reloadingTimer -= Time.deltaTime;
        }
        
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
                    
                    Sounds.Instance.PlayRandom(transform.position, "bow_pull");

                    // MousePointer.Instance.PlayBowMaxCharge();
                }
            }
        }

        if (_flyingTimer > 0)
        {
            _flyingTimer -= Time.deltaTime;

            if (_flyingTimer < 0) _flyingTimer = 0;

            var progress = (_flyingDuration - _flyingTimer) / _flyingDuration;
            
            transform.position = Vector3.Lerp(_flyingOrigin, _flyingTarget.position, progress);
            transform.rotation = Quaternion.Lerp(transform.rotation, _flyingTarget.rotation, progress);
            if (_flyingTimer <= 0)
            {
                _flyingCallback?.Invoke();
                _flyingCallback = null;
                _flyingTarget = null;
            }
        }
    }

    public Vector3 HoldTrigger()
    {
        if (_reloadingTimer > 0)
        {
            return Vector3.zero;
        }
        
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
            
            Sounds.Instance.PlayRandom(transform.position, "bow_shot");
            
            _springTimer = 0;
            _arming = false;
            _armedMax = false;

            _reloadingTimer = reloadingTime;
        }
    }

    public void FlyTo(Transform target, float duration, Action callback)
    {
        _flyingOrigin = transform.position;
        _flyingDuration = duration;
        _flyingTarget = target;
        _flyingTimer = duration;
        _flyingCallback = callback;
    }
}
