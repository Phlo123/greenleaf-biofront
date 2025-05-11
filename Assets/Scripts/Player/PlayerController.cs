using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f; // Lower = more delay, Higher = snappier
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

        // Smooth rotation toward mouse
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
        if (groundPlane.Raycast(ray, out float hitDist))
        {
            Vector3 point = ray.GetPoint(hitDist);
            Vector3 lookDir = point - transform.position;
            lookDir.y = 0f;

            if (lookDir != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(lookDir);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
    }


    void FixedUpdate()
    {
        // Manually set velocity for instant movement
        rb.velocity = new Vector3(moveInput.x * moveSpeed, 0f, moveInput.z * moveSpeed);

        // Hard stop momentum by zeroing velocity if no input
        if (moveInput == Vector3.zero)
        {
            rb.velocity = Vector3.zero;
        }

        // Lock Y position
        rb.position = new Vector3(rb.position.x, 0f, rb.position.z);
    }
}
