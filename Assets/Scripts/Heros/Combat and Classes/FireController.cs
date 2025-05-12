using UnityEngine;

public class FireController : MonoBehaviour
{
    [SerializeField] private float fireAlignmentThreshold = 8f;

    private float fireCooldown = 0f;
    private bool isAutoFire = false;
    private bool isClickHeld = false;

    private HeroAbilityController abilityController;

    void Start()
    {
        abilityController = GetComponent<HeroAbilityController>();
    }

    void Update()
    {
        fireCooldown -= Time.deltaTime;
        isClickHeld = Input.GetMouseButton(0);

        if (Input.GetKeyDown(KeyCode.Q))
            isAutoFire = !isAutoFire;

        Vector3 direction = Vector3.zero;

        if (isAutoFire && !isClickHeld)
        {
            direction = abilityController.GetAutoTargetDirection();
            if (direction == Vector3.zero) return;
        }
        else
        {
            Vector3 mouseWorld = GetMouseWorldPos();
            direction = (mouseWorld - transform.position).normalized;
        }

        Quaternion targetRot = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, abilityController.RotationSpeed * Time.deltaTime);

        float angle = Quaternion.Angle(transform.rotation, targetRot);

        // Normal firing condition
        if (fireCooldown <= 0f && angle <= fireAlignmentThreshold)
        {
            if (isAutoFire || (!isAutoFire && isClickHeld))
            {
                Transform target = GetTargetFromDirection(direction);
                abilityController.Fire(direction, target);
                fireCooldown = abilityController.FireRate;
            }
        }
        // Grace period: allow slight delay if angle is close and cooldown just missed
        else if (fireCooldown <= -0.1f && angle <= fireAlignmentThreshold * 1.5f)
        {
            if (isAutoFire || (!isAutoFire && isClickHeld))
            {
                Transform target = GetTargetFromDirection(direction);
                abilityController.Fire(direction, target);
                fireCooldown = abilityController.FireRate;
            }
        }
    }

    private Transform GetTargetFromDirection(Vector3 direction)
    {
        Ray ray = new Ray(transform.position + Vector3.up * 1f, direction);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, LayerMask.GetMask("Enemy")))
            return hit.transform;

        return null;
    }

    Vector3 GetMouseWorldPos()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        new Plane(Vector3.up, 0).Raycast(ray, out float dist);
        return ray.GetPoint(dist);
    }
}
