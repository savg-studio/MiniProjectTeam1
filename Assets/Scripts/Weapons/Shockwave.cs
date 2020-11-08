using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shockwave : Explosion
{
    // Projectile
    private Projectile projectile;

    public void Init()
    {
        projectile = GetComponent<Projectile>();
        projectile.Init();
    }

    public void Launch(Vector2 dir, float force, ForceMode2D mode)
    {
        projectile.Launch(dir, force, mode);
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        var rigidBody = collision.attachedRigidbody;

        var spaceship = collision.gameObject.GetComponent<Spaceship>();
        if (spaceship && spaceship.gameObject.layer != LayerMask.NameToLayer("Player"))
            spaceship.Stun();

        if(collision.gameObject.layer != LayerMask.NameToLayer("Player"))
            AddExplosionForce(rigidBody);
    }

    private void OnBecameInvisible()
    {
        GameObject.Destroy(this.gameObject);
    }
}
