using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserWeapon : WeaponBase
{
    // Public params
    public float laserDuration;
    private float laserDurationLeft;

    // Cache components
    private GameObject laserObject;

    // Start is called before the first frame update
    void Start()
    {
        // Find component
        laserObject = transform.Find("LaserAttack").gameObject;
    }

    protected override void OnUse()
    {
        laserObject.SetActive(true);

        StartActiveFrames();
    }

    protected override void OnUpdate()
    {
        if(IsActive())
        {
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
}
