using UnityEngine;

public static class TransformExtensions
{
    public static void SetParentZero(this Transform origin, Transform parent)
    {
        origin.SetParent(parent);
        origin.localPosition = Vector3.zero;
        origin.localRotation = Quaternion.identity;
    }
}
