using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomAgent : SteeringAgent
{
    // Objective
    public Player objective;



    protected override void OnStart()
    {

    }

    // Update is called once per frame
    protected override Vector2 CalculateSteering()
    {
        //return Pursuit(GetTargetPos(), objective.GetVelocity());
        return Wander();
    }

    protected Vector2 GetTargetPos()
    {
        return objective.transform.position;
    }
}
