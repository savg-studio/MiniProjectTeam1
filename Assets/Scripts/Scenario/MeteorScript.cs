using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorScript : Projectile
{
    protected override void OnInit()
    {
    }

    protected override void OnLaunch(float force)
    {
        rigidbody2D.AddTorque(force / 5);
    }

    public void Resize(float minSize, float maxSize)
    {
        float size = Random.Range(minSize, maxSize);
        transform.localScale = Vector3.one * size;
        rigidbody2D.mass = baseMass * size;
    }
}
