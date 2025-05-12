using UnityEngine;
using UnityEngine.Events;

public class EnemyDummy : MonoBehaviour
{
    [Header("Stats")]
    public float health = 1000f;

    public enum EnemyType { Default, IceElemental, Boss }

    [Header("Enemy Type")]
    public EnemyType type = EnemyType.Default;

    [Header("Events")]
    public UnityEvent onDeath;
    public UnityEvent<float> onTakeDamage;

    public void TakeDamage(float amount)
    {
        health -= amount;
        Debug.Log($"{gameObject.name} took {amount} damage. Remaining: {health}");

        onTakeDamage?.Invoke(amount);
        ShowHitEffect();

        if (health <= 0f)
            Die();
    }

    void Die()
    {
        Debug.Log($"{gameObject.name} has died.");
        onDeath?.Invoke();
        Destroy(gameObject);
    }

    // Add hit effects (particles, flash, sound)
    void ShowHitEffect()
    {
        // Placeholder — replace with actual visuals/audio later
        Debug.Log($"{gameObject.name} reacts to damage.");
    }

    // Status effects — expand as needed
    public void ApplyFrost()
    {
        Debug.Log($"{gameObject.name} affected by frost.");
        // TODO: apply slow or visual effect here
    }

    public void ApplyBurn(float duration)
    {
        Debug.Log($"{gameObject.name} is burning for {duration} seconds.");
        // TODO: implement DoT or fire visuals
    }

    public void ApplySlow(float amount, float duration)
    {
        Debug.Log($"{gameObject.name} is slowed by {amount * 100f}% for {duration} sec.");
        // TODO: reduce movement speed here
    }
}
