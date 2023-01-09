using UnityEngine;

public static class TransformExtensions
{
    public static void SetParentZero(this Transform origin, Transform parent)
    {
        origin.SetParent(parent);
        origin.localPosition = Vector3.zero;
        origin.localRotation = Quaternion.identity;
    }

    public static float DistanceTo(this Transform origin, Transform target)
    {
        return Vector3.Distance(origin.position, target.position);
    }
    
    public static float DistanceTo(this Transform origin, Vector3 position)
    {
        return Vector3.Distance(origin.position, position);
    }
}
