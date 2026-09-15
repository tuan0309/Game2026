using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CharacterStats characterStats;

    [Header("Animation")]
    [SerializeField] private PlayerAnimator playerAnimator;

    [Header("State Machine")]
    [SerializeField] private PlayerStateMachine stateMachine;

    private float currentHealth;

    public float CurrentHealth => currentHealth;

    public float MaxHealth
    {
        get
        {
            if (characterStats == null)
            {
                return 0f;
            }

            return characterStats.MaxHealth;
        }
    }

    public bool IsDead => currentHealth <= 0f;

    private void Awake()
    {
        if (characterStats == null)
        {
            characterStats = GetComponent<CharacterStats>();
        }

        if (playerAnimator == null)
        {
            playerAnimator = GetComponent<PlayerAnimator>();
        }

        if (stateMachine == null)
        {
            stateMachine = GetComponent<PlayerStateMachine>();
        }

        if (characterStats == null)
        {
            Debug.LogError(
                "CharacterStats is missing on Player!"
            );

            return;
        }

        currentHealth = MaxHealth;
    }

    public void TakeDamage(float damage)
    {
        if (IsDead)
        {
            return;
        }

        if (damage <= 0f)
        {
            return;
        }

        currentHealth -= damage;

        currentHealth = Mathf.Max(
            currentHealth,
            0f
        );

        Debug.Log(
            "Player Health: " +
            currentHealth +
            " / " +
            MaxHealth
        );

        if (IsDead)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        if (IsDead)
        {
            return;
        }

        if (amount <= 0f)
        {
            return;
        }

        currentHealth += amount;

        currentHealth = Mathf.Min(
            currentHealth,
            MaxHealth
        );

        Debug.Log(
            "Player Health: " +
            currentHealth +
            " / " +
            MaxHealth
        );
    }

    private void Die()
    {
        Debug.Log("Player Died!");

        if (stateMachine != null)
        {
            stateMachine.SetDead();
        }

        if (playerAnimator != null)
        {
            playerAnimator.SetDead();
        }
    }
}