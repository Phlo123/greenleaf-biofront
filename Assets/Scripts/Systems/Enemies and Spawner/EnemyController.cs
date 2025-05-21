using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class EnemyController : MonoBehaviour
{
    [Header("Spawn Points")]
    public Transform projectileSpawnPoint;

    [Header("Core Settings")]
    public float moveSpeed = 4f;
    public float attackRange = 2f;
    public float attackCooldown = 2f;
    public float thinkInterval = 0.5f;

    [Header("References")]
    public Animator animator;
    public GameObject telegraphConePrefab;
    public Transform telegraphSpawnPoint;
    public EnemyAttackBehavior attackBehavior;
    public Transform player { get; private set; }

    private CharacterController controller;
    private float lastAttackTime = -Mathf.Infinity;
    private float lastThinkTime = -Mathf.Infinity;
    private bool isAttacking = false;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        if (animator == null)
            animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (!isAttacking && Time.time > lastAttackTime + attackCooldown && distance <= attackRange)
        {
            StartAttack();
            return;
        }

        if (!isAttacking)
        {
            MoveTowardPlayer();
        }

        if (Time.time > lastThinkTime + thinkInterval)
        {
            lastThinkTime = Time.time;
            // future logic: handle target switching, threat evaluation
        }
    }

    private void MoveTowardPlayer()
    {
        animator.SetBool("IsMoving", true);
        Vector3 dir = (player.position - transform.position).normalized;
        dir.y = 0f;
        Vector3 move = dir * moveSpeed * Time.deltaTime;
        controller.Move(move);

        // After moving, reset Y in case physics pulled it down
        Vector3 fixedPos = transform.position;
        fixedPos.y = 0f;
        transform.position = fixedPos;


        if (dir.sqrMagnitude > 0.01f)
            transform.rotation = Quaternion.LookRotation(dir);
    }

    private void StartAttack()
    {
        isAttacking = true;
        lastAttackTime = Time.time;
        animator.SetTrigger("AttackTrigger");
        animator.SetBool("IsMoving", false);

        if (telegraphConePrefab && telegraphSpawnPoint)
        {
            GameObject telegraph = Instantiate(telegraphConePrefab, telegraphSpawnPoint.position, telegraphSpawnPoint.rotation, transform);
            Destroy(telegraph, 0.5f); // Show for 0.5s
        }
    }

    // This method is called via Animation Event
    public void FireFromAnimation()
    {
        if (attackBehavior != null)
        {
            attackBehavior.Execute(this);
        }

        isAttacking = false;
    }
}
