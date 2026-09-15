using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private EnemyController enemyController;
    [SerializeField] private EnemyWeaponHitbox weaponHitbox;

    // =========================
    // Animator Parameters
    // =========================

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
        // Tìm Animator nếu chưa gán
        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }

        // Tìm EnemyController nếu chưa gán
        if (enemyController == null)
        {
            enemyController = GetComponentInParent<EnemyController>();

            if (enemyController == null)
            {
                enemyController = GetComponent<EnemyController>();
            }
        }

        // Tìm hitbox vũ khí nếu chưa gán
        if (weaponHitbox == null)
        {
            weaponHitbox =
                GetComponentInChildren<EnemyWeaponHitbox>();
        }

        // Kiểm tra reference
        if (animator == null)
        {
            Debug.LogError(
                "EnemyAnimator: Animator not found!",
                this
            );
        }

        if (enemyController == null)
        {
            Debug.LogWarning(
                "EnemyAnimator: EnemyController not found!",
                this
            );
        }

        if (weaponHitbox == null)
        {
            Debug.LogWarning(
                "EnemyAnimator: EnemyWeaponHitbox not found!",
                this
            );
        }
    }

    // =========================
    // MOVEMENT
    // =========================

    public void SetSpeed(float speed)
    {
        if (animator == null)
        {
            return;
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

        // Nếu đang chết thì không Hit nữa
        if (animator.GetBool(DeadHash))
        {
            return;
        }

        animator.SetTrigger(
            HitHash
        );
    }

    // =========================
    // DEATH
    // =========================

    public void SetDead()
    {
        if (animator == null)
        {
            return;
        }

        // Dừng movement
        animator.SetFloat(
            SpeedHash,
            0f
        );

        // Xóa trigger cũ
        animator.ResetTrigger(
            AttackHash
        );

        animator.ResetTrigger(
            HitHash
        );

        // Tắt hitbox đề phòng chết giữa lúc attack
        if (weaponHitbox != null)
        {
            weaponHitbox.EndAttack();
        }

        animator.SetBool(
            DeadHash,
            true
        );
    }

    // =====================================================
    // ANIMATION EVENTS
    // Các hàm dưới đây được gọi từ clip Attack của Enemy
    // =====================================================

    // Bật hitbox khi kiếm bắt đầu đi vào vùng gây damage
    public void Animation_EnableWeaponHitbox()
    {
        if (weaponHitbox == null)
        {
            Debug.LogWarning(
                "EnemyAnimator: WeaponHitbox is missing!",
                this
            );

            return;
        }

        weaponHitbox.StartAttack();

        Debug.Log(
            "ENEMY ANIMATION EVENT: HITBOX ON"
        );
    }

    // Tắt hitbox sau khi kiếm đã quét qua
    public void Animation_DisableWeaponHitbox()
    {
        if (weaponHitbox == null)
        {
            return;
        }

        weaponHitbox.EndAttack();

        Debug.Log(
            "ENEMY ANIMATION EVENT: HITBOX OFF"
        );
    }

    // Gọi ở cuối animation Attack
    public void Animation_EndAttack()
    {
        // Đảm bảo hitbox luôn tắt
        if (weaponHitbox != null)
        {
            weaponHitbox.EndAttack();
        }

        if (enemyController != null)
        {
            enemyController.EndAttack();
        }

        Debug.Log(
            "ENEMY ANIMATION EVENT: ATTACK END"
        );
    }
}