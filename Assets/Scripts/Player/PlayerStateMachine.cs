using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    [Header("Current State")]
    [SerializeField] private PlayerState currentState;

    public PlayerState CurrentState => currentState;

    private void Awake()
    {
        currentState = PlayerState.Idle;
    }

    public void ChangeState(PlayerState newState)
    {
        if (currentState == newState)
        {
            return;
        }

        currentState = newState;

        Debug.Log("Player State: " + currentState);
    }

    public void UpdateMovementState(
        Vector2 moveInput,
        bool isSprinting
    )
    {
        // Nếu đã chết thì không đổi state nữa.
        if (currentState == PlayerState.Dead)
        {
            return;
        }

        if (moveInput.magnitude <= 0.01f)
        {
            ChangeState(PlayerState.Idle);
            return;
        }

        if (isSprinting)
        {
            ChangeState(PlayerState.Sprint);
            return;
        }

        ChangeState(PlayerState.Move);
    }

    public void SetDead()
    {
        ChangeState(PlayerState.Dead);
    }
}