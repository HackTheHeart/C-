using UnityEngine;

public class UI_ButtonHandler : MonoBehaviour
{
    public void OnClick_OpenUI(string uiName)
    {
        UIManager.Instance.OpenPage(uiName);
    }

    public void OnClick_CloseAllUI()
    {
        UIManager.Instance.CloseAllPages();
    }
}
