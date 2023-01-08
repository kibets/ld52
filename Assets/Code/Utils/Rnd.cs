using UnityEngine;

public class Rnd
{
    public static float Spread(float spread)
    {
        return Random.Range(-spread, spread);
    }

    public static float ValueSpread(float value, float spread)
    {
        return value + Spread(spread);
    }
    
    public static Vector3 InRadius(float radius = 1f)
    {
        var angle = Random.value * Mathf.PI * 2f;
        var x = Mathf.Cos(angle) * radius;
        var y = Mathf.Sin(angle) * radius;
        
        return new(x, y, 0f);
    }

    public static Vector3 InCircle(float radius = 1f)
    {
        var v2 = Random.insideUnitCircle * radius;
        return new(v2.x, v2.y, 0f);
    }
}