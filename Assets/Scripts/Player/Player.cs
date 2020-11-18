using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Spaceship
{
    // UI
    public ArmorDisplay armorDisplay;
    public Text speedDisplay;
    public MissionTracker missionTracker;

    // Camera
    public Camera mainCamera;
    public float maxCameraZoom;
    private float baseCameraSize;

    // Speed
    public float maxSpeed;
    public float minSpeed;
    public float baseSpeed;
    public float rotationSpeed;
    public float acceleration;
    private float speed;
    private float accumulatedSpeed;

    // Second weapon
    private WeaponBase secondWeapon;

    // Components
    private Rigidbody2D rigidBody2D;
    private Animation deathAnimation;

    // Start is called before the first frame update
    protected override void OnStart()
    {
        // Components
        rigidBody2D = GetComponent<Rigidbody2D>();
        deathAnimation = GetComponent<Animation>();

        // UI
        armorDisplay.SetMaxArmor(maxArmor);
        armorDisplay.SetCurrentArmor(maxArmor);

        // Camera
        baseCameraSize = mainCamera.orthographicSize;
    }

    // Update is called once per frame
    protected override void OnUpdate() 
    {
        // Movement speed
        float axis = (Input.GetAxis("Vertical") + 1) / 2;
        float speed = axis * (maxSpeed - minSpeed) + minSpeed;

        if (speed >= maxSpeed)
            SetSpeed(maxSpeed + accumulatedSpeed);
        else
        {
            SetSpeed(speed);
            accumulatedSpeed = 0;
        }

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

    protected override void  OnFixedUpdate()
    {
        if (!HasFlag(SpaceshipStateFlags.STUNNED))
        {
            Vector3 newRotation = GetInputRotation();
            Rotate(newRotation);
            IncreaseAccumulatedSpeed();
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

    public void SetSpeed(float speed)
    {
        this.speed = speed;

        // Camera
        ScaleCameraBySpeed(speed);

        // Speed display
        const float speedScaler = 100f;
        int fakeSpeed = Mathf.RoundToInt(speed * speedScaler);
        speedDisplay.text = fakeSpeed + " km/h";
    }

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

    private void IncreaseAccumulatedSpeed()
    {
        accumulatedSpeed += acceleration;
    }

    private void Move()
    {
        Vector3 dir = GetCurrentDirection();
        Vector2 nextPos = transform.position + dir * speed * Time.deltaTime;
        rigidBody2D.MovePosition(nextPos);
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

    protected override void OnStun()
    {

        accumulatedSpeed = 0;
    }

    // Damage

    protected override void OnDamageTaken()
    {
        armorDisplay.SetCurrentArmor(currentArmor);
    }

    // Armor

    protected override void OnRestoreArmor()
    {
        armorDisplay.SetCurrentArmor(currentArmor);
    }

    // Death

    protected override void OnDeath()
    {
        missionTracker.Fail();
        deathAnimation.Play();
    }

    // Camera
    private void ScaleCameraBySpeed(float speed)
    {
        var calcRatio = Mathf.Max((speed / maxSpeed), 1);
        var ratio = Mathf.Min(calcRatio, maxCameraZoom);
        mainCamera.orthographicSize = baseCameraSize * ratio;
    }
}
