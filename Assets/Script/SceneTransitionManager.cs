using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance;
    [SerializeField] private Vector2 houseSpawnPosition;
    [SerializeField] private Vector2 farmSpawnPosition;
    [SerializeField] private Vector2 farmSpawnFromBarnPosition;
    [SerializeField] private Vector2 barnSpawnPosition;
    [SerializeField] private Vector2 caveSpawnPosition;
    [SerializeField] private Vector2 harborSpawnPosition;
    [SerializeField] private Vector2 farmSpawnFromCavePosition;
    [SerializeField] private Vector2 farmSpawnFromHarborPosition;
    // just added no logic for trnasition
    [SerializeField] private Vector2 farmSpawnFromTownPosition;
    [SerializeField] private Vector2 townSpawnFromFarmPosition;
    [SerializeField] private Vector2 townSpawnFromBeachPosition;
    [SerializeField] private Vector2 townSpawnFromMinePosition;

    [SerializeField] private Vector2 caveSpawnFromTownPosition;
    [SerializeField] private Vector2 beachSpawnFromTownPosition;

    [SerializeField] private Vector2 generalStoreSpawnFromTownPosition;
    [SerializeField] private Vector2 townspawnFromgeneralStorePosition;

    [SerializeField] private Vector2 blacksmithSpawnFromgeneralStorePosition;
    [SerializeField] private Vector2 townSpawnFromgeneralBlacksmithPosition;

    [SerializeField] private Vector2 joshSpawnFromgeneralStorePosition;
    [SerializeField] private Vector2 townSpawnFromgeneralJoshPosition;

    [SerializeField] private Vector2 manuSpawnFromgeneralStorePosition;
    [SerializeField] private Vector2 townSpawnFromgeneralManuPosition;

    [SerializeField] private Vector2 ranchSpawnFromgeneralStorePosition;
    [SerializeField] private Vector2 townSpawnFromgeneralRanchPosition;
    private Vector2 playerSpawnPosition;
    private string previousScene;
    [SerializeField] private Animator fadeAnimator;
    [SerializeField] private float fadeDuration = 1f;
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
    public void LoadHouseScene()
    {
        previousScene = SceneManager.GetActiveScene().name;
        playerSpawnPosition = houseSpawnPosition;
        StartCoroutine(LoadSceneAsync("HouseScene"));
    }
    public void LoadBarnScene()
    {
        previousScene = SceneManager.GetActiveScene().name;
        playerSpawnPosition = barnSpawnPosition;
        StartCoroutine(LoadSceneAsync("Barn Interior Scene"));
    }
    public void SetFarmReturnFromBarnPosition(Vector2 position)
    {
        farmSpawnFromBarnPosition = position;
    }
    public void LoadFarmScene()
    {
        if (BarnManager.Instance != null)
        {
            BarnManager.Instance.SaveAllAnimals();
        }
        if (previousScene == "HouseScene")
        {
            playerSpawnPosition = farmSpawnPosition;
        }
        else if (previousScene == "Barn Interior Scene")
        {
            playerSpawnPosition = farmSpawnFromBarnPosition;
        }
        else if (previousScene == "TownScene")
        {
            playerSpawnPosition = farmSpawnFromTownPosition;
        }
        else
        {
            playerSpawnPosition = farmSpawnFromTownPosition;
        }
        StartCoroutine(LoadSceneAsync("SampleScene"));
    }
    public void LoadTownScene()
    {
        previousScene = SceneManager.GetActiveScene().name;
        if (previousScene == "SampleScene") 
        {
            playerSpawnPosition = townSpawnFromFarmPosition;
        }
        else if (previousScene == "HarborScene")
        {
            playerSpawnPosition = townSpawnFromBeachPosition;
        }
        else if (previousScene == "CaveScene")
        {
            playerSpawnPosition = townSpawnFromMinePosition;
        }
        else if (previousScene == "GeneralStoreScene")
        {
            playerSpawnPosition = townspawnFromgeneralStorePosition;
        }
        else if (previousScene == "BlackSmith Scene")
        {
            playerSpawnPosition = townSpawnFromgeneralBlacksmithPosition;
        }
        else if (previousScene == "JoshHouseScene")
        {
            playerSpawnPosition = townSpawnFromgeneralJoshPosition;
        }
        else if (previousScene == "ManuHouseScene")
        {
            playerSpawnPosition = townSpawnFromgeneralManuPosition;
        }
        else if (previousScene == "BarnAndPetScene")
        {
            playerSpawnPosition = townSpawnFromgeneralRanchPosition;
        }
        else
        {
            playerSpawnPosition = townSpawnFromFarmPosition;
        }
        StartCoroutine(LoadSceneAsync("TownScene"));
    }
    public void LoadGeneralStoreScene()
    {
        previousScene = SceneManager.GetActiveScene().name;
        playerSpawnPosition = generalStoreSpawnFromTownPosition;
        StartCoroutine(LoadSceneAsync("GeneralStoreScene"));
    }
    public void LoadBlackSmithScene()
    {
        previousScene = SceneManager.GetActiveScene().name;
        playerSpawnPosition = blacksmithSpawnFromgeneralStorePosition;
        StartCoroutine(LoadSceneAsync("BlackSmith Scene"));
    }
    public void LoadRanchStoreScene()
    {
        previousScene = SceneManager.GetActiveScene().name;
        playerSpawnPosition = ranchSpawnFromgeneralStorePosition;
        StartCoroutine(LoadSceneAsync("BarnAndPetScene"));
    }
    public void LoadJoshHouseScene()
    {
        previousScene = SceneManager.GetActiveScene().name;
        playerSpawnPosition = joshSpawnFromgeneralStorePosition;
        StartCoroutine(LoadSceneAsync("JoshHouseScene"));
    }
    public void LoadManuHouseScene()
    {
        previousScene = SceneManager.GetActiveScene().name;
        playerSpawnPosition = manuSpawnFromgeneralStorePosition;
        StartCoroutine(LoadSceneAsync("ManuHouseScene"));
    }
    public void LoadCaveScene()
    {
        previousScene = SceneManager.GetActiveScene().name;
        playerSpawnPosition = caveSpawnPosition;
        StartCoroutine(LoadSceneAsync("CaveScene"));
    }
    public void LoadHarborScene()
    {
        previousScene = SceneManager.GetActiveScene().name;
        playerSpawnPosition = harborSpawnPosition;
        StartCoroutine(LoadSceneAsync("HarborScene"));
    }
    private System.Collections.IEnumerator LoadSceneAsync(string sceneName)
    {
        if (fadeAnimator != null)
        {
            fadeAnimator.SetTrigger("FadeOut");
            yield return new WaitForSeconds(fadeDuration);
        }
        yield return SceneManager.LoadSceneAsync(sceneName);
        Player player = FindFirstObjectByType<Player>();
        if (player != null)
        {
            Transform spawnPoint = GameObject.Find("SpawnPoint")?.transform;
            if (spawnPoint != null)
            {
                player.SetPosition(spawnPoint.position);
            }
            else
            {
                player.SetPosition(playerSpawnPosition);
            }
        }
        GameObject boundary = GameObject.Find("Scene Boundary");
        if (boundary != null)
        {
            CameraManager.Instance?.SetCameraBounds(boundary);
            CameraManager.Instance.DisableCinemachine();
            CameraManager.Instance?.ResetCameraToPlayer();
            CameraManager.Instance.EnableCinemachineDelayed();
        }
        else
        {
        }

        if (fadeAnimator != null)
        {
            fadeAnimator.SetTrigger("FadeIn");
            yield return new WaitForSeconds(fadeDuration);
        }
    }
    //private IEnumerator LoadSceneAsync(string sceneName)
    //{
    //    yield return SceneManager.LoadSceneAsync(sceneName);
    //    yield return null; 

    //    Player player = FindFirstObjectByType<Player>();
    //    GameObject boundary = GameObject.Find("Scene Boundary");

    //    if (player != null)
    //    {
    //        Transform spawnPoint = GameObject.Find("SpawnPoint")?.transform;
    //        player.SetPosition(spawnPoint != null ? spawnPoint.position : playerSpawnPosition);
    //    }

    //    yield return null; 

    //    if (CameraManager.Instance != null && boundary != null)
    //    {
    //        CameraManager.Instance.DisableConfiner();
    //        CameraManager.Instance.ForceSnapToPlayer();
    //        yield return null;
    //        CameraManager.Instance.SetCameraBounds(boundary);
    //    }
    //}

    [SerializeField] private int currentCaveFloor = 1;
    public int CurrentCaveFloor => currentCaveFloor;
    public void LoadCaveFloor(int floor)
    {
        previousScene = SceneManager.GetActiveScene().name;
        currentCaveFloor = floor;

        string sceneToLoad = GetSceneForFloor(floor);
        playerSpawnPosition = caveSpawnPosition;
        StartCoroutine(LoadSceneAsync(sceneToLoad));
    }
    private string GetSceneForFloor(int floor)
    {
        int index = ((floor - 1) / 4) % 5;
        return index switch
        {
            0 => "CaveFirstMapScene",
            1 => "CaveSecondMapScene",
            2 => "CaveThirdMapScene",
            3 => "CaveFourthMapScene",
            4 => "CaveFifthMapScene",
            _ => "CaveFirstMapScene"
        };
    }
}
