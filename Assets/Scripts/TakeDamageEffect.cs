using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class TakeDamageEffect : MonoBehaviour
{
    private Volume volume;
    private Vignette vignette;

    public float flashIntensity = 1f;
    public float fadeSpeed = 3f;

    private float currentIntensity = 0f;

    void Start()
    {
        volume = GetComponent<Volume>();

        if (volume == null)
        {
            Debug.LogError("No Volume component found on TakeDamageEffect object!");
            return;
        }

        volume.profile.TryGet(out vignette);

        if (vignette == null)
        {
            Debug.LogError("No Vignette override found in the PostProcessing profile!");
        }
    }

    void Update()
    {
        if (vignette != null && currentIntensity > 0)
        {
            currentIntensity -= Time.deltaTime * fadeSpeed;
            vignette.intensity.value = currentIntensity;
        }
    }

    public void FlashRed()
    {
        if (vignette == null) return;

        currentIntensity = flashIntensity;
        vignette.intensity.value = currentIntensity;
    }
}
