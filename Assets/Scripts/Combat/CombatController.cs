using UnityEngine;
using UnityEngine.InputSystem;

public class CombatController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Weapon weapon;
    [SerializeField] private SwordHitbox swordHitbox;
    [SerializeField] private PlayerStateMachine stateMachine;
    [SerializeField] private PlayerAnimator playerAnimator;

    private float lastAttackTime;
    private bool isAttacking;

    private void Awake()
    {
        if (weapon == null)
        {
            weapon = GetComponentInChildren<Weapon>();
        }

        if (swordHitbox == null)
        {
            swordHitbox = GetComponentInChildren<SwordHitbox>();
        }

        if (stateMachine == null)
        {
            stateMachine = GetComponent<PlayerStateMachine>();
        }

        if (playerAnimator == null)
        {
            playerAnimator = GetComponent<PlayerAnimator>();
        }

        if (weapon == null)
        {
            Debug.LogWarning(
                "CombatController: Weapon is missing!"
            );
        }

        if (swordHitbox == null)
        {
            Debug.LogWarning(
                "CombatController: SwordHitbox is missing!"
            );
        }
    }

    private void Update()
    {
        if (Keyboard.current == null)
        {
            return;
        }

        if (Keyboard.current.jKey.wasPressedThisFrame)
        {
            TryAttack();
        }
    }

    private void TryAttack()
    {
        if (weapon == null)
        {
            return;
        }

        if (stateMachine != null &&
            stateMachine.CurrentState == PlayerState.Dead)
        {
            return;
        }

        if (isAttacking)
        {
            return;
        }

        if (Time.time <
            lastAttackTime + weapon.AttackCooldown)
        {
            return;
        }

        lastAttackTime = Time.time;
        isAttacking = true;

        if (stateMachine != null)
        {
            stateMachine.ChangeState(
                PlayerState.Attack
            );
        }

        if (playerAnimator != null)
        {
            playerAnimator.SetAttack();
        }

        Debug.Log("ATTACK START");
    }

    // Gọi bằng Animation Event
    public void EnableSwordHitbox()
    {
        if (!isAttacking)
        {
            return;
        }

        if (swordHitbox != null)
        {
            swordHitbox.StartAttack();
        }
    }

    // Gọi bằng Animation Event
    public void DisableSwordHitbox()
    {
        if (swordHitbox != null)
        {
            swordHitbox.EndAttack();
        }
    }

    // Gọi ở cuối animation Attack
    public void EndAttack()
    {
        if (!isAttacking)
        {
            return;
        }

        isAttacking = false;

        if (swordHitbox != null)
        {
            swordHitbox.EndAttack();
        }

        if (stateMachine != null &&
            stateMachine.CurrentState == PlayerState.Attack)
        {
            stateMachine.ChangeState(
                PlayerState.Idle
            );
        }

        Debug.Log("ATTACK END");
    }
}