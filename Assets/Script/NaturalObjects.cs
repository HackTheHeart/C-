using UnityEngine;
using System.Collections;

public abstract class NaturalObjects : MonoBehaviour
{
    protected Vector3 originalPosition;
    protected virtual void Start()
    {
        originalPosition = transform.position;
    }
    public abstract void Interact();
    protected virtual IEnumerator ShakeObject()
    {
        for (int i = 0; i < 5; i++)
        {
            float shakeAmount = UnityEngine.Random.Range(-0.05f, 0.05f); 
            transform.position = originalPosition + new Vector3(shakeAmount, 0, 0);
            yield return new WaitForSeconds(0.05f);
        }
        transform.position = originalPosition;
    }
}