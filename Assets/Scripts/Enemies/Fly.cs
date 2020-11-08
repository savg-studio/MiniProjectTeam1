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
    private Explosion explosion;

    protected override void OnAIStart()
    {
        state = FlyState.WANDER;
        explosion = GetComponentInChildren<Explosion>();
    }

    protected override void OnAIFixedUpdate()
    {
        switch(state)
        {
            case FlyState.WANDER:
                agent.Wander();
                break;
            case FlyState.CHASING_PLAYER:
                agent.Pursuit(GetPlayerPos(), player.GetVelocity());
                //agent.Seek(GetPlayerPos());
                break;
        }
    }

    protected override void OnPlayerFound(Player player)
    {
        state = FlyState.CHASING_PLAYER;
        agent.maxSpeed = 0.06f;
    }

    protected override void OnDeathAnimationEnd()
    {
        base.OnDeathAnimationEnd();
        Explode();
    }

    protected void Explode()
    {
        explosion.Explode();
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

        if (potentialPlayer)
        {
            potentialPlayer.Stun();
            Die();
        }
    }

}
