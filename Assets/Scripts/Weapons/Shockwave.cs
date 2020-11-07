using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shockwave : Projectile
{
    // Params
    public float explosionForce;
    public ForceMode2D mode;

    // Cache vars
    private Vector2 center;
    private float radius;

    // Cache components
    private CircleCollider2D circle;

    private void Start()
    {
        circle = GetComponent<CircleCollider2D>();
        center = (Vector2)gameObject.transform.position + circle.offset;
        radius = circle.radius;
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        var rigidBody = collision.attachedRigidbody;

        var spaceship = collision.gameObject.GetComponent<Spaceship>();
        if (spaceship)
            spaceship.Stun();

        ExplodeObject(rigidBody, explosionForce, center, radius, mode);
    }

    private void ExplodeObject(Rigidbody2D rigidbody, float explosionForce, Vector2 explosionCenter, float explosionRadius, ForceMode2D mode)
    {
        Vector2 objectPos = rigidbody.transform.position;
        var distV = objectPos - explosionCenter;

        var dist = distV.magnitude;
        var forceDir = distV.normalized;

        var finalForce = explosionForce / dist;
        var force = forceDir * finalForce;

        rigidbody.AddForce(force, mode);
    }

    
}
