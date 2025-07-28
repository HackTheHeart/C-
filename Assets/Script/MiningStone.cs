using System.Collections;
using UnityEngine;

//public class MiningStone : NaturalObjects
//{
//    [SerializeField] private int miningCounts = 1;
//    [SerializeField] private GameObject stonePrefab;
//    [SerializeField] private int stoneCount = 2;
//    private int currentCounts = 0;
//    public override void Interact()
//    {
//        currentCounts++;
//        if (currentCounts >= miningCounts)
//        {
//            SpawnStone();
//            Destroy(gameObject);
//        }
//    }
//    private void SpawnStone()
//    {
//        for (int i = 0; i < stoneCount; i++)
//        {
//            Vector2 spawnPosition = (Vector2)transform.position + Random.insideUnitCircle * 1f;
//            GameObject stone = Instantiate(stonePrefab, spawnPosition, Quaternion.identity);
//            Rigidbody2D rb = stone.GetComponent<Rigidbody2D>();
//            if (rb != null)
//            {
//                Vector2 randomForce = Random.insideUnitCircle * 5f;
//                rb.AddForce(randomForce, ForceMode2D.Impulse);
//            }
//        }
//    }
//}
public class MiningStone : NaturalObjects
{
    [SerializeField] private int miningCounts = 1;
    [SerializeField] private GameObject stonePrefab;
    [SerializeField] private int stoneCount = 2;
    private int currentCounts = 0;
    public override void Interact()
    {
        //Debug.Log("Stone Interacted!");
        Player player = Player.Instance;
        if (player != null && !player.IsPerformingAction()) 
        {
            StartCoroutine(MineStone(player));
        }
    }
    private IEnumerator MineStone(Player player)
    {
        player.SetPerformingAction(true);
        PlayerAnimation playerAnimation = FindFirstObjectByType<PlayerAnimation>();
        if (playerAnimation != null)
        {
            Vector2 mineDirection = player.LastInteractDir;
            playerAnimation.PlayMineAnimation(mineDirection);
        }
        yield return StartCoroutine(ShakeObject());
        yield return new WaitForSeconds(0.40f);
        currentCounts++;
        if (currentCounts >= miningCounts)
        {
            SpawnStone();
            Destroy(gameObject);
        }
        player.SetPerformingAction(false);
    }
    private void SpawnStone()
    {
        for (int i = 0; i < stoneCount; i++)
        {
            Vector2 spawnPosition = (Vector2)transform.position + UnityEngine.Random.insideUnitCircle * 1f;
            GameObject stone = Instantiate(stonePrefab, spawnPosition, Quaternion.identity);
            Rigidbody2D rb = stone.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 randomForce = UnityEngine.Random.insideUnitCircle * 5f;
                rb.AddForce(randomForce, ForceMode2D.Impulse);
            }
        }
    }
}

