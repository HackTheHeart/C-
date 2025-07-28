using UnityEngine;

public class UI_Player : MonoBehaviour
{
    // Biến tĩnh để giữ instance duy nhất của UI_Player
    public static UI_Player Instance;

    void Awake()
    {
        // Kiểm tra nếu đã có một instance tồn tại
        if (Instance != null && Instance != this)
        {
            // Nếu có instance khác, hủy đối tượng này
            Destroy(gameObject);
        }
        else
        {
            // Gán instance là đối tượng này và không xóa khi chuyển cảnh
            Instance = this;
            DontDestroyOnLoad(gameObject); // Giữ lại UI_Player khi chuyển cảnh
        }
    }

    // Phương thức để hiển thị UI của Player
    public void ShowUI()
    {
        // Code để hiển thị UI của Player
        Debug.Log("UI của Player đang hiển thị.");
    }

    // Phương thức để ẩn UI của Player
    public void HideUI()
    {
        // Code để ẩn UI của Player
        Debug.Log("UI của Player đang ẩn.");
    }
}
