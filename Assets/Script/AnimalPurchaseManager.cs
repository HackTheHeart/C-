using UnityEngine;
using UnityEngine.UI;

public class AnimalPurchaseManager : MonoBehaviour
{
    //public GameObject cowPrefab;
    public GameObject chickenPrefab;
    public GameObject duckPrefab;
    //public GameObject goatPrefab;
    //public GameObject ostrichPrefab;
    //public GameObject pigPrefab;
    //public GameObject sheepPrefab;
    //public Button cowButton;
    public Button chickenButton;
    public Button duckButton;
    //public Button goatButton;
    //public Button ostrichButton;
    //public Button pigButton;
    //public Button sheepButton;
    private BarnManager barnManager;
    private void Start()
    {
        barnManager = BarnManager.Instance;
        //cowButton.onClick.AddListener(() => PurchaseAnimal("Cow"));
        chickenButton.onClick.AddListener(() => PurchaseAnimal("Chicken"));
        duckButton.onClick.AddListener(() => PurchaseAnimal("Duck"));
        //goatButton.onClick.AddListener(() => PurchaseAnimal("Goat"));
        //ostrichButton.onClick.AddListener(() => PurchaseAnimal("Ostrich"));
        //pigButton.onClick.AddListener(() => PurchaseAnimal("Pig"));
        //sheepButton.onClick.AddListener(() => PurchaseAnimal("Sheep"));
    }
    private void PurchaseAnimal(string animalType)
    {
        if (barnManager.IsBarnFull())
        {
            Debug.Log("Barn hiện tại đã đầy! Không thể mua thêm động vật.");
            return;
        }
        Vector3 spawnPosition = GetRandomSpawnPosition();
        SaveAnimalData(animalType, spawnPosition);
        Debug.Log($"{animalType} đã được mua và thêm vào barn!");
    }
    //private void SaveAnimalData(string animalType, Vector3 spawnPosition)
    //{
    //    GameObject prefabToSave = null;
    //    switch (animalType)
    //    {
    //        //case "Cow":
    //        //    prefabToSave = cowPrefab;
    //        //    break;
    //        case "Chicken":
    //            prefabToSave = chickenPrefab;
    //            break;
    //        case "Duck":
    //            prefabToSave = duckPrefab;
    //            break;
    //        //case "Goat":
    //        //    prefabToSave = goatPrefab;
    //        //    break;
    //        //case "Ostrich":
    //        //    prefabToSave = ostrichPrefab;
    //        //    break;
    //        //case "Pig":
    //        //    prefabToSave = pigPrefab;
    //        //    break;
    //        //case "Sheep":
    //        //    prefabToSave = sheepPrefab;
    //        //    break;
    //        default:
    //            Debug.LogError("Loại động vật không hợp lệ!");
    //            return;
    //    }
    //    AnimalData newAnimalData = new AnimalData
    //    {
    //        animalID = prefabToSave.name,
    //        animalType = animalType,
    //        position = spawnPosition,
    //        mood = 0,  
    //        friendship = 0,
    //        foodCountToday = 0,
    //        isBaby = true
    //    };
    //    barnManager.SaveAnimalData(newAnimalData);
    //}
    private void SaveAnimalData(string animalType, Vector3 spawnPosition)
    {
        GameObject prefabToSave = null;
        string babyType = ""; // dùng để set animalType chính xác

        switch (animalType)
        {
            case "Chicken":
                prefabToSave = chickenPrefab;
                babyType = "BabyChicken";
                break;
            case "Duck":
                prefabToSave = duckPrefab;
                babyType = "BabyDuck";
                break;
            default:
                Debug.LogError("Loại động vật không hợp lệ!");
                return;
        }

        AnimalData newAnimalData = new AnimalData
        {
            animalID = prefabToSave.name,
            animalType = babyType,
            position = spawnPosition,
            mood = 0,
            friendship = 0,
            foodCountToday = 0,
            isBaby = true,
            growthProgress = 0f
        };

        barnManager.SaveAnimalData(newAnimalData);
    }

    private Vector3 GetRandomSpawnPosition()
    {
        float x = Random.Range(-5f, 5f);
        float y = 0f;
        float z = Random.Range(-5f, 5f);
        return new Vector3(x, y, z);
    }
}
