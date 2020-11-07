using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    protected Rigidbody2D rigidbody2D;

    protected float baseMass;
    protected float baseSize;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Init()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        baseMass = rigidbody2D.mass;
        baseSize = transform.localScale.x;

        OnInit();
    }

    protected virtual void OnInit()
    {

    }

    public void Launch(Vector2 dir, float force, ForceMode2D mode = ForceMode2D.Force)
    {
        rigidbody2D.AddForce(dir * force);
    }

    public void Launch(Vector2 dir, float minForce, float maxForce)
    {
        float force = Random.Range(minForce, maxForce);
        Launch(dir, force);
        OnLaunch(force);
    }

    protected virtual void OnLaunch(float force)
    {

    }

    public float GetForce()
    {
        return rigidbody2D.mass * rigidbody2D.inertia;
    }

    public void SetRotation(float rotation)
    {
        var rot = transform.rotation.eulerAngles;
        rot.z = rotation;
        var quat = Quaternion.Euler(rot);
        transform.rotation = quat;
    }

    public Vector2 GetCentreOfMass()
    {
        return rigidbody2D.centerOfMass;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var player = collision.gameObject.GetComponent<Player>();

        if (player)
        {
            player.CollideWith(this, collision);
        }

        OnCollide(collision);
    }

    public virtual void OnCollide(Collision2D collision)
    {

    }

    private void OnBecameInvisible()
    {
    }
}
