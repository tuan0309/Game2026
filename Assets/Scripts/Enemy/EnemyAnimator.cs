using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator animator;

    private void Awake()
    {
        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }

        if (animator == null)
        {
            Debug.LogError(
                "EnemyAnimator: Animator is missing!"
            );
        }
    }

    public void SetHit()
    {
        if (animator == null)
        {
            return;
        }

        animator.SetTrigger("Hit");
    }

    public void SetDead()
    {
        if (animator == null)
        {
            return;
        }

        animator.SetBool("Dead", true);
    }
}