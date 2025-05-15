using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerRandomAttack : MonoBehaviour
{
    public Animator animator;
    public int numberOfAttacks = 3;

    public void TriggerAttack()
    {
        int random = Random.Range(0, numberOfAttacks);
        Debug.Log("Random AttackIndex: " + random);
        animator.SetInteger("AttackIndex", random);
        animator.SetTrigger("IsFiring");
    }

    private IEnumerator ResetAttackIndexAfterDelay(int indexUsed)
    {
        // Get current clip length
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(1); // Layer 1 = AttackLayer
        float waitTime = stateInfo.length + 0.1f;

        yield return new WaitForSeconds(0.5f); // or animation length
        animator.SetInteger("AttackIndex", 0); // Neutral value
    }
}
