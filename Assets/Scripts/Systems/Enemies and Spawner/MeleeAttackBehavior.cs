using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GreenLeaf/Enemy Behaviors/MeleeAttack")]
public class MeleeAttackBehavior : EnemyAttackBehavior
{
    public float damage = 10f;
    public float radius = 1.5f;
    public LayerMask hitLayers;

    public override void Execute(EnemyController context)
    {
        Vector3 origin = context.transform.position + context.transform.forward * radius * 0.5f;
        Collider[] hits = Physics.OverlapSphere(origin, radius, hitLayers);

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                // Apply damage here (via your health system)
                Debug.Log($"[Enemy] Hit player for {damage} damage.");
            }
        }
    }
}
