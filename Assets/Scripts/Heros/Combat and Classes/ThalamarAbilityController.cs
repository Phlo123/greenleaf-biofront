using UnityEngine;

public class ThalamarAbilityController : HeroAbilityController
{
    [Header("Combat Stats")]
    public ScaledStat damage;
    public ScaledStat fireRate;
    public ScaledStat attackRange;
    public ScaledStat coneAngle;
    public ScaledStat rotationSpeed;
    public ScaledStat projectileSpeed;
    public ScaledStat aoeRadius;

    public override float FireRate => fireRate.Value;
    public override float AttackRange => attackRange.Value;
    public override float RotationSpeed => rotationSpeed.Value;
    public float TotalDamage => damage.Value;
    public float ConeAngle => coneAngle.Value;

    [Header("Firing")]
    public GameObject spearPrefab;
    public Transform firePoint;
    public LayerMask enemyLayer;

    [Header("Talent Modifiers")]
    public int projectileCount = 1;
    public float spreadAngle = 10f;
    public bool pierces = false;
    public bool appliesFrost = false;

    public override void Fire(Vector3 direction, Transform target)
    {
        if (target == null || !IsInCone(target)) return;

        Vector3 aimDir = (target.position - firePoint.position).normalized;
        aimDir.y = 0f;
        firePoint.rotation = Quaternion.LookRotation(aimDir);

        for (int i = 0; i < projectileCount; i++)
        {
            float angleOffset = (projectileCount == 1) ? 0f :
                (i - (projectileCount - 1) / 2f) * spreadAngle;

            Vector3 rotatedDir = Quaternion.Euler(0, angleOffset, 0) * firePoint.forward;
            Vector3 spawn = firePoint.position + rotatedDir * 0.5f;

            GameObject spear = Instantiate(spearPrefab, spawn, Quaternion.LookRotation(rotatedDir));
            var spearScript = spear.GetComponent<ThalamarSpear>();
            if (spearScript != null)
            {
                spearScript.pierces = pierces;
                spearScript.appliesFrost = appliesFrost;
                spearScript.damage = Mathf.RoundToInt(TotalDamage);
                spearScript.speed = projectileSpeed.Value;
                spearScript.aoeRadius = aoeRadius.Value;
            }
        }
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
}
