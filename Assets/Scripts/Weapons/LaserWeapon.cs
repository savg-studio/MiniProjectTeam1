using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserWeapon : WeaponBase
{
    // Public params
    public float laserDuration;
    public float laserMaxDistance;
    public LayerMask layerMask;
    private float laserDurationLeft;
    public float laserScaler;

    // Cache components
    private GameObject laserObject;
    private GameObject laserRef;

    // Start is called before the first frame update
    void Start()
    {
        // Find component
        laserObject = transform.Find("LaserAttack").gameObject;
        laserRef = transform.Find("LaserRef").gameObject;
    }

    protected override void OnUse()
    {
        ResizeLaser();
        laserObject.SetActive(true);

        StartActiveFrames();
    }

    protected override void OnUpdate()
    {
        if(IsActive())
        {
            ResizeLaser();

            bool over = DecreaseDuration();
            if (over)
                OnActiveFramesExpired();
        }
    }

    private bool IsActive()
    {
        return laserDurationLeft > 0;
    }

    private bool DecreaseDuration()
    {
        laserDurationLeft -= Time.deltaTime;

        return !IsActive();
    }

    private void StartActiveFrames()
    {
        laserDurationLeft = laserDuration;
    }

    private void OnActiveFramesExpired()
    {
        laserObject.SetActive(false);
    }

    private void ResizeLaser()
    {
        var rayOrigin = laserRef.transform.position;
        var dir = owner.GetFacingDir();
        var hit = Physics2D.Raycast(rayOrigin, dir, laserMaxDistance, layerMask);
        if (hit)
        {
            Debug.Log("Hit against object " + hit.collider.gameObject.name + " of layer " + LayerMask.LayerToName(hit.collider.gameObject.layer));
            var baseScale = laserObject.transform.localScale;
            var basePos = laserObject.transform.localPosition;
            var parentScaler = (1 / owner.transform.localScale.x);
            baseScale.x = hit.distance * parentScaler + laserRef.transform.localPosition.x;
            // Adds custom offset to guarantee collisions
            baseScale.x = baseScale.x * laserScaler;
            basePos.x = baseScale.x / 2;

            laserObject.transform.localScale = baseScale;
            laserObject.transform.localPosition = basePos;
        }
    }
}
