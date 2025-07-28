using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private float raycastDistance = 1f;
    [SerializeField] private UI_Inventory uiIventory;
    [SerializeField] private UI_Toolbar uiToolbar;
    [SerializeField] private GameObject itemAboveHead;
    [SerializeField] private LayerMask npcLayerMask;
    private string playerName = "Gerald";
    private CharacterStats characterStats;
    private int selectedSlotIndex = 0;
    private Vector2 lastInteractDir;
    private Rigidbody2D rb;
    private Inventory inventory;
    private Inventory toolbar;
    private PlayerAnimation playerAnimation;
    public string PlayerName { get { return playerName; } }
    public CharacterStats GetCharacterStats() { return characterStats; }   
    public Inventory Inventory => inventory;
    public Inventory Toolbar => toolbar;
    public CharacterStats getCharacterstats () { return characterStats; }
    public static Player Instance { get; private set; }
    public Vector2 LastInteractDir
    {
        get { return lastInteractDir; }
    }
    private void Awake()
    {
        PlayerAnimation playerAnimation = FindFirstObjectByType<PlayerAnimation>();
        rb = GetComponent<Rigidbody2D>();
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        inventory = new Inventory(16);
        toolbar = new Inventory(10);
        toolbar.OnInventoryChanged += UpdateItemAboveHead;
    }
    public Inventory GetInventory(){return inventory;}
    public Inventory GetToolbar(){return toolbar; }
    public UI_Inventory GetUIInventory() {return uiIventory;}
    public UI_Toolbar GetUIToolbar() {return uiToolbar; }
    public void PlayPickUpFruitAnimation(Vector2 PickUpDirection)
    {
        PlayerAnimation playerAnimation = FindFirstObjectByType<PlayerAnimation>();
        playerAnimation.PlayPickUpFruitAnimation(PickUpDirection);  
    }
    private void Start()
    {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
        gameInput.OnAlInteractAction += GameInput_OnAlInteractAction;
        gameInput.OnInventoryAction += GameInput_OnInventoryAction;
        gameInput.OnGardeningAction += GameInput_OnGardeningAction;
        gameInput.OnNPCEncounteringAction += GameInput_OnNPCEncounteringAction;
        Invoke(nameof(DelayedSetInventory), 0.1f);
        Invoke(nameof(DelaySetToolBar), 0.1f);
        characterStats = GetComponent<CharacterStats>();
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
    }
    public void SetPosition(Vector2 newPosition)
    {
        transform.position = newPosition;
    }
    private void DelayedSetInventory(){ uiIventory.SetInventory(inventory); }
    private void DelaySetToolBar() { uiToolbar.SetToolbar(toolbar); }
    public Items GetSelectedItem()
    {
        List<Items> toolbarItems = toolbar.GetItemsList();
        if (selectedSlotIndex >= 0 && selectedSlotIndex < toolbarItems.Count)
        {
            return toolbarItems[selectedSlotIndex];
        }
        return null;
    }
    private void GameInput_OnGardeningAction(object sender, System.EventArgs e)
    {
        Items selectedItem = GetSelectedItem();
        if (selectedItem == null)
        {
            Debug.Log("SelectedItemIsNull");
            return; }
        Vector2 playerPosition = transform.position;
        ItemData selectedItemData = itemsAssets.Instance.GetItemData(selectedItem.itemName);
        PlayerAnimation playerAnimation = FindFirstObjectByType<PlayerAnimation>();
        switch (selectedItem.GetItemType())
        {
            case ItemType.Tool:
                if (selectedItem.itemName == "Hoe")
                {
                    if (TilemapDiggingManager.Instance == null)
                    {
                        Debug.Log("TileMapDiggingManagerIsNull");
                        characterStats.UseStamina(2);
                        playerAnimation?.PlayHoeAnimation(Player.Instance);
                        return;
                    }
                    if (!TilemapDiggingManager.Instance.SoilTilemap.HasTile(TilemapDiggingManager.Instance.SoilTilemap.WorldToCell(playerPosition)))
                    {
                        characterStats.UseStamina(2);
                        playerAnimation?.PlayHoeAnimation(Player.Instance);
                        TilemapDiggingManager.Instance.TryDigAtPosition(playerPosition);
                    }
                }
                else if (selectedItem.itemName == "Wateringcan")
                {
                    if (TilemapDiggingManager.Instance == null)
                    {
                        characterStats.UseStamina(2);
                        playerAnimation?.PlayWateringAnimation(Player.Instance);
                        Debug.Log("TileMapDiggingManagerIsNull");
                        return;
                    }
                    if (TilemapDiggingManager.Instance.SoilTilemap.HasTile(TilemapDiggingManager.Instance.SoilTilemap.WorldToCell(playerPosition)))
                    {
                        characterStats.UseStamina(2);
                        playerAnimation?.PlayWateringAnimation(Player.Instance);
                        TilemapDiggingManager.Instance.TryWaterAtPosition(playerPosition);
                    }
                }
                else if (selectedItem.itemName == "Sword")
                {
                    //characterStats.UseStamina(5);
                    playerAnimation?.PlayAttackAnimation();
                    StartCoroutine(HandleSwordAttack());
                }
                else if (selectedItem.itemName == "Axe" || selectedItem.itemName == "Pickaxe")
                {
                    characterStats.UseStamina(2);
                    HandleInteractions(selectedItem); 
                }
                else if (selectedItem.itemName == "Fishingrod")
                {
                    FishingManager.Instance.TryStartFishing();
                }
                break;
            case ItemType.Seed:
                if (selectedItemData.cropData != null)
                {
                    bool planted = CropManager.Instance.PlantCrop(playerPosition, selectedItemData.cropData);
                    if (planted)
                    {
                        toolbar.RemoveItem(selectedItem.itemName, 1);
                    }
                }
                break;
            case ItemType.Placeable:
                break;
            case ItemType.Dish:
                break;
            default:
                break;
        }
    }
    private void GameInput_OnInventoryAction(object sender, System.EventArgs e)
    {
        //SetPerformingAction(true);
        //uiIventory.ToggleInventory();
        //SetPerformingAction(false);

        if (UIManager.Instance.AnyUIPanelOpen())
        {
            UIManager.Instance.CloseAllPages();
        }
        else
        {
            UIManager.Instance.uiInventory.ToggleInventory();
        }
    }
    private void GameInput_OnAlInteractAction(object sender, System.EventArgs e)
    {
        Debug.Log("Pressed");
        if (isPerformingAction) return;
        Vector2 inputVector = gameInput.GetvectorMovement();
        inputVector = inputVector.normalized;
        Vector2 moveDir = new Vector2(inputVector.x, inputVector.y);
        if (moveDir != Vector2.zero)
        {
            lastInteractDir = moveDir;
        }
        Vector2 rayStart = rb.position + lastInteractDir;
        int layerMask = ~LayerMask.GetMask("Player", "CameraBounds");
        RaycastHit2D hit = Physics2D.Raycast(rb.position, lastInteractDir, raycastDistance, layerMask);
        if (hit.collider != null)
        {
            GameObject hitObject = hit.collider.gameObject;
            Debug.Log("Đã va chạm với: " + hit.collider.name);
            if (hitObject.TryGetComponent<Chest>(out Chest Chest))
            {
                Chest.Interact();
            }
            else if (hitObject.TryGetComponent<ShippingBin>(out ShippingBin shippingbin)){
                shippingbin.Interact();
            }
        }
    }
    private void GameInput_OnInteractAction(object sender, System.EventArgs e)
    {
        if (isPerformingAction) return;
        Vector2 inputVector = gameInput.GetvectorMovement();
        inputVector = inputVector.normalized;
        Vector2 moveDir = new Vector2(inputVector.x, inputVector.y);
        if (moveDir != Vector2.zero)
        {
            lastInteractDir = moveDir;
        }
        Vector2 rayStart = rb.position + lastInteractDir;
        int layerMask = ~LayerMask.GetMask("Player", "CameraBounds");
        RaycastHit2D hit = Physics2D.Raycast(rb.position, lastInteractDir, raycastDistance, layerMask);
        Debug.DrawRay(rayStart, lastInteractDir * raycastDistance, Color.red);
        if (hit.collider != null)
        {
            GameObject hitObject = hit.collider.gameObject;
            if (hitObject.TryGetComponent<Animal>(out Animal animal))
            {
                animal.Interact();
            }
        }
    }
    //private bool isWalkDown;
    //private bool isWalkUp;
    //private bool isWalkRight;
    //private bool isWalkLeft;
    //private bool isInteracting = false;
    private bool isWalking;
    private float xinput;
    private float yinput;
    private bool isHoldingItem = false;
    //private bool isFacingRight;
    //private bool isFacingLeft;
    //private bool isFacingDown;
    private void HandleInteractions(Items selectedItem)
    {
        int rayCount = 5;
        float angleSpread = 30f; 
        float angleStep = angleSpread / (rayCount - 1);
        float baseAngle = Mathf.Atan2(lastInteractDir.y, lastInteractDir.x) * Mathf.Rad2Deg;
        float startAngle = baseAngle - angleSpread / 2f;
        if (isPerformingAction) return;
        Vector2 inputVector = gameInput.GetvectorMovement().normalized;
        Vector2 moveDir = new Vector2(inputVector.x, inputVector.y);
        if (moveDir != Vector2.zero)
        {
            lastInteractDir = moveDir;
        }
        //Vector2 rayStart = rb.position + lastInteractDir;
        int layerMask = ~LayerMask.GetMask("Player", "CameraBounds");
        //RaycastHit2D hit = Physics2D.Raycast(rb.position, lastInteractDir, raycastDistance, layerMask);
        //Debug.DrawRay(rayStart, lastInteractDir * raycastDistance, Color.red);
        bool hasInteracted = false;
        for (int i = 0; i < rayCount; i++)
        {
            float angle = startAngle + i * angleStep;
            Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Rad2Deg), Mathf.Sin(angle * Mathf.Rad2Deg));
            RaycastHit2D hit = Physics2D.Raycast(rb.position, direction, raycastDistance, layerMask);
            Debug.DrawRay(rb.position, direction * raycastDistance, Color.red);

            if (hit.collider != null)
            {
                Debug.Log("Đã va chạm với: " + hit.collider.name);
                GameObject hitObject = hit.collider.gameObject;
                if (hitObject.TryGetComponent<NaturalObjects>(out NaturalObjects naturalObject))
                {
                    switch (selectedItem.itemName)
                    {
                        case "Axe":
                            if (naturalObject is WoodTrees || naturalObject is RootTree)
                            {
                                naturalObject.Interact();
                                hasInteracted = true;
                            }
                            break;
                        case "Pickaxe":
                            if (naturalObject is MiningStone)
                            {
                                naturalObject.Interact();
                                hasInteracted = true;
                            }
                            break;
                    }

                    if (hasInteracted) break;
                }
            }
        }
        PlayerAnimation playerAnimation = FindFirstObjectByType<PlayerAnimation>();
        if (!hasInteracted)
        {
            if (playerAnimation != null)
            {
                switch (selectedItem.itemName)
                {
                    case "Axe":
                        playerAnimation.PlayChoppingAnimation(Player.Instance);
                        break;
                    case "Pickaxe":
                        playerAnimation.PlayMiningAnimation(Player.Instance);
                        break;
                }
            }
        }
    }
    private void HandleMovement()
    {
        if (isPerformingAction) return;
        //if (isInteracting) return;
        Vector2 inputVector = gameInput.GetvectorMovement();
        inputVector = inputVector.normalized;
        Vector2 moveDir = new Vector2(inputVector.x, inputVector.y);

        Vector2 moveDirX = new Vector2(moveDir.x, 0);
        Vector2 moveDirY = new Vector2(0, moveDir.y);
        //bool canMove = CanMove(inputVector);
        //if (!canMove)
        //{
        //    if (inputVector.x != 0)
        //    {
        //        canMove = CanMove(moveDirX);
        //        if (canMove)
        //        {
        //            inputVector = moveDirX;
        //        }
        //    }

        //    if (!canMove && inputVector.y != 0)
        //    {
        //        canMove = CanMove(moveDirY);
        //        if (canMove)
        //        {
        //            inputVector = moveDirY;
        //        }
        //    }
        //}

        //if (canMove)
        //{
        //    rb.MovePosition(rb.position + moveDir * moveSpeed * Time.fixedDeltaTime);
        //}
        rb.MovePosition(rb.position + moveDir * moveSpeed * Time.fixedDeltaTime);
        isWalking = moveDir != Vector2.zero;
        if (moveDir != Vector2.zero)
        {
            lastInteractDir = moveDir;
        }

        //isWalkDown = (moveDir.y < 0);
        //isWalkUp = (moveDir.y > 0);
        //isWalkLeft = (moveDir.x < 0);
        //isWalkRight = (moveDir.x > 0);

        //if (Mathf.Abs(moveDir.x) > Mathf.Abs(moveDir.y))
        //{
        //    isWalkLeft = (moveDir.x < 0);
        //    isWalkRight = (moveDir.x > 0);

        //    isWalkUp = false;
        //    isWalkDown = false;
        //}
        //else
        //{
        //    isWalkUp = (moveDir.y > 0);
        //    isWalkDown = (moveDir.y < 0);

        //    isWalkLeft = false;
        //    isWalkRight = false;
        //}
        if (moveDir != Vector2.zero)
        {
            xinput = moveDir.x;
            yinput = moveDir.y;
        }
        //if (moveDir != Vector2.zero)
        //{

        //}
    }
    private void HandleToolbarSelection()
    {
        int previousSlotIndex = selectedSlotIndex;
        if (Input.mouseScrollDelta.y > 0)
        {
            selectedSlotIndex = (selectedSlotIndex + 1) % 10;
        }
        else if (Input.mouseScrollDelta.y < 0)
        {
            selectedSlotIndex = (selectedSlotIndex - 1 + 10) % 10;
        }
        if (Input.GetKeyDown(KeyCode.Alpha1)) selectedSlotIndex = 0;
        if (Input.GetKeyDown(KeyCode.Alpha2)) selectedSlotIndex = 1;
        if (Input.GetKeyDown(KeyCode.Alpha3)) selectedSlotIndex = 2;
        if (Input.GetKeyDown(KeyCode.Alpha4)) selectedSlotIndex = 3;
        if (Input.GetKeyDown(KeyCode.Alpha5)) selectedSlotIndex = 4;
        if (Input.GetKeyDown(KeyCode.Alpha6)) selectedSlotIndex = 5;
        if (Input.GetKeyDown(KeyCode.Alpha7)) selectedSlotIndex = 6;
        if (Input.GetKeyDown(KeyCode.Alpha8)) selectedSlotIndex = 7;
        if (Input.GetKeyDown(KeyCode.Alpha9)) selectedSlotIndex = 8;
        if (Input.GetKeyDown(KeyCode.Alpha0)) selectedSlotIndex = 9;
        uiToolbar.SelectSlot(selectedSlotIndex);
        if (selectedSlotIndex != previousSlotIndex)
        {
            Items selectedItem = GetSelectedItem();
            bool shouldShowItem = selectedItem != null && selectedItem.GetItemType() != ItemType.Tool && selectedItem.amount > 0;
            SetPickupState(shouldShowItem, selectedItem);
        }
    }
    private void Update()
    {
        HandleMovement();
        //HandleInteractions();
        HandleToolbarSelection();
        //HandleClickOnNPC();
    }
    //private void GameInput_OnNPCEncounteringAction(object sender, System.EventArgs e)
    //{
    //    Debug.Log("Clicked");
    //    Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //    RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
    //    if (hit.collider != null && hit.collider.TryGetComponent<NPC>(out NPC npc))
    //    {
    //        Debug.Log("Raycast hit: " + hit.collider.name);
    //        float interactRange = 2f;
    //        float distanceToPlayer = Vector2.Distance(npc.transform.position, Player.Instance.transform.position);
    //        Items selectedItem = GetSelectedItem();
    //        if (npc.isInteractable && distanceToPlayer <= interactRange)
    //        {
    //            if (selectedItem != null && selectedItem.GetItemType() != ItemType.Tool)
    //            {
    //                npc.GiveGift(selectedItem);
    //                toolbar.RemoveItem(selectedItem.itemName, 1);
    //            }
    //            else
    //            {
    //                npc.Interact();
    //                Debug.Log("npc interacted");
    //            }
    //        }
    //    }
    //}
    //private void HandleClickOnNPC()
    //{
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
    //        if (hit.collider != null && hit.collider.TryGetComponent<NPC>(out NPC npc))
    //        {
    //            Debug.Log("Raycast hit: " + hit.collider.name);
    //            float interactRange = 2f;
    //            float distanceToPlayer = Vector2.Distance(npc.transform.position, Player.Instance.transform.position);
    //            if (npc.isInteractable && distanceToPlayer <= interactRange)
    //            {
    //                npc.Interact();
    //                Debug.Log("npc interacted");
    //            }
    //        }
    //    }
    //}
    private NPC GetNPCUnderCursor(float clickRadius = 0.15f)
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Dùng OverlapCircle để kiểm tra collider trong bán kính clickRadius
        Collider2D hit = Physics2D.OverlapCircle(mousePosition, clickRadius, npcLayerMask);

        if (hit != null && hit.TryGetComponent<NPC>(out NPC npc))
        {
            return npc;
        }

        return null;
    }
    private void GameInput_OnNPCEncounteringAction(object sender, System.EventArgs e)
    {
        Debug.Log("Clicked");
        NPC npc = GetNPCUnderCursor();

        if (npc == null) return;

        float interactRange = 2f;
        float distanceToPlayer = Vector2.Distance(npc.transform.position, Player.Instance.transform.position);
        Items selectedItem = GetSelectedItem();

        if (npc.isInteractable && distanceToPlayer <= interactRange)
        {
            if (selectedItem != null && selectedItem.GetItemType() != ItemType.Tool)
            {
                npc.GiveGift(selectedItem);
                toolbar.RemoveItem(selectedItem.itemName, 1);
            }
            else
            {
                npc.Interact();
                Debug.Log("npc interacted");
            }
        }
    }

    private bool CanMove(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(rb.position, direction, raycastDistance);

        if (hit.collider != null)
        {
            if (hit.collider.gameObject == gameObject)
            {
                return true;
            }
            return false;
        }
        return true;    
    }
    //public bool IsWalkDown() { return isWalkDown; }
    //public bool IsWalkUp() { return isWalkUp; }
    //public bool IsWalkLeft() { return isWalkLeft; }
    //public bool IsWalkRight() { return isWalkRight; }
    public bool IsWalking() { return isWalking; }
    public float Xinput() { return xinput; }
    public float Yinput() { return yinput; }
    public bool IsPickingUp()
    {
        return isHoldingItem && !IsWalking();
    }
    public bool IsPickUpRun()
    {
        return isHoldingItem && IsWalking();
    }
    private void ShowItemAboveHead(Items item)
    {
        if (itemAboveHead != null)
        {
            itemAboveHead.SetActive(true);
            itemAboveHead.GetComponent<SpriteRenderer>().sprite = item.GetSprite();
            itemAboveHead.GetComponent<SpriteRenderer>().transform.localScale = Vector3.one * 0.7f;
        }
    }
    private void HideItemAboveHead()
    {
        if (itemAboveHead != null)
        {
            itemAboveHead.SetActive(false);
        }
    }
    public void SetPickupState(bool isPickingUp, Items item)
    {
        isHoldingItem = isPickingUp;
        if (isPickingUp)
        {
            ShowItemAboveHead(item);
        }
        else
        {
            HideItemAboveHead();
        }
    }
    private void UpdateItemAboveHead()
    {
        Items selectedItem = GetSelectedItem();
        bool shouldShowItem = selectedItem != null && selectedItem.GetItemType() != ItemType.Tool && selectedItem.amount > 0;
        SetPickupState(shouldShowItem, selectedItem);
    }
    private bool isPerformingAction = false;
    public void SetPerformingAction(bool state)
    {
        isPerformingAction = state;
    }
    public bool IsPerformingAction()
    {
        return isPerformingAction;
    }
    //public bool IsFacingRight() { return isFacingRight; }
    //public bool IsFacingLeft() { return isFacingLeft; }
    //public bool IsFacingDown() { return isFacingDown; }


    //if (moveDir != Vector3.zero)
    //{
    //    if (moveDir.y < 0)
    //        isWalkDown = true;
    //    else
    //        isWalkDown = false;
    //}
    //isWalkDown = (moveDir.y < 0 && Mathf.Abs(moveDir.y) > Mathf.Abs(moveDir.x));
    //isWalkUp = (moveDir.y > 0 && Mathf.Abs(moveDir.y) > Mathf.Abs(moveDir.x));
    //isWalkLeft = (moveDir.x < 0 && Mathf.Abs(moveDir.x) > Mathf.Abs(moveDir.y));
    //isWalkRight = (moveDir.x > 0 && Mathf.Abs(moveDir.x) > Mathf.Abs(moveDir.y));

    //private void UpdateFacing(Vector3 moveDir)
    //{
    //    if (moveDir.magnitude > 0)
    //    {
    //        if (Mathf.Abs(moveDir.x) > Mathf.Abs(moveDir.y))
    //        {
    //            isFacingRight = moveDir.x > 0;
    //            isFacingLeft = moveDir.x < 0;

    //            isFacingDown = false;
    //        }
    //        else
    //        {
    //            isFacingDown = moveDir.y < 0;

    //            isFacingRight = false;
    //            isFacingLeft = false;
    //        }
    //    }
    //}
    public bool AddToInventory(string itemName, int amount = 1)
    {
        if (amount <= 0) return false;
        bool addedToToolbar = toolbar.AddItem(itemName, amount);
        if (addedToToolbar)
        {
            uiToolbar.RefreshToolbarUI();
            return true;
        }
        bool addedToInventory = inventory.AddItem(itemName, amount);
        if (addedToInventory)
        {
            uiIventory.RefreshInventoryUI();
            return true;
        }
        Debug.Log("Inventory is full! Cannot add item: " + itemName);
        return false;
    }
    public bool AddOnlyToInventory(string itemName, int amount = 1)
    {
        if (amount <= 0) return false;

        bool added = inventory.AddItem(itemName, amount);
        if (added)
        {
            uiIventory.RefreshInventoryUI();
            return true;
        }

        Debug.Log("Inventory is full! Cannot add item: " + itemName);
        return false;
    }

    public bool IsInventoryFull()
    {
        return inventory.IsFull() && toolbar.IsFull();
    }
    public bool HasItem(string itemName, int amount)
    {
        int totalAmount = 0;
        totalAmount += inventory.GetItemAmount(itemName);
        totalAmount += toolbar.GetItemAmount(itemName);
        return totalAmount >= amount;
    }
    public bool ConsumeItem(string itemName, int amount)
    {
        int remaining = amount;
        int toolbarAmount = toolbar.GetItemAmount(itemName);
        if (toolbarAmount > 0)
        {
            int toRemove = Mathf.Min(remaining, toolbarAmount);
            toolbar.RemoveItem(itemName, toRemove);
            remaining -= toRemove;
        }
        if (remaining > 0)
        {
            int inventoryAmount = inventory.GetItemAmount(itemName);
            if (inventoryAmount < remaining) return false;
            inventory.RemoveItem(itemName, remaining);
        }
        uiToolbar.RefreshToolbarUI();
        uiIventory.RefreshInventoryUI();
        return true;
    }
    private IEnumerator HandleSwordAttack()
    {
        SetPerformingAction(true);
        yield return new WaitForSeconds(0.2f);
        Vector2 attackDirection = Player.Instance.LastInteractDir.normalized;
        float attackRange = 1.5f;
        LayerMask enemyLayer = LayerMask.GetMask("Enemy");
        RaycastHit2D hit = Physics2D.Raycast(transform.position, attackDirection, attackRange, enemyLayer);
        if (hit.collider != null)
        {
            GameObject enemy = hit.collider.gameObject;
            Rigidbody2D enemyRb = enemy.GetComponent<Rigidbody2D>();
            if (enemyRb != null)
            {
                float knockbackForce = 1f; 
                enemyRb.linearVelocity = Vector2.zero;
                enemyRb.AddForce(attackDirection * knockbackForce, ForceMode2D.Impulse);
            }
            if (enemy.TryGetComponent(out GoblinAI goblinAI))
            {
                goblinAI.TakeDamage(20);
            }
            else if (enemy.TryGetComponent(out GoblinRangedAI goblinRangeAI))
            {
                goblinRangeAI.TakeDamage(10);
            }
        }
    }
    public bool IsSleeping() { return isSleeping; }
    private bool isSleeping;
    public void SetSleeping(bool value)
    {
        isSleeping = value;
    }
    public Transform fruitPickupLeft;
    public Transform fruitPickupRight;
    public Transform fruitPickupTop;
    public Transform fruitPickupBottom;
    public Transform fruitPickupTarget;
}
