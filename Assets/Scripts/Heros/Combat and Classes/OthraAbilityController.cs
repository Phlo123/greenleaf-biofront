using UnityEngine;

public class OthraAbilityController : HeroAbilityController
{
    [Header("Combat Stats")]
    public ScaledStat damage;
    public ScaledStat fireRate;
    public ScaledStat attackRange;
    public ScaledStat coneAngle;
    public ScaledStat rotationSpeed;

    public override float FireRate => fireRate.Value;
    public override float AttackRange => attackRange.Value;
    public override float RotationSpeed => rotationSpeed.Value;
    public float ConeAngle => coneAngle.Value;
    public float TotalDamage => damage.Value;

    [Header("Visuals")]
    public GameObject lightningPrefab;
    public Transform firePoint;
    public LayerMask enemyLayer;

    [Header("Lightning Settings")]
    public int boltCount = 3;
    public int segments = 10;
    public float spread = 0.2f;
    public float zigZagAmplitude = 0.1f;

    public override void Fire(Vector3 direction, Transform target)
    {
        if (target == null || !IsInCone(target)) return;

        var enemy = target.GetComponent<EnemyDummy>();
        if (enemy != null)
            enemy.TakeDamage(TotalDamage);

        for (int i = 0; i < boltCount; i++)
            CreateZigZagLightning(firePoint.position, target.position);
    }

    public override Vector3 GetAutoTargetDirection()
    {
        Collider[] enemies = Physics.OverlapSphere(transform.position, AttackRange, enemyLayer);
        Transform best = null;
        float bestDist = Mathf.Infinity;

        foreach (var e in enemies)
        {
            float dist = Vector3.Distance(transform.position, e.transform.position);
            if (dist < bestDist)
            {
                best = e.transform;
                bestDist = dist;
            }
        }

        if (best == null) return Vector3.zero;

        Vector3 dir = best.position - transform.position;
        dir.y = 0f;
        return dir.normalized;
    }

    private bool IsInCone(Transform target)
    {
        Vector3 toTarget = target.position - transform.position;
        toTarget.y = 0f;
        return Vector3.Angle(transform.forward, toTarget) <= ConeAngle * 0.5f;
    }

    private void CreateZigZagLightning(Vector3 start, Vector3 end)
    {
        GameObject zap = Instantiate(lightningPrefab);
        LineRenderer lr = zap.GetComponent<LineRenderer>();

        if (lr != null)
        {
            lr.positionCount = segments + 1;
            Vector2 randOffset = Random.insideUnitCircle * spread;
            Vector3 startOffset = start + new Vector3(randOffset.x, 0, randOffset.y);
            Vector3 direction = (end - startOffset).normalized;
            Vector3 side = Vector3.Cross(direction, Vector3.up);

            for (int i = 0; i <= segments; i++)
            {
                float t = i / (float)segments;
                Vector3 point = Vector3.Lerp(startOffset, end, t);
                if (i != 0 && i != segments)
                    point += side * Random.Range(-zigZagAmplitude, zigZagAmplitude);
                lr.SetPosition(i, point);
            }
        }

        Destroy(zap, 0.1f);
    }
#if UNITY_EDITOR
private void OnDrawGizmosSelected()
{
    if (firePoint == null) return;

    Gizmos.color = Color.cyan;

    Vector3 origin = firePoint.position;
    Vector3 forward = firePoint.forward;
    float angle = coneAngle.Value;
    float range = attackRange.Value;

    // Draw two lines to indicate the cone edges
    Quaternion leftRot = Quaternion.Euler(0, -angle * 0.5f, 0);
    Quaternion rightRot = Quaternion.Euler(0, angle * 0.5f, 0);

    Vector3 leftDir = leftRot * forward * range;
    Vector3 rightDir = rightRot * forward * range;

    Gizmos.DrawLine(origin, origin + leftDir);
    Gizmos.DrawLine(origin, origin + rightDir);

    // Draw the arc outline with a few points
    int segments = 12;
    Vector3 prevPoint = origin + (Quaternion.Euler(0, -angle * 0.5f, 0) * forward * range);

    for (int i = 1; i <= segments; i++)
    {
        float lerpT = i / (float)segments;
        float currentAngle = Mathf.Lerp(-angle * 0.5f, angle * 0.5f, lerpT);
        Vector3 nextPoint = origin + (Quaternion.Euler(0, currentAngle, 0) * forward * range);
        Gizmos.DrawLine(prevPoint, nextPoint);
        prevPoint = nextPoint;
    }
}
#endif
}

