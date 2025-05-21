using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviorLoader : MonoBehaviour
{
    public EnemyAttackBehavior attackBehavior;

    void Awake()
    {
        EnemyController ec = GetComponent<EnemyController>();
        ec.attackBehavior = attackBehavior;
    }
}