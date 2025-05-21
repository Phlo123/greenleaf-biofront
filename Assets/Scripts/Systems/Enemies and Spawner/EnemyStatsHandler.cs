using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatsHandler : MonoBehaviour
{
    [Header("Base Stats")]
    public float maxHP = 100f;
    private float currentHP;

    [Header("Gameplay Values")]
    public int XPValue = 5;
    public int RevivesRemaining = 0;

    [Header("References")]
    public GameObject xpDropPrefab;

    private bool isDead = false;

    void Awake()
    {
        currentHP = maxHP;
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHP -= amount;
        if (currentHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;

        // Unregister from tracker
        EnemyTracker.Instance?.UnregisterEnemy(gameObject);

        // Drop XP
        if (xpDropPrefab != null)
        {
            GameObject orb = Instantiate(xpDropPrefab, transform.position, Quaternion.identity);
            XPOrb xp = orb.GetComponent<XPOrb>();
            if (xp != null)
                xp.xpAmount = XPValue;

        }

        // TODO: Add revive logic (if RevivesRemaining > 0)
        Destroy(gameObject);
    }

    public float GetHealthPercent()
    {
        return currentHP / maxHP;
    }
}
