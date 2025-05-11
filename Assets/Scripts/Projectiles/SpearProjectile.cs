using UnityEngine;

public class SpearProjectile : MonoBehaviour
{
    public float speed = 20f;
    public float damage = 10f;
    public float aoeRadius = 3f;
    public LayerMask enemyLayer;
    public GameObject explosionEffect;
    public float lifetime = 5f;

    private Vector3 direction;

    void Start()
    {
        // Capture the initial forward direction at spawn
        direction = transform.forward;

        // Destroy the spear after X seconds no matter what
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        // Move the projectile forward
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        Explode();
    }

    void Explode()
    {
        if (explosionEffect)
            Instantiate(explosionEffect, transform.position, Quaternion.identity);

        Collider[] hits = Physics.OverlapSphere(transform.position, aoeRadius, enemyLayer);
        foreach (var hit in hits)
        {
            Debug.Log("Spear AOE hit: " + hit.name);
            // hit.GetComponent<Enemy>()?.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}
