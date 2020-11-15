using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    // Inner
    private Vector2 chunkIndex;
    private SpriteRenderer spriteRenderer;

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
    }
}
