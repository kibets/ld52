using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    private bool _exploded;
    
    private void OnCollisionEnter(Collision collision)
    {
        if (_exploded) return;

        if (collision.impulse.magnitude > 5f)
        {
            _exploded = true;
            Explode();
        }
    }

    private void Explode()
    {
        var exp = Prefabs.Instance.Produce<Explosion>();
        exp.transform.position = transform.position;
        exp.Setup(4f);
        
        Destroy(gameObject);
    }
}
