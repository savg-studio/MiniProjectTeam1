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
    public LayerMask mask;

    // Cache components
    protected Rigidbody2D rigidbody;

    // Cache gameobjects
    protected GameObject leftWing;
    protected GameObject rightWing;

    // Inner
    protected Vector2 currentVelocity;
    protected Vector2 currentSteering;
    public bool isStopped;
    public bool isRotationStopped;


    // Start is called before the first frame update
    protected virtual void Awake()
    {
        // Cache
        rigidbody = GetComponent<Rigidbody2D>();
        leftWing = transform.Find("Left").gameObject;
        rightWing = transform.Find("Right").gameObject;

        // Inner
        currentVelocity = new Vector2(1, 0) * maxSpeed;
        isStopped = false;
        isRotationStopped = false;

        OnStart();
    }

    protected virtual void OnStart()
    {

    }

    public void UpdateAgent()
    {
        if(!isStopped && !isRotationStopped)
            FaceCurrentDir();
    }

    public void FixedUpdateAgent()
    {
        if (!isStopped)
        {
            var steering = currentSteering;
            steering = Truncate(steering, maxForce);
            currentVelocity = Truncate(steering + currentVelocity, maxSpeed);
            Vector2 newPos = GetPos() + currentVelocity;

            Move(newPos);

            ResetSteering();
        }
    }

    protected void ResetSteering()
    {
        currentSteering = Vector2.zero;
    }

    public void Seek(Vector2 targetPos)
    {
        currentSteering += GetSeekForce(targetPos);
    }

    protected Vector2 GetSeekForce(Vector2 targetPos)
    {
        var desiredVelocity = (targetPos - GetPos()).normalized * maxSpeed;
        var steering = desiredVelocity - currentVelocity;

        return steering;
    }

    public void Pursuit(Vector2 targetPos, Vector2 targetVelocity)
    {
        currentSteering += GetPursuitForce(targetPos, targetVelocity);
    }

    protected Vector2 GetPursuitForce(Vector2 targetPos, Vector2 targetVelocity)
    {
        var dist = (targetPos - GetPos()).magnitude;
        int T = Mathf.RoundToInt(dist / maxSpeed);
        Vector2 newTargetPos = targetPos + targetVelocity * T;

        return GetSeekForce(newTargetPos);
    }

    public void Wander()
    {
        currentSteering += GetWanderForce();
    }

    protected Vector2 GetWanderForce()
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

        return GetSeekForce(wanderForce);
    }

    public void CollisonAvoidance()
    {
        currentSteering += GetAvoidanceForce();
    }

    protected Vector2 GetAvoidanceForce()
    {
        Vector2 avoidance = Vector2.zero;

        Vector2 originLeft = leftWing.transform.position;
        Vector2 originRight = rightWing.transform.position;
        Vector2 dir = currentVelocity.normalized;
        float distance = (currentVelocity.magnitude / maxSpeed) * ahead;
        LayerMask obstacles = LayerMask.GetMask("Obstacles", "Boundaries");

        var hitLeft = Physics2D.Raycast(originLeft, dir, distance, obstacles);
        Debug.DrawRay(originLeft, dir * distance);
        var hitRight = Physics2D.Raycast(originRight, dir, distance, obstacles);
        Debug.DrawRay(originRight, dir * distance);

        if (hitLeft.collider || hitRight.collider)
        {
            RaycastHit2D hit;
            if (hitLeft.collider && !hitRight.collider)
                hit = hitLeft;
            else if (hitRight.collider && !hitLeft.collider)
                hit = hitRight;
            else
                hit = hitLeft.distance < hitRight.distance ? hitLeft : hitRight;
            
            var surfaceNormal = hit.normal;
            var perp = Vector2.Perpendicular(surfaceNormal);
            var perp2 = -perp;

            var goodPerp = Vector2.Dot(dir, perp) > Vector2.Dot(dir, perp2) ? perp : perp2;
            avoidance = goodPerp * avoidanceForce;
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
