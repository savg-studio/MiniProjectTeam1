using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISpaceship : Spaceship
{
    // Public params
    public float deathSpinningForce;

    // Ai behavior    
    protected Player player;

    // Cache
    protected SteeringAgent agent;
    protected Rigidbody2D rigidbody2D;
    private Animation deathAnim;
    private float deathAnimationDuration;

    protected override void OnStart()
    {
        // Cache
        agent = GetComponent<SteeringAgent>();
        rigidbody2D = GetComponent<Rigidbody2D>();
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

    }

    protected override void OnUpdate()
    {
        if (!HasFlag(SpaceshipStateFlags.STUNNED) && !HasFlag(SpaceshipStateFlags.DEAD))
            agent.UpdateAgent();
    }

    // Death

    protected override void OnDeath()
    {
        StartDeathAnimation();
    }

    private void StartDeathAnimation()
    {
        rigidbody2D.AddTorque(deathSpinningForce);
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

        OnPlayerFound(player);
    }

    protected virtual void OnPlayerFound(Player player)
    {
    }

    // AggroRange
    private void OnTriggerEnter2D(Collider2D other)
    {
        var potentialPlayer = other.gameObject.GetComponent<Player>();

        if (potentialPlayer && player == null)
            PlayerFound(potentialPlayer);
    }
}
