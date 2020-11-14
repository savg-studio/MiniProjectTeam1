using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    // Public params
    public GameObject chunkPrefab;
    public Chunk originChunk;
    public GameObject player;
    public Transform chunkContainer;
    public int chunkDistance;
    public GameObject blackHolePrefab;
    public float blackHoleSpawnTime;

    // Inner
    private Dictionary<Vector2, Chunk> chunks = new Dictionary<Vector2, Chunk>();

    private void Start()
    {
        CreateOriginChunk();
        GenerateOrActivateChunksAround(Vector2.zero);
        Invoke("SpawnBlackHole", blackHoleSpawnTime);
    }

    private void CreateOriginChunk()
    {
        chunks.Add(Vector2.zero, originChunk);
        originChunk.Construct(Vector2.zero, this);

        if (chunkContainer)
            originChunk.transform.parent = chunkContainer;
    }

    private void GenerateOrActivateChunksAround(Vector2 basePos)
    {
        for(int i = -chunkDistance; i < chunkDistance; i++)
        {
            for(int j = -chunkDistance; j < chunkDistance; j++)
            {
                Vector2 pos = new Vector2(i, j);
                Vector2 chunkPos = pos + basePos;
                if (chunks.ContainsKey(chunkPos))
                {
                    var chunk = chunks[chunkPos];
                    chunk.gameObject.SetActive(true);
                }
                else
                {
                    var chunk = CreateChunkAt(chunkPos);
                    chunks.Add(chunkPos, chunk);
                }
            }
        }
    }

    private Chunk CreateChunkAt(Vector2 pos)
    {
        var chunkGo = GameObject.Instantiate(chunkPrefab);
        var chunk = chunkGo.GetComponent<Chunk>();
        
        chunk.Construct(pos, this);

        // Set real pos
        pos.Scale(chunk.GetSize());
        chunkGo.transform.position = pos;

        // Set object parent
        if (chunkContainer)
            chunkGo.transform.parent = chunkContainer;

        // Enable chunk to spawn objects
        chunkGo.SetActive(true);

        return chunk;
    }

    public void OnPlayerEnterChunk(Vector2 chunkIndex)
    {
        DisableEveryChunk();
        GenerateOrActivateChunksAround(chunkIndex);
    }

    public void DisableEveryChunk()
    {
        foreach(var chunkPair in chunks)
        {
            Chunk chunk = chunkPair.Value;
            chunk.gameObject.SetActive(false);
        }
    }


    private void SpawnBlackHole()
    {
        GameObject.Instantiate(blackHolePrefab);
    }
}

