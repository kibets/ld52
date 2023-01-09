using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Environment : Singleton<Environment>
{
    [SerializeField] private CinemachineConfiner2D confiner2D;

    [SerializeField] private PolygonCollider2D colliderFirst;
    [SerializeField] private PolygonCollider2D colliderSecond;

    public void SwitchColliderToEndGame()
    {
        confiner2D.m_BoundingShape2D = colliderSecond;
        confiner2D.InvalidateCache();
    }
}
