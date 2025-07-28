using UnityEngine;

public class InteractableStaircase : MonoBehaviour
{
    public enum StairType { Up, Down }
    public StairType stairType;
    public float interactionRange = 1.5f;
    private Camera mainCam;
    private Transform player;
    private void Start()
    {
        mainCam = Camera.main;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
            Collider2D hit = Physics2D.OverlapPoint(mousePos);
            if (hit != null && hit.gameObject == this.gameObject)
            {
                float distance = Vector2.Distance(player.position, transform.position);
                if (distance <= interactionRange)
                {
                    if (stairType == StairType.Up)
                    {
                        SceneTransitionManager.Instance.LoadCaveScene();
                    }
                    else if (stairType == StairType.Down)
                    {
                        int currentFloor = SceneTransitionManager.Instance.CurrentCaveFloor;
                        SceneTransitionManager.Instance.LoadCaveFloor(currentFloor + 1);
                    }
                }
            }
        }
    }
}
