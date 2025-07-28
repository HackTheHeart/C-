using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    [System.Serializable]
    public class UIPage
    {
        public string name;
        public GameObject panel;
    }
    public List<UIPage> pages = new List<UIPage>();
    public UI_Inventory uiInventory;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    public void OpenPage(string name)
    {
        if (uiInventory.IsOpen())
        {
            uiInventory.ForceClose();
        }

        foreach (var page in pages)
        {
            page.panel.SetActive(page.name == name);
        }
    }
    public void CloseAllPages()
    {
        foreach (var page in pages)
        {
            page.panel.SetActive(false);
        }

        if (uiInventory.IsOpen())
        {
            uiInventory.ForceClose();
        }
    }
    public bool AnyUIPanelOpen()
    {
        foreach (var page in pages)
        {
            if (page.panel.activeSelf) return true;
        }

        if (uiInventory.IsOpen()) return true;

        return false;
    }
}
