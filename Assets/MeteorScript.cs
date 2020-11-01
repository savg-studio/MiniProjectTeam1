using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorScript : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody2D rigidbody2D;

    private float baseMass;
    private float baseSize;

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
    }

    public void Launch(Vector2 dir, float minForce, float maxForce)
    {
        float force = Random.Range(minForce, maxForce);
        rigidbody2D.AddForce(dir * force);
        rigidbody2D.AddTorque(force / 5);
    }

    public void Resize(float minSize, float maxSize)
    {
        float size = Random.Range(minSize, maxSize);
        transform.localScale = Vector3.one * size;
        rigidbody2D.mass = baseMass * size;
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

        if(player)
        {
            player.CollideWith(this, collision);
        }
    }

    private void OnBecameInvisible()
    {
    }
}
