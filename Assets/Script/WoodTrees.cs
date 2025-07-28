using System;
using System.Collections;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

//public class WoodTrees : NaturalObjects
//{
//    [SerializeField] private int cuttingCounts = 5;
//    [SerializeField] private GameObject woodPrefab;
//    [SerializeField] private int woodCount = 6; 
//    private int currentCounts = 0;
//    public override void Interact()
//    {
//        PlayerAnimation playerAnimation = FindFirstObjectByType<PlayerAnimation>();
//        if (playerAnimation != null)
//        {
//            playerAnimation.PlayChopAnimation();
//        }
//        currentCounts++;
//        if (currentCounts >= cuttingCounts)
//        {
//            SpawnWood();
//            Destroy(gameObject);
//        }
//    }
//    private void SpawnWood()
//    {
//        for (int i = 0; i < woodCount; i++)
//        {
//            Vector2 spawnPosition = (Vector2)transform.position + UnityEngine.Random.insideUnitCircle * 1f;
//            GameObject wood = Instantiate(woodPrefab, spawnPosition, Quaternion.identity);
//            Rigidbody2D rb = wood.GetComponent<Rigidbody2D>();
//            if (rb != null)
//            {
//                Vector2 randomForce = UnityEngine.Random.insideUnitCircle * 5f; 
//                rb.AddForce(randomForce, ForceMode2D.Impulse);     
//            }
//        }
//    }
//}
public class WoodTrees : NaturalObjects
{
    [SerializeField] private int cuttingCounts = 5;
    [SerializeField] private GameObject woodPrefab;
    [SerializeField] private int woodCount = 6;
    [SerializeField] private Sprite[] seasonSprites;
    [SerializeField] private GameObject rootTreePrefab;
    private int currentCounts = 0;
    private SpriteRenderer spriteRenderer;
    protected override void Start()
    {
        originalPosition = transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();
        GameTimeManager.Instance.OnNewDayStarted += UpdateTreeAppearance;
        UpdateTreeAppearance();
    }
    public override void Interact()
    {
        //Debug.Log("Tree Interacted!");
        Player player = Player.Instance;
        if (player != null && !player.IsPerformingAction())
        {
            StartCoroutine(ChopTree(player));
        }
    }
    private IEnumerator ChopTree(Player player)
    {
        player.SetPerformingAction(true);
        PlayerAnimation playerAnimation = FindFirstObjectByType<PlayerAnimation>();
        if (playerAnimation != null)
        {
            Vector2 chopDirection = player.LastInteractDir;
            playerAnimation.PlayChopAnimation(chopDirection); 
        }
        yield return StartCoroutine(ShakeObject());
        yield return new WaitForSeconds(0.40f);
        currentCounts++;
        if (currentCounts >= cuttingCounts)
        {
            SpawnWood();
            SpawnRootTree();
            Destroy(gameObject);
        }
        player.SetPerformingAction(false);
    }
    private void UpdateTreeAppearance()
    {
        int currentSeason = GameTimeManager.Instance.Season;
        if (seasonSprites != null && seasonSprites != null && seasonSprites.Length > currentSeason)
        {
            spriteRenderer.sprite = seasonSprites[currentSeason];
        }
    }
    private void SpawnRootTree()
    {
        if (rootTreePrefab != null)
        {
            Vector2 offset = new Vector2(0.02f, -1.06f);
            Vector2 rootPosition = (Vector2)transform.position + offset;
            Instantiate(rootTreePrefab, rootPosition, Quaternion.identity);
        }
    }
    private void SpawnWood()
    {
        for (int i = 0; i < woodCount; i++)
        {
            Vector2 spawnPosition = (Vector2)transform.position + UnityEngine.Random.insideUnitCircle * 1f;
            GameObject wood = Instantiate(woodPrefab, spawnPosition, Quaternion.identity);
            Rigidbody2D rb = wood.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 randomForce = UnityEngine.Random.insideUnitCircle * 5f;
                rb.AddForce(randomForce, ForceMode2D.Impulse);
            }
        }
    }
}
