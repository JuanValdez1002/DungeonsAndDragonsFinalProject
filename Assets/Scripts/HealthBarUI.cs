using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthBarUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Image foregroundBar;   // The main bar

    private bool isLowHPBlinking = false;
    private float pulseSpeed = 3.5f; // Speed of blinking/pulsing under 10% HP

    /// <summary>
    /// Updates the health bar visuals.
    /// </summary>
    public void UpdateHealthBar(float current, float max)
    {
        float t = current / max;

        // Update bar instantly
        foregroundBar.fillAmount = t;

        // Apply color gradient
        foregroundBar.color = HealthColorGradient(t);

        // Low HP pulse
        if (t <= 0.10f)
        {
            if (!isLowHPBlinking)
                StartCoroutine(LowHPBlink());
        }
        else
        {
            if (isLowHPBlinking)
            {
                StopAllCoroutines();
                isLowHPBlinking = false;
            }

            // Restore correct gradient color
            foregroundBar.color = HealthColorGradient(t);
        }
    }

    /// <summary>
    /// Smooth gradient: RED (<10%) → ORANGE → YELLOW → GREEN
    /// </summary>
    private Color HealthColorGradient(float t)
    {
        if (t <= 0.10f)
            return Color.red;

        if (t <= 0.50f)
        {
            float lerp = (t - 0.10f) / 0.40f; // maps 0.1–0.5 to 0–1
            return Color.Lerp(Color.red, Color.yellow, lerp);
        }

        return Color.Lerp(Color.yellow, Color.green, (t - 0.50f) / 0.50f);
    }

    /// <summary>
    /// Pulse/blink effect while under 10% HP
    /// </summary>
    private IEnumerator LowHPBlink()
    {
        isLowHPBlinking = true;

        while (true)
        {
            float pulse = (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f;

            foregroundBar.color = Color.Lerp(
                new Color(0.7f, 0f, 0f),     // dark red
                new Color(1f, 0.2f, 0.2f),   // bright red
                pulse
            );

            yield return null;
        }
    }
}
