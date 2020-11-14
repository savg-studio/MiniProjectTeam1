using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    // Public params
    public float generationCooldown;

    // Inner
    private Vector2 chunkIndex;
    private WorldGenerator worldGenerator;
    private SpriteRenderer spriteRenderer;

    private bool generationEnabled = true;

    public Vector2 GetSize()
    {
        return spriteRenderer.size;
    }

    public void Construct(Vector2 chunkIndex, WorldGenerator worldGenerator)
    {
        this.chunkIndex = chunkIndex;
        this.worldGenerator = worldGenerator;
        gameObject.name = "Chunk" + chunkIndex;

        this.spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Player potentialPlayer = collision.gameObject.GetComponent<Player>();

        if(potentialPlayer && worldGenerator && generationEnabled)
        {
            CallWorldGenerator();
        }
    }

    private void CallWorldGenerator()
    {
        worldGenerator.OnPlayerEnterChunk(chunkIndex);
        generationEnabled = false;
        Invoke("EnableGeneration", generationCooldown);
    }

    private void EnableGeneration()
    {
        generationEnabled = true;
    }
}
