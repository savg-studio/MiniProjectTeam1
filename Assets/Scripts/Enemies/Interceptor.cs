﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interceptor : AISpaceship
{
    public int frames;

    protected override void OnAIStart()
    {
    }

    protected override void UpdateOnCombat()
    {
        var distanceToPlayer = GetVectorToPlayer().magnitude;
        var effectiveFrames = frames / distanceToPlayer;
        var point = GetPlayerPos() - player.GetVelocity() * effectiveFrames;
        Vector2 pos = transform.position;
        //rigidbody2D.SetRotation(T1Utils.Vector2ToAngle(point - pos));

        Debug.DrawLine(transform.position, point);
        agent.Seek(point);
        agent.CollisonAvoidance();

        if (CanUseWeapon())
            weapon.Use();
    }

    private Vector2 GetVectorToPlayer()
    {
        Vector2 myPos = transform.position;
        Vector2 playerPos = GetPlayerPos();
        Vector2 distV = playerPos - myPos;

        return distV;
    }
}
