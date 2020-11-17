using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

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

    private Vector2 currentChunkIndex;

    // Inner
    private Dictionary<Vector2, Chunk> chunks = new Dictionary<Vector2, Chunk>();
    private Vector2 chunkSize;

    private void Start()
    {
        var chunk = chunkPrefab.GetComponent<Chunk>();
        chunk.Construct(Vector2.zero);
        chunkSize = chunk.GetSize();

        CreateOriginChunk();
        OnPlayerEnterChunk(Vector2.zero);
        Invoke("SpawnBlackHole", blackHoleSpawnTime);
    }

    private void Update()
    {
        //Debug.Log("Current chunk is " + CalculateCurrentChunkIndex());
        var chunkIndex = CalculateCurrentChunkIndex();
        if(chunkIndex != currentChunkIndex)
        {
            OnPlayerEnterChunk(chunkIndex);
            //Debug.Log("Current chunk is " + chunkIndex);
        }
    }

    public Vector2 CalculateCurrentChunkIndex()
    {
        Vector2 playerPos = player.transform.position;
        Vector2 chunkOffset = chunkSize / 2;

        int indexX = Mathf.FloorToInt((playerPos.x + chunkOffset.x) / chunkSize.x);
        int indexY = Mathf.FloorToInt((playerPos.y + chunkOffset.y) / chunkSize.y);

        return new Vector2(indexX, indexY);
    }

    private void CreateOriginChunk()
    {
        chunks.Add(Vector2.zero, originChunk);
        originChunk.Construct(Vector2.zero);

        if (chunkContainer)
            originChunk.transform.parent = chunkContainer;
    }

    public void OnPlayerEnterChunk(Vector2 chunkIndex)
    {
        var chunksToActivate = GetChunksToActivateAround(chunkIndex);
        var chunksToDisable = GetChunksToDisable(chunksToActivate);

        StartCoroutine(GenerateOrActivateChunksCR(chunksToActivate));
        StartCoroutine(DisableChunksCR(chunksToDisable));

        //GenerateOrActivateChunks(chunksToActivate);
        //DisableChunks(chunksToDisable);
        
        currentChunkIndex = chunkIndex;
    }

    private IEnumerator GenerateOrActivateChunksCR(List<Vector2> chunksToActivate)
    {
        foreach (var chunkPos in chunksToActivate)
        {
            GenerateOrActivateChunk(chunkPos);
            yield return new WaitForFixedUpdate();
        }
    }

    private void GenerateOrActivateChunks(List<Vector2> chunksToActivate)
    {
        foreach (var chunkPos in chunksToActivate)
        {
            GenerateOrActivateChunk(chunkPos);
        }
    }

    private void GenerateOrActivateChunk(Vector2 chunkPos)
    {
        if (chunks.ContainsKey(chunkPos))
        {
            var chunk = chunks[chunkPos];
            chunk.gameObject.SetActive(true);
        }
        else
        {
            var chunk = CreateChunkAt(chunkPos);
            chunks.Add(chunkPos, chunk);
            chunk.gameObject.SetActive(true);
        }
    }

    private Chunk CreateChunkAt(Vector2 pos)
    {
        var chunkPrefabChunk = chunkPrefab.GetComponent<Chunk>();
        var scaledPos = pos * chunkSize;
        var chunkGo = GameObject.Instantiate(chunkPrefab, scaledPos, Quaternion.identity, chunkContainer);
        var chunk = chunkGo.GetComponent<Chunk>();

        chunk.Construct(pos);

        return chunk;
    }

    private List<Vector2> GetChunksToActivateAround(Vector2 basePos)
    {
        List<Vector2> chunksToActivate = new List<Vector2>();
        for (int i = -chunkDistance; i <= chunkDistance; i++)
        {
            for (int j = -chunkDistance; j <= chunkDistance; j++)
            {
                Vector2 pos = new Vector2(i, j);
                Vector2 chunkPos = pos + basePos;
                chunksToActivate.Add(chunkPos);
            }
        }

        return chunksToActivate;
    }

    private List<Vector2> GetChunksToDisable(List<Vector2> chunksToActivate)
    {
        List<Vector2> chunksToDisable = new List<Vector2>();

        foreach(var chunkPair in chunks)
        {
            if (!chunksToActivate.Contains(chunkPair.Key))
                chunksToDisable.Add(chunkPair.Key);
        }

        return chunksToDisable;
    }

    public IEnumerator DisableChunksCR(List<Vector2> chunkToDisable)
    {
        foreach(var chunkPos in chunkToDisable)
        {
            Chunk chunk = chunks[chunkPos];
            if (chunk.gameObject.activeInHierarchy)
            {
                chunk.gameObject.SetActive(false);
                yield return new WaitForFixedUpdate();
            }
        }
    }

    public void DisableChunks(List<Vector2> chunkToDisable)
    {
        foreach (var chunkPos in chunkToDisable)
        {
            Chunk chunk = chunks[chunkPos];
            if (chunk.gameObject.activeInHierarchy)
            {
                chunk.gameObject.SetActive(false);
            }
        }
    }

    // Black hole
    private void SpawnBlackHole()
    {
        GameObject.Instantiate(blackHolePrefab);
    }
}

