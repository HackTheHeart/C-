using UnityEngine;
public class ScenePortal : MonoBehaviour
{
    public string targetScene;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            switch (targetScene)
            {
                case "Farm":
                    SceneTransitionManager.Instance.LoadFarmScene();
                    break;
                case "Town":
                    SceneTransitionManager.Instance.LoadTownScene();
                    break;
                case "Mine":
                    SceneTransitionManager.Instance.LoadCaveScene();
                    break;
                case "Harbor":
                    SceneTransitionManager.Instance.LoadHarborScene();
                    break;
                case "GeneralStore":
                    SceneTransitionManager.Instance.LoadGeneralStoreScene();
                    break;
                case "BlackSmith":
                    SceneTransitionManager.Instance.LoadBlackSmithScene();
                    break;
                case "JoshHouse":
                    SceneTransitionManager.Instance.LoadJoshHouseScene();
                    break;
                case "ManuHouse":
                    SceneTransitionManager.Instance.LoadManuHouseScene();
                    break;
                case "Ranch":
                    SceneTransitionManager.Instance.LoadRanchStoreScene();
                    break;
                default:
                    Debug.LogWarning("⚠ Haven't Added that scene yet.");
                    break;
            }
        }
    }
}
