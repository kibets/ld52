using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private Transform art;
    [SerializeField] private MeshRenderer mesh;

    public void Setup(float radius)
    {
        art.DOScale(radius, 0.11f).SetEase(Ease.OutBack);
        
        foreach (var mat in mesh.materials)
        {
            mat.DOColor(Color.clear, "_EmissionColor", 0.27f);
            mat.DOColor(Color.clear, "_BaseColor", 0.27f);
            mat.DOColor(Color.clear, "_Color", 0.27f);
        }
        
        Destroy(gameObject, 0.3f);
    }
}
