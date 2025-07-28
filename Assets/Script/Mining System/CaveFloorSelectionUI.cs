using UnityEngine;
using UnityEngine.UI;

public class CaveFloorSelectionUI : MonoBehaviour
{
    public Button floor5Button;
    public Button floor10Button;
    public Button floor15Button;
    public Button floor20Button;
    public Button closeButton;
    private void OnEnable()
    {
        UpdateButtons();
        if (closeButton != null)
            closeButton.onClick.AddListener(CloseUI);
    }
    private void UpdateButtons()
    {
        floor5Button.interactable = CaveProgressManager.Instance.IsFloorUnlocked(5);
        floor10Button.interactable = CaveProgressManager.Instance.IsFloorUnlocked(10);
        floor15Button.interactable = CaveProgressManager.Instance.IsFloorUnlocked(15);
        floor20Button.interactable = CaveProgressManager.Instance.IsFloorUnlocked(20);
    }
    public void LoadFloor(int floor)
    {
        gameObject.SetActive(false);
        SceneTransitionManager.Instance.LoadCaveFloor(floor);
    }

    public void CloseUI()
    {
        gameObject.SetActive(false);
    }
}
