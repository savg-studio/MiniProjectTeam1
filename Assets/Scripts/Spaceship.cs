using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spaceship : MonoBehaviour
{
    public WeaponBase weapon;
    public float stunDuration = 0.75f;

    // State
    protected bool stunned = false;

    // Cache
    private SteeringAgent agent;

    private void Start()
    {
        agent = GetComponent<SteeringAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        if(!stunned)
            agent.FixedUpdateAgent();    
    }

    public virtual void Stun()
    {
        stunned = true;
        Invoke("RecoverStun", stunDuration);
    }

    public void RecoverStun()
    {
        stunned = false;
    }

    public virtual Vector2 GetFacingDir()
    {
        return Vector2.zero;
    }

    public void SetWeapon(WeaponBase weapon)
    {
        this.weapon = weapon;
        weapon.owner = this;
    }
}
