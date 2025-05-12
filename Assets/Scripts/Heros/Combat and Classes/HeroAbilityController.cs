using UnityEngine;

public abstract class HeroAbilityController : MonoBehaviour
{
    public abstract float FireRate { get; }
    public abstract float AttackRange { get; }
    public abstract void Fire(Vector3 direction, Transform target);
    public abstract Vector3 GetAutoTargetDirection();
    public abstract float RotationSpeed { get; }
}


