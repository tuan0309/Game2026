using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int maxHealth = 100;

    [Header("References")]
    [SerializeField] private PlayerAnimator playerAnimator;

    private int currentHealth;
    private bool isDead;

    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;
    public bool IsDead => isDead;

    private void Awake()
    {
        currentHealth = maxHealth;

        if (playerAnimator == null)
        {
            playerAnimator =
                GetComponent<PlayerAnimator>();
        }

        if (playerAnimator == null)
        {
            playerAnimator =
                GetComponentInChildren<PlayerAnimator>();
        }

        if (playerAnimator == null)
        {
            Debug.LogWarning(
                "Health: PlayerAnimator not found!",
                this
            );
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead)
        {
            return;
        }

        if (damage <= 0)
        {
            return;
        }

        currentHealth -= damage;

        currentHealth =
            Mathf.Max(
                currentHealth,
                0
            );

        Debug.Log(
            "PLAYER DAMAGE: " +
            damage +
            " | HP: " +
            currentHealth +
            "/" +
            maxHealth
        );

        // =========================
        // DEAD
        // =========================

        if (currentHealth <= 0)
        {
            Die();
            return;
        }

        // =========================
        // HIT REACTION
        // =========================

        if (playerAnimator != null)
        {
            playerAnimator.SetHit();
        }
        else
        {
            Debug.LogError(
                "Health: PlayerAnimator reference is NULL!",
                this
            );
        }
    }

    private void Die()
    {
        if (isDead)
        {
            return;
        }

        isDead = true;

        currentHealth = 0;

        if (playerAnimator != null)
        {
            playerAnimator.SetDead();
        }

        Debug.Log("PLAYER DEAD");
    }

    // Dùng để test hoặc hồi máu sau này
    public void Heal(int amount)
    {
        if (isDead)
        {
            return;
        }

        if (amount <= 0)
        {
            return;
        }

        currentHealth += amount;

        currentHealth =
            Mathf.Min(
                currentHealth,
                maxHealth
            );
    }
}