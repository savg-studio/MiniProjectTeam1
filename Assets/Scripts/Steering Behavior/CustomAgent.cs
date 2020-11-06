using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomAgent : SteeringAgent
{
    // Objective
    public Player objective;
    public GameObject posTarget;


    protected override void OnStart()
    {

    }

    // Update is called once per frame
    protected override Vector2 CalculateSteering()
    {
        return Wander() + CollisionAvoidance();
        //return Wander() + CollisionAvoidance();
        //return Seek(posTarget.transform.position) + CollisionAvoidance();
    }

    protected Vector2 GetTargetPos()
    {
        return objective.transform.position;
    }
}
