using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringAgent : MonoBehaviour
{
    // Seek
    public float maxForce;
    public float maxSpeed;

    // Wander
    public float circleRadius;
    public float circleDistance;
    public float turnChance;
    private Vector2 wanderForce;

    // CollisionAvoidance
    public float ahead;
    public float avoidanceForce;

    // Cache components
    protected Rigidbody2D rigidbody;

    // Inner
    protected Vector2 currentVelocity;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        currentVelocity = new Vector2(1, 0) * maxSpeed;

        OnStart();
    }

    protected virtual void OnStart()
    {

    }

    private void Update()
    {
        FaceCurrentDir();
    }

    void FixedUpdate()
    {
        var steering = CalculateSteering();

        steering = Truncate(steering, maxForce);
        currentVelocity = Truncate(steering + currentVelocity, maxSpeed);
        Vector2 newPos = GetPos() + currentVelocity;

        Move(newPos);
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
        var value = Random.value;
        if (value < turnChance)
        {
            var circleCenter = currentVelocity.normalized * circleDistance;
            var randomPoint = Random.insideUnitCircle;
            var displacement = randomPoint * circleRadius;
            displacement = Quaternion.LookRotation(currentVelocity) * displacement;

            wanderForce = circleCenter + displacement;
        }

        return Seek(wanderForce);
    }

    protected Vector2 CollisionAvoidance()
    {
        Vector2 avoidance = Vector2.zero;

        Vector2 origin = GetPos();
        Vector2 dir = currentVelocity.normalized;
        float distance = (currentVelocity.magnitude / maxSpeed) * ahead;
        LayerMask obstacles = LayerMask.GetMask("Obstacles", "Boundaries");

        var hit = Physics2D.Raycast(origin, dir, distance, obstacles);
        Debug.DrawRay(origin, dir * distance);
        if(hit.collider)
        {
            Debug.Log("Detected obstacle " + hit.collider.gameObject.name);
            avoidance = hit.normal * avoidanceForce;
        }

        return avoidance;
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
