using UnityEngine;

public class CavePortal : MonoBehaviour
{
    public GameObject floorSelectionUI;

    private void OnMouseDown()
    {
        Debug.Log("Clicked");
        if (Vector2.Distance(Player.Instance.transform.position, transform.position) < 2f)
        {
            if (CaveProgressManager.Instance.HasUnlockedAnyFloor())
            {
                floorSelectionUI.SetActive(true);
            }
            else
            {
                SceneTransitionManager.Instance.LoadCaveFloor(1);
            }
        }
    }
}

