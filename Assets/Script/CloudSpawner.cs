using UnityEngine;

public class CloudSpawner : MonoBehaviour
{
    public GameObject cloudPrefab;
    public float spawnRate = 3f;
    public Vector2 spawnRangeY = new Vector2(1f, 5f);
    private Transform playerCamera;
    void Start()
    {
        playerCamera = Camera.main.transform;
        InvokeRepeating(nameof(SpawnCloud), 1f, spawnRate);
    }
    void SpawnCloud()
    {
        float spawnY = Random.Range(spawnRangeY.x, spawnRangeY.y);
        Vector2 spawnPosition = new Vector2(playerCamera.position.x + 10f, spawnY);
        Instantiate(cloudPrefab, spawnPosition, Quaternion.identity);
    }
}
