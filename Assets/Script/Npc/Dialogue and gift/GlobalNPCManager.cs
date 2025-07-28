using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class GlobalNPCManager : MonoBehaviour
{
    public static GlobalNPCManager Instance;
    public GameObject npcPrefab;
    private Dictionary<string, NPCState> npcStates = new();
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadNPCData();
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        float deltaTime = Time.deltaTime;
        float timeOfDay = GameTimeManager.Instance.TimeOfDay;

        foreach (var npc in npcStates.Values)
        {
            npc.UpdateState(timeOfDay, deltaTime);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SpawnNPCsForScene(scene.name);
    }

    public void RegisterNPC(string npcName, string startScene, Vector2 startPos, List<NPCScheduleEntry> schedule)
    {
        if (!npcStates.ContainsKey(npcName))
        {
            npcStates[npcName] = new NPCState(npcName, startScene, startPos, schedule);
        }
    }

    public NPCState GetNPCState(string name) => npcStates.ContainsKey(name) ? npcStates[name] : null;

    public void SpawnNPCsForScene(string sceneName)
    {
        foreach (var npc in npcStates.Values)
        {
            if (npc.currentScene == sceneName)
            {
                GameObject go = Instantiate(npcPrefab);
                go.transform.position = npc.currentPosition;

                var npcComponent = go.GetComponent<NPC>();
                npcComponent.InitFromState(npc);
            }
        }
    }

    #region Save/Load

    [System.Serializable]
    public class NPCStateCollection
    {
        public List<NPCState> allNPCs = new();
    }

    public void SaveNPCData()
    {
        var data = new NPCStateCollection();
        data.allNPCs.AddRange(npcStates.Values);

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(GetSavePath(), json);
        Debug.Log("[GlobalNPCManager] Đã lưu NPC data.");
    }

    public void LoadNPCData()
    {
        if (!File.Exists(GetSavePath()))
        {
            Debug.LogWarning("[GlobalNPCManager] Chưa có file lưu NPC.");
            return;
        }

        string json = File.ReadAllText(GetSavePath());
        var data = JsonUtility.FromJson<NPCStateCollection>(json);

        npcStates.Clear();
        foreach (var npc in data.allNPCs)
        {
            npcStates[npc.npcName] = npc;
        }

        Debug.Log("[GlobalNPCManager] Đã load NPC data.");
    }

    private string GetSavePath()
    {
        return Path.Combine(Application.persistentDataPath, "npc_data.json");
    }

    #endregion
}
