using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public float health;
    public float moveSpeed;
    public float attackRange;
    public LayerMask targetLayer;
    public bool isAttacking = false;
    protected Transform player;
    protected EnemyAnimation _enemyAnimation;

    protected virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        _enemyAnimation = GetComponentInChildren<EnemyAnimation>();
    }

    protected virtual void Update()
    {
        if (player == null || isAttacking) return;

        HandleMovement();
        HandleAttack();
    }

    protected abstract void HandleMovement();
    protected abstract void HandleAttack();

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    protected abstract void Die();

    public void DealDamageToTarget()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, (player.position - transform.position).normalized, attackRange, targetLayer);
        if (hit.collider != null)
        {
            Enemy target = hit.collider.GetComponent<Enemy>();
            if (target != null)
            {
                target.TakeDamage(20);
            }
        }
    }
}
