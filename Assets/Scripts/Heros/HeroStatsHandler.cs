using UnityEngine;

public class HeroStatsHandler : MonoBehaviour
{
    public HeroStats heroStats;

    [HideInInspector] public float currentHealth;
    [HideInInspector] public bool isDead = false;
    [HideInInspector] public int currentDashCharges;
    private float lastDashTime;

    private void Awake()
    {
        if (heroStats == null)
        {
            Debug.LogError("HeroStatsHandler: No HeroStats assigned!");
            enabled = false;
            return;
        }

        currentHealth = heroStats.maxHealth;
        currentDashCharges = heroStats.dashCharges;
        lastDashTime = -Mathf.Infinity;
    }

    private void RegenerateDashCharge()
    {
        if (currentDashCharges < heroStats.dashCharges)
        {
            if (Time.time >= lastDashTime + heroStats.dashCooldown)
            {
                currentDashCharges++;
                lastDashTime = Time.time;
            }
        }
    }
    private void Update()
    {
        RegenerateDashCharge();
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            isDead = true;
            Debug.Log("Hero died.");
            // Handle death later
        }
    }

    public void Heal(float amount)
    {
        if (isDead) return;

        currentHealth = Mathf.Min(currentHealth + amount, heroStats.maxHealth);
    }
}
