using UnityEngine;

public class SceneTrigger : MonoBehaviour
{
    public enum SceneType { Cave, Harbor }
    public SceneType targetScene;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            switch (targetScene)
            {
                case SceneType.Cave:
                    SceneTransitionManager.Instance.LoadCaveScene();
                    break;
                case SceneType.Harbor:
                    SceneTransitionManager.Instance.LoadHarborScene();
                    break;
            }
        }
    }
}
