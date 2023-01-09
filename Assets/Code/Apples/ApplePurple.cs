using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplePurple : MonoBehaviour
{
    [SerializeField] private float HeroInRadius;
    
    private Apple _apple;
    
    private void Awake()
    {
        _apple = GetComponent<Apple>();

        _apple.OnExpired += OnExpired;
    }

    private void OnExpired()
    {
        if (transform.DistanceTo(Hero.Instance.transform) < HeroInRadius)
        {
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        var enemy = Prefabs.Instance.Produce<Enemy>();

        enemy.transform.position = transform.position;
    }
}
