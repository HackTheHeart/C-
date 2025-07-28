using UnityEngine;
using System.Collections.Generic;

public class FarmSceneLoader : MonoBehaviour
{
    [Header("Animal Prefabs")]
    public GameObject cowPrefab;
    public GameObject babyCowPrefab;
    public GameObject chickenPrefab;
    public GameObject babyChickenPrefab;
    public GameObject duckPrefab;
    public GameObject babyDuckPrefab;
    public GameObject goatPrefab;
    public GameObject babyGoatPrefab;
    public GameObject ostrichPrefab;
    public GameObject babyOstrichPrefab;
    public GameObject pigPrefab;
    public GameObject babyPigPrefab;
    public GameObject sheepPrefab;
    public GameObject babySheepPrefab;
    public Transform barnSpawnPoint;
    private Dictionary<string, GameObject> animalPrefabs;
    private void Start()
    {
        animalPrefabs = new Dictionary<string, GameObject>
        {
            { "Cow", cowPrefab },
            { "BabyCow", babyCowPrefab },
            { "Chicken", chickenPrefab },
            { "BabyChicken", babyChickenPrefab },
            { "Duck", duckPrefab },
            { "BabyDuck", babyDuckPrefab },
            { "Goat", goatPrefab },
            { "BabyGoat", babyGoatPrefab },
            { "Ostrich", ostrichPrefab },
            { "BabyOstrich", babyOstrichPrefab },
            { "Pig", pigPrefab },
            { "BabyPig", babyPigPrefab },
            { "Sheep", sheepPrefab },
            { "BabySheep", babySheepPrefab },
        };
        LoadAnimals();
    }
    private void LoadAnimals()
    {
        if (BarnManager.Instance == null)
        {
            Debug.LogError("BarnManager chưa được khởi tạo!");
            return;
        }
        List<AnimalData> animalDataList = BarnManager.Instance.GetAnimalDataList();
        if (animalDataList.Count == 0)
        {
            Debug.Log("Không có động vật nào để tải.");
            return;
        }
        foreach (AnimalData data in animalDataList)
        {
            if (animalPrefabs.TryGetValue(data.animalType, out GameObject prefab))
            {
                GameObject newAnimal = Instantiate(prefab, data.position, Quaternion.identity);

                if (data.isBaby)
                {
                    BabyAnimal babyScript = newAnimal.GetComponent<BabyAnimal>();
                    if (babyScript != null)
                    {
                        babyScript.mood = data.mood;
                        babyScript.friendship = data.friendship;
                        babyScript.FoodCountToday = data.foodCountToday;
                        babyScript.SetHasEatenToday(data.hasEatenToday);
                        typeof(BabyAnimal)
                            .GetField("growthProgress", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                            ?.SetValue(babyScript, data.growthProgress);
                        float growthTime = AnimalManager.Instance.GetGrowthTime(data.animalType);
                        if (data.growthProgress >= growthTime)
                        {
                            babyScript.TransformToAdult();
                        }
                    }
                }
                else
                {
                    Animal animalScript = newAnimal.GetComponent<Animal>();
                    if (animalScript != null)
                    {
                        animalScript.mood = data.mood;
                        animalScript.friendship = data.friendship;
                        animalScript.FoodCountToday = data.foodCountToday;
                        animalScript.SetHasEatenToday(data.hasEatenToday);
                    }
                }
            }

        }
        Debug.Log($"Đã tải {animalDataList.Count} động vật vào Barn.");
    }
    [SerializeField] private Transform[] productSpawnPositions;

    private void SpawnProducts()
    {
        var productNames = BarnManager.Instance.GetPendingProductNames();
        foreach (var name in productNames)
        {
            GameObject prefab = Resources.Load<GameObject>($"Prefabs/Products/{name}");
            if (prefab != null)
            {
                Vector3 pos = productSpawnPositions[Random.Range(0, productSpawnPositions.Length)].position;
                Instantiate(prefab, pos, Quaternion.identity);
            }
        }
    }
    //private void SpawnProducts()
    //{
    //    var pending = BarnManager.Instance.GetPendingProducts();

    //    foreach (var data in pending)
    //    {
    //        Vector3 pos = productSpawnPositions[Random.Range(0, productSpawnPositions.Length)].position;
    //        GameObject prefab = Resources.Load<GameObject>($"Prefabs/Products/{data.prefabName}");

    //        if (prefab != null)
    //        {
    //            GameObject obj = Instantiate(prefab, pos, Quaternion.identity);
    //            obj.name = $"Product_{data.uniqueID}";

    //            var pickup = obj.AddComponent<CollectibleProduct>();
    //            pickup.productID = data.uniqueID;
    //        }
    //    }
    //}


}
