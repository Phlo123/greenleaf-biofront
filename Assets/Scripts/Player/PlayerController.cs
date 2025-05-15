using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Input")]
    public PlayerInputActions inputActions;

    private CharacterController controller;
    private Camera mainCamera;
    private Vector2 moveInput;
    private Vector3 moveDirection;
    private Animator animator;
    private float lastDashTime = -Mathf.Infinity;
    private bool isDashing = false;
    private float dashTimeRemaining = 0f;
    private bool autoFireEnabled = false;
    private AbilityController abilityController;
    private HeroStatsHandler statsHandler;
    private float autoTargetRange = 30f; // can expose later in HeroStats if needed

    private void Awake()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        mainCamera = Camera.main;
        inputActions = new PlayerInputActions();
        statsHandler = GetComponent<HeroStatsHandler>();
        abilityController = GetComponent<AbilityController>();
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Dash.performed += ctx => TryDash();
        inputActions.Player.AutoFire.performed += ctx => autoFireEnabled = !autoFireEnabled;
    }

    private void OnDisable()
    {
        inputActions.Player.Disable();
    }

    private void Update()
    {
        animator.SetFloat("AttackSpeed", statsHandler.heroStats.fireRate);
        moveInput = inputActions.Player.Move.ReadValue<Vector2>();
        HandleMovement();
        RotateToMouse();

        bool isFiring = autoFireEnabled || inputActions.Player.Fire.IsPressed();

        if (isFiring && abilityController.CanCast())
        {
            abilityController.LaunchBasicAttack(); // Fire NOW
            animator.SetTrigger("AttackTrigger");   // Just for visual
        }
    }


    private void HandleMovement()
    {
        if (isDashing)
        {
            float dashSpeed = statsHandler.heroStats.dashDistance / statsHandler.heroStats.dashDuration;
            Vector3 dashDir = (dashEnd - dashStart).normalized;
            controller.Move(dashDir * dashSpeed * Time.deltaTime);

            dashTimeRemaining -= Time.deltaTime;
            if (dashTimeRemaining <= 0f)
                isDashing = false;
        }
        else
        {
            Vector3 input = new Vector3(moveInput.x, 0, moveInput.y);
            moveDirection = Quaternion.Euler(0, mainCamera.transform.eulerAngles.y, 0) * input.normalized;
            controller.Move(moveDirection * statsHandler.heroStats.moveSpeed * Time.deltaTime);
        }
    }

    private void RotateToMouse()
    {
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 lookDirection = hit.point - transform.position;
            lookDirection.y = 0;
            if (lookDirection.sqrMagnitude > 0.01f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, statsHandler.heroStats.rotationSpeed * Time.deltaTime);
            }
        }
    }


    private Vector3 dashStart;
    private Vector3 dashEnd;

    private void TryDash()
    {
        if (statsHandler.currentDashCharges > 0 && !isDashing)
        {
            statsHandler.currentDashCharges--;
            lastDashTime = Time.time;

            dashStart = transform.position;
            dashEnd = dashStart + moveDirection.normalized * statsHandler.heroStats.dashDistance;
            dashTimeRemaining = statsHandler.heroStats.dashDuration;
            isDashing = true;
        }
    }
}
