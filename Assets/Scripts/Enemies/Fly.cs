using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FlyState
{
    WANDER,
    CHASING_PLAYER
}

public class Fly : AISpaceship
{
    private FlyState state;

    // Cache
    private PolygonCollider2D collider;

    protected override void OnAIStart()
    {
        state = FlyState.WANDER;

        collider = GetComponent<PolygonCollider2D>();
    }

    protected override void OnAIFixedUpdate()
    {
        switch(state)
        {
            case FlyState.WANDER:
                agent.Wander();
                agent.CollisonAvoidance();
                break;
            case FlyState.CHASING_PLAYER:
                //agent.Pursuit(GetPlayerPos(), player.GetVelocity());
                agent.Seek(GetPlayerPos());
                agent.CollisonAvoidance();
                break;
        }
    }

    protected override void OnPlayerFound(Player player)
    {
        state = FlyState.CHASING_PLAYER;
        agent.maxSpeed = 0.05f;
    }

    protected override void OnDeathAnimationEnd()
    {
        base.OnDeathAnimationEnd();
    }

    protected override void OnDeath()
    {
        base.OnDeath();

        collider.enabled = false;
    }

    protected Vector2 GetPlayerPos()
    {
        Vector2 playerPos = Vector2.zero;

        if (player)
            playerPos = player.transform.position;

        return playerPos;
    }

    protected override void OnCollision(Collision2D collision)
    {
        base.OnCollision(collision);

        var potentialPlayer = collision.gameObject.GetComponent<Player>();

        if (potentialPlayer && !HasFlag(SpaceshipStateFlags.DEAD))
        {
            potentialPlayer.Stun();
            potentialPlayer.TakeDamage();

            this.Die();
        }
    }

}
