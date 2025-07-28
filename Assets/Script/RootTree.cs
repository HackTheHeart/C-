using System.Collections;
using UnityEngine;

public class RootTree : NaturalObjects
{
    [SerializeField] private int choppingCounts = 2;
    [SerializeField] private GameObject woodPrefab;
    [SerializeField] private int woodCount = 2;
    private int currentCounts = 0;
    public override void Interact()
    {
        originalPosition = transform.position;
        Player player = Player.Instance;
        if (player != null && !player.IsPerformingAction())
        {
            StartCoroutine(ChopRoot(player));
        }
    }
    private IEnumerator ChopRoot(Player player)
    {
        player.SetPerformingAction(true);
        PlayerAnimation playerAnimation = FindFirstObjectByType<PlayerAnimation>();
        if (playerAnimation != null)
        {
            playerAnimation.PlayChopAnimation(player.LastInteractDir);
        }
        yield return StartCoroutine(ShakeObject());
        yield return new WaitForSeconds(0.40f);
        currentCounts++;
        if (currentCounts >= choppingCounts)
        {
            SpawnWood();
            Destroy(gameObject);
        }
        player.SetPerformingAction(false);
    }
    private void SpawnWood()
    {
        for (int i = 0; i < woodCount; i++)
        {
            Vector2 spawnPosition = (Vector2)transform.position + UnityEngine.Random.insideUnitCircle * 0.5f;
            GameObject wood = Instantiate(woodPrefab, spawnPosition, Quaternion.identity);
            Rigidbody2D rb = wood.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 randomForce = UnityEngine.Random.insideUnitCircle * 2f;
                rb.AddForce(randomForce, ForceMode2D.Impulse);
            }
        }
    }
}
