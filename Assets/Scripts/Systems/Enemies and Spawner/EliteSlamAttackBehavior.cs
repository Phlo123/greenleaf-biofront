using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GreenLeaf/Enemy Behaviors/EliteSlamAoE")]
public class EliteSlamAttackBehavior : EnemyAttackBehavior
{
    public float radius = 4f;
    public float damage = 25f;
    public LayerMask hitLayers;

    public override void Execute(EnemyController context)
    {
        Vector3 origin = context.transform.position;
        Collider[] hits = Physics.OverlapSphere(origin, radius, hitLayers);

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                Debug.Log($"[Elite AoE] Slam hit player for {damage} damage.");
                // Apply damage here
            }
        }

        // Optional visual FX
        // Instantiate(slamVFX, origin, Quaternion.identity);
    }
}
