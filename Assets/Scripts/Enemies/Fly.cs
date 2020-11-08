using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fly : AISpaceship
{

    protected override void OnAIStart()
    {
        
    }

    protected override void OnAIFixedUpdate()
    {
        agent.Wander();
    }
}
