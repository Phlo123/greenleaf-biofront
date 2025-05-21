using UnityEngine;

public class ProjectileEnemy : MonoBehaviour
{
    public float damage = 10f;
    public float lifeTime = 5f;
    public LayerMask hitLayers;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & hitLayers) != 0)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log($"[Enemy Projectile] Hit player for {damage} damage.");
                // Apply damage here
            }
            Destroy(gameObject);
        }
    }
}
