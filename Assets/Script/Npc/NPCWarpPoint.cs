////using UnityEngine;

////public class NPCWarpPoint : MonoBehaviour
////{
////    public string npcNameFilter;
////    public string destinationScene;
////    public Vector2 destinationPosition;
////    private void OnTriggerEnter(Collider other)
////    {
////        NPC npc = other.GetComponent<NPC>();
////        if (npc != null && npc.npcName == npcNameFilter)
////        {
////            NPCWarpManager.Instance.WarpNPC(npc.npcName, destinationScene, destinationPosition);
////            Destroy(npc.gameObject);
////        }
////    }
////}
//using UnityEngine;

//public class ScenePortal : MonoBehaviour
//{
//    public string fromScene;
//    public string toScene;
//    public Vector2 targetSpawnPosition;
//}
//public class WarpManager : MonoBehaviour
//{
//    public static WarpManager Instance;
//    private Dictionary<string, ScenePortal> portals = new();

//    private void Awake()
//    {
//        Instance = this;
//        CachePortals();
//    }

//    private void CachePortals()
//    {
//        portals.Clear();
//        foreach (var portal in FindFirstObjectByType<ScenePortal>())
//        {
//            string key = $"{portal.fromScene}_To_{portal.toScene}";
//            portals[key] = portal;
//        }
//    }

//    public ScenePortal GetPortal(string from, string to)
//    {
//        string key = $"{from}_To_{to}";
//        if (portals.TryGetValue(key, out var portal))
//            return portal;

//        Debug.LogWarning($"[WarpManager] Không tìm th?y portal: {key}");
//        return null;
//    }
//}

