using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WaypointMoveAIComponent : MoveAIComponent
{
    public List<GameObject> waypointZones;

    private int currentIndex = 0;

    public override void OnStartAIComponent()
    {
        base.OnStartAIComponent();
    }

    protected override void OnWaypointReached()
    {
        var waypoint = GetNextWaypoint();
        SetCurrentWaypoint(waypoint);
        StartMoving();
    }

    private Vector2 GetNextWaypoint()
    {
        currentIndex = (currentIndex + 1) % waypointZones.Count;
        var zone = waypointZones[currentIndex].GetComponent<BoxCollider2D>();
        var bounds = zone.bounds;
        return T1Utils.GetRandomPointInBounds(bounds);
    }
}
