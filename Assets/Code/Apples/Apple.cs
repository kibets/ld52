using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class AppleStage
{
    public string Name;
    public Transform Art;
    public float Duration;
    public float ColliderRadius;
}

public class Apple : MonoBehaviour
{
    [SerializeField] private AppleJoint parentJoint;
    [SerializeField] private SphereCollider mainCollider;

    [SerializeField] private float floorTime;

    [SerializeField] private List<AppleStage> stages;

    private AppleStage _stage;
    private int _stageIndex;
    
    public event Action<AppleStage> OnStageChanged;
    public event Action OnExpired;
    
    public bool Collected { get; set; }
    
    private Rigidbody _rig;

    private float _stageTimer;

    private bool _damaged;
    private bool _jointBreak;

    private bool _touchedFloor;
    
    private float _floorTimer;
    private Coroutine _blinkRoutine;
    private bool _expired;

    public AppleStage Stage => _stage;

    private int _hits;
    
    private void Awake()
    {
        _stage = stages[0];

        foreach (var stage in stages)
        {
            stage.Duration += stage.Duration * Random.value * 0.2f;
        }

        stages[0].Duration *= Progress.Instance.AppleFirstStageMod;

        _rig = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        foreach (var stage in stages)
        {
            stage.Art.localScale = Vector3.one * 0.1f;
        }

        SelectStage(_stage);
        
        if (Registry.Instance != null) Registry.Instance.Add(this);
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
        if (_jointBreak || _damaged || _expired) return;
        
        _stageTimer += Time.deltaTime * Progress.Instance.AppleRipeSpeedMod;
        
        if (_stageTimer > _stage.Duration)
        {
            _stageTimer = 0;

            _stage.Art.DOScale(0.1f, 0.17f);

            _stageIndex += 1;

            if (_stageIndex >= stages.Count)
            {
                _expired = true;

                mainCollider.enabled = false;
                
                OnExpired?.Invoke();
            
                if (parentJoint != null)
                {
                    Destroy(parentJoint.gameObject, 0.18f);
                }
                Destroy(transform.gameObject, 0.18f);
            }
            else
            {
                SelectStage(stages[_stageIndex]);
            }
        }
    }
    
    private void SelectStage(AppleStage stage)
    {
        _stage = stage;
        
        _stage.Art.DOScale(1f, 0.17f);
        mainCollider.radius = _stage.ColliderRadius;
        
        OnStageChanged?.Invoke(stage);
    }
    
    public void SetLastStage()
    {
        _stageTimer = 0;
        
        if (_stage != null)
        {
            _stage.Art.DOScale(0.1f, 0.17f);
        }

        _stageIndex = stages.Count - 1;
        SelectStage(stages[_stageIndex]);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_expired) return;
        
        if (Checks.IsGround(collision.collider) && !_touchedFloor)
        {
            _touchedFloor = true;
            _floorTimer = floorTime;
        }
        
        if (collision.collider.CompareTag("ArrowTip") && collision.gameObject.TryGetComponent<Arrow>(out var arrow))
        {
            if (collision.contactCount > 0 && !arrow.Reflected)
            {
                var point = collision.GetContact(0);

                var effect = Prefabs.Instance.Produce("AppleHitFx");
                effect.transform.SetParent(transform);
                effect.transform.position = point.point;
                effect.transform.rotation = arrow.transform.rotation;
            
                _damaged = true;
                    
                arrow.StickTo(_rig);

                _hits += 1;

                if (!_touchedFloor && _hits == 2)
                {
                    //
                }
            }
            
        }
    }

    public void Collect()
    {
        Collected = true;
        Destroy(gameObject);
        
        if (parentJoint != null)
        {
            Destroy(parentJoint.gameObject);
        }
    }

    private void OnDestroy()
    {
        if (Registry.Instance != null) Registry.Instance.Remove(this);
    }

}
