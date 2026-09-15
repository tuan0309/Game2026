using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private CombatController combatController;

    [Header("Movement")]
    [SerializeField] private float walkAnimationSpeed = 0.5f;
    [SerializeField] private float runAnimationSpeed = 1f;

    private static readonly int SpeedHash =
        Animator.StringToHash("Speed");

    private static readonly int AttackHash =
        Animator.StringToHash("Attack");

    private static readonly int HitHash =
        Animator.StringToHash("Hit");

    private static readonly int DeadHash =
        Animator.StringToHash("Dead");

    private void Awake()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }

        if (combatController == null)
        {
            combatController =
                GetComponentInParent<CombatController>();

            if (combatController == null)
            {
                combatController =
                    GetComponent<CombatController>();
            }
        }

        if (animator == null)
        {
            Debug.LogError(
                "PlayerAnimator: Animator not found!",
                this
            );
        }
    }

    // =========================
    // MOVEMENT
    // =========================

    public void UpdateMovementAnimation(
        Vector2 moveInput,
        bool isSprinting
    )
    {
        if (animator == null)
        {
            return;
        }

        float speed = 0f;

        if (moveInput.sqrMagnitude > 0.01f)
        {
            speed = isSprinting
                ? runAnimationSpeed
                : walkAnimationSpeed;
        }

        animator.SetFloat(
            SpeedHash,
            speed
        );
    }

    // =========================
    // ATTACK
    // =========================

    public void SetAttack()
    {
        if (animator == null)
        {
            return;
        }

        animator.ResetTrigger(HitHash);

        animator.SetTrigger(
            AttackHash
        );
    }

    // =========================
    // HIT
    // =========================

    public void SetHit()
    {
        if (animator == null)
        {
            return;
        }

        if (animator.GetBool(DeadHash))
        {
            return;
        }

        // Tránh Attack tiếp tục giữ state khi bị đánh
        animator.ResetTrigger(AttackHash);

        // Dừng locomotion trong lúc phản ứng Hit
        animator.SetFloat(
            SpeedHash,
            0f
        );

        animator.SetTrigger(
            HitHash
        );

        Debug.Log("PLAYER HIT TRIGGER");
    }

    // =========================
    // DEAD
    // =========================

    public void SetDead()
    {
        if (animator == null)
        {
            return;
        }

        animator.SetFloat(
            SpeedHash,
            0f
        );

        animator.ResetTrigger(
            AttackHash
        );

        animator.ResetTrigger(
            HitHash
        );

        animator.SetBool(
            DeadHash,
            true
        );
    }

    // =========================
    // PLAYER ATTACK EVENTS
    // =========================

    public void Animation_EnableSwordHitbox()
    {
        if (combatController != null)
        {
            combatController.EnableSwordHitbox();
        }
    }

    public void Animation_DisableSwordHitbox()
    {
        if (combatController != null)
        {
            combatController.DisableSwordHitbox();
        }
    }

    public void Animation_EndAttack()
    {
        if (combatController != null)
        {
            combatController.EndAttack();
        }
    }
}