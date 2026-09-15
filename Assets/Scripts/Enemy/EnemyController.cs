using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private EnemyAnimator enemyAnimator;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float rotationSpeed = 8f;

    [Header("Detection")]
    [SerializeField] private float detectionRange = 8f;

    [Header("Attack")]
    [SerializeField] private float attackRange = 1.8f;
    [SerializeField] private float attackCooldown = 1.5f;

    private float lastAttackTime;
    private bool isAttacking;

    private void Awake()
    {
        if (enemyAnimator == null)
        {
            enemyAnimator = GetComponent<EnemyAnimator>();
        }

        if (player == null)
        {
            GameObject playerObject =
                GameObject.FindGameObjectWithTag("Player");

            if (playerObject != null)
            {
                player = playerObject.transform;
            }
            else
            {
                Debug.LogWarning(
                    "EnemyController: Không tìm thấy Player!",
                    this
                );
            }
        }
    }

    private void Update()
    {
        if (player == null)
        {
            StopMovement();
            return;
        }

        float distanceToPlayer =
            Vector3.Distance(
                transform.position,
                player.position
            );

        // Ngoài vùng phát hiện
        if (distanceToPlayer > detectionRange)
        {
            StopMovement();
            return;
        }

        // Luôn quay về phía Player
        FacePlayer();

        // Nếu đang Attack thì không di chuyển
        if (isAttacking)
        {
            StopMovement();
            return;
        }

        // Trong tầm đánh
        if (distanceToPlayer <= attackRange)
        {
            StopMovement();
            TryAttack();
            return;
        }

        // Chưa tới tầm đánh thì đuổi theo
        MoveTowardPlayer();
    }

    private void MoveTowardPlayer()
    {
        Vector3 direction =
            player.position - transform.position;

        direction.y = 0f;

        if (direction.sqrMagnitude <= 0.001f)
        {
            StopMovement();
            return;
        }

        direction.Normalize();

        transform.position +=
            direction *
            moveSpeed *
            Time.deltaTime;

        if (enemyAnimator != null)
        {
            enemyAnimator.SetSpeed(1f);
        }
    }

    private void FacePlayer()
    {
        Vector3 direction =
            player.position - transform.position;

        direction.y = 0f;

        if (direction.sqrMagnitude <= 0.001f)
        {
            return;
        }

        Quaternion targetRotation =
            Quaternion.LookRotation(direction);

        transform.rotation =
            Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed *
                Time.deltaTime
            );
    }

    private void StopMovement()
    {
        if (enemyAnimator != null)
        {
            enemyAnimator.SetSpeed(0f);
        }
    }

    private void TryAttack()
    {
        if (Time.time <
            lastAttackTime + attackCooldown)
        {
            return;
        }

        lastAttackTime = Time.time;
        isAttacking = true;

        if (enemyAnimator != null)
        {
            enemyAnimator.SetAttack();
        }

        Debug.Log("ENEMY ATTACK");
    }

    // Tạm thời gọi từ Animation Event ở cuối animation Attack
    public void EndAttack()
    {
        isAttacking = false;

        Debug.Log("ENEMY ATTACK END");
    }

    private void OnDrawGizmosSelected()
    {
        // Detection range
        Gizmos.DrawWireSphere(
            transform.position,
            detectionRange
        );

        // Attack range
        Gizmos.DrawWireSphere(
            transform.position,
            attackRange
        );
    }
}