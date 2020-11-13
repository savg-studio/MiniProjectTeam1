using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    // Public params
    public GameObject chunkPrefab;
    public GameObject player;
    public int chunkDistance;

    // Inner
    private Dictionary<Vector2, Chunk> chunks = new Dictionary<Vector2, Chunk>();

    private void Start()
    {
        GenerateChunksAround(Vector2.zero);
    }

    private void GenerateChunksAround(Vector2 basePos)
    {
        for(int i = -chunkDistance; i < chunkDistance; i++)
        {
            for(int j = -chunkDistance; j < chunkDistance; j++)
            {
                Vector2 pos = new Vector2(i, j);
                Vector2 chunkPos = pos + basePos;
                if (!chunks.ContainsKey(chunkPos))
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

        // Enable chunk to spawn objects
        chunkGo.SetActive(true);

        return chunk;
    }

    public void OnPlayerEnterChunk(Vector2 chunkIndex)
    {
        GenerateChunksAround(chunkIndex);
    }
}
