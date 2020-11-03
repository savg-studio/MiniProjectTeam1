using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeriodicSpawn : MonoBehaviour
{
    public float minSpawnInterval = 4f;
    public float maxSpawnInterval = 8f;

    public Spawn child;

    private void Start()
    {
        float firstSpawnTime = GetNextSpawnTime();
        Invoke("OnSpawnObjects", firstSpawnTime);
    }

    protected void OnSpawnObjects()
    {
        float nextSpawnTime = Random.Range(minSpawnInterval, maxSpawnInterval);
        child.SpawnObjects();
        Invoke("OnSpawnObjects", nextSpawnTime);
    }

    private float GetNextSpawnTime()
    {
        return Random.Range(minSpawnInterval, maxSpawnInterval);
    }
}
