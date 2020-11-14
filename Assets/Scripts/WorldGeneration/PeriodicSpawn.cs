using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeriodicSpawn : MonoBehaviour
{
    public float minSpawnInterval = 4f;
    public float maxSpawnInterval = 8f;
    public bool startEnabled;

    public Spawn child;

    private void Awake()
    {
        float firstSpawnTime = GetNextSpawnTime();
        child.Start();
        child.enabled = startEnabled;
        Invoke("OnSpawnObjects", firstSpawnTime);
    }

    protected void OnSpawnObjects()
    {
        child.enabled = true;

        float nextSpawnTime = Random.Range(minSpawnInterval, maxSpawnInterval);
        if(child.ShouldSpawn())
            child.SpawnObjects();
        Invoke("OnSpawnObjects", nextSpawnTime);
    }

    private float GetNextSpawnTime()
    {
        return Random.Range(minSpawnInterval, maxSpawnInterval);
    }
}
