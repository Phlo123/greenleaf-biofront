using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Camera mainCam;

    private Rigidbody rb;
    private Vector3 moveInput;
    private Plane groundPlane;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        mainCam = Camera.main;
        groundPlane = new Plane(Vector3.up, Vector3.zero);
    }


    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");
        moveInput = new Vector3(moveX, 0f, moveZ).normalized;
    }


    void FixedUpdate()
    {
        // Move the player
        rb.velocity = moveInput * moveSpeed;

        // Lock Y position
        rb.position = new Vector3(rb.position.x, 0f, rb.position.z);

        // — no rotation code here any more —
    }
}
