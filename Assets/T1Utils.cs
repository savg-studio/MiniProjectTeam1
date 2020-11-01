using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class T1Utils
{
    public static Vector2 GetRandomPointInBounds(Bounds bounds)
    {
        Vector2 point;

        point.x = Random.Range(bounds.min.x, bounds.max.x);
        point.y = Random.Range(bounds.min.y, bounds.max.y);

        return point;
    }
}
