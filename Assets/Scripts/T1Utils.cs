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

    public static Vector2 AngleToVector2(float angle)
    {
        return new Vector2(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle));
    }

    public static float Vector2ToAngleRads(Vector2 vector)
    {
        return Mathf.Atan2(vector.y, vector.x);
    }

    public static float Vector2ToAngle(Vector2 vector)
    {
        return Vector2ToAngleRads(vector) * Mathf.Rad2Deg;
    }
}
