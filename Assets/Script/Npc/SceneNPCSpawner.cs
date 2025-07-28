//using UnityEngine.SceneManagement;
//using UnityEngine;

//public class SceneNPCSpawner : MonoBehaviour
//{
//    public static SceneNPCSpawner Instance;
//    public GameObject npcPrefab;
//    private void Awake()
//    {
//        if (Instance == null) Instance = this;
//        else Destroy(gameObject);
//    }
//    public void SpawnNPCFromState(NPCStateData state)
//    {
//        GameObject npcGO = Instantiate(npcPrefab, state.currentPosition, Quaternion.identity);
//        NPC npc = npcGO.GetComponent<NPC>();
//        npc.npcName = state.npcId;}
//    private void Start()
//    {
//        string currentScene = SceneManager.GetActiveScene().name;
//        foreach (var state in NPCManager.Instance.GetNPCStatesInScene(currentScene))
//        {
//            SpawnNPCFromState(state);
//        }
//    }
//}
