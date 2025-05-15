using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoftFollowBone : MonoBehaviour
{
    public Transform target; // Typically the animated pelvis or root
    public float positionLag = 0.1f; // 0 = instant, higher = more drag
    public float rotationLag = 0.1f;

    private Vector3 velocity;
    private Vector3 lastTargetPosition;
    private Quaternion lastTargetRotation;

    void Start()
    {
        if (target == null)
            target = transform.parent;

        lastTargetPosition = target.position;
        lastTargetRotation = target.rotation;
    }

    void LateUpdate()
    {
        // Position: smooth follow with drag
        Vector3 desiredPosition = target.position;
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, positionLag);

        // Rotation: smooth follow with slerp
        Quaternion desiredRotation = target.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, Time.deltaTime / rotationLag);
    }
}
