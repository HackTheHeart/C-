using UnityEngine;

public class FarmSceneController : MonoBehaviour
{
    private void OnDisable()
    {
        if (BarnManager.Instance == null)
        {
            Debug.LogError("BarnManager chưa được khởi tạo!");
            return;
        }
        BarnManager.Instance.SaveAllAnimals();
        Debug.Log("Tất cả động vật đã được lưu trước khi thoát Scene.");
    }
}
