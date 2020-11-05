using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringAgent : MonoBehaviour
{
    // Var
    public float maxForce;
    public float maxSpeed;

    public float circleRadius;
    public float circleDistance;
    public float angleChange;

    // Cache components
    protected Rigidbody2D rigidbody;

    // Inner
    protected Vector2 currentVelocity;
    protected float wanderAngle;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        currentVelocity = new Vector2(1, 0) * maxSpeed;
        wanderAngle = 15f;

        OnStart();
    }

    protected virtual void OnStart()
    {

    }

    void FixedUpdate()
    {
        var steering = CalculateSteering();

        steering = Truncate(steering, maxForce);
        currentVelocity = Truncate(steering + currentVelocity, maxSpeed);
        Vector2 newPos = GetPos() + currentVelocity;

        Move(newPos);
        FaceCurrentDir();
    }

    protected virtual Vector2 CalculateSteering()
    {
        return Vector2.zero;
    }

    protected Vector2 Seek(Vector2 targetPos)
    {
        var desiredVelocity = (targetPos - GetPos()).normalized * maxSpeed;
        var steering = desiredVelocity - currentVelocity;

        return steering;
    }

    protected Vector2 Pursuit(Vector2 targetPos, Vector2 targetVelocity)
    {
        var dist = (targetPos - GetPos()).magnitude;
        int T = Mathf.RoundToInt(dist / maxSpeed);
        Vector2 newTargetPos = targetPos + targetVelocity * T;

        return Seek(newTargetPos);
    }

    protected Vector2 Wander()
    {
        var displacement = T1Utils.AngleToVector2(wanderAngle) * circleRadius;
        wanderAngle += Random.Range(-0.5f, 0.5f) * angleChange;

        var circleCenter = currentVelocity.normalized * circleDistance;
        var wanderForce = circleCenter + displacement;

        return wanderForce;
    }

    protected Vector2 Truncate(Vector2 steering, float maxForce)
    {
        float magnitude = steering.magnitude;
        float force = Mathf.Min(magnitude, maxForce);
        Vector2 capped = steering.normalized * force;

        return capped;
    }

    protected Vector2 GetPos()
    {
        return transform.position;
    }

    // Rigidbody

    private void FaceCurrentDir()
    {
        float angle = T1Utils.Vector2ToAngle(currentVelocity);
        rigidbody.SetRotation(angle);
    }

    protected void Move(Vector2 pos)
    {
        rigidbody.MovePosition(pos);
    }    
        
}
