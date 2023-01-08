using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Socket : MonoBehaviour
{
    [SerializeField] private Color socketColor = new Color(1, 0f, 0, 0.5f);

    #if UNITY_EDITOR
    void OnDrawGizmos()
    {
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);

        Gizmos.color = socketColor;
        Gizmos.DrawSphere(Vector3.zero, 0.1f);

    }
    #endif 
}
