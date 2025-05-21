using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GreenLeaf/Enemy Spawn Data")]
public class EnemySpawnData : ScriptableObject
{
    [Header("Spawn Clustering")]
    public int clusterCountMin = 0;
    public int clusterCountMax = 0;
    public GameObject enemyPrefab;
    public float spawnCost = 1f;
    public float spawnChance = 1f;
    public float minTimeToSpawn = 0f;
    public bool isElite = false;
}