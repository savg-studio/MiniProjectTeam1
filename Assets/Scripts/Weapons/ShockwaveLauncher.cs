using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockwaveLauncher : WeaponBase
{
    // GameObject
    public GameObject shockwaveGo;

    // Public params
    public float launchForce;

    protected override void OnUse()
    {
        var shockwaveClone = GameObject.Instantiate(shockwaveGo, GetOwnerPos(), Quaternion.identity);
        var shockwave = shockwaveClone.GetComponent<Shockwave>();
        shockwave.Init();
        shockwave.Launch(owner.GetFacingDir(), launchForce, ForceMode2D.Impulse);
    }
}
