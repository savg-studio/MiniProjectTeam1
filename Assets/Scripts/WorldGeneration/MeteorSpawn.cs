using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorSpawn : SpaceTrashSpawn
{
    public GameObject spawnTarget;
    public float minForce;
    public float maxForce;

    private MeteorZone meteorZone;

    // Start is called before the first frame update
    protected override void OnStart()
    {
        meteorZone = spawnTarget.GetComponent<MeteorZone>();

        base.OnStart();
    }

    override protected void OnSpawn(GameObject go) 
    {
        base.OnSpawn(go);

        Meteor meteor = go.GetComponent<Meteor>();
        meteor.Init();

        // Launch direction
        Vector3 point = meteorZone.GetRandomPointInBounds();
        var dir = point - meteor.transform.position;
        meteor.Launch(dir.normalized, minForce, maxForce);

        meteorZone.AddMeteor(meteor);
    }

    protected override bool OnShouldSpawn()
    {
        return !meteorZone.IsFull();
    }
}
