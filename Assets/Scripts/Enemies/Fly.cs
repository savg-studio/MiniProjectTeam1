using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fly : AISpaceship
{
    public float combatSpeed;

    protected override void UpdateOnCombat()
    {
        agent.Pursuit(GetPlayerPos(), player.GetVelocity());
        //agent.Seek(GetPlayerPos());
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

        var spaceship = collision.gameObject.GetComponent<Spaceship>();

        if (spaceship && (!HasFlag(SpaceshipStateFlags.DEAD) && HasFlag(SpaceshipStateFlags.STUNNED)
                      || spaceship is Player && !spaceship.HasFlag(SpaceshipStateFlags.INVULNERABLE)) )
        {
            spaceship.TakeDamage();

            this.Die();
        }
    }

}
