using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ChunkManager : MonoBehaviour
{
    [Header("Chunk Settings")]
    public GameObject[] chunkPrefabs;
    public int chunkSize = 20;
    public int loadRadius = 3;
    public int unloadRadius = 4;
    public Transform player;

    private Dictionary<Vector2Int, GameObject> activeChunks = new();
    private Queue<GameObject> chunkPool = new();

    void Update()
    {
        Vector2Int currentChunkCoord = GetChunkCoordFromPosition(player.position);
        HashSet<Vector2Int> requiredChunks = new();

        // Load required chunks
        for (int x = -loadRadius; x <= loadRadius; x++)
        {
            for (int y = -loadRadius; y <= loadRadius; y++)
            {
                Vector2Int coord = currentChunkCoord + new Vector2Int(x, y);
                requiredChunks.Add(coord);

                if (!activeChunks.ContainsKey(coord))
                {
                    GameObject chunk = GetChunkFromPool(coord);
                    activeChunks.Add(coord, chunk);
                }
            }
        }

        // Unload far chunks
        List<Vector2Int> toRemove = new();
        foreach (var kvp in activeChunks)
        {
            float distance = Vector2Int.Distance(kvp.Key, currentChunkCoord);
            if (distance > unloadRadius)
            {
                kvp.Value.SetActive(false);
                chunkPool.Enqueue(kvp.Value);
                toRemove.Add(kvp.Key);
            }
        }
        foreach (var coord in toRemove)
        {
            activeChunks.Remove(coord);
        }
    }

    Vector2Int GetChunkCoordFromPosition(Vector3 pos)
    {
        return new Vector2Int(Mathf.FloorToInt(pos.x / chunkSize), Mathf.FloorToInt(pos.z / chunkSize));
    }

    GameObject GetChunkFromPool(Vector2Int coord)
    {
        GameObject chunk = (chunkPool.Count > 0)
            ? chunkPool.Dequeue()
            : Instantiate(chunkPrefabs[Random.Range(0, chunkPrefabs.Length)]);

        chunk.transform.position = new Vector3(coord.x * chunkSize, 0, coord.y * chunkSize);
        chunk.GetComponent<ChunkMetadata>().gridPosition = coord;
        chunk.SetActive(true);
        chunk.GetComponent<ChunkMetadata>().GenerateProps();
        return chunk;
    }
}

