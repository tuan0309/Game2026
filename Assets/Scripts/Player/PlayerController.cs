using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Character Data")]
    [SerializeField] private CharacterData characterData;

    [Header("Animation")]
    [SerializeField] private PlayerAnimator playerAnimator;

    [Header("State Machine")]
    [SerializeField] private PlayerStateMachine stateMachine;

    [Header("Mouse Rotation")]
    [SerializeField] private float mouseSensitivity = 0.08f;

    [Header("Gravity")]
    [SerializeField] private float gravity = -20f;

    private CharacterController controller;

    private Vector2 moveInput;
    private Vector3 velocity;

    private bool isSprinting;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();

        if (playerAnimator == null)
        {
            playerAnimator = GetComponent<PlayerAnimator>();
        }

        if (stateMachine == null)
        {
            stateMachine = GetComponent<PlayerStateMachine>();
        }

        if (characterData == null)
        {
            Debug.LogError(
                "CharacterData is missing on Player!"
            );
        }

        if (playerAnimator == null)
        {
            Debug.LogError(
                "PlayerAnimator is missing on Player!"
            );
        }

        if (stateMachine == null)
        {
            Debug.LogError(
                "PlayerStateMachine is missing on Player!"
            );
        }
    }

    private void Update()
    {
        RotateWithMouse();
        Move();
        ApplyGravity();
        UpdateAnimation();
        UpdateState();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        isSprinting = context.ReadValueAsButton();
    }

    private void RotateWithMouse()
    {
        // Khi chết vẫn cho phép camera/player rotation.
        if (Mouse.current == null)
        {
            return;
        }

        if (!Mouse.current.rightButton.isPressed)
        {
            return;
        }

        Vector2 mouseDelta =
            Mouse.current.delta.ReadValue();

        float mouseX = mouseDelta.x;

        if (Mathf.Abs(mouseX) < 0.01f)
        {
            return;
        }

        float rotationAmount =
            mouseX * mouseSensitivity;

        transform.Rotate(
            0f,
            rotationAmount,
            0f
        );
    }

    private void Move()
    {
        // Player đã chết thì không được di chuyển.
        if (stateMachine != null &&
            stateMachine.CurrentState == PlayerState.Dead)
        {
            return;
        }

        if (characterData == null)
        {
            return;
        }

        Vector3 forward = transform.forward;
        Vector3 right = transform.right;

        Vector3 direction =
            forward * moveInput.y +
            right * moveInput.x;

        direction = Vector3.ClampMagnitude(
            direction,
            1f
        );

        if (direction.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation =
                Quaternion.LookRotation(direction);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                characterData.rotationSpeed *
                Time.deltaTime
            );
        }

        float currentSpeed =
            isSprinting
                ? characterData.runSpeed
                : characterData.walkSpeed;

        controller.Move(
            direction *
            currentSpeed *
            Time.deltaTime
        );
    }

    private void UpdateAnimation()
    {
        if (playerAnimator == null)
        {
            return;
        }

        // Khi chết không cập nhật Speed nữa.
        if (stateMachine != null &&
            stateMachine.CurrentState == PlayerState.Dead)
        {
            return;
        }

        playerAnimator.UpdateMovementAnimation(
            moveInput,
            isSprinting
        );
    }

    private void UpdateState()
    {
        if (stateMachine == null)
        {
            return;
        }

        stateMachine.UpdateMovementState(
            moveInput,
            isSprinting
        );
    }

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