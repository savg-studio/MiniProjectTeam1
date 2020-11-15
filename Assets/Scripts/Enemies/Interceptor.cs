using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interceptor : AISpaceship
{
    public int frames;
    public bool scaleByDist;

    protected override void OnAIStart()
    {
    }

    protected override void UpdateOnCombat()
    {
        var distanceToPlayer = GetVectorToPlayer().magnitude;
        var effectiveFrames = scaleByDist ? Mathf.RoundToInt(frames / distanceToPlayer) : frames;
        var point = GetPlayerPos() - player.GetVelocity() * effectiveFrames;
        Vector2 pos = transform.position;
        //rigidbody2D.SetRotation(T1Utils.Vector2ToAngle(point - pos));

        Debug.DrawLine(transform.position, point);
        agent.Seek(point);
        //agent.Pursuit(GetPlayerPos(), player.GetVelocity());
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
