using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleSpawner : MonoBehaviour
{
    public float SpawnTime;
    public float SpawnTimeSpread;
    
    public float Radius;

    private float _timer;

    private void Start()
    {
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
        var apple = Prefabs.Instance.Produce<AppleJoint>();

        apple.transform.position = transform.position + Rnd.InCircle(Radius);
    }
}
