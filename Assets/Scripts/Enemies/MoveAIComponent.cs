using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class MoveAIComponent : EnemyAIComponent
{
    private Vector2 currentWaypoint;
    public float dirThreshold;
    public float speed;

    // Cache
    Transform enemyTransform;
    Rigidbody2D rigidbody2D;

// Public:

    public override void OnStartAIComponent()
    {
        enemyTransform = enemyAI.gameObject.transform;
        rigidbody2D = enemyAI.GetComponent<Rigidbody2D>();
    }

    public override void OnUpdateAIComponent()
    {
        Move();
        if(WaypointReached())
        {
            StopMoving();
            enemyAI.OnWaypointReached();
        }
    }

    Vector2 GetCurrentPos()
    {
        return enemyTransform.position;
    }

    public void StartMoving()
    {
        this.enabled = true;
    }

    public void StopMoving()
    {
        this.enabled = false;
    }

    public void SetMovementSpeed(float speed)
    {
        this.speed = speed;
    }

    public void SetMovementSpeed(float minSpeed, float maxSpeed)
    {
        var speed = Random.Range(minSpeed, maxSpeed);
        SetMovementSpeed(speed);
    }

    public void SetCurrentWaypoint(Vector2 waypoint)
    {
        currentWaypoint = waypoint;
    }

// Protected:

    protected virtual void OnWaypointReached()
    {

    }

    Vector2 GetDirToWaypoint()
    {
        return (currentWaypoint - GetCurrentPos()).normalized;
    }

    float GetDistanceToWaypoint()
    {
        return (currentWaypoint - GetCurrentPos()).magnitude;
    }

    protected bool WaypointReached()
    {
        return GetDistanceToWaypoint() <= dirThreshold;
    }

    // Private:

    private void Move()
    {
        var nextPos = GetCurrentPos() + GetDirToWaypoint() * speed;
        rigidbody2D.MovePosition(nextPos);
    }
}
