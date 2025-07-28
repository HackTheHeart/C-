using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public enum GoblinType { Archer, Bomber }

public class GoblinRangedAI : MonoBehaviour
{
    public GoblinType goblinType;
    public float health = 15f;
    public float moveSpeed = 1.5f;
    public float chaseSpeed = 2.7f;
    public float patrolInterval = 5f;
    public float chaseRange = 6f;
    public float stopChaseDistance = 3f;
    public float attackCooldown = 1.4f;
    public float attackRange = 3f;
    public LayerMask obstacleLayer;
    public GameObject arrowPrefab;
    public GameObject bombPrefab;
    public Transform firePoint;
    public GridScanner gridScanner;
    private Vector2 lastDirection = Vector2.right;
    private Transform player;
    private Vector2 patrolPoint;
    private bool isChasing = false;
    private bool isAttacking = false;
    private bool canAttack = true;
    private bool isRetreating = false;
    private float patrolTimer;
    private EnemyAnimation _enemyAnimation;
    private bool isDead = false;
    private bool isStunned = false;
    private Vector2 lastPlayerPosition = Vector2.positiveInfinity;
    private float recalcPathThreshold = 0.2f;
    private List<Vector2> path = new List<Vector2>();
    private int pathIndex = 0;
    void Start()
    {
        gridScanner = FindFirstObjectByType<GridScanner>();
        if (gridScanner == null) { Debug.Log("gridScanner null"); }
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 30;
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        _enemyAnimation = GetComponentInChildren<EnemyAnimation>();
        patrolTimer = patrolInterval;
    }
    public float actionCooldown = 0.01f;
    private float actionTimer = 0f;
    void Update()
    {
        if (player == null) return;

        actionTimer += Time.deltaTime;

        //if (actionTimer >= actionCooldown)
        //{
            actionTimer = 0f;
            float distance = Vector2.Distance(transform.position, player.position);
            Vector2 dir = (player.position - transform.position).normalized;
            if (PlayerInSight())
            {
                if (isDead || player == null || isStunned) return;
                if (distance < attackRange && canAttack && !isAttacking)
                {
                    TryAttack(dir);
                }
                else if (distance < attackRange && !canAttack && !isAttacking && distance >= attackRange / 2)
                {
                    _enemyAnimation.SetMovement(Vector2.zero, patrolling: false, chasing: false);
                }
                else if (distance <= chaseRange + 6 && distance > attackRange)
                {
                    isChasing = true;
                    Chase(dir);
                }
                else if (distance <= attackRange && distance >= attackRange / 2 && !isAttacking)
                {
                    _enemyAnimation.SetMovement(Vector2.zero, patrolling: false, chasing: false);
                }
                else if (distance < attackRange / 2 && !isAttacking)
                {
                    Retreat();
                }
            }
            else if (!isAttacking)
            {
                isChasing = false;
                Patrol();
            }
        //}
    }
    bool PlayerInSight()
    {
        float radius = chaseRange;
        Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, radius, LayerMask.GetMask("Player"));
        if (playerCollider != null)
        {
            Vector2 directionToPlayer = (player.position - transform.position).normalized;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, radius, obstacleLayer);
            if (hit.collider == null || hit.collider.CompareTag("Player"))
            {
                return true;
            }
        }
        return false;
    }
    void Retreat()
    {
        if (player == null) return;

        Vector2 directionAwayFromPlayer = (transform.position - player.position).normalized;
        Vector2 retreatTarget = (Vector2)player.position + directionAwayFromPlayer * Random.Range(3f, 6f);

        // Kiểm tra xem vị trí lùi có đi được không
        if (!IsWalkable(retreatTarget))
        {
            Debug.Log("Goblin không tìm được chỗ lùi → đứng yên.");
            _enemyAnimation.SetMovement(Vector2.zero, false, false);
            return;
        }

        List<Vector2> path = Pathfinding.Instance.FindPath(transform.position, retreatTarget);
        if (path.Count > 1)
        {
            Vector2 nextPosition = path[1];
            transform.position = Vector2.MoveTowards(transform.position, nextPosition, chaseSpeed * Time.deltaTime);
            _enemyAnimation.SetMovement((nextPosition - (Vector2)transform.position).normalized, false, true);
        }
        else
        {
            Debug.Log("Goblin không tìm được đường rút lui hợp lệ → đứng yên.");
            _enemyAnimation.SetMovement(Vector2.zero, false, false);
        }
    }

    //void Chase(Vector2 direction)
    //{
    //    List<Vector2> path = Pathfinding.Instance.FindPath(transform.position, player.position);
    //    Debug.Log("Path Count: " + path.Count);
    //    if (path.Count > 1)
    //    {
    //        Vector2 nextPosition = path[1];
    //        Debug.Log("Next Position: " + nextPosition);
    //        transform.position = Vector2.MoveTowards(transform.position, nextPosition, chaseSpeed * Time.deltaTime);
    //        _enemyAnimation.SetMovement(direction, false, true);
    //    }
    //}
    void Chase(Vector2 direction)
    {
        float distanceMoved = Vector2.Distance(player.position, lastPlayerPosition);
        if (distanceMoved > recalcPathThreshold || path.Count == 0 || pathIndex >= path.Count)
        {
            lastPlayerPosition = player.position;
            path = Pathfinding.Instance.FindPath(transform.position, player.position);
            pathIndex = path.Count > 1 ? 1 : 0;
        }

        if (path.Count > pathIndex)
        {
            Vector2 nextPosition = path[pathIndex];
            transform.position = Vector2.MoveTowards(transform.position, nextPosition, chaseSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, nextPosition) < 0.1f)
                pathIndex++;

            _enemyAnimation.SetMovement(direction, false, true);
        }
    }

    void TryAttack(Vector2 dir)
    {
        RaycastHit2D hit = Physics2D.Linecast(transform.position, player.position, obstacleLayer);
        if (hit.collider == null || hit.collider.CompareTag("Player"))
        {
            isAttacking = true;
            canAttack = false;
            _enemyAnimation.PlayAttackAnimation(dir);
            Invoke(nameof(PerformAttack), 0.4f);
            Invoke(nameof(EndAttackAnimation), 0.4f);
            Invoke(nameof(ResetAttack), attackCooldown);
        }
    }
    void PerformAttack()
    {
        if (goblinType == GoblinType.Archer)
        {
            GameObject arrow = Instantiate(arrowPrefab, firePoint.position, Quaternion.identity);
            arrow.GetComponent<Projectile>().Initialize((player.position - firePoint.position).normalized);
        }
        else if (goblinType == GoblinType.Bomber)
        {
            GameObject bomb = Instantiate(bombPrefab, transform.position, Quaternion.identity);
            bomb.GetComponent<Bomb>().Initialize(player.position);
        }
        //Invoke(nameof(Retreat), 0.4f);
    }
    //void Retreat()
    //{
    //    if (player == null) return;
    //    isRetreating = true;
    //    Vector2 directionToPlayer = (transform.position - player.position).normalized;
    //    List<Vector2Int> validRetreatPositions = new List<Vector2Int>();
    //    foreach (var tile in gridScanner.walkableTiles)
    //    {
    //        if (tile.Value)
    //        {
    //            Vector2Int position = tile.Key;
    //            Vector2 worldPosition = gridScanner.groundTilemap.GetCellCenterWorld(new Vector3Int(position.x, position.y, 0));
    //            Vector2 directionToTile = (worldPosition - (Vector2)transform.position).normalized;
    //            if (Vector2.Dot(directionToPlayer, directionToTile) < 0)
    //            {
    //                validRetreatPositions.Add(position);
    //            }
    //        }
    //    }
    //    if (validRetreatPositions.Count > 0)
    //    {
    //        Vector2Int retreatPosition = validRetreatPositions[Random.Range(0, validRetreatPositions.Count)];
    //        Vector2 retreatTarget = gridScanner.groundTilemap.GetCellCenterWorld(new Vector3Int(retreatPosition.x, retreatPosition.y, 0));
    //        StartCoroutine(MoveToPosition(retreatTarget, 0.5f));
    //    }
    //    else
    //    {
    //        Vector2Int randomPosition = gridScanner.walkableTiles.Keys.ElementAt(Random.Range(0, gridScanner.walkableTiles.Count));
    //        Vector2 randomTarget = gridScanner.groundTilemap.GetCellCenterWorld(new Vector3Int(randomPosition.x, randomPosition.y, 0));
    //        StartCoroutine(MoveToPosition(randomTarget, 0.5f));
    //    }
    //}
    IEnumerator MoveToPosition(Vector2 target, float duration)
    {
        float elapsed = 0;
        Vector2 start = transform.position;
        while (elapsed < duration)
        {
            transform.position = Vector2.Lerp(start, target, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = target;
        //isRetreating = false;
        _enemyAnimation.SetMovement(Vector2.zero, patrolling: false, chasing: false);
    }
    void ResetAttack()
    {
        canAttack = true;
    }
    void EndAttackAnimation()
    {
        isAttacking = false;
        _enemyAnimation.SetMovement(Vector2.zero, patrolling: false, chasing: false);
    }

    void Patrol()
    {
        patrolTimer -= Time.deltaTime;
        if (patrolTimer <= 0)
        {
            patrolTimer = patrolInterval;
            List<Vector2Int> walkable = new List<Vector2Int>();
            foreach (var tile in gridScanner.walkableTiles)
            {
                if (tile.Value)
                    walkable.Add(tile.Key);
            }
            if (walkable.Count == 0) return;
            Vector2Int random = walkable[Random.Range(0, walkable.Count)];
            patrolPoint = gridScanner.groundTilemap.GetCellCenterWorld(new Vector3Int(random.x, random.y, 0));
        }
        Vector2 direction = (patrolPoint - (Vector2)transform.position).normalized;
        if (direction != Vector2.zero)
            lastDirection = direction;
        transform.position = Vector2.MoveTowards(transform.position, patrolPoint, moveSpeed * Time.deltaTime);
        _enemyAnimation.SetMovement(direction, true, false);
        if (Vector2.Distance(transform.position, patrolPoint) < 0.05f)
        {
            _enemyAnimation.SetMovement(lastDirection, false, false);
            return;
        }
    }
    bool IsWalkable(Vector2 position)
    {
        Vector3Int tilePosition = gridScanner.groundTilemap.WorldToCell(position);
        return gridScanner.walkableTiles.ContainsKey(new Vector2Int(tilePosition.x, tilePosition.y)) && gridScanner.walkableTiles[new Vector2Int(tilePosition.x, tilePosition.y)];
    }
    public void TakeDamage(int dmg)
    {
        health -= dmg;
        _enemyAnimation.PlayDamageAnimation(_enemyAnimation.lastInteractionDir);
        StartCoroutine(StunForSeconds(0.5f));

        if (health <= 0)
        {
            isDead = true;
            _enemyAnimation.PlayDeadAnimation(_enemyAnimation.lastInteractionDir);
            Destroy(gameObject, 0.4f);
        }
    }
    IEnumerator StunForSeconds(float duration)
    {
        isStunned = true;
        yield return new WaitForSeconds(duration);
        isStunned = false;
    }

}
