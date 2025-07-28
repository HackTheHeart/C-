using UnityEngine;
using UnityEngine.EventSystems;

public class ItemClickDebugger : MonoBehaviour, IPointerClickHandler
{
    // Hàm này sẽ được gọi khi người dùng click vào đối tượng
    public void OnPointerClick(PointerEventData eventData)
    {
        // In ra thông tin khi click
        Debug.Log("Đã click vào đối tượng: " + gameObject.name);
    }
}
