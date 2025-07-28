using UnityEngine;
using System.Collections;

public class FlowerShake : MonoBehaviour
{
    private Vector3 originalRotation;
    private bool isShaking = false;
    private void Start()
    {
        originalRotation = transform.eulerAngles;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") && !isShaking)
        {
            StartCoroutine(ShakeFlower());
        }
    }
    private IEnumerator ShakeFlower()
    {
        isShaking = true;
        float duration = 0.5f;
        float timer = 0;
        while (timer < duration)
        {
            float angle = Mathf.Sin(timer * 20) * 5;
            transform.eulerAngles = originalRotation + new Vector3(0, 0, angle);
            timer += Time.deltaTime;
            yield return null;
        }
        transform.eulerAngles = originalRotation;
        isShaking = false;
    }
}
