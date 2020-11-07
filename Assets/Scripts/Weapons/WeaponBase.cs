using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBase : MonoBehaviour
{
    // Public params
    public float cooldownDuration;

    // State
    private float cooldownLeft;

    // Refs
    public Spaceship owner;

    private void Update()
    {
        if(IsInCooldown())
        {
            bool ready = DecreaseCooldown();
            if (ready)
                OnCooldownRestored();
        }

        OnUpdate();
    }

    protected virtual void OnUpdate()
    {

    }

    public void Use()
    {
        if (!IsInCooldown())
        {
            StartCooldown();

            OnUse();
        }
    }

    protected virtual void OnUse()
    {
        
    }

    public bool IsInCooldown()
    {
        return cooldownLeft > 0;
    }

    private bool DecreaseCooldown()
    {
        cooldownLeft -= Time.deltaTime;

        return !IsInCooldown();
    }

    private void StartCooldown()
    {
        cooldownLeft = cooldownDuration;
    }

    protected virtual void OnCooldownRestored()
    {

    }
}
