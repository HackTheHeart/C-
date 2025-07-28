using System.Collections;
using UnityEngine;

public class LaserBeam : MonoBehaviour
{
    public float damageInterval = 0.3f;
    public float duration = 1.5f;
    public int damageAmount = 5;
    private float timer;
    void Start()
    {
        Destroy(gameObject, duration);
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                Player.Instance.GetCharacterStats().TakeDamage(damageAmount);
                timer = damageInterval;
            }
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            timer = 0f;
        }
    }
}
