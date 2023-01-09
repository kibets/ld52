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
        art.DOScale(radius * 2f, 0.11f).SetEase(Ease.OutBack);
        
        foreach (var mat in mesh.materials)
        {
            mat.DOColor(Color.clear, "_EmissionColor", 0.27f);
            mat.DOColor(Color.clear, "_BaseColor", 0.27f);
            mat.DOColor(Color.clear, "_Color", 0.27f);
        }

        var enemies = Registry.Instance.GetEnemies(transform.position, radius + 1f);
        foreach (var enemy in enemies) enemy.Die();
        
        var arrows  = Registry.Instance.GetArrows(transform.position, radius);
        foreach (var arrow in arrows) if (!arrow.Fake) Destroy(arrow.gameObject);

        var apples = Registry.Instance.GetApples(transform.position, radius);
        foreach (var apple in apples)
        {
            apple.SetLastStage();
        }

        if (transform.DistanceTo(Hero.Instance.transform) <= radius)
        {
            Hero.Instance.Bite();
        }
        
        Destroy(gameObject, 0.3f);
    }
}
