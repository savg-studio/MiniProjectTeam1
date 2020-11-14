using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceTrashSpawn : Spawn
{
    public float minTorque;
    public float maxTorque;

    public float minSize;
    public float maxSize;

    protected override void OnSpawn(GameObject go)
    {
        var rigidBody = go.GetComponent<Rigidbody2D>();

        SetRandomSize(go.transform);
        AddToque(rigidBody);
        SetRandomRotation(rigidBody);
    }

    protected void SetRandomSize(Transform transform)
    {
        var size = Random.Range(minSize, maxSize);
        transform.localScale = new Vector3(size, size, 1);
    }

    protected void AddToque(Rigidbody2D rigidBody)
    {
        var dir = Random.Range(0, 1) == 1 ? 1 : -1;
        var torque = Random.Range(minTorque, maxTorque) * dir;
        rigidBody.AddTorque(torque);
    }

    protected void SetRandomRotation(Rigidbody2D rigidbody)
    {
        float rotation = Random.Range(-180, 180);
        rigidbody.SetRotation(rotation);
    }
}
