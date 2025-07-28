using System.Collections;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    private const string IsWalking = "IsWalking";
    private const string IsRunning = "IsRunning";
    private const string Xinput = "XInput";
    private const string Yinput = "YInput";
    private const string AttackTrigger = "IsAttacking";
    private const string DamageTrigger = "IsDamaged";
    private const string DeadTrigger = "IsDead"; 
    private const string PrimaryDirection = "PrimaryDirection";
    private bool isPatrolling;
    private bool isChasing;
    //public Animator _animator;
    //private Enemy _enemy;
    public Animator _animator;
    private Vector2 movementInput;
    private bool isWalking;
    private bool isRunning;
    public Vector2 lastInteractionDir = Vector2.right;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        //_enemy = GetComponent<Enemy>();
    }
    //private void LateUpdate()
    //{
    //    _animator.SetBool(IsWalking, _enemy.IsWalking());
    //    _animator.SetBool(IsRunnig, _enemy.IsRunning());
    //    _animator.SetFloat(Xinput, _enemy.Xinput);
    //    _animator.SetFloat(Yinput, _enemy.Yinput);
    //}
    private void LateUpdate()
    {
        _animator.SetBool(IsWalking, isWalking);
        _animator.SetBool(IsRunning, isRunning);
        _animator.SetFloat(Xinput, movementInput.x);
        _animator.SetFloat(Yinput, movementInput.y);
    }
    public void SetMovement(Vector2 input, bool patrolling, bool chasing)
    {
        if (input.sqrMagnitude > 0.01f)
        {
            lastInteractionDir = input.normalized;
        }
        isPatrolling = patrolling;
        isChasing = chasing;
        movementInput = input;
        isWalking = isPatrolling;
        isRunning = isChasing;
        if (!isWalking && !isRunning)
        {
            movementInput = Vector2.zero;
        }

    }
    public void PlayAttackAnimation(Vector2 attackDirection)
    {
        _animator.SetTrigger(AttackTrigger);
        _animator.SetInteger(PrimaryDirection, GetPrimaryDirection(attackDirection.x, attackDirection.y));
    }
    public void PlayDamageAnimation(Vector2 hitDirection)
    {
        _animator.SetTrigger(DamageTrigger);
        _animator.SetInteger(PrimaryDirection, GetPrimaryDirection(hitDirection.x, hitDirection.y));
    }
    public void PlayDeadAnimation(Vector2 deathDirection)
    {
        _animator.SetTrigger(DeadTrigger);
        _animator.SetInteger(PrimaryDirection, GetPrimaryDirection(deathDirection.x, deathDirection.y));
    }
    private int GetPrimaryDirection(float x, float y)
    {
        if (Mathf.Abs(x) >= Mathf.Abs(y))
        {
            return (x > 0) ? 0 : 1;
        }
        else
        {
            return (y > 0) ? 2 : 3;
        }
    }
}
