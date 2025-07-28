using UnityEngine;

public class CollectibleObjects : MonoBehaviour
{
    [SerializeField] private float pickUpDistance = 2f;
    [SerializeField] private float speed = 3f;
    [SerializeField] private ItemData itemData;
    private bool isCollected = false;
    private Transform playerTransform;
    protected virtual void Start()
    {
        if (Player.Instance != null)
        {
            playerTransform = Player.Instance.transform;
        }
    }
    protected virtual void Update()
    {
        if (isCollected || playerTransform == null) return;
        float distance = Vector3.Distance(transform.position, playerTransform.position);
        if (distance > pickUpDistance) return;
        if (distance < pickUpDistance && !Player.Instance.IsInventoryFull())
        {
            transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, speed * Time.deltaTime);
            CollectObjects();
        }
    }
    protected virtual void CollectObjects()
    {
        if (isCollected || itemData == null) return;
        isCollected = true;
        if (Player.Instance != null)
        {
            bool added = Player.Instance.AddToInventory(itemData.itemName);
            if (added)
            {
                Player.Instance.GetUIInventory().RefreshInventoryUI();
                Destroy(gameObject);
            }
            else
            {
                isCollected = false;
            }
        }
    }
}
