﻿using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Security.Cryptography;
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

    // State
    public float stunDuration = 2f;
    private bool stunned = false;

    // Push
    public float pushMod = 1f;

    // Components
    private Rigidbody2D rigidBody2D;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
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
    }

    void FixedUpdate()
    {
        if (!dashing && !stunned)
        {
            ResetAngularVelocity();

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

    private void Stun(MeteorScript ms)
    {
        stunned = true;

        //rigidBody2D.freezeRotation = false;
    }

    private void Push(Vector2 dir, float force)
    {
        rigidBody2D.AddForce(dir * force);
    }
    private void Recover()
    {
        stunned = false;
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

    public void CollideWith(MeteorScript ms, Collision2D collision)
    {
        Stun(ms);
        Invoke("Recover", stunDuration);
    }
}
