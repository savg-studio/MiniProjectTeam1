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

public class Spaceship : MonoBehaviour, PoolGameObject
{
    // Public Params
    public WeaponBase weapon;
    public float stunDuration;
    public float invulnerabilityDuration;

    // State
    protected SpaceshipStateFlags stateFlags;
    private float stunTimeLeft;
    private float invulnerabilityTimeLeft;

    // HP
    public uint maxArmor;
    protected uint currentArmor;

    // Cache
    private Shield shield;
    private SpriteRenderer spriteRenderer;
    private Animation blinkAnimation;
    protected Rigidbody2D rigidbody2D;

    // Reset data
    private Vector3 baseScale;

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
        rigidbody2D = GetComponent<Rigidbody2D>();

        // ResetData
        baseScale = transform.localScale;

        SetWeapon(weapon);

        OnStart();
    }

    protected virtual void OnStart()
    {

    }

    void Update()
    {
        if(HasFlag(SpaceshipStateFlags.INVULNERABLE))
            UpdateInvulnerability();
        if (HasFlag(SpaceshipStateFlags.STUNNED))
            UpdateStun();

        OnUpdate();
    }

    protected virtual void OnUpdate()
    {

    }

    void FixedUpdate()
    {
        if (!HasFlag(SpaceshipStateFlags.STUNNED))
        {
            ResetRigidbody();
        }

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
        stunTimeLeft = duration;
        OnStun();
    }

    protected virtual void OnStun()
    {

    }

    private void UpdateStun()
    {
        stunTimeLeft -= Time.deltaTime;
        if (stunTimeLeft <= 0)
            RecoverStun();
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

    public void RestoreArmor(uint amount = 3)
    {
        currentArmor = (uint)Mathf.Min(currentArmor + amount, maxArmor);
        OnRestoreArmor();
    }

    protected virtual void OnRestoreArmor()
    {

    }

    // Invulnerability
    protected void StartInvulnerability()
    {
        SetFlag(SpaceshipStateFlags.INVULNERABLE);
        blinkAnimation.Play();

        invulnerabilityTimeLeft = invulnerabilityDuration;
    }

    private void UpdateInvulnerability()
    {
        invulnerabilityTimeLeft -= Time.deltaTime;
        if (invulnerabilityTimeLeft <= 0)
            StopInvulnerability();
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

    // Rigibody
    private void ResetRigidbody()
    {
        rigidbody2D.angularVelocity = 0;
        rigidbody2D.velocity = Vector2.zero;
    }

    // Collisions
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

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public void InitGameObject()
    {
        Start();
    }

    public void ResetGameObject()
    {
        // Reset state
        if (HasFlag(SpaceshipStateFlags.DEAD))
            transform.localScale = baseScale;

        if (HasFlag(SpaceshipStateFlags.INVULNERABLE))
            StopInvulnerability();

        if (HasFlag(SpaceshipStateFlags.STUNNED))
            RecoverStun();

        // Restore shield
        if (shield)
            shield.Enable();

        // Reset flags
        stateFlags = 0;

        // Restore armor
        currentArmor = maxArmor;

        OnResetGameObject();
    }

    protected virtual void OnResetGameObject()
    {

    }
}
