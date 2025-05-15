using UnityEngine;
using System.Collections.Generic;

public class Projectile : MonoBehaviour
{   
    public ProjectileData data;
    public Transform target;
    public LayerMask hitLayers;

    private Vector3 direction;
    private float lifeRemaining;
    private int remainingPierces;
    private Rigidbody rb;
    private float homingTimer;
    private bool wasHoming = false;
    private HashSet<Transform> piercedTargets = new HashSet<Transform>();
    private void Start()
    {
        lifeRemaining = data.lifetime;
        remainingPierces = data.pierceCount;
        transform.localScale = Vector3.one * data.size;

        rb = GetComponent<Rigidbody>();
        homingTimer = 0f;

        direction = transform.forward; // key change
        wasHoming = (target != null);
    }

    private void Update()
    {
        lifeRemaining -= Time.deltaTime;
        homingTimer += Time.deltaTime;
        //Debug.Log($"Homing Timer: {homingTimer}, Style: {data.homingStyle}");

        if (lifeRemaining <= 0f)
        {
            Destroy(gameObject);
            return;
        }
        bool homingActive = data.homingStyle == HomingStyle.Instant || homingTimer >= data.homingDelay;

        // Retarget if target is gone
        if ((target == null || !target.gameObject.activeInHierarchy) && data.homingStrength > 0f)
        {
            target = EnemyTargetingSystem.FindClosestEnemy(transform.position, 30f, hitLayers);
        }

        if (homingActive && wasHoming && target != null && data.homingStrength > 0f)
        {
            Vector3 toTarget = (target.position - transform.position).normalized;
            float maxTurnAngle = Mathf.Lerp(0f, 90f, data.homingStrength) * Time.deltaTime;

            float angleToTarget = Vector3.Angle(direction, toTarget);

            if (angleToTarget <= 60f)
            {
                direction = Vector3.RotateTowards(direction, toTarget, Mathf.Deg2Rad * maxTurnAngle, 0f);
            }
            // else: skip homing this frame (turn too sharp)
        }


        rb.MovePosition(transform.position + direction * data.speed * Time.deltaTime);
    }



    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & hitLayers) == 0)
            return;

        Transform hitTarget = other.transform;

        // Prevent hitting the same target more than once
        if (piercedTargets.Contains(hitTarget))
            return;

        piercedTargets.Add(hitTarget);

        // Apply damage, effects etc.
        //Debug.Log($"Projectile hit {hitTarget.name}");

        remainingPierces--;
        if (remainingPierces < 0)
        {
            if (data.impactEffect)
                Instantiate(data.impactEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

}
