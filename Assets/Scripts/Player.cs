using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
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

    // Shoot
    public float laserDuration;
    public float laserInterval;
    public bool laserRecoilEnabled;
    public float laserRecoilForce;
    public float laserRecoilDuration;

    // State
    public float stunDuration;
    private bool stunned = false;

    public float invulnerabilityDuration;
    private bool invulnerable = false;

    private bool laserReady = true;
    private bool inRecoil = false;

    // Components
    private Rigidbody2D rigidBody2D;
    private Animation blinkAnimation;
    private SpriteRenderer spriteRenderer;

    private GameObject attack;


    // Start is called before the first frame update
    void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        blinkAnimation = GetComponentInChildren<Animation>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        attack = transform.Find("Attack").gameObject;
    }

    // Update is called once per frame
    void Update() 
    {
        if (Input.GetKeyDown(KeyCode.Space) && CanStartDash())
            Dash();
        else
        {
            if (Input.GetAxis("Vertical") == 1)
                speed = maxSpeed;
            else if (Input.GetAxis("Vertical") == -1) 
                speed = minSpeed;
            else
                speed = baseSpeed;
        }

        if(!stunned)
        {
            if(Input.GetButton("Fire1") && laserReady)
            {
                Laser();
            }
        }
    }

    void FixedUpdate()
    {
        if (!stunned)
            ResetAngularVelocity();

        if (!dashing && !stunned && !inRecoil)
        {
            Vector3 newRotation = GetInputRotation();
            Rotate(newRotation);
            Move();
        }
    }

    private void Laser()
    {
        attack.SetActive(true);
        laserReady = false;
        if(laserRecoilEnabled)
            Recoil();

        Invoke("StopLaser", laserDuration);
        Invoke("RecoverLaser", laserInterval);
        Invoke("RecoverFromRecoil", laserRecoilDuration);
    }

    private void StopLaser()
    {
        attack.SetActive(false);
    }
    private void RecoverLaser()
    {
        laserReady = true;
    }

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
        var recoilDir =  -1 * GetCurrentDirection();
        return recoilDir;
    }

    private bool CanLaser()
    {
        return laserReady;
    }

    private Vector3 GetInputRotation()
    {
        Vector3 currentRotation = transform.rotation.eulerAngles;
        float newZRotation = currentRotation.z - Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime;
        return new Vector3(currentRotation.x, currentRotation.y, newZRotation);
    }

    private void Rotate(Vector3 rotation)
    {
        // Updates rigidbody velocity
        Vector2 direction = AngleToVector2(rotation.z); // If angle is zero, default direction should be (0, 1) instead of (1, 0)
        direction.Normalize();
        rigidBody2D.SetRotation(rotation.z);
    }

    private void Move()
    {
        Vector3 dir = GetCurrentDirection();
        Vector2 nextPos = transform.position + dir * speed * Time.deltaTime;
        rigidBody2D.MovePosition(nextPos);
    }
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
        return !dashing && !dashInCooldown;
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

    private void Stun(Projectile p)
    {
        stunned = true;
        Invoke("Recover", stunDuration);
        StartInvulnerability();
    }

    private void Recover()
    {
        stunned = false;
        ResetAngularVelocity();
    }

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

    private void ResetAngularVelocity()
    {
        rigidBody2D.angularVelocity = 0;
    }

    Vector2 GetCurrentDirection()
    {
        Vector3 currentRotation = transform.rotation.eulerAngles;
        Vector2 dir = AngleToVector2(currentRotation.z);
        dir.Normalize();
        return dir;
    }

    Vector2 AngleToVector2(float angle)
    {
        return new Vector2(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle));
    }

    public void CollideWith(Projectile p, Collision2D collision)
    {
        if (!invulnerable)
        {
            Stun(p);
        }
    }
}
