using UnityEngine;

public class ParentSingleton : MonoBehaviour
{
    private static ParentSingleton instance;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            Transform child = transform.Find("Inventory_UI");
            if (child != null)
            {
                child.gameObject.SetActive(false);
            }
            else
            {
                Debug.LogWarning("Child 'Inventory_UI' not found under " + gameObject.name);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
