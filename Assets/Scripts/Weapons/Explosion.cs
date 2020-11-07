using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    // Params
    public float explosionForce;
    public ForceMode2D mode;

    // Cache vars
    protected Vector2 center;
    protected float radius;

    // Cache components
    private CircleCollider2D circle;

    protected void Start()
    {
        circle = GetComponent<CircleCollider2D>();
        center = (Vector2)gameObject.transform.position + circle.offset;
        radius = circle.radius * gameObject.transform.localScale.x;
    }

    protected void AddExplosionForce(Rigidbody2D rigidBody)
    {
        BeforeImpactObject(rigidBody.gameObject);
        T1Utils.AddExplosionForce(rigidBody, explosionForce, center, radius, mode);
    }

    protected virtual void BeforeImpactObject(GameObject go)
    {

    }

    protected void Explode(LayerMask mask)
    {
        var colliders = Physics2D.OverlapCircleAll(center, radius, mask);

        foreach(var collider in colliders)
        {
            var rigidbody = collider.attachedRigidbody;
            if(rigidbody)
            {
                AddExplosionForce(rigidbody);
            }
        }
    }
}
