using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyAttackBehavior : ScriptableObject
{
    public abstract void Execute(EnemyController context);
}