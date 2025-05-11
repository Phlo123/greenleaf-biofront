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
    {void Start()
{
    direction = transform.forward;
    
    // Adjust spear model tilt if needed
    transform.Rotate(new Vector3(-90f, 0f, 0f)); // adjust if model points up by default
    
    Destroy(gameObject, lifetime);
}

    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Explode on any collider (enemy, wall, object)
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
            // TODO: hit.GetComponent<Enemy>()?.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}
