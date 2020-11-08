using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISpaceship : Spaceship
{
    // Public params
    public float deathSpinningForce;

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

    protected override void OnDeath()
    {
        StartDeathAnimation();
    }

    private void StartDeathAnimation()
    {
        rigidbody2D.AddTorque(deathSpinningForce);
        deathAnim.Play();
    }

    private void EndDeathAnimation()
    {
        deathAnim.Stop();

        OnDeathAnimationEnd();
    }

    protected void OnDeathAnimationEnd()
    {
        GameObject.Destroy(this.gameObject);
    }
}
