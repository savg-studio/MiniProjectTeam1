using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    // Inner
    private Vector2 chunkIndex;
    private SpriteRenderer spriteRenderer;
    private bool isActive;

    // Cache
    private Spawn[] spawns;
    private GameObject scenario;

    public Vector2 GetSize()
    {
        return spriteRenderer.size;
    }

    public Vector2 GetCenter()
    {
        var center = chunkIndex;
        center.Scale(GetSize());
        return center;
    }

    public void Construct(Vector2 chunkIndex)
    {
        this.chunkIndex = chunkIndex;
        gameObject.name = "Chunk" + chunkIndex;

        this.spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        this.spawns = GetComponentsInChildren<Spawn>(true);
        isActive = true;
    }

    public void Activate()
    {
        gameObject.SetActive(true);
        isActive = true;

        /*
        foreach (var spawn in spawns)
            spawn.SpawnObjects();
        */
    }

    public void Deactivate()
    {
        
        gameObject.SetActive(false);
        isActive = false;
    }

    public bool IsActive()
    {
        return isActive;
    }
}
