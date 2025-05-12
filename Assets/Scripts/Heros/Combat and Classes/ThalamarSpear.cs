using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ThalamarSpear : MonoBehaviour
{
    [Header("General Settings")]
    public float speed = 20f;
    public float lifetime = 5f;
    public int damage = 1;
    public LayerMask hitLayers;
    public float aoeRadius = 0f;
    [Header("Modifiers")]
    public bool pierces = false;
    public int maxPierces = 3;
    public bool appliesFrost = false;

    private int pierceCount = 0;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Only hit valid layers
        if (((1 << other.gameObject.layer) & hitLayers) == 0)
            return;

        EnemyDummy enemy = other.GetComponent<EnemyDummy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);

            if (appliesFrost)
            {
                StatusEffectManager sem = enemy.GetComponent<StatusEffectManager>();
                if (sem == null)
                    sem = enemy.gameObject.AddComponent<StatusEffectManager>();

                sem.ApplyChill();
            }
        }

        if (pierces)
        {
            pierceCount++;
            if (pierceCount >= maxPierces)
                Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
