using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorSpawn : SpaceTrashSpawn
{
    public float minForce;
    public float maxForce;

    // Start is called before the first frame update
    protected override void OnStart()
    {
        base.OnStart();
    }

    override protected void OnSpawn(GameObject go)
    {
        base.OnSpawn(go);

        Meteor meteor = go.GetComponent<Meteor>();
        meteor.Init();

        // Launch direction
        var dir = Random.insideUnitCircle;
        meteor.Launch(dir.normalized, minForce, maxForce);
    }
}