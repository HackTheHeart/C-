using UnityEngine;
using System.Collections;

public class Bomb : MonoBehaviour
{
    public float arcHeight = 2f;
    public float travelTime = 1.2f;
    public float explosionDelay = 0.4f;
    public float explosionRadius = 1.5f;
    public int damage = 10;
    private Vector2 startPos;
    private Vector2 targetPos;
    private float timer;
    private Animator animator;
    private bool hasExploded = false;
    public void Initialize(Vector2 target)
    {
        startPos = transform.position;
        targetPos = target;
        animator = GetComponent<Animator>();
        StartCoroutine(TravelToTarget());
    }
    IEnumerator TravelToTarget()
    {
        timer = 0f;

        while (timer < travelTime)
        {
            float t = timer / travelTime;
            Vector2 currentPos = Vector2.Lerp(startPos, targetPos, t);
            float height = Mathf.Sin(Mathf.PI * t) * arcHeight;
            transform.position = new Vector3(currentPos.x, currentPos.y + height, transform.position.z);
            timer += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPos;
        Explode();
    }
    void Explode()
    {
        if (hasExploded) return;
        hasExploded = true;
        if (animator != null)
        {
            animator.SetTrigger("Explode");
        }
        Invoke(nameof(DoDamage), 0.55f); 
        Destroy(gameObject, explosionDelay);
    }
    void DoDamage()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                Player.Instance.GetCharacterStats().TakeDamage(damage);
            }
        }
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
