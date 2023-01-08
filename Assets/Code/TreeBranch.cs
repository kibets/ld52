using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class TreeBranch : MonoBehaviour
{
    [SerializeField] private List<Transform> branchSockets;
    [SerializeField] private List<Transform> fruitSockets;
    
    public float SpawnTime;
    public float SpawnTimeSpread;
    
    private float _timer;
    
    
    private void Start()
    {
        var origin = transform.localRotation.eulerAngles.z;

        var angle = Rnd.ValueSpread(5f, 5f);
        var duration = 7f;
        
        transform.DOLocalRotate(new Vector3(0f, 0f, origin + angle), duration)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutBack)
            .SetDelay(transform.position.x/10f + 1f );
        
        _timer = Rnd.ValueSpread(SpawnTime, SpawnTimeSpread) * 0.1f;
    }

    private void Update()
    {
        _timer -= Time.deltaTime;

        if (_timer < 0)
        {
            _timer += Rnd.ValueSpread(SpawnTime, SpawnTimeSpread);

            SpawnOnce();
        }

    }

    private void SpawnOnce()
    {
        var socket = fruitSockets.Where(f => f.childCount == 0).PickRandom();

        if (socket != null)
        {
            var appleJoint = Prefabs.Instance.Produce<AppleJoint>();

            appleJoint.transform.SetParentZero(socket);
        }
    }
}
