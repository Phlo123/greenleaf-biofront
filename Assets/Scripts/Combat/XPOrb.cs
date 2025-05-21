using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XPOrb : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float lifetime = 10f;
    public int xpAmount;

    private Transform target;

    void Start()
    {
        Destroy(gameObject, lifetime);
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            target = playerObj.transform;
    }

    void Update()
    {
        if (target != null)
        {
            Vector3 dir = (target.position - transform.position).normalized;
            transform.position += dir * moveSpeed * Time.deltaTime;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log($"[XP] Player gained {xpAmount} XP.");
            // TODO: Add XP to player stat system here
            Destroy(gameObject);
        }
    }
}

