using UnityEngine;

public class CloudController : MonoBehaviour
{
    public float minSpeed = 1f;
    public float maxSpeed = 3f;
    private float speed;
    private float destroyOffset = 2f;
    private Transform playerCamera;
    void Start()
    {
        playerCamera = Camera.main.transform;
        speed = Random.Range(minSpeed, maxSpeed);
    }
    void Update()
    {
        transform.position = new Vector2(transform.position.x - speed * Time.deltaTime, transform.position.y);
        if (transform.position.x < playerCamera.position.x - destroyOffset)
        {
            Destroy(gameObject);
        }
    }
}
