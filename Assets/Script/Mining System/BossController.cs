using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossController : MonoBehaviour
{
    [Header("Movement & Combat")]
    public float moveSpeed = 3f;
    public float attackRange = 10f;
    public float rangeAttackCooldown = 6f;
    public float meleeAttackCooldown = 2f;
    public float laserAttackCooldown = 6f;

    [Header("Damage Settings")]
    public float maxHealth = 500f;
    public int meleeDamage = 15;
    public int rangedDamage = 10;
    public int laserDamage = 25;
    private float currentHealth;

    [Header("Spawn Points")]
    public GameObject handProjectilePrefab;
    public Transform leftHandPoint;
    public Transform rightHandPoint;
    public Transform laserPoint;

    [Header("UI")]
    public Image healthBarFill;

    private BossAnimatorController animatorController;
    private List<Vector2> currentPath;
    private int pathIndex;
    private float attackTimer;
    private Transform player;

    void Start()
    {
        animatorController = GetComponentInChildren<BossAnimatorController>();
        player = Player.Instance.transform;
        currentHealth = maxHealth;
        InvokeRepeating(nameof(UpdatePath), 0f, 0.2f);
    }

    void Update()
    {
        if (player == null || currentHealth <= 0) return;

        attackTimer -= Time.deltaTime;
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            TryMeleeAttack();
        }
        else if (distanceToPlayer <= 6f)
        {
            TryRangedAttack();
        }

        if (currentHealth <= maxHealth / 2f && distanceToPlayer <= 8f)
        {
            TryLaserAttack();
        }

        MoveAlongPath();
    }

    void UpdatePath()
    {
        currentPath = Pathfinding.Instance.FindPath(transform.position, player.position);
        pathIndex = 0;
    }

    void MoveAlongPath()
    {
        if (currentPath == null || pathIndex >= currentPath.Count) return;

        Vector2 targetPos = currentPath[pathIndex];
        if (Vector2.Distance(transform.position, targetPos) < 0.1f)
        {
            pathIndex++;
            return;
        }

        Vector2 direction = (targetPos - (Vector2)transform.position).normalized;
        transform.position += (Vector3)(direction * moveSpeed * Time.deltaTime);
    }
    void TryMeleeAttack()
    {
        if (attackTimer > 0) return;

        animatorController.TriggerMeleeAttack();
        StartCoroutine(DealDamageAfterDelay(1f, "melee"));
        attackTimer = meleeAttackCooldown;
    }
    void TryRangedAttack()
    {
        if (attackTimer > 0) return;

        animatorController.TriggerRangedAttack();
        StartCoroutine(SpawnProjectileAfterDelay(1f));
        attackTimer = rangeAttackCooldown;
    }
    void TryLaserAttack()
    {
        if (attackTimer > 0) return;

        animatorController.TriggerLaser();
        StartCoroutine(FireLaserAfterDelay(1f));
        attackTimer = laserAttackCooldown;
    }
    IEnumerator DealDamageAfterDelay(float delay, string type)
    {
        yield return new WaitForSeconds(delay);
        float dist = Vector2.Distance(transform.position, player.position);
        if (type == "melee" && dist <= attackRange)
        {
            Player.Instance.GetCharacterStats().TakeDamage(meleeDamage);
        }
    }
    IEnumerator SpawnProjectileAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (handProjectilePrefab != null)
        {
            SpawnProjectileFromPoint(leftHandPoint);
            SpawnProjectileFromPoint(rightHandPoint);
        }
    }
    void SpawnProjectileFromPoint(Transform point)
    {
        if (point == null) return;
        GameObject proj = Instantiate(handProjectilePrefab, point.position, Quaternion.identity);
        Vector2 dir = (player.position - point.position).normalized;
        proj.GetComponent<Rigidbody2D>().linearVelocity = dir * 5f;
    }
    private IEnumerator FireLaserAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Vector2 direction = (player.position - laserPoint.position).normalized;
        Quaternion rotation = Quaternion.FromToRotation(Vector3.right, direction);
        GameObject laser = Instantiate(Resources.Load<GameObject>("LaserBeam"), laserPoint.position, rotation);
        LaserBeam beam = laser.GetComponent<LaserBeam>();
        beam.damageAmount = 5;
    }
    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI();
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    void UpdateHealthUI()
    {
        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = currentHealth / maxHealth;
        }
    }
    void Die()
    {
        Debug.Log("Boss Died!");
        animatorController.TriggerDeath();
        this.enabled = false; 
    }
}
