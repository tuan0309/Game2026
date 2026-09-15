using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CharacterController characterController;
    [SerializeField] private PlayerAnimator playerAnimator;

    [Header("Movement")]
    [SerializeField] private float walkSpeed = 3f;
    [SerializeField] private float sprintSpeed = 6f;

    [Header("Rotation")]
    [SerializeField] private float rotationSpeed = 10f;

    [Header("Gravity")]
    [SerializeField] private float gravity = -20f;

    private Vector2 moveInput;
    private float verticalVelocity;

    private void Awake()
    {
        if (characterController == null)
        {
            characterController = GetComponent<CharacterController>();
        }

        if (playerAnimator == null)
        {
            playerAnimator = GetComponent<PlayerAnimator>();
        }

        if (characterController == null)
        {
            Debug.LogError(
                "PlayerController: CharacterController not found!",
                this
            );
        }

        if (playerAnimator == null)
        {
            Debug.LogWarning(
                "PlayerController: PlayerAnimator not found!",
                this
            );
        }
    }

    private void Update()
    {
        ReadInput();
        HandleMovement();
    }

    private void ReadInput()
    {
        if (Keyboard.current == null)
        {
            moveInput = Vector2.zero;
            return;
        }

        float horizontal = 0f;
        float vertical = 0f;

        if (Keyboard.current.aKey.isPressed)
        {
            horizontal -= 1f;
        }

        if (Keyboard.current.dKey.isPressed)
        {
            horizontal += 1f;
        }

        if (Keyboard.current.sKey.isPressed)
        {
            vertical -= 1f;
        }

        if (Keyboard.current.wKey.isPressed)
        {
            vertical += 1f;
        }

        moveInput = new Vector2(horizontal, vertical);

        if (moveInput.sqrMagnitude > 1f)
        {
            moveInput.Normalize();
        }
    }

    private void HandleMovement()
    {
        if (characterController == null)
        {
            return;
        }

        bool isSprinting =
            Keyboard.current != null &&
            Keyboard.current.leftShiftKey.isPressed &&
            moveInput.y > 0.01f;

        float currentSpeed =
            isSprinting
                ? sprintSpeed
                : walkSpeed;

        Vector3 moveDirection =
            CalculateMovementDirection();

        // =========================
        // ROTATION
        // =========================

        // Khi không bấm S thuần túy:
        // W, A, D, W+A, W+D đều có thể xoay Player.
        //
        // Khi S:
        // Player đi lùi nhưng giữ nguyên hướng mặt.
        if (moveDirection.sqrMagnitude > 0.01f &&
            moveInput.y >= 0f)
        {
            RotatePlayer(moveDirection);
        }

        // =========================
        // GRAVITY
        // =========================

        if (characterController.isGrounded &&
            verticalVelocity < 0f)
        {
            verticalVelocity = -2f;
        }

        verticalVelocity +=
            gravity * Time.deltaTime;

        // =========================
        // MOVEMENT
        // =========================

        Vector3 velocity =
            moveDirection *
            currentSpeed;

        velocity.y =
            verticalVelocity;

        characterController.Move(
            velocity * Time.deltaTime
        );

        // =========================
        // ANIMATION
        // =========================

        if (playerAnimator != null)
        {
            playerAnimator.UpdateMovementAnimation(
                moveInput,
                isSprinting
            );
        }
    }

    private Vector3 CalculateMovementDirection()
    {
        Vector3 forward =
            transform.forward;

        Vector3 right =
            transform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        Vector3 direction =
            forward * moveInput.y +
            right * moveInput.x;

        if (direction.sqrMagnitude > 1f)
        {
            direction.Normalize();
        }

        return direction;
    }

    private void RotatePlayer(Vector3 direction)
    {
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.001f)
        {
            return;
        }

        Quaternion targetRotation =
            Quaternion.LookRotation(
                direction,
                Vector3.up
            );

        transform.rotation =
            Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                1f -
                Mathf.Exp(
                    -rotationSpeed *
                    Time.deltaTime
                )
            );
    }
}