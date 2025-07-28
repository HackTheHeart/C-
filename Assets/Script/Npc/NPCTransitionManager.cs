//using UnityEngine;
//using UnityEngine.SceneManagement;

//public class NPCWarpManager : MonoBehaviour
//{
//    public static NPCWarpManager Instance;
//    private void Awake()
//    {
//        if (Instance == null) Instance = this;
//        else Destroy(gameObject);
//    }
//    public void WarpNPC(string npcName, string targetScene, Vector2 spawnPosition)
//    {
//        var state = NPCManager.Instance.GetNPCState(npcName);
//        state.currentScene = targetScene;
//        state.currentPosition = spawnPosition;
//        if (SceneManager.GetActiveScene().name == targetScene)
//        {
//            SceneNPCSpawner.Instance.SpawnNPCFromState(state);
//        }
//    }
//}
