using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Hero") && collision.gameObject.TryGetComponent<Hero>(out var hero))
        {
            hero.Bite();
        }
    }
}
