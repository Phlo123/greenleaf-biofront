using UnityEngine;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(HeroStatsHandler))]
public class AbilityController : MonoBehaviour
{
    [Header("Attack Settings")]
    public GameObject lineFxPrefab;
    public Transform firePoint;
    private float attackCooldown;
    private float nextAttackTime = 0f;
    public ProjectileData basicAttackProjectile;
    public LayerMask enemyLayer;
    public GameObject projectilePrefab;
    public GameObject attackVfxPrefab;
    private TriggerRandomAttack attackTrigger;
    private HeroStatsHandler statsHandler;

    private void Awake()
    {
        statsHandler = GetComponent<HeroStatsHandler>();
        if (statsHandler == null)
            Debug.LogError("AbilityController: No HeroStatsHandler found.");
    }

    private void Start()
    {
        statsHandler = GetComponent<HeroStatsHandler>();
        attackTrigger = GetComponent<TriggerRandomAttack>();

        if (statsHandler == null)
            Debug.LogError("AbilityController: No HeroStatsHandler found.");

        if (attackTrigger == null)
            Debug.LogError("AbilityController: No TriggerRandomAttack found.");
    }

    private void Update()
    {
        if (CanAutoAttack())
            LaunchBasicAttack();
    }

    private void SpawnAttackVFX()
    {
        if (attackVfxPrefab)
        {
            GameObject vfx = Instantiate(attackVfxPrefab, firePoint.position, firePoint.rotation);
            Destroy(vfx, 1.5f); // Auto-cleanup after short delay
        }
    }

    public void LaunchBasicAttack()
    {
        if (!CanCast()) return;

        if (attackTrigger != null)
            attackTrigger.TriggerAttack();
        else
            Debug.LogWarning("AbilityController: attackTrigger is null");

        int count = basicAttackProjectile.count;
        float spread = basicAttackProjectile.spreadAngle;
        float step = (count > 1) ? spread / (count - 1) : 0f;
        float startAngle = -spread / 2f;
        attackTrigger.TriggerAttack();

        // Request up to "count" unique targets
        List<Transform> targets = EnemyTargetingSystem.FindMultipleEnemies(transform.position, 30f, count, enemyLayer);

        for (int i = 0; i < count; i++)
        {
            float angle = startAngle + (step * i);
            Quaternion rotation = Quaternion.Euler(0, angle, 0) * firePoint.rotation;

            GameObject p = Instantiate(projectilePrefab, firePoint.position, rotation);
            Projectile proj = p.GetComponent<Projectile>();
            proj.data = basicAttackProjectile;
            proj.hitLayers = enemyLayer;

            // Assign forced target (autofire) or multi-target from list
            if (basicAttackProjectile.homingStrength > 0f)
            {
                if (i < targets.Count)
                    proj.target = targets[i];

            }
        }

        nextAttackTime = Time.time + (1f / statsHandler.heroStats.fireRate);
    }



    public bool CanCast()
    {
        if (statsHandler == null)
        {
            Debug.LogWarning("AbilityController: statsHandler is null in CanCast()");
            return false;
        }
        return Time.time >= nextAttackTime && !statsHandler.isDead;
    }

    private bool CanAutoAttack()
    {
        // Placeholder — controlled by PlayerController
        return false;
    }
}
