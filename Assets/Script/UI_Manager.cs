using UnityEngine;

public class UI_Manager : MonoBehaviour
{
    // Biến tĩnh để tham chiếu đến instance duy nhất của UI_Manager
    public static UI_Manager Instance;

    // Danh sách các UI quan trọng cần giữ lại
    public GameObject[] uiToPreserve;

    void Awake()
    {
        // Kiểm tra nếu đã có một instance khác của UI_Manager
        if (Instance != null && Instance != this)
        {
            // Nếu đã có instance khác, xóa đối tượng này
            Destroy(gameObject);
        }
        else
        {
            // Gán instance là đối tượng này và không xóa khi chuyển cảnh
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Giữ lại các UI quan trọng trong uiToPreserve
            foreach (var ui in uiToPreserve)
            {
                DontDestroyOnLoad(ui); // Không xóa các UI này khi chuyển cảnh
            }
        }
    }

    // Phương thức để cập nhật danh sách UI cần giữ lại
    public void AddUIToPreserve(GameObject ui)
    {
        GameObject[] newUIList = new GameObject[uiToPreserve.Length + 1];
        uiToPreserve.CopyTo(newUIList, 0);
        newUIList[uiToPreserve.Length] = ui;
        uiToPreserve = newUIList;
    }
}
