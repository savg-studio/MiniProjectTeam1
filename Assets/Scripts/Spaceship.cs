using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum SpaceshipStateFlags
{
    NONE = 0,
    STUNNED = 1,
    DEAD = 2,
    INVULNERABLE = 4
}

public class Spaceship : MonoBehaviour
{
    // Public Params
    public WeaponBase weapon;
    public float stunDuration;
    public float invulnerabilityDuration;

    // State
    protected SpaceshipStateFlags stateFlags;

    // HP
    public uint maxArmor;
    protected uint currentArmor;

    // Cache
    private Shield shield;
    private SpriteRenderer spriteRenderer;
    private Animation blinkAnimation;

    // Unity methods and custom hooks
    protected void Start()
    {
        // Hp
        currentArmor = maxArmor;

        // Cache
        shield = GetComponentInChildren<Shield>();
        var son = transform.Find("Sprite");
        blinkAnimation = son.GetComponent<Animation>();
        spriteRenderer = son.GetComponent<SpriteRenderer>();

        SetWeapon(weapon);

        OnStart();
    }

    protected virtual void OnStart()
    {

    }

    void Update()
    {
        OnUpdate();
    }

    protected virtual void OnUpdate()
    {

    }

    void FixedUpdate()
    {
        OnFixedUpdate();
    }

    protected virtual void OnFixedUpdate()
    {

    }

    // State

    public bool HasFlag(SpaceshipStateFlags flag)
    {
        return stateFlags.HasFlag(flag);
    }

    public void SetFlag(SpaceshipStateFlags flag)
    {
        stateFlags = stateFlags | flag;
    }

    public void RemoveFlag(SpaceshipStateFlags flag)
    {
        stateFlags = stateFlags & ~flag;
    }

    // State - Stun

    public void Stun()
    {
        Stun(stunDuration);
    }

    public void Stun(float duration)
    {
        SetFlag(SpaceshipStateFlags.STUNNED);
        Invoke("RecoverStun", duration);
    }

    public void RecoverStun()
    {
        RemoveFlag(SpaceshipStateFlags.STUNNED);
    }

    // State - HP

    public virtual void TakeDamage()
    {
        if (!HasFlag(SpaceshipStateFlags.DEAD) && !HasFlag(SpaceshipStateFlags.INVULNERABLE))
        {
            if (!shield || !shield.IsActive())
            {
                if (currentArmor == 0)
                    Die();
                else
                    currentArmor--;
            }

            if(shield)
                shield.Disable();

            Stun();
            StartInvulnerability();
            OnDamageTaken();
        }
    }

    protected virtual void OnDamageTaken()
    {

    }

    // Invulnerability
    protected void StartInvulnerability()
    {
        SetFlag(SpaceshipStateFlags.INVULNERABLE);
        blinkAnimation.Play();

        Invoke("StopInvulnerability", invulnerabilityDuration);
    }

    private void StopInvulnerability()
    {
        RemoveFlag(SpaceshipStateFlags.INVULNERABLE);
        blinkAnimation.Stop();
        spriteRenderer.enabled = true;
    }

    // State - Death

    public void Die()
    {
        SetFlag(SpaceshipStateFlags.DEAD);

        OnDeath();
    }

    protected virtual void OnDeath()
    {

    }

    // Movement
    public virtual Vector2 GetFacingDir()
    {
        return Vector2.zero;
    }

    // Weapons
    public void SetWeapon(WeaponBase weapon)
    {
        this.weapon = weapon;
        if(weapon)
            weapon.owner = this;
    }

    public bool CanUseWeapon(WeaponBase weapon)
    {
        return weapon && !weapon.IsInCooldown() && !HasFlag(SpaceshipStateFlags.DEAD) && !HasFlag(SpaceshipStateFlags.STUNNED);
    }

    public bool CanUseWeapon()
    {
        return weapon && !weapon.IsInCooldown() && !HasFlag(SpaceshipStateFlags.DEAD) && !HasFlag(SpaceshipStateFlags.STUNNED);
    }




    public void OnCollisionEnter2D(Collision2D collision)
    {
        var layer = collision.gameObject.layer;

        if (layer == LayerMask.NameToLayer("Projectile") || layer == LayerMask.NameToLayer("Meteor"))
            TakeDamage();
        else if (layer == LayerMask.NameToLayer("Obstacles") && HasFlag(SpaceshipStateFlags.STUNNED))
            TakeDamage();

        OnCollision(collision);
    }

    protected virtual void OnCollision(Collision2D collision)
    {

    }
}
