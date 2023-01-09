using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public float SpawnTime;
    public float SpawnTimeSpread;
    
    public float Radius;

    private float _timer;

    private void Start()
    {
        _timer = Rnd.ValueSpread(SpawnTime, SpawnTimeSpread) * 0.5f;
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
        var enemy = Prefabs.Instance.Produce<Enemy>();

        enemy.transform.position = transform.position + Rnd.InCircle(Radius);
    }
}
