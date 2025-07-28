//using System.Collections.Generic;
//using UnityEngine;

//public class BarnManager : MonoBehaviour
//{
//    public static BarnManager Instance;
//    private Dictionary<int, List<AnimalData>> barns = new Dictionary<int, List<AnimalData>>();
//    private int currentBarnID = -1;
//    private void Awake()
//    {
//        if (Instance == null)
//        {
//            Instance = this;
//            DontDestroyOnLoad(gameObject);
//        }
//        else
//        {
//            Destroy(gameObject);
//        }
//    }
//    public void SetCurrentBarnID(int barnID)
//    {
//        currentBarnID = barnID;
//        if (!barns.ContainsKey(barnID))
//        {
//            barns[barnID] = new List<AnimalData>();
//        }
//        Debug.Log($"Người chơi đang ở Barn {barnID}");
//    }
//    public void SaveAnimalData(AnimalData data)
//    {
//        if (currentBarnID == -1)
//        {
//            Debug.LogError("Không có Barn nào đang được chọn!");
//            return;
//        }
//        if (!barns.ContainsKey(currentBarnID))
//        {
//            barns[currentBarnID] = new List<AnimalData>();
//        }
//        barns[currentBarnID].RemoveAll(a => a.animalID == data.animalID);
//        barns[currentBarnID].Add(data);
//        Debug.Log($"{data.animalType} đã được thêm vào Barn {currentBarnID} với vị trí {data.position}");
//    }

//    public void SaveAnimalData(Animal animal)
//    {
//        if (currentBarnID == -1)
//        {
//            Debug.LogError("Không có Barn nào đang được chọn!");
//            return;
//        }
//        if (!barns.ContainsKey(currentBarnID))
//        {
//            barns[currentBarnID] = new List<AnimalData>();
//        }
//        if (barns[currentBarnID].Count >= 8)
//        {
//            Debug.LogError($"Barn {currentBarnID} đã đầy! Không thể thêm động vật.");
//            return;
//        }
//        AnimalData data = new AnimalData
//        {
//            animalID = animal.gameObject.name,
//            animalType = animal.GetType().Name,
//            position = animal.transform.position,
//            mood = animal.mood,
//            friendship = animal.friendship,
//            foodCountToday = animal.getFoodCountTody(),
//        };
//        barns[currentBarnID].RemoveAll(a => a.animalID == data.animalID);
//        barns[currentBarnID].Add(data);
//    }
//    public bool IsBarnFull()
//    {
//        if (currentBarnID == -1)
//        {
//            Debug.LogError("Không có Barn nào đang được chọn!");
//            return false;
//        }
//        return barns[currentBarnID].Count >= 8;
//    }
//    public List<AnimalData> GetAnimalsData()
//    {
//        return barns.ContainsKey(currentBarnID) ? barns[currentBarnID] : new List<AnimalData>();
//    }
//    public void SaveAllAnimals()
//    {
//        if (currentBarnID == -1)
//        {
//            Debug.LogError("Không có Barn nào để lưu!");
//            return;
//        }
//        Animal[] animals = FindObjectsByType<Animal>(FindObjectsSortMode.None);
//        foreach (var animal in animals)
//        {
//            SaveAnimalData(animal);
//        }
//        Debug.Log($"Đã lưu {animals.Length} động vật trong Barn {currentBarnID}");
//    }
//    public void ClearAnimals()
//    {
//        if (currentBarnID != -1 && barns.ContainsKey(currentBarnID))
//        {
//            barns[currentBarnID].Clear();
//        }
//    }
//    public Dictionary<int, bool> CheckAllBarnsFull()
//    {
//        Dictionary<int, bool> result = new Dictionary<int, bool>();

//        foreach (var barn in barns)
//        {
//            result[barn.Key] = barn.Value.Count >= 8;
//        }

//        return result;
//    }
//}

using System.Collections.Generic;
using UnityEngine;

public class BarnManager : MonoBehaviour
{
    public static BarnManager Instance;
    private List<AnimalData> animalList = new List<AnimalData>();
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void SaveAnimalData(Animal animal)
    {
        var data = animal.GetAnimalData();
        animalList.RemoveAll(a => a.animalID == data.animalID);
        animalList.Add(data);
    }
    public List<AnimalData> GetAnimalDataList()
    {
        return animalList;
    }
    public void SaveAllAnimals()
    {
        Animal[] animals = FindObjectsByType<Animal>(FindObjectsSortMode.None);
        foreach (var animal in animals)
        {
            SaveAnimalData(animal);
        }
        Debug.Log($"Đã lưu {animals.Length} động vật trong barn.");
    }
    public void SaveAnimalData(AnimalData data)
    {
        animalList.RemoveAll(a => a.animalID == data.animalID);
        animalList.Add(data);
    }

    public void ClearAnimalList()
    {
        animalList.Clear();
    }
    public bool IsBarnFull()
    {
        return animalList.Count >= 8;
    }
    [SerializeField] private List<string> pendingProductPrefabNames = new();

    public List<string> GetPendingProductNames() => pendingProductPrefabNames;
    [SerializeField] private List<ProductData> pendingProducts = new();
    public List<ProductData> GetPendingProducts() => pendingProducts;
    public void RemoveCollectedProduct(string productID)
    {
        pendingProducts.RemoveAll(p => p.uniqueID == productID);
    }

    private void OnEnable()
    {
        GameTimeManager.Instance.OnNewDayStarted += HandleNewDay;
    }

    private void OnDisable()
    {
        if (GameTimeManager.Instance != null)
            GameTimeManager.Instance.OnNewDayStarted -= HandleNewDay;
    }

    private void HandleNewDay()
    {
        List<AnimalData> updatedList = new();
        pendingProducts.Clear();
        pendingProductPrefabNames.Clear();
        foreach (var animal in animalList)
        {
            if (animal.isBaby)
            {
                animal.growthProgress++;
                float growthTime = AnimalManager.Instance.GetGrowthTime(animal.animalType);
                if (animal.growthProgress >= growthTime)
                {
                    //luu danh sach da qua thoi han growth con lai thi update growth nhu binh thuong
                    //farmsceneloader khi gap nhung data nay instantiate con vat day len va goi baby.transformtoadult
                }
            }
            else
            {
                //if (animal.hasEatenToday)
                //{
                //    string productToAdd = animal.friendship >= 80
                //        ? animal.maxFriendshipProductPrefabName
                //        : animal.productPrefabName;

                //    if (!string.IsNullOrEmpty(productToAdd))
                //        pendingProductPrefabNames.Add(productToAdd);
                //}
                if (animal.hasEatenToday)
                {
                    string prefabName = animal.friendship >= 80
                        ? animal.maxFriendshipProductPrefabName
                        : animal.productPrefabName;

                    if (!string.IsNullOrEmpty(prefabName))
                    {
                        pendingProducts.Add(new ProductData
                        {
                            prefabName = prefabName,
                            uniqueID = System.Guid.NewGuid().ToString()
                        });
                    }
                }
            }

            animal.hasEatenToday = false;
            animal.foodCountToday = 0;
            updatedList.Add(animal);
        }

        animalList = updatedList;
        Debug.Log("Đã cập nhật dữ liệu tăng trưởng và sản phẩm cho ngày mới.");
    }
}
[System.Serializable]
public class ProductData
{
    public string prefabName;
    public string uniqueID;
}

public class CollectibleProduct : MonoBehaviour
{
    public string productID;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            BarnManager.Instance.RemoveCollectedProduct(productID);
            Destroy(gameObject);
        }
    }
}
