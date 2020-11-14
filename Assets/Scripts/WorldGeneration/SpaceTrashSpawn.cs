using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceTrashSpawn : Spawn
{
    public float minAngularVelocity;
    public float maxAngularVelocity;

    public float minSize;
    public float maxSize;

    protected override void OnSpawn(GameObject go)
    {
        SetRandomSize(go.transform);

        SpaceTrash st = go.GetComponent<SpaceTrash>();

        st.Init();
        st.SetRandomRotation();
        st.SetAngularVelocity(Random.Range(minAngularVelocity, maxAngularVelocity));
    }

    protected void SetRandomSize(Transform transform)
    {
        var size = Random.Range(minSize, maxSize);
        transform.localScale = new Vector3(size, size, 1);
    }

    
}
