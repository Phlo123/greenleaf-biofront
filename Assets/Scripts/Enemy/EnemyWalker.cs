using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemyWalker : MonoBehaviour
{
    public float speed = 2f;
    public Transform[] waypoints;
    private int currentIndex = 0;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    void FixedUpdate()
    {
        if (waypoints.Length == 0) return;

        Vector3 dir = waypoints[currentIndex].position - transform.position;
        dir.y = 0f;

        if (dir.magnitude < 0.2f)
        {
            currentIndex = (currentIndex + 1) % waypoints.Length;
            return;
        }

        rb.MovePosition(transform.position + dir.normalized * speed * Time.fixedDeltaTime);
    }
}
