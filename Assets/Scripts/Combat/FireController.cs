using UnityEngine;

public class FireController : MonoBehaviour
{
    public HeroAttackProfile profile;
    public Transform firePoint;
    public LayerMask enemyLayer;
    public GameObject lightningPrefab;

    private float fireCooldown = 0f;
    private bool isAutoFire = false;
    private bool isClickHeld = false;

    void Update()
    {
        fireCooldown -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Q)) isAutoFire = !isAutoFire;
        if (Input.GetMouseButtonDown(0)) isClickHeld = true;
        if (Input.GetMouseButtonUp(0)) isClickHeld = false;

        if (isClickHeld) ManualFire();
        else if (isAutoFire) AutoFire();
    }

    void ManualFire()
    {
        Vector3 target = GetMouseWorldPos();
        Vector3 dir = (target - transform.position).normalized;
        dir.y = 0f;
        transform.rotation = Quaternion.LookRotation(dir);

        TryFire(dir);
    }

    void AutoFire()
    {
        Collider[] enemies = Physics.OverlapSphere(transform.position, profile.attackRange * 3f, enemyLayer);
        if (enemies.Length == 0) return;

        Transform closest = enemies[0].transform;
        float dist = Vector3.Distance(transform.position, closest.position);

        foreach (var enemy in enemies)
        {
            float d = Vector3.Distance(transform.position, enemy.transform.position);
            if (d < dist) { dist = d; closest = enemy.transform; }
        }

        Vector3 dir = (closest.position - transform.position).normalized;
        dir.y = 0f;
        transform.rotation = Quaternion.LookRotation(dir);

        TryFire(dir);
    }

    void TryFire(Vector3 direction)
    {
        if (fireCooldown > 0f) return;

        switch (profile.attackType)
        {
            case HeroAttackProfile.AttackType.Raycast:
                ZapTargetInCone(direction);
                break;

            case HeroAttackProfile.AttackType.Melee:
                Vector3 center = firePoint.position + dir * profile.attackRange * 0.5f;
                Collider[] hits = Physics.OverlapSphere(center, profile.attackRange, enemyLayer);
                foreach (var h in hits)
                {
                    Debug.Log("Melee hit: " + h.name);
                    // TODO: h.GetComponent<Enemy>()?.TakeDamage(profile.damage);
                }
                break;

            case HeroAttackProfile.AttackType.Projectile:
                Vector3 spawnPos = firePoint.position + transform.forward * 1f;
                GameObject proj = Instantiate(profile.projectilePrefab, spawnPos, Quaternion.LookRotation(dir, Vector3.up));

                // Prevent it from hitting the player instantly
                Collider projectileCollider = proj.GetComponent<Collider>();
                Collider playerCollider = GetComponent<Collider>();
                if (projectileCollider != null && playerCollider != null)
                {
                    Physics.IgnoreCollision(projectileCollider, playerCollider);
                }

                // Add Rigidbody or script to handle movement/explosion
                break;
        }

        fireCooldown = profile.fireRate;
    }
    void ZapTargetInCone(Vector3 direction)
    {
        float coneAngle = 60f;          // Half-angle of cone in degrees
        float maxDistance = 15f;        // Max range
        Collider[] enemies = Physics.OverlapSphere(firePoint.position, maxDistance, enemyLayer);

        Transform bestTarget = null;
        float closestDist = Mathf.Infinity;

        foreach (var enemy in enemies)
        {
            Vector3 toEnemy = enemy.transform.position - transform.position;
            toEnemy.y = 0f;

            float angle = Vector3.Angle(direction, toEnemy);
            if (angle <= coneAngle * 0.5f)
            {
                float dist = toEnemy.magnitude;
                if (dist < closestDist)
                {
                    closestDist = dist;
                    bestTarget = enemy.transform;
                }
            }
        }

        if (bestTarget != null)
        {
            Debug.Log("Zapped: " + bestTarget.name);

            // TODO: Damage enemy
            // bestTarget.GetComponent<Enemy>()?.TakeDamage(profile.damage);

            // TODO: Spawn lightning bolt visual
            // e.g. CreateLightningVFX(firePoint.position, bestTarget.position);
        }
    }

    void CreateLightningVFX(Vector3 start, Vector3 end)
    {
        GameObject zap = Instantiate(lightningPrefab); // prefab with LineRenderer
        LineRenderer lr = zap.GetComponent<LineRenderer>();
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        Destroy(zap, 0.1f); // destroy after short time
    }


    Vector3 GetMouseWorldPos()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        new Plane(Vector3.up, 0).Raycast(ray, out float dist);
        return ray.GetPoint(dist);
    }
}
