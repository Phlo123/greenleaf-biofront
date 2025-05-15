
using UnityEngine;
using System;
public enum HomingStyle { Instant, Delayed }

[CreateAssetMenu(menuName = "GreenLeaf/Projectile Data", fileName = "ProjectileData_SO")]
public class ProjectileData : ScriptableObject
{
    public float speed = 20f;
    public float lifetime = 1f;
    public int count = 1;
    public float spreadAngle = 0f;
    public int pierceCount = 0;
    public float size = 1f;
    public float homingStrength = 0f;
    public HomingStyle homingStyle = HomingStyle.Instant;
    public float homingDelay = 0.2f;
    public GameObject impactEffect;
}
