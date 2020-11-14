using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    // Public params
    public float minTimeInside;
    public float generationCooldown;

    // Inner
    private Vector2 chunkIndex;
    private WorldGenerator worldGenerator;
    private SpriteRenderer spriteRenderer;

    private bool playerInside = false;
    private bool generationEnabled = true;
    private Timer activateTimer;

    private void Update()
    {
        activateTimer.Update();
    }

    public Vector2 GetSize()
    {
        return spriteRenderer.size;
    }

    public void Construct(Vector2 chunkIndex, WorldGenerator worldGenerator)
    {
        this.chunkIndex = chunkIndex;
        this.worldGenerator = worldGenerator;
        gameObject.name = "Chunk" + chunkIndex;

        activateTimer = new Timer(minTimeInside);
        activateTimer.SetCallback(CallWorldGenerator);
        activateTimer.Stop();

        this.spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Player potentialPlayer = collision.gameObject.GetComponent<Player>();

        if(potentialPlayer && worldGenerator && !playerInside)
        {
            playerInside = true;
            activateTimer.Restart();
        }
    }

    private void CallWorldGenerator()
    {
        if (generationEnabled)
        {
            Debug.Log("Called world generation from " + chunkIndex);
            worldGenerator.OnPlayerEnterChunk(chunkIndex);
            generationEnabled = false;
            Invoke("EnableWorldGeneration", generationCooldown);
        }
    }

    private void EnableWorldGeneration()
    {
        generationEnabled = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Player potentialPlayer = collision.gameObject.GetComponent<Player>();

        if(potentialPlayer)
        {
            playerInside = false;
            activateTimer.Stop();
        }
    }
}
