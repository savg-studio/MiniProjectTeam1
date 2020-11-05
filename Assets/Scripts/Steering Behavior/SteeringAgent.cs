using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringAgent : MonoBehaviour
{
    public float maxForce;
    public float maxSpeed;

    // Objective
    public GameObject objective;

    // Cache components
    private Rigidbody2D rigidbody;

    // Inner
    private Vector2 currentVelocity;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        currentVelocity = new Vector2(1, 0) * maxSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        currentVelocity = CalculateSteering(currentVelocity);
        Vector2 newPosition = GetPos() + currentVelocity;
        Move(newPosition);
        FaceCurrentDir();
    }

    private Vector2 CalculateSteering(Vector2 velocity)
    {
        var desiredVelocity = (GetTargetPos() - GetPos()).normalized * maxSpeed;
        var steering = desiredVelocity - currentVelocity;
        steering = Truncate(steering, maxForce);

        velocity = Truncate(steering + velocity, maxSpeed);

        return velocity;
    }

    private Vector2 Truncate(Vector2 steering, float maxForce)
    {
        float magnitude = steering.magnitude;
        float force = Mathf.Min(magnitude, maxForce);
        Vector2 capped = steering.normalized * force;

        return capped;
    }
        
    private Vector2 GetTargetPos()
    {
        return objective.transform.position;
    }

    private Vector2 GetPos()
    {
        return transform.position;
    }

    // Rigidbody

    private void FaceCurrentDir()
    {
        float angle = T1Utils.Vector2ToAngle(currentVelocity);
        rigidbody.SetRotation(angle);
    }

    private void Move(Vector2 pos)
    {
        rigidbody.MovePosition(pos);
    }    
        
}
