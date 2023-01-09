using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleBomb : MonoBehaviour
{
    private Apple _apple;
    
    private void Awake()
    {
        _apple = GetComponent<Apple>();

        _apple.OnExpired += OnExpired;
    }

    private void OnExpired()
    {
        if (transform.DistanceTo(Hero.Instance.transform) < 40f)
        {
            SpawnBomb();
        }
    }

    private void SpawnBomb()
    {
        var bomb = Prefabs.Instance.Produce<Bomb>();

        bomb.transform.position = transform.position;
    }
}
