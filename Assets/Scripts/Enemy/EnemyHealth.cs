using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float maxHealth = 100f;

    [Header("Animation")]
    [SerializeField] private EnemyAnimator enemyAnimator;

    private float currentHealth;

    public float CurrentHealth => currentHealth;

    public float MaxHealth => maxHealth;

    public bool IsDead => currentHealth <= 0f;

    private void Awake()
    {
        if (enemyAnimator == null)
        {
            enemyAnimator = GetComponent<EnemyAnimator>();
        }

        currentHealth = maxHealth;
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
            gameObject.name +
            " Health: " +
            currentHealth +
            " / " +
            maxHealth
        );

        if (IsDead)
        {
            Die();
        }
        else
        {
            Hit();
        }
    }

    private void Hit()
    {
        Debug.Log(
            gameObject.name +
            " HIT!"
        );

        if (enemyAnimator != null)
        {
            enemyAnimator.SetHit();
        }
    }

    private void Die()
    {
        Debug.Log(
            gameObject.name +
            " DIED!"
        );

        if (enemyAnimator != null)
        {
            enemyAnimator.SetDead();
        }
    }
}