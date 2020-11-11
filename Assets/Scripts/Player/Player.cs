using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Spaceship
{
    // UI
    public ArmorDisplay display;

    // Speed
    public float maxSpeed;
    public float minSpeed;
    public float baseSpeed;
    public float rotationSpeed;
    private float speed;

    // Second weapon
    private WeaponBase secondWeapon;

    // Components
    private Rigidbody2D rigidBody2D;

    // GameObjects
    private GameObject sprite;

    // Start is called before the first frame update
    protected override void OnStart()
    {
        // GameObjects
        sprite = transform.Find("Sprite").gameObject;

        // Components
        rigidBody2D = GetComponent<Rigidbody2D>();

        // UI
        display.SetMaxArmor(maxArmor);
        display.SetCurrentArmor(maxArmor);
    }

    // Update is called once per frame
    protected override void OnUpdate() 
    {
        // Movement speed
        if (Input.GetAxis("Vertical") == 1)
            speed = maxSpeed;
        else if (Input.GetAxis("Vertical") == -1) 
            speed = minSpeed;
        else
            speed = baseSpeed;
     

        // Attack
        if(Input.GetButtonDown("Fire1") && CanUseWeapon(weapon))
        {
            weapon.Use();
        }
        
        if(Input.GetButtonDown("Fire2") && CanUseWeapon(secondWeapon))
        {
            secondWeapon.Use();
        }
    }

    void FixedUpdate()
    {
        if (!HasFlag(SpaceshipStateFlags.STUNNED))
            ResetAngularVelocity();

        if (!HasFlag(SpaceshipStateFlags.STUNNED))
        {
            Vector3 newRotation = GetInputRotation();
            Rotate(newRotation);
            Move();
        }
    }

    private Vector3 GetInputRotation()
    {
        Vector3 currentRotation = transform.rotation.eulerAngles;
        float newZRotation = currentRotation.z - Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime;
        return new Vector3(currentRotation.x, currentRotation.y, newZRotation);
    }

    // Spacechip

    public override Vector2 GetFacingDir()
    {
        return GetCurrentDirection();
    }

    // Movement

    public Vector2 GetVelocity()
    {
        Vector2 dir = GetCurrentDirection();
        Vector2 velocity = dir * speed * Time.deltaTime;

        return velocity;
    }    

    private void Rotate(Vector3 rotation)
    {
        // Updates rigidbody velocity
        Vector2 direction = T1Utils.AngleToVector2(rotation.z); // If angle is zero, default direction should be (0, 1) instead of (1, 0)
        direction.Normalize();
        rigidBody2D.SetRotation(rotation.z);
    }

    private void Move()
    {
        Vector3 dir = GetCurrentDirection();
        Vector2 nextPos = transform.position + dir * speed * Time.deltaTime;
        rigidBody2D.MovePosition(nextPos);
    }

    private void ResetAngularVelocity()
    {
        rigidBody2D.angularVelocity = 0;
    }

    private Vector2 GetCurrentDirection()
    {
        Vector3 currentRotation = transform.rotation.eulerAngles;
        Vector2 dir = T1Utils.AngleToVector2(currentRotation.z);
        dir.Normalize();
        return dir;
    }

    // Weapon

    public void SetSecondWeapon(WeaponBase weapon)
    {
        // Destroy previous weapon
        if (secondWeapon)
            GameObject.Destroy(secondWeapon.gameObject);

        secondWeapon = weapon;
        secondWeapon.owner = this;
    }

    // Stun
    public override void Stun()
    {
        SetFlag(SpaceshipStateFlags.STUNNED);
        Invoke("Recover", stunDuration);
    }

    private void Recover()
    {
        RemoveFlag(SpaceshipStateFlags.STUNNED);
    }

    protected override void OnDamageTaken()
    {
        display.SetCurrentArmor(currentArmor);
    }

    protected override void OnDeath()
    {

    }
}
