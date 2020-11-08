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

    // Dash
    public float dashForce;
    public float dashDuration;
    public float dashCooldown;

    private bool dashing = false;
    private bool dashInCooldown = false;

    // Laser Recoil
    private bool inRecoil = false;
    public bool laserRecoilEnabled;
    public float laserRecoilForce;
    public float laserRecoilDuration;

    // Stun effects

    // Invulnerability
    public float invulnerabilityDuration;
    private bool invulnerable = false;

    // Shields
    public float shieldCooldown;
    private GameObject shieldGO;
    private float shieldTimeLeft;

    // Components
    private Rigidbody2D rigidBody2D;
    private Animation blinkAnimation;
    private SpriteRenderer spriteRenderer;

    // GameObjects
    private GameObject sprite;

    // Start is called before the first frame update
    void Start()
    {
        // GameObjects
        sprite = transform.Find("Sprite").gameObject;
        shieldGO = transform.Find("Shield").gameObject;

        // Components
        rigidBody2D = GetComponent<Rigidbody2D>();
        blinkAnimation = GetComponentInChildren<Animation>();
        spriteRenderer = sprite.GetComponent<SpriteRenderer>();

        // Set owner
        SetWeapon(weapon);

        // UI
        display.SetMaxArmor(maxArmor);
        display.SetCurrentArmor(maxArmor);
    }

    // Update is called once per frame
    void Update() 
    {
        // Dash
        if (Input.GetKeyDown(KeyCode.Space) && CanStartDash())
            Dash();
        // Movement speed
        else
        {
            if (Input.GetAxis("Vertical") == 1)
                speed = maxSpeed;
            else if (Input.GetAxis("Vertical") == -1) 
                speed = minSpeed;
            else
                speed = baseSpeed;
        }

        if(!HasFlag(SpaceshipStateFlags.STUNNED))
        {
            // Attack
            if(Input.GetButton("Fire1") && WeaponReady())
            {
                weapon.Use();
            }
        }

        // Shield
        if(!IsShieldUp())
        {
            UpdateShieldTimer();
            if (IsShieldReady())
                EnableShield();
        }
    }

    void FixedUpdate()
    {
        if (!HasFlag(SpaceshipStateFlags.STUNNED))
            ResetAngularVelocity();

        if (!dashing && !HasFlag(SpaceshipStateFlags.STUNNED) && !inRecoil)
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

    // Dash
    private void Dash()
    {
        Vector2 force = GetCurrentDirection() * dashForce * speed;
        dashing = true;
        dashInCooldown = true;
        Invoke("StopDashing", dashDuration);
        Invoke("EnableDash", dashCooldown);
        rigidBody2D.AddForce(force, ForceMode2D.Impulse);
    }

    private bool CanStartDash()
    {
        return !dashing && !dashInCooldown && !inRecoil;
    }

    private void StopDashing()
    {
        dashing = false;
        rigidBody2D.velocity = Vector2.zero;
        rigidBody2D.angularVelocity = 0;
    }

    private void EnableDash()
    {
        dashInCooldown = false;
    }

    // Weapon

    private void Recoil()
    {
        inRecoil = true;
        rigidBody2D.velocity = Vector2.zero;
        rigidBody2D.AddForce(GetRecoilDir() * laserRecoilForce);
    }

    private void RecoverFromRecoil()
    {
        inRecoil = false;
    }

    private Vector2 GetRecoilDir()
    {
        var recoilDir = -1 * GetCurrentDirection();
        return recoilDir;
    }

    private bool WeaponReady()
    {
        return !weapon.IsInCooldown();
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

    // Invulnerability
    private void StartInvulnerability()
    {
        invulnerable = true;
        blinkAnimation.Play();

        Invoke("StopInvulnerability", invulnerabilityDuration);
    }
   
    private void StopInvulnerability()
    {
        invulnerable = false;
        blinkAnimation.Stop();
        spriteRenderer.enabled = true;
    }

    // HP
    public override void TakeDamage()
    {
        display.SetCurrentArmor(currentArmor);
    }

    private bool IsShieldUp()
    {
        return shieldGO.activeSelf;
    }

    private void DisableShield()
    {
        shieldGO.SetActive(false);
    }

    private void EnableShield()
    {
        shieldGO.SetActive(true);
    }

    private void ResetShieldTimer()
    {
        shieldTimeLeft = shieldCooldown;
    }
    private bool IsShieldReady()
    {
        return shieldTimeLeft < 0;
    }

    private void UpdateShieldTimer()
    {
        shieldTimeLeft -= Time.deltaTime;
    }

    protected override void OnDeath()
    {

    }

    // Collision
    public void CollideWith(Projectile p, Collision2D collision)
    {
        if (IsShieldUp())
        {
            DisableShield();
            StartInvulnerability();

            ResetShieldTimer();
        }
        else if (!invulnerable)
        {
            Stun();
            TakeDamage();
            StartInvulnerability();

            ResetShieldTimer();
        }
    }
}
