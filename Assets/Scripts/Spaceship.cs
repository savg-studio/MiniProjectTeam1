using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum SpaceshipStateFlags
{
    NONE = 0,
    STUNNED = 1,
    DEAD = 2
}

public class Spaceship : MonoBehaviour
{
    public WeaponBase weapon;
    public float stunDuration = 0.75f;

    // State
    protected SpaceshipStateFlags stateFlags;

    // HP
    public uint maxArmor;
    protected uint currentArmor;


    // Unity methods and custom hooks
    private void Start()
    {
        // Hp
        currentArmor = maxArmor;

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

    public virtual void Stun()
    {
        SetFlag(SpaceshipStateFlags.STUNNED);
        Invoke("RecoverStun", stunDuration);
    }

    public void RecoverStun()
    {
        RemoveFlag(SpaceshipStateFlags.STUNNED);
    }

    // HP

    public virtual void TakeDamage()
    {
        if (!HasFlag(SpaceshipStateFlags.DEAD))
        {
            if (currentArmor == 0)
                Die();
            else
                currentArmor--;

            OnDamageTaken();
        }
    }

    protected virtual void OnDamageTaken()
    {

    }

    protected void Die()
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
        weapon.owner = this;
    }


    public void OnCollisionEnter2D(Collision2D collision)
    {
        var layer = collision.gameObject.layer;

        if (layer == LayerMask.NameToLayer("Projectile"))
            TakeDamage();
        else if (layer == LayerMask.NameToLayer("Obstacles") && HasFlag(SpaceshipStateFlags.STUNNED))
            TakeDamage();

        OnCollision(collision);
    }

    protected virtual void OnCollision(Collision2D collision)
    {

    }
}
