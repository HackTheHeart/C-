using UnityEngine;

public class Animal : MonoBehaviour
{
    public enum AnimalType
    {
        Chicken,
        BabyChicken,
        Duck,
        BabyDuck
    }
    [SerializeField] private AnimalType animalType;
    private Rigidbody2D rb;
    private Vector2 targetPosition;
    private Vector2 lastMoveDirection = Vector2.down;   
    private bool isWalking = false;
    private float stopTimer;
    public float moveSpeed = 1.5f;
    public float stopTime = 10f;
    public float moveRadius = 2f;
    public int mood = 50;
    public int friendship = 0;
    private float moodDecayTimer = 10f;
    private int foodCountToday = 0;

    public GameObject HappyEffect;
    public GameObject ProductPrefab;
    public GameObject MaxFriendShipProductPrefab;
    //public Transform productSpawnPoint;
    private AnimalAnimation animalAnimation;

    private bool hasEatenToday = false; 
    public bool HasEatenToday => hasEatenToday;
    public void SetHasEatenToday(bool value)
    {
        hasEatenToday = value;
    }

    public void Eat()
    {
        if (hasEatenToday) return;
        mood = Mathf.Clamp(mood + 10, 0, 100);
        friendship = Mathf.Clamp(friendship + 5, 0, 100);
        foodCountToday++;
        hasEatenToday = true;
        animalAnimation?.PlayEatAnimation();
    }
    public int getFoodCountTody() { return foodCountToday; }
    public int FoodCountToday
    {
        get { return foodCountToday; }
        set { foodCountToday = value; }
    }

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ChooseNewPosition();
        InvokeRepeating(nameof(DecreaseMood), moodDecayTimer, moodDecayTimer);
        //GameTimeManager.Instance.OnNewDayStarted += CheckForDailyProduction;
        animalAnimation = GetComponentInChildren<AnimalAnimation>();
    }

    void Update()
    {
        if (isWalking)
        {
            float distance = Vector2.Distance(transform.position, targetPosition);
            if (distance < 0.1f)
            {
                StopMoving();
            }
        }
        else
        {
            stopTimer -= Time.deltaTime;
            if (stopTimer <= 0)
            {
                ChooseNewPosition();
            }
        }
    }
    void FixedUpdate()
    {
        if (isWalking)
        {
            //Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
            //rb.linearVelocity = direction * moveSpeed;
            //if (direction.magnitude > 0.1f)
            //    lastMoveDirection = direction;
            //animalAnimation?.UpdateAnimation(direction.x, direction.y, true);
            Vector2 currentPos = rb.position;
            Vector2 newPos = Vector2.MoveTowards(currentPos, targetPosition, moveSpeed * Time.fixedDeltaTime);
            rb.MovePosition(newPos);

            Vector2 direction = (targetPosition - currentPos).normalized;
            if (direction.magnitude > 0.1f)
                lastMoveDirection = direction;
            animalAnimation?.UpdateAnimation(direction.x, direction.y, true);

            if (Vector2.Distance(newPos, targetPosition) < 0.1f)
                StopMoving();
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
            animalAnimation?.UpdateAnimation(lastMoveDirection.x, lastMoveDirection.y, false);
        }
    }
    void ChooseNewPosition()
    {
        isWalking = true;
        Vector2 randomOffset = Random.insideUnitCircle * moveRadius;
        targetPosition = (Vector2)transform.position + randomOffset;
    }
    void StopMoving()
    {
        isWalking = false;
        stopTimer = stopTime;
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Log ra thông tin về đối tượng va chạm
        Debug.Log("Collision with: " + collision.gameObject.name);
        Debug.Log("This object's layer: " + LayerMask.LayerToName(gameObject.layer));
        Debug.Log("Other object's layer: " + LayerMask.LayerToName(collision.gameObject.layer));

        // Kiểm tra nếu đối tượng va chạm với collider khác
        if (collision.gameObject.layer != gameObject.layer)
        {
            Debug.Log("Collision detected, stopping movement.");
            StopMoving();
        }
        else
        {
            Debug.Log("No collision or same layer.");
        }
    }

    public void ResetDailyState()
    {
        foodCountToday = 0;
        hasEatenToday = false;
    }
    public AnimalData GetAnimalData()
    {
        return new AnimalData
        {
            animalID = gameObject.name,
            animalType = animalType.ToString(),
            position = transform.position,
            mood = mood,
            friendship = friendship,
            foodCountToday = foodCountToday,
            hasEatenToday = hasEatenToday,
            productPrefabName = ProductPrefab?.name,
            maxFriendshipProductPrefabName = MaxFriendShipProductPrefab?.name
        };
    }

    //public void Eat()
    //{
    //    mood = Mathf.Clamp(mood + 10, 0, 100);
    //    friendship = Mathf.Clamp(friendship + 5, 0, 100);
    //    foodCountToday++;
    //    animalAnimation?.PlayEatAnimation();
    //}
    public void Interact()
    {
        mood = Mathf.Clamp(mood + 5, 0, 100);
        friendship = Mathf.Clamp(friendship + 10, 0, 100);
        if (HappyEffect != null)
        {
            HappyEffect.SetActive(true);
            Invoke(nameof(HideHappyEffect), 1f);
        }
    }
    void HideHappyEffect()
    {
        if (HappyEffect != null)
            HappyEffect.SetActive(false);
    }
    void DecreaseMood()
    {
        mood = Mathf.Clamp(mood - 2, 0, 100);
        if (mood == 0) friendship = Mathf.Clamp(friendship - 1, 0, 100);
    }
    protected virtual void CheckForDailyProduction()
    {
        if (foodCountToday > 0)
        {
            int productAmount = 1;
            if (mood >= 80) productAmount += 1;
            for (int i = 0; i < productAmount; i++)
            {
                //Instantiate(ProductPrefab, productSpawnPoint.position, Quaternion.identity);
            }
        }
        foodCountToday = 0;
    }
}
