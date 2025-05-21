using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GreenLeaf/Enemy Behaviors/RangedAttack")]
public class RangedAttackBehavior : EnemyAttackBehavior
{
    public GameObject projectilePrefab;
    public float projectileSpeed = 15f;
    public float damage = 10f;
    public LayerMask hitLayers;

    public override void Execute(EnemyController context)
    {
        // Rotate toward the player at time of fire
        if (context.player != null)
        {
            Vector3 lookDir = (context.player.position - context.transform.position).normalized;
            lookDir.y = 0f;
            context.transform.rotation = Quaternion.LookRotation(lookDir);
        }

        // Fire projectile
        Transform spawnPoint = context.projectileSpawnPoint ?? context.transform;
        GameObject proj = Instantiate(projectilePrefab, spawnPoint.position, spawnPoint.rotation);

        Rigidbody rb = proj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = spawnPoint.forward * projectileSpeed;
        }

        ProjectileEnemy projectile = proj.GetComponent<ProjectileEnemy>();
        if (projectile != null)
        {
            projectile.damage = damage;
            projectile.hitLayers = hitLayers;
        }
    }

}
