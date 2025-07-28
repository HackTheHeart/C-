using UnityEngine;

public class FruitPickupEffect : MonoBehaviour
{
    private Vector3 targetPosition;
    private float moveSpeed = 5f;
    public void Setup(Sprite sprite, Vector3 target)
    {
        GetComponent<SpriteRenderer>().sprite = sprite;
        targetPosition = target;
    }
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, targetPosition) < 0.05f)
        {
            Destroy(gameObject);
        }
    }
}
