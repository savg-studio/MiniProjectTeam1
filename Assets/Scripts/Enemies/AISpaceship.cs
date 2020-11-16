using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AIState
{
    WANDER,
    COMBAT
}

public class AISpaceship : Spaceship
{
    // Ai behavior 
    private AIState state;
    protected Player player;

    // Cache
    protected SteeringAgent agent;
    private PolygonCollider2D pCollider;
    private Animation deathAnim;
    private float deathAnimationDuration;

    protected override void OnStart()
    {
        state = AIState.WANDER;

        // Cache
        agent = GetComponent<SteeringAgent>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        pCollider = GetComponent<PolygonCollider2D>();
        deathAnim = GetComponent<Animation>();
        deathAnimationDuration = deathAnim.GetClip("Death").length;
        
        OnAIStart();
    }

    protected virtual void OnAIStart()
    {

    }

    protected override void OnFixedUpdate()
    {
        if (!HasFlag(SpaceshipStateFlags.STUNNED) && !HasFlag(SpaceshipStateFlags.DEAD))
            agent.FixedUpdateAgent();

        OnAIFixedUpdate();
    }

    protected virtual void OnAIFixedUpdate()
    {
        switch (state)
        {
            case AIState.WANDER:
                agent.Wander();
                agent.CollisonAvoidance();
                break;
            case AIState.COMBAT:
                UpdateOnCombat();
                break;
        }
    }

    protected virtual void UpdateOnCombat()
    {

    }

    protected override void OnUpdate()
    {
        if (!HasFlag(SpaceshipStateFlags.STUNNED) && !HasFlag(SpaceshipStateFlags.DEAD))
            agent.UpdateAgent();
    }

    // Death

    protected override void OnDeath()
    {
        pCollider.enabled = false;

        StartDeathAnimation();
    }

    private void StartDeathAnimation()
    {
        deathAnim.Play();
        Invoke("EndDeathAnimation", deathAnimationDuration);
    }

    private void EndDeathAnimation()
    {
        deathAnim.Stop();

        OnDeathAnimationEnd();
    }

    protected virtual void OnDeathAnimationEnd()
    {
        GameObject.Destroy(this.gameObject);
    }

    // Player interaction

    protected void PlayerFound(Player player)
    {
        this.player = player;
        state = AIState.COMBAT;

        OnEnterCombat();
    }

    protected virtual void OnEnterCombat()
    {
        
    }

    // AggroRange
    private void OnTriggerEnter2D(Collider2D other)
    {
        var potentialPlayer = other.gameObject.GetComponent<Player>();

        if (potentialPlayer && player == null)
            PlayerFound(potentialPlayer);
    }

    // Utils
    protected Vector2 GetPlayerPos()
    {
        Vector2 playerPos = Vector2.zero;

        if (player)
            playerPos = player.transform.position;

        return playerPos;
    }

    public override Vector2 GetFacingDir()
    {
        return agent.GetCurrentVelocity().normalized;
    }
}
