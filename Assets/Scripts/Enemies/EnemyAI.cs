using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    // Stats
    public float maxHealth;
    public float currentHealth;

    // Unity cache
    public GameObject movementZone;
    private Bounds movementBounds;

    // AI Components
    
    public MoveAIComponent moveAIComponent;

    // Start is called before the first frame update
    void Start()
    {
        // AI Components
        moveAIComponent.SetEnemyAI(this);
        moveAIComponent.StartAIComponent();

        // Unity Components
        var collider = movementZone.GetComponent<BoxCollider2D>();
        movementBounds = collider.bounds;
        Vector2 waypoint = T1Utils.GetRandomPointInBounds(movementBounds);

        // Set first waypoint
        moveAIComponent.SetCurrentWaypoint(waypoint);
    }

    // Update is called once per frame
    void Update()
    {
        moveAIComponent.UpdateAIComponent();
    }

    // Component Callbacks
    public void OnWaypointReached()
    {
        Vector2 point = T1Utils.GetRandomPointInBounds(movementBounds);
        moveAIComponent.SetCurrentWaypoint(point);
        moveAIComponent.StartMoving();
    }

}
