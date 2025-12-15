using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class BossDeathEnding : MonoBehaviour
{
    public CanvasGroup fadeCanvas;
    public TextMeshProUGUI messageText;
    public float fadeDuration = 2f;
    public float messageHoldTime = 3f;
    public string nextSceneName;

    // 🔥 ADD THIS
    public void TriggerEnding()
    {
        StartEndingSequence();
    }

    public void StartEndingSequence()
    {
        StartCoroutine(EndingRoutine());
    }

    IEnumerator EndingRoutine()
    {
        // Freeze gameplay
        Time.timeScale = 0f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        fadeCanvas.gameObject.SetActive(true);

        float t = 0f;

        // Fade to black
        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            fadeCanvas.alpha = Mathf.Lerp(0f, 1f, t / fadeDuration);
            yield return null;
        }

        fadeCanvas.alpha = 1f;

        // Show message
        messageText.gameObject.SetActive(true);

        yield return new WaitForSecondsRealtime(messageHoldTime);

        // Restore time
        Time.timeScale = 1f;

        SceneManager.LoadScene(nextSceneName);
    }
}
