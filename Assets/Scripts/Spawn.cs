using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject objecToSpawn;
    public bool oneTime = false;
    public bool instant = true;
    public float minSpawnInterval = 4f;
    public float maxSpawnInterval = 8f;
    public float chanceToSpawn = 0.75f;

    private BoxCollider2D boxCollider;

    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();

        float firstSpawnTime = instant ? 0 : GetNextSpawnTime();
        Invoke("SpawnObject", firstSpawnTime);

        OnStart();
    }

    protected virtual void OnStart()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnObject()
    {
        if (DoSpawn())
        {
            Vector3 spawnPoint = GetRandomPointInBounds();
            var go = GameObject.Instantiate(objecToSpawn, spawnPoint, Quaternion.identity);

            OnSpawn(go);
        }

        if (!oneTime)
        {
            float nextSpawnTime = Random.Range(minSpawnInterval, maxSpawnInterval);
            Invoke("SpawnObject", nextSpawnTime);
        }
    }

    private bool DoSpawn()
    {
        float roll = Random.Range(0, 1);
        return ShouldSpawn() && roll < chanceToSpawn;
    }

    private float GetNextSpawnTime()
    {
        return Random.Range(minSpawnInterval, maxSpawnInterval);
    }

    protected virtual void OnSpawn(GameObject go)
    {
            
    }

    protected virtual bool ShouldSpawn()
    {
        return true;
    }

    public Vector2 GetRandomPointInBounds()
    {
        return T1Utils.GetRandomPointInBounds(boxCollider.bounds);
    }
}
