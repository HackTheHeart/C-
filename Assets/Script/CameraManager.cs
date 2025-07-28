using UnityEngine;
using Unity.Cinemachine;
using System.Collections;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance { get; private set; }

    [SerializeField] private Camera mainCamera;
    [SerializeField] private CinemachineCamera cinemachineCam;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        if (mainCamera == null)
            mainCamera = Camera.main;
        if (cinemachineCam == null)
            cinemachineCam = FindFirstObjectByType<CinemachineCamera>();
    }
    //public void SetCameraBounds(GameObject cameraBoundsObject)
    //{
    //    if (cinemachineCam != null && cameraBoundsObject != null)
    //    {
    //        var confiner = cinemachineCam.GetComponent<CinemachineConfiner2D>();
    //        if (confiner != null)
    //        {
    //            confiner.BoundingShape2D = cameraBoundsObject.GetComponent<Collider2D>();
    //            confiner.InvalidateBoundingShapeCache();
    //        }
    //    }
    //}
    public void SetCameraBounds(GameObject cameraBoundsObject)
    {
        if (cinemachineCam != null && cameraBoundsObject != null)
        {
            var confiner = cinemachineCam.GetComponent<CinemachineConfiner2D>();
            var polygon = cameraBoundsObject.GetComponent<PolygonCollider2D>();

            if (confiner != null && polygon != null)
            {
                confiner.BoundingShape2D = polygon;
                confiner.InvalidateBoundingShapeCache();
            }
            else
            {
                Debug.LogWarning("Confiner or PolygonCollider2D not found on CameraBoundary object.");
            }
        }
    }

    public void DisableConfiner()
    {
        var confiner = cinemachineCam;
        if (confiner != null)
        {
            confiner.enabled = false;
        }
    }
    public void EnableConfiner()
    {
        var confiner = cinemachineCam;
        if (confiner != null)
        {
            confiner.enabled = true;
        }
    }
    public void ForceSnapToPlayer()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj == null) return;
        cinemachineCam.transform.position = new Vector3(
            playerObj.transform.position.x,
            playerObj.transform.position.y,
            cinemachineCam.transform.position.z); 
    }
    public void ResetCameraToPlayer()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj == null) return;
        var confiner = cinemachineCam.GetComponent<CinemachineConfiner2D>();
        if (confiner != null) confiner.enabled = false;
        cinemachineCam.transform.position = new Vector3(
            playerObj.transform.position.x,
            playerObj.transform.position.y,
            cinemachineCam.transform.position.z);
        if (confiner != null) confiner.enabled = true;
    }
    // Tắt hoàn toàn Cinemachine camera (tắt GameObject)
    public void DisableCinemachine()
    {
        if (cinemachineCam != null)
        {
            cinemachineCam.gameObject.SetActive(false);
            Debug.Log("Cinemachine camera disabled.");
        }
        else
        {
            Debug.LogWarning("Cinemachine camera is null.");
        }
    }

    // Bật lại Cinemachine camera (bật GameObject)
    public void EnableCinemachineDelayed()
    {
        StartCoroutine(EnableAfterFrame());
    }

    private IEnumerator EnableAfterFrame()
    {
        yield return null;
        if (cinemachineCam != null)
        {
            cinemachineCam.gameObject.SetActive(true);
        }
    }


    public void SetProjection(bool isOrthographic)
    {
        if (mainCamera != null)
            mainCamera.orthographic = isOrthographic;
    }
}
