using UnityEngine;

[CreateAssetMenu(menuName = "GreenLeaf/Hero Stats", fileName = "HeroStats_SO")]
public class HeroStats : ScriptableObject
{
    [Header("Dash")]
    public float dashCooldown = 1f;      // Time per charge
    public float dashDistance = 6f;      // Distance per dash
    public float dashDuration = 0.2f;    // Time it takes to dash that distance
    public int dashCharges = 2;          // Max charges

    [Header("Core Stats")]
    public float maxHealth = 100f;
    public float moveSpeed = 6f;
    public float rotationSpeed = 10f;

    [Header("Combat")]
    public float baseDamage = 10f;
    public float fireRate = 1f; // attacks per second
}
