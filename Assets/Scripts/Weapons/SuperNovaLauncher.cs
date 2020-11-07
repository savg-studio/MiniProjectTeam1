using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperNovaLauncher : WeaponBase
{
    public GameObject superNovaGo;

    protected override void OnUse()
    {
        GameObject.Instantiate(superNovaGo, GetOwnerPos(), Quaternion.identity);
    }
}
