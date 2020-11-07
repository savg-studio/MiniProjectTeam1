using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spaceship : MonoBehaviour
{
    public WeaponBase weapon;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Stun()
    {

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
