using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkMetadata : MonoBehaviour
{
    public Vector2Int gridPosition;

    [Header("Procedural Props")]
    public GameObject[] propPrefabs; // Assign in prefab inspector
    public Transform propsContainer;

    public void GenerateProps()
    {
        // Clear existing props
        foreach (Transform child in propsContainer)
        {
            Destroy(child.gameObject);
        }

        int seed = gridPosition.x * 73856093 ^ gridPosition.y * 19349663; // Large primes for unique seeds
        System.Random rng = new System.Random(seed);

        int propCount = rng.Next(3, 7);
        for (int i = 0; i < propCount; i++)
        {
            float x = (float)rng.NextDouble() * 20f;
            float z = (float)rng.NextDouble() * 20f;
            Vector3 pos = transform.position + new Vector3(x, 0f, z);

            GameObject prefab = propPrefabs[rng.Next(0, propPrefabs.Length)];
            GameObject obj = Instantiate(prefab, pos, Quaternion.Euler(0f, rng.Next(0, 360), 0f), propsContainer);

            float scale = 0.9f + (float)rng.NextDouble() * 0.4f;
            obj.transform.localScale = Vector3.one * scale;
        }
    }
}
