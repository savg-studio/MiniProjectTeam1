using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceTrash : MonoBehaviour
{
    private Vector2 velocity;
    private float angularVelocity;

    private Rigidbody2D rigidbody;

    public void Start()
    {
        Init();
    }

    public void Update()
    {
        velocity = rigidbody.velocity;
        angularVelocity = rigidbody.angularVelocity;
    }

    public void Init()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    public void SetAngularVelocity(float angularVel)
    {
        rigidbody.angularVelocity = angularVel;
    }

    public void SetRandomRotation()
    {
        float rotation = Random.Range(-180, 180);
        //rigidbody.SetRotation(rotation);

        var baseRot = transform.localRotation.eulerAngles;
        var quat = Quaternion.Euler(baseRot.x, baseRot.y, rotation);
        transform.localRotation = quat;
    }

    private void OnEnable()
    {
        if (rigidbody)
        {
            rigidbody.velocity = velocity;
            rigidbody.angularVelocity = angularVelocity;
        }
    }
}
