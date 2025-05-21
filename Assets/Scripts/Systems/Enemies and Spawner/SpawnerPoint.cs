using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerPoint : MonoBehaviour
{
    public LayerMask playerLOSCheck;
    public float spawnOffset = 2f;

    private Transform player;

    private void Awake()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
        else
            Debug.LogWarning("[SpawnerPoint] No GameObject tagged 'Player' found in the scene.");
    }

    public bool TrySpawn(EnemySpawnData data)
    {
        if (player == null) return false;

        Vector3 dirToPlayer = (player.position - transform.position).normalized;
        if (Physics.Raycast(transform.position, dirToPlayer, out RaycastHit hit, 100f, playerLOSCheck))
        {
            if (hit.transform.CompareTag("Player")) return false;
        }

        Vector3 spawnPos = transform.position + Random.insideUnitSphere * spawnOffset;
        spawnPos.y = 0f;
        GameObject enemy = Instantiate(data.enemyPrefab, spawnPos, Quaternion.identity);
        EnemyTracker.Instance?.RegisterEnemy(enemy, data.spawnCost);
        return true;
    }
}
