//using UnityEngine;

//public class Projectile : MonoBehaviour
//{
//    public float speed = 2f;
//    public int damage = 5;
//    private Vector2 direction;
//    public GameObject arrowUp;
//    public GameObject arrowDown;
//    public GameObject arrowLeft;
//    public GameObject arrowRight;
//    //public void Initialize(Vector2 dir)
//    //{
//    //    direction = dir.normalized;
//    //    Destroy(gameObject, 5f);
//    //    SetDirectionVisual();
//    //}
//    void Update()
//    {
//        transform.position += (Vector3)(direction * speed * Time.deltaTime);
//    }
//    //void SetDirectionVisual()
//    //{
//    //    arrowUp.SetActive(false);
//    //    arrowDown.SetActive(false);
//    //    arrowLeft.SetActive(false);
//    //    arrowRight.SetActive(false);
//    //    if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
//    //    {
//    //        if (direction.x > 0) arrowRight.SetActive(true);
//    //        else arrowLeft.SetActive(true);
//    //    }
//    //    else
//    //    {
//    //        if (direction.y > 0) arrowUp.SetActive(true);
//    //        else arrowDown.SetActive(true);
//    //    }
//    //}
//    public void Initialize(Vector2 dir)
//    {
//        direction = dir.normalized;
//        RotateTowardsPlayer();
//        Destroy(gameObject, 5f);
//        //SetDirectionVisual();
//    }

//    void RotateTowardsPlayer()
//    {
//        if (Player.Instance == null) return;

//        Vector3 toPlayer = Player.Instance.transform.position - transform.position;
//        float angle = Mathf.Atan2(toPlayer.y, toPlayer.x) * Mathf.Rad2Deg;
//        transform.rotation = Quaternion.Euler(0, 0, angle);
//    }

//    //void OnTriggerEnter2D(Collider2D col)
//    //{
//    //    Debug.Log("Collided with: " + col.name);
//    //    if (col.CompareTag("Player"))
//    //    {
//    //        Player.Instance.GetCharacterStats().TakeDamage(damage);
//    //        Destroy(gameObject);
//    //    }
//    //    else if (!col.isTrigger)
//    //    {
//    //        Destroy(gameObject);
//    //    }
//    //}
//    public void OnChildTriggerEnter(Collider2D col)
//    {
//        Debug.Log("Collided with: " + col.name);
//        if (col.CompareTag("Player"))
//        {
//            Player.Instance.GetCharacterStats().TakeDamage(damage);
//            Destroy(gameObject);
//        }
//        else if (!col.isTrigger)
//        {
//            Destroy(gameObject);
//        }
//    }
//}
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 2f;
    public int damage = 5;
    private Vector2 direction;
    public void Initialize(Vector2 dir)
    {
        direction = dir.normalized;
        RotateTowardsPlayer();
        Destroy(gameObject, 5f);
    }
    void Update()
    {
        transform.position += (Vector3)(direction * speed * Time.deltaTime);
    }
    void RotateTowardsPlayer()
    {
        if (Player.Instance == null) return;
        Vector3 toPlayer = Player.Instance.transform.position - transform.position;
        float angle = Mathf.Atan2(toPlayer.y, toPlayer.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
    }
    public void OnChildTriggerEnter(Collider2D col)
    {
        Debug.Log("Collided with: " + col.name);
        if (col.CompareTag("Player"))
        {
            Player.Instance.GetCharacterStats().TakeDamage(damage);
            Destroy(gameObject);
        }
        else if (!col.isTrigger)
        {
            Destroy(gameObject);
        }
    }
}
