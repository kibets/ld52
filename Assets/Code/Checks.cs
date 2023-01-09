using UnityEngine;

public static class Checks
{
    public static bool IsGround(Component component)
    {
        return component.CompareTag("Floor") || component.CompareTag("Spikes");
    }    
}
