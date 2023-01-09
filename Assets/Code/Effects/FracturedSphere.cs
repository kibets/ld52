using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class FracturedSphere : MonoBehaviour
{
    [SerializeField] private List<Rigidbody> _rigs;
    [SerializeField] private List<MeshRenderer> _meshs;

    public void Setup(Vector3 velocity)
    {
        foreach (var rig in _rigs)
        {
            rig.velocity = velocity;

            rig.transform.DOScale(Vector3.one * 0.1f, 0.1f).SetDelay(3f);
        }
        
        foreach (var mesh in _meshs)
        {
            var mat = mesh.materials[0];
            mat.DOColor(Color.gray, "_EmissionColor", 0.34f);
            mat.DOColor(Color.gray, "_BaseColor", 0.34f);
            mat.DOColor(Color.gray, "_Color", 0.34f);
        }
        
        Destroy(gameObject, 3.2f);
    }
    
}
