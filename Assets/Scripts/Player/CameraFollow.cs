using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform target;
    public Vector3 offset = new Vector3(0f, 15f, -10f);
    public float followSpeed = 5f;

    void LateUpdate()
    {
        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                target = player.transform;
        }

        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);
    }

}
