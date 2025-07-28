using UnityEngine;

public class ReturnToFarmTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SceneTransitionManager.Instance.LoadFarmScene();
        }
    }
}
