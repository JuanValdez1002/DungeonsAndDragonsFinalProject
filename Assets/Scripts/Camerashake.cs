using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    public float shakeDuration = 0.2f;
    public float shakeMagnitude = 0.1f;

    private Vector3 originalPosition;
    private Coroutine shakeCoroutine;

    void Start()
    {
        originalPosition = transform.localPosition;
    }

    public void Shake()
    {
        // 🔒 Prevent shake after game over
        if (GameManager.IsGameOver)
            return;

        StopShake();
        shakeCoroutine = StartCoroutine(ShakeRoutine());
    }

    public void StopShake()
    {
        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
            shakeCoroutine = null;
        }

        transform.localPosition = originalPosition;
    }

    IEnumerator ShakeRoutine()
    {
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            // 🔥 Emergency exit if game ends mid-shake
            if (GameManager.IsGameOver)
                break;

            Vector3 randomPoint =
                originalPosition + Random.insideUnitSphere * shakeMagnitude;

            transform.localPosition = randomPoint;

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPosition;
        shakeCoroutine = null;
    }
}
