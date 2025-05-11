using UnityEngine;

[CreateAssetMenu(fileName = "HeroAttackProfile", menuName = "GreenLeaf/Hero Attack Profile")]
public class HeroAttackProfile : ScriptableObject
{
    public enum AttackType { Melee, Raycast, Projectile }
    public AttackType attackType;

    public float fireRate = 0.5f;
    public float damage = 10f;
    public float attackRange = 2f;
    public float aoeRadius = 0f;

    public GameObject projectilePrefab; // For Thalamar only
    public Transform firePoint; // Will be set dynamically from FireController
}
