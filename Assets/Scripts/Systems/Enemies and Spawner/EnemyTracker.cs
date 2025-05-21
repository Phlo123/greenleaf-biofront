using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTracker : MonoBehaviour
{
    public static EnemyTracker Instance;
    private List<EnemyData> activeEnemies = new();

    void Awake()
    {
        Instance = this;
    }

    public void UnregisterEnemy(GameObject enemy)
    {
        activeEnemies.RemoveAll(e => e.enemy == enemy);
    }

    public void RegisterEnemy(GameObject enemy, float cost)
    {
        activeEnemies.Add(new EnemyData(enemy, cost));
    }

    public int TotalEnemies() => activeEnemies.RemoveAll(e => e.enemy == null) >= 0 ? activeEnemies.Count : 0;

    public float CurrentUsedBudget()
    {
        activeEnemies.RemoveAll(e => e.enemy == null);
        float total = 0;
        foreach (var e in activeEnemies) total += e.cost;
        return total;
    }

    private struct EnemyData
    {
        public GameObject enemy;
        public float cost;
        public EnemyData(GameObject e, float c)
        {
            enemy = e;
            cost = c;
        }
    }
}
