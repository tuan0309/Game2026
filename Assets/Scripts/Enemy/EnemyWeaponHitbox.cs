using System.Collections.Generic;
using UnityEngine;

public class EnemyWeaponHitbox : MonoBehaviour
{
    [Header("Damage")]
    [SerializeField] private int damage = 10;

    private Collider hitboxCollider;
    private bool isAttacking;

    private readonly HashSet<Health> hitPlayers =
        new HashSet<Health>();

    private void Awake()
    {
        hitboxCollider = GetComponent<Collider>();

        if (hitboxCollider == null)
        {
            Debug.LogError(
                "EnemyWeaponHitbox: Collider not found!",
                this
            );

            return;
        }

        // Hitbox chỉ dùng để trigger damage
        hitboxCollider.isTrigger = true;

        // Mặc định tắt
        hitboxCollider.enabled = false;
    }

    public void StartAttack()
    {
        isAttacking = true;

        // Mỗi cú đánh cho phép damage lại Player
        hitPlayers.Clear();

        if (hitboxCollider != null)
        {
            hitboxCollider.enabled = true;
        }

        Debug.Log("ENEMY HITBOX ON");
    }

    public void EndAttack()
    {
        isAttacking = false;

        if (hitboxCollider != null)
        {
            hitboxCollider.enabled = false;
        }

        Debug.Log("ENEMY HITBOX OFF");
    }

    private void OnDisable()
    {
        isAttacking = false;
        hitPlayers.Clear();

        if (hitboxCollider != null)
        {
            hitboxCollider.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isAttacking)
        {
            return;
        }

        // Tìm Health trên Player hoặc parent của collider
        Health playerHealth =
            other.GetComponentInParent<Health>();

        if (playerHealth == null)
        {
            return;
        }

        // Tránh damage nhiều lần trong cùng 1 attack
        if (hitPlayers.Contains(playerHealth))
        {
            return;
        }

        hitPlayers.Add(playerHealth);

        playerHealth.TakeDamage(damage);

        Debug.Log(
            "ENEMY HIT PLAYER - DAMAGE: " +
            damage
        );
    }
}