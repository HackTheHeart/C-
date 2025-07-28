using UnityEngine;

public class BossAnimatorController : MonoBehaviour
{
    public Animator animator;
    public Transform target;
    void Update()
    {
        UpdateDirection();
    }
    void UpdateDirection()
    {
        if (target == null || animator == null) return;
        float xDir = target.position.x - transform.position.x;
        float xInput = xDir > 0 ? 1f : -1f;
        animator.SetFloat("XInput", xInput);
        Vector3 scale = transform.localScale;
        scale.x = xInput;
        transform.localScale = scale;
    }
    public void TriggerMeleeAttack()
    {
        animator.SetTrigger("IsMeleeAttacking");
    }
    public void TriggerRangedAttack()
    {
        animator.SetTrigger("IsRagedAttacking");
    }
    public void TriggerLaser()
    {
        animator.SetTrigger("IsLazering");
    }
    public void TriggerImmune()
    {
        animator.SetTrigger("IsImmune");
    }
    public void TriggerDeath()
    {
        animator.SetTrigger("IsDeath");
    }
}
