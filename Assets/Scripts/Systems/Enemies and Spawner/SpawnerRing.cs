using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerRing : MonoBehaviour
{
    [Header("Spawner Ring Settings")]
    public int spawnerCount = 8;
    public float ringRadius = 40f;
    public GameObject spawnerPrefab;
    public Transform player;

    [Header("Spawn Timing")]
    public float spawnCheckInterval = 1f;
    private float spawnTimer;
    private bool didInitialFill = false;

    [Header("Spawn Budget")]
    public float baseSpawnRate = 5f;
    public AnimationCurve spawnGrowthCurve;
    public List<EnemySpawnData> enemyTypes;

    [Header("Enemy Limits")]
    public List<TimeBoundLimit> enemyCaps;
    public int softOverflowAllowance = 3;

    [Header("Cluster Spawning")]
    public Vector2Int clusterSizeRange = new Vector2Int(1, 3); // min-max enemies per spawner use

    [Header("Debug")]
    public bool debugSpawns = false;

    [HideInInspector]
    public bool spawnersPaused = false;

    private List<SpawnerPoint> spawners = new();

    void Start()
    {
        GenerateRing();
    }

    void Update()
    {
        if (spawnersPaused) return;

        if (!didInitialFill)
        {
            TryFillEnemies();
            didInitialFill = true;
        }

        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnCheckInterval)
        {
            spawnTimer = 0f;
            TryFillEnemies();
        }
    }

    void GenerateRing()
    {
        for (int i = 0; i < spawnerCount; i++)
        {
            float angle = (360f / spawnerCount) * i * Mathf.Deg2Rad;
            Vector3 pos = player.position + new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * ringRadius;
            GameObject spawner = Instantiate(spawnerPrefab, pos, Quaternion.identity, transform);
            var spawnerScript = spawner.GetComponent<SpawnerPoint>();
            spawners.Add(spawnerScript);
        }
    }

    void TryFillEnemies()
    {
        float time = Time.timeSinceLevelLoad;
        int enemyCap = GetEnemyCap(time);
        int currentEnemyCount = EnemyTracker.Instance?.TotalEnemies() ?? 0;

        float maxBudget = baseSpawnRate + spawnGrowthCurve.Evaluate(time);
        float usedBudget = EnemyTracker.Instance?.CurrentUsedBudget() ?? 0f;
        float availableBudget = maxBudget - usedBudget;

        if (debugSpawns)
        {
            Debug.Log($"[SpawnerRing] Time: {time:F1}s | Budget: {availableBudget:F1}/{maxBudget} | Enemies: {currentEnemyCount}/{enemyCap}");
        }

        if (currentEnemyCount >= enemyCap + softOverflowAllowance || availableBudget <= 0f) return;

        List<EnemySpawnData> candidates = enemyTypes.FindAll(e =>
            time >= e.minTimeToSpawn &&
            availableBudget >= e.spawnCost &&
            EliteAllowed(time, e) &&
            Random.value <= e.spawnChance);

        if (candidates.Count == 0) return;

        List<SpawnerPoint> shuffledSpawners = new(spawners);
        Shuffle(shuffledSpawners);

        while (availableBudget > 0f && currentEnemyCount < enemyCap + softOverflowAllowance)
        {
            EnemySpawnData chosen = GetRandomSpawnableEnemy(candidates, availableBudget);
            if (chosen == null) break;

            int minCluster = chosen.clusterCountMin > 0 ? chosen.clusterCountMin : clusterSizeRange.x;
            int maxCluster = chosen.clusterCountMax > 0 ? chosen.clusterCountMax : clusterSizeRange.y;
            int clusterCount = Random.Range(minCluster, maxCluster + 1);

            foreach (var spawner in shuffledSpawners)
            {
                for (int i = 0; i < clusterCount; i++)
                {
                    if (spawner.TrySpawn(chosen))
                    {
                        currentEnemyCount++;
                        availableBudget -= chosen.spawnCost;

                        if (currentEnemyCount >= enemyCap + softOverflowAllowance || availableBudget < chosen.spawnCost)
                            return;
                    }
                }
            }
        }
    }

    int GetEnemyCap(float time)
    {
        foreach (var cap in enemyCaps)
        {
            if (time < cap.timeLimit) return cap.maxEnemies;
        }
        return enemyCaps[^1].maxEnemies;
    }

    bool EliteAllowed(float time, EnemySpawnData data)
    {
        if (!data.isElite) return true;

        foreach (var cap in enemyCaps)
        {
            if (time < cap.timeLimit) return cap.eliteAllowed;
        }

        return enemyCaps[^1].eliteAllowed;
    }

    EnemySpawnData GetRandomSpawnableEnemy(List<EnemySpawnData> pool, float available)
    {
        List<EnemySpawnData> valid = pool.FindAll(e => e.spawnCost <= available);
        return valid.Count > 0 ? valid[Random.Range(0, valid.Count)] : null;
    }

    void Shuffle<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }
}
