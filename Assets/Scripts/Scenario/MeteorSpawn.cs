using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorSpawn : Spawn
{
    public GameObject spawnTarget;
    public float minForce;
    public float maxForce;

    public float minSize;
    public float maxSize;

    private MeteorZone meteorZone;

    // Start is called before the first frame update
    protected override void OnStart()
    {
        meteorZone = spawnTarget.GetComponent<MeteorZone>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    override protected void OnSpawn(GameObject go) 
    {
        MeteorScript meteor = go.GetComponent<MeteorScript>();
        meteor.Init();

        // Size
        meteor.Resize(minSize, maxSize);

        // Rotation
        float rotation = Random.Range(-180, 180);
        meteor.SetRotation(rotation);

        // Launch direction
        Vector3 point = meteorZone.GetRandomPointInBounds();
        var dir = point - meteor.transform.position;
        meteor.Launch(dir.normalized, minForce, maxForce);

        meteorZone.AddMeteor(meteor);
    }

    protected override bool ShouldSpawn()
    {
        return !meteorZone.IsFull();
    }
}
