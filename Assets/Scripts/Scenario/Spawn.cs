using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject objectToSpawn;
    public float chanceToSpawn = 0.75f;
    public uint minAmount;
    public uint maxAmount;

    private BoxCollider2D boxCollider;

    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();

        OnStart();
    }

    protected virtual void OnStart()
    {
        SpawnObjects();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnObjects()
    {
        // Spawns minimum amount
        for (uint i = 0; i < minAmount; i++)
            SpawnObject();

        for (uint i = minAmount; i < maxAmount; i++)
            if (ShouldSpawn())
                SpawnObject();

        OnSpawnObjects();
    }
    protected virtual void OnSpawnObjects()
    {

    }

    protected void SpawnObject()
    {
        Vector3 spawnPoint = GetRandomPointInBounds();
        var go = GameObject.Instantiate(objectToSpawn, spawnPoint, Quaternion.identity);

        OnSpawn(go);
    }

    public bool ShouldSpawn()
    {
        float roll = Random.Range(0f, 1f);
        return OnShouldSpawn() && roll < chanceToSpawn;
    }

    protected virtual void OnSpawn(GameObject go)
    {
            
    }

    protected virtual bool OnShouldSpawn()
    {
        return true;
    }

    public Vector2 GetRandomPointInBounds()
    {
        return T1Utils.GetRandomPointInBounds(boxCollider.bounds);
    }
}
