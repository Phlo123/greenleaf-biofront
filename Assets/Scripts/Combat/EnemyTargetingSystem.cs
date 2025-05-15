using System.Collections.Generic;
using UnityEngine;

public static class EnemyTargetingSystem
{
    public static Transform FindClosestEnemy(Vector3 origin, float range, LayerMask enemyLayer)
    {
        Collider[] hits = Physics.OverlapSphere(origin, range, enemyLayer);

        float closestDist = Mathf.Infinity;
        Transform closest = null;

        foreach (var hit in hits)
        {
            float dist = Vector3.Distance(origin, hit.transform.position);
            if (dist < closestDist)
            {
                closest = hit.transform;
                closestDist = dist;
            }
        }

        return closest;
    }

    public static List<Transform> FindMultipleEnemies(Vector3 origin, float range, int maxCount, LayerMask enemyLayer)
    {
        Collider[] hits = Physics.OverlapSphere(origin, range, enemyLayer);
        List<Transform> targets = new List<Transform>();

        foreach (var hit in hits)
        {
            if (targets.Count >= maxCount) break;
            targets.Add(hit.transform);
        }

        return targets;
    }

    public static Transform FindEnemyInFront(Vector3 origin, Vector3 forward, float range, float maxAngle, LayerMask enemyLayer)
    {
        Collider[] hits = Physics.OverlapSphere(origin, range, enemyLayer);
        Transform bestTarget = null;
        float closestAngle = maxAngle;

        foreach (var hit in hits)
        {
            Vector3 dir = (hit.transform.position - origin).normalized;
            float angle = Vector3.Angle(forward, dir);
            if (angle <= maxAngle && angle < closestAngle)
            {
                closestAngle = angle;
                bestTarget = hit.transform;
            }
        }

        return bestTarget;
    }
}
