using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GoblinAI : MonoBehaviour
{
    public int damageTaken = 7;
    public float health = 21f;
    public float moveSpeed = 2f;
    public float runSpeed = 4f;
    public float chaseRange = 3f;
    public float patrolWaitTime = 2f;
    public LayerMask obstacleLayer;
    public GridScanner gridScanner;
    private Transform player;
    private List<Vector2> path = new List<Vector2>();
    private int pathIndex = 0;
    private Vector2 lastSeenPosition;
    private bool chasing = false;
    private bool waiting = false;
    private Vector2 currentPatrolPoint;
    private EnemyAnimation _enemyAnimation;
    private bool hasPatrolPoint = false;
    private bool isAttacking = false;
    private bool isDead = false;
    private bool isStunned = false;

    private Vector2 lastPlayerPosition = Vector2.positiveInfinity;
    private float recalcThreshold = 0.2f;
    //private bool isRetreating = false;
    void Start()
    {
        gridScanner = FindFirstObjectByType<GridScanner>();
        if(gridScanner == null) { Debug.Log("gridScanner null"); }
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 30;
        _enemyAnimation = GetComponentInChildren<EnemyAnimation>();
        int playerLayer = LayerMask.NameToLayer("Player");
        if (playerLayer == -1)
        {
            return;
        }
        if (Player.Instance != null)
        {
            player = Player.Instance.transform;
            lastPlayerPosition = player.position;
        }
        else
        {
            Debug.LogWarning("Player.Instance is null in GoblinAI.");
        }
        StartPatrolling();
    }
    void Update()
    {
            if (isDead || player == null || isStunned) return;
            if (isAttacking) return;
            if (PlayerVisible())
            {
                lastSeenPosition = player.position;
                chasing = true;
                waiting = false;

                float distanceMoved = Vector2.Distance(player.position, lastPlayerPosition);
                if (distanceMoved > recalcThreshold)
                {
                    lastPlayerPosition = player.position;
                    FollowTarget();
                }
            }
            else if (chasing)
                {
                    MoveToLastSeenPosition();
                }
            else if (!waiting)
            {
                Patrol();
            }
            if (chasing && InAttackRange() && !canAttack)
            {
                _enemyAnimation.SetMovement(Vector2.zero, false, false);
                return;
            }
            MoveAlongPath();
            if (chasing && PlayerVisible() && InAttackRange())
            {
                TryAttack();
            }
    }
    bool InAttackRange()
    {
        return Vector2.Distance(transform.position, player.position) <= attackRange;
    }
    bool PlayerVisible()
    {
        if (Vector2.Distance(transform.position, player.position) > chaseRange) return false;
        Vector2 directionToPlayer = (player.position - transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, chaseRange, obstacleLayer);
        if (hit.collider == null)
        {
            return true;
        }
        return false;
    }
    void FollowTarget()
    {
        path = Pathfinding.Instance.FindPath(transform.position, player.position);
        if (path.Count > 1)
            pathIndex = 1;
        else
            pathIndex = 0;
    }
    void MoveToLastSeenPosition()
    {
        path = Pathfinding.Instance.FindPath(transform.position, lastSeenPosition);
        if (path.Count > 1)
            pathIndex = 1;
        else
            pathIndex = 0;
        if (Vector2.Distance(transform.position, lastSeenPosition) < 0.5f)
        {
            chasing = false;
            waiting = true;
            Invoke(nameof(StartPatrolling), patrolWaitTime);
        }
    }
    public void TakeDamage(int damage)
    {
        health -= damage;
        _enemyAnimation.PlayDamageAnimation(_enemyAnimation.lastInteractionDir);
        StartCoroutine(StunForSeconds(0.5f));

        if (health <= 0)
        {
            isDead = true;
            Die();
        }
    }
    IEnumerator StunForSeconds(float duration)
    {
        isStunned = true;
        yield return new WaitForSeconds(duration);
        isStunned = false;
    }

    public void Die()
    {
        _enemyAnimation.PlayDeadAnimation(_enemyAnimation.lastInteractionDir);
        Destroy(gameObject, 0.4f);
    }
    void Patrol()
    {
        if (hasPatrolPoint) return;
        List<Vector2Int> walkablePositions = new List<Vector2Int>();
        foreach (var tile in gridScanner.walkableTiles)
        {
            if (tile.Value)
            {
                walkablePositions.Add(tile.Key);
            }
        }
        if (walkablePositions.Count == 0)
        {
            return;
        }
        Vector2Int randomGridPos = walkablePositions[Random.Range(0, walkablePositions.Count)];
        currentPatrolPoint = gridScanner.groundTilemap.GetCellCenterWorld(new Vector3Int(randomGridPos.x, randomGridPos.y, 0));
        path = Pathfinding.Instance.FindPath(transform.position, currentPatrolPoint);
        if (path.Count > 1)
            pathIndex = 1;
        else
            pathIndex = 0;
        hasPatrolPoint = true;
    }
    void StartPatrolling()
    {
        waiting = false;
        Patrol();
    }
    void MoveAlongPath()
    {
        if (path == null || path.Count == 0 || pathIndex >= path.Count)
        {
            hasPatrolPoint = false;
            Invoke(nameof(Patrol), 1f);
            return;
        }
        Vector2 nextPosition = path[pathIndex];
        Debug.Log("next target point:" + nextPosition);
        Vector2 moveDirection = (nextPosition - (Vector2)transform.position).normalized;
        if (moveDirection != Vector2.zero)
        {
            _enemyAnimation.lastInteractionDir = moveDirection;
        }
        float speed = chasing ? runSpeed : moveSpeed;
        transform.position = Vector2.MoveTowards(transform.position, nextPosition, speed * Time.deltaTime);
        Debug.Log("Da den vi tri chi dinh");
        _enemyAnimation.SetMovement(moveDirection, !chasing, chasing);
        if (Vector2.Distance(transform.position, nextPosition) < 0.1f)
        {
            pathIndex++;
            Debug.Log("tang path len" + pathIndex);
            if (pathIndex >= path.Count && chasing)
            {
                FollowTarget();
            }
        }
        if (Vector2.Distance(transform.position, currentPatrolPoint) < 0.5f)
        {
            hasPatrolPoint = false; 
            Invoke(nameof(Patrol), patrolWaitTime);
        }
    }
    public float attackCooldown = 1.4f;
    public int attackDamage = 10;
    private bool canAttack = true;
    float attackRange = 1f;
    void TryAttack()
    {
        if (canAttack && Vector2.Distance(transform.position, player.position) < attackRange)
        {
            RaycastHit2D hit = Physics2D.Linecast(transform.position, player.position, obstacleLayer);
            if (hit.collider == null || hit.collider.CompareTag("Player"))
            {
                Attack();
            }
        }
    }
    void Attack()
    {
        canAttack = false;
        isAttacking = true;
        Debug.Log("Goblin tấn công Player!");
        Vector2 attackDirection = (player.position - transform.position).normalized;
        _enemyAnimation.PlayAttackAnimation(attackDirection);
        Player.Instance.GetCharacterStats().TakeDamage(attackDamage);
        //Invoke(nameof(Retreat), 0.5f);
        Invoke(nameof(EndOfAnimation), 0.4f);
        Invoke(nameof(ResetAttack), attackCooldown);
    }
    //void Retreat()
    //{
    //    if (player == null) return;
    //    isRetreating = true;
    //    Vector2 retreatDirection = (transform.position - player.position).normalized;
    //    Vector2 retreatTarget = (Vector2)transform.position + retreatDirection * 1.5f;
    //    StartCoroutine(MoveToPosition(retreatTarget, 0.5f));
    //}
    //IEnumerator MoveToPosition(Vector2 target, float duration)
    //{
    //    float elapsedTime = 0;
    //    Vector2 startPos = transform.position;
    //    while (elapsedTime < duration)
    //    {
    //        transform.position = Vector2.Lerp(startPos, target, elapsedTime / duration);
    //        elapsedTime += Time.deltaTime;
    //        yield return null;
    //    }
    //    transform.position = target;
    //    isRetreating = false;
    //}
    void EndOfAnimation()
    {
        isAttacking = false;
    }
    void ResetAttack()
    {
        canAttack = true;
    }
}