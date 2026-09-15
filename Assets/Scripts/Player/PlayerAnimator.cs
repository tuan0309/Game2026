using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator animator;

    [Header("Animation")]
    [SerializeField] private float walkAnimationSpeed = 0.5f;
    [SerializeField] private float runAnimationSpeed = 1f;

    private void Awake()
    {
        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }
    }

    public void UpdateMovementAnimation(
        Vector2 moveInput,
        bool isSprinting
    )
    {
        if (animator == null)
        {
            return;
        }

        float movementAmount = moveInput.magnitude;

        float speed = 0f;

        if (movementAmount > 0.01f)
        {
            if (isSprinting)
            {
                speed = runAnimationSpeed;
            }
            else
            {
                speed = walkAnimationSpeed;
            }
        }

        animator.SetFloat("Speed", speed);
    }

    public void SetDead()
    {
        if (animator == null)
        {
            return;
        }

        animator.SetBool("Dead", true);
    }

    public void SetAttack()
    {
        if (animator == null)
        {
            return;
        }

        animator.SetTrigger("Attack");
    }
}