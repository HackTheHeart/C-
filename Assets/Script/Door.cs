using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private bool isHouseDoor;
    [SerializeField] private bool isBarnDoor;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (isHouseDoor)
            {
                SceneTransitionManager.Instance.LoadHouseScene();
            }
            else if (isBarnDoor)
            {
                SceneTransitionManager.Instance.LoadBarnScene();
            }
            else
            {
                SceneTransitionManager.Instance.LoadFarmScene();
            }
        }
    }
}
