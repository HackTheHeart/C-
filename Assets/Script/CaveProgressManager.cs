//using UnityEngine;

//public class CaveProgressManager : MonoBehaviour
//{
//    public static CaveProgressManager Instance;
//    public int highestFloorReached = 1;
//    public int maxFloorReached = 20;
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
//    public bool IsWarpUnlocked(int floor)
//    {
//        return highestFloorReached >= floor;
//    }
//    public void SetFloorReached(int floor)
//    {
//        if (floor > highestFloorReached)
//            highestFloorReached = floor;
//    }
//}
