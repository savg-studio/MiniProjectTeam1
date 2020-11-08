using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fly : AISpaceship
{
    public float combatSpeed;

    protected override void UpdateOnCombat()
    {
        //agent.Pursuit(GetPlayerPos(), player.GetVelocity());
        agent.Seek(GetPlayerPos());
        agent.CollisonAvoidance();
    }

    protected override void OnEnterCombat()
    {
        base.OnEnterCombat();

        agent.maxSpeed = combatSpeed;
    }

    protected override void OnCollision(Collision2D collision)
    {
        base.OnCollision(collision);

        var potentialPlayer = collision.gameObject.GetComponent<Player>();

        if (potentialPlayer && !HasFlag(SpaceshipStateFlags.DEAD))
        {
            potentialPlayer.TakeDamage();

            this.Die();
        }
    }

}
