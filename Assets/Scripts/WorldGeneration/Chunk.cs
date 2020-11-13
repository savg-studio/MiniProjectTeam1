using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    // Inner
    private Vector2 chunkIndex;
    private WorldGenerator worldGenerator;

    private SpriteRenderer spriteRenderer;

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
        Debug.Log("Something entered this chunk");

        if(potentialPlayer && worldGenerator)
        {
            worldGenerator.OnPlayerEnterChunk(chunkIndex);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("Something stayed this chunk");
    }
}
