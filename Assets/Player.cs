using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Security.Cryptography;
using UnityEngine;

public enum Direction
{
    NONE,
    LEFT = -1,
    RIGHT = 1
}

public class Player : MonoBehaviour
{
    public float baseSpeed;
    public float rotationSpeed;
    private float speed;

    public float dashForce;
    public float dashDuration;
    public float dashCooldown;

    private bool dashing = false;
    private bool dashInCooldown = false;
    private Rigidbody2D rigidBody2D;

    public float rotateThreshold;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        speed = baseSpeed;
    }

    // Update is called once per frame
    void Update() 
    {
        if (Input.GetButton("Fire1") && CanStartDash())
            Dash();
    }

    void FixedUpdate()
    {
        if (!dashing)
        {
            Direction dir = GetRotateDirection();
            Rotate(dir);
            Move();
        }
    }

    private void Rotate(Direction dir)
    {
        // Updates rigidbody velocity
        if (dir != Direction.NONE)
        {
            Vector3 currentRotation = transform.rotation.eulerAngles;
            var rotationSpeedDir = dir == Direction.RIGHT ? rotationSpeed : -rotationSpeed;
            float newZRotation = currentRotation.z - rotationSpeedDir * Time.deltaTime;
            Vector2 direction = AngleToVector2(newZRotation);
            direction.Normalize();
            rigidBody2D.SetRotation(newZRotation);
        }
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
    Direction GetRotateDirection()
    {
        Direction dir = Direction.NONE;

        Vector2 inputPlayerDir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;
        Vector2 currentDir = GetCurrentDirection();

        float py = inputPlayerDir.x * -currentDir.y + inputPlayerDir.y * currentDir.x;
        float px = inputPlayerDir.y * currentDir.y + inputPlayerDir.x * currentDir.x;
        if(Mathf.Abs(py) > rotateThreshold || px < 0)
            dir = py >= 0 ? Direction.LEFT : Direction.RIGHT;

        return dir;
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
}
