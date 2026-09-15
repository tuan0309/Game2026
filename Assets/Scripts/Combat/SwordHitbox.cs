using System.Collections.Generic;
using UnityEngine;

public class SwordHitbox : MonoBehaviour
{
    [Header("Damage")]
    [SerializeField] private Weapon weapon;

    private Collider hitboxCollider;

    private bool isAttacking;

    private readonly HashSet<EnemyHealth> hitEnemies =
        new HashSet<EnemyHealth>();

    private void Awake()
    {
        hitboxCollider =
            GetComponent<Collider>();

        if (weapon == null)
        {
            weapon =
                GetComponentInParent<Weapon>();
        }

        SetHitbox(false);

        if (weapon == null)
        {
            Debug.LogWarning(
                "SwordHitbox: Weapon is missing!"
            );
        }
    }

    private void OnDisable()
    {
        isAttacking = false;

        hitEnemies.Clear();

        SetHitbox(false);
    }

    public void StartAttack()
    {
        isAttacking = true;

        hitEnemies.Clear();

        SetHitbox(true);

        Debug.Log(
            "Sword Hitbox ON"
        );
    }

    public void EndAttack()
    {
        isAttacking = false;

        SetHitbox(false);

        Debug.Log(
            "Sword Hitbox OFF"
        );
    }

    private void SetHitbox(
        bool active
    )
    {
        if (hitboxCollider == null)
        {
            return;
        }

        hitboxCollider.enabled =
            active;
    }

    private void OnTriggerEnter(
        Collider other
    )
    {
        if (!isAttacking)
        {
            return;
        }

        if (
            other.GetComponentInParent<PlayerController>()
            != null
        )
        {
            return;
        }

        EnemyHealth enemyHealth =
            other.GetComponentInParent<EnemyHealth>();

        if (enemyHealth == null)
        {
            return;
        }

        if (hitEnemies.Contains(enemyHealth))
        {
            return;
        }

        if (weapon == null)
        {
            Debug.LogWarning(
                "SwordHitbox: Weapon is missing!"
            );

            return;
        }

        hitEnemies.Add(enemyHealth);

        enemyHealth.TakeDamage(
            weapon.Damage
        );

        Debug.Log(
            "DAMAGE DEALT: " +
            weapon.Damage
        );
    }
}