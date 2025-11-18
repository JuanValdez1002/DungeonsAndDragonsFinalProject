using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    public float shakeDuration = 0.2f;
    public float shakeMagnitude = 0.1f;

    private Vector3 originalPosition;

    private void Start()
    {
        originalPosition = transform.localPosition;
    }

    public void Shake()
    {
        StopAllCoroutines();
        StartCoroutine(ShakeRoutine());
    }

    IEnumerator ShakeRoutine()
    {
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            Vector3 randomPoint = originalPosition + Random.insideUnitSphere * shakeMagnitude;
            transform.localPosition = randomPoint;

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPosition;
    }
}
