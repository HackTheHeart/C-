using UnityEngine;

public class ProjectileChildCollider : MonoBehaviour
{
    private Projectile parentProjectile;
    void Start()
    {
        parentProjectile = GetComponentInParent<Projectile>();
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        parentProjectile?.OnChildTriggerEnter(col);
    }
}
