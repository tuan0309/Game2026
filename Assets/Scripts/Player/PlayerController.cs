using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 8f;

    [Header("Rotation")]
    [SerializeField] private float rotationSpeed = 6f;

    [Header("Mouse Rotation")]
    [SerializeField] private float mouseSensitivity = 0.08f;

    [Header("Gravity")]
    [SerializeField] private float gravity = -20f;

    [Header("Animation")]
    [SerializeField] private Animator animator;

    private CharacterController controller;

    private Vector2 moveInput;
    private Vector3 velocity;

    private bool isSprinting;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();

        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }
    }

    private void Update()
    {
        RotateWithMouse();
        Move();
        ApplyGravity();
        UpdateAnimation();
    }

    // =========================================
    // INPUT
    // =========================================

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        Debug.Log("Move Input: " + moveInput);
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        isSprinting = context.ReadValueAsButton();

        Debug.Log("Sprint: " + isSprinting);
    }

    // =========================================
    // MOUSE ROTATION
    // =========================================

    private void RotateWithMouse()
    {
        if (Mouse.current == null)
        {
            return;
        }

        if (!Mouse.current.rightButton.isPressed)
        {
            return;
        }

        Vector2 mouseDelta = Mouse.current.delta.ReadValue();

        float mouseX = mouseDelta.x;

        if (Mathf.Abs(mouseX) < 0.01f)
        {
            return;
        }

        float rotationAmount = mouseX * mouseSensitivity;

        transform.Rotate(
            0f,
            rotationAmount,
            0f
        );
    }

    // =========================================
    // MOVEMENT
    // =========================================

    private void Move()
    {
        Vector3 forward = transform.forward;
        Vector3 right = transform.right;

        Vector3 direction =
            forward * moveInput.y +
            right * moveInput.x;

        direction = Vector3.ClampMagnitude(
            direction,
            1f
        );

        // Player quay theo hướng di chuyển
        if (direction.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation =
                Quaternion.LookRotation(direction);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }

        float currentSpeed = isSprinting
            ? runSpeed
            : walkSpeed;

        controller.Move(
            direction *
            currentSpeed *
            Time.deltaTime
        );
    }

    // =========================================
    // ANIMATION
    // =========================================

    private void UpdateAnimation()
    {
        if (animator == null)
        {
            Debug.LogWarning("Animator is NULL!");
            return;
        }

        float movementAmount = moveInput.magnitude;

        float animationSpeed = 0f;

        // Không di chuyển
        if (movementAmount <= 0.01f)
        {
            animationSpeed = 0f;
        }
        // Đang chạy
        else if (isSprinting)
        {
            animationSpeed = 1f;
        }
        // Đang đi bộ
        else
        {
            animationSpeed = 0.5f;
        }

        // Gửi Speed vào Animator
        animator.SetFloat(
            "Speed",
            animationSpeed
        );

        // Đọc ngược lại Speed từ Animator
        float animatorSpeed =
            animator.GetFloat("Speed");

        Debug.Log(
            "Animation Speed = " +
            animationSpeed +
            " | Animator Speed = " +
            animatorSpeed
        );
    }

    // =========================================
    // GRAVITY
    // =========================================

    private void ApplyGravity()
    {
        if (controller.isGrounded && velocity.y < 0f)
        {
            velocity.y = -2f;
        }

        velocity.y +=
            gravity *
            Time.deltaTime;

        controller.Move(
            velocity *
            Time.deltaTime
        );
    }
}