using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverUI;
    public GameObject pauseUI;              //  ADD: Pause canvas reference
    public GameObject player;

    public static bool IsGameOver = false;
    public static bool IsPaused = false;    //  ADD: Pause state flag

    public void Start()
    {
        IsGameOver = false;          //  RESET GAME STATE
        IsPaused = false;            //  RESET PAUSE STATE
        Time.timeScale = 1f;         //  SAFETY RESET

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        if (pauseUI != null)         // ensure pause UI starts hidden
            pauseUI.SetActive(false);
    }

    void Update()
    {
        // ❌ Do NOT allow pause when game is over
        if (!IsGameOver && Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }

        // Existing Game Over cursor logic (UNCHANGED)
        if (gameOverUI.activeInHierarchy)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    // ================= PAUSE LOGIC (NEW) =================

    void TogglePause()
    {
        if (IsPaused)
            ResumeGame();
        else
            PauseGame();
    }

    void PauseGame()
    {
        IsPaused = true;
        Time.timeScale = 0f;

        if (pauseUI != null)
            pauseUI.SetActive(true);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void ResumeGame()
    {
        IsPaused = false;
        Time.timeScale = 1f;

        if (pauseUI != null)
            pauseUI.SetActive(false);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // ================= GAME OVER (UNCHANGED) =================

    public void gameOver()
    {
        IsGameOver = true;
        gameOverUI.SetActive(true);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        // 🔥 STOP CAMERA SHAKE
        if (Camera.main != null)
        {
            CameraShake shake = Camera.main.GetComponent<CameraShake>();
            if (shake != null)
                shake.StopShake();
        }
    }

    // ================= BUTTON FUNCTIONS (UNCHANGED) =================

    public void RestartLevel()
    {
        ResumeGame(); // 🔹 ensures timescale resets if restarting while paused
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Debug.Log("Restarting Level...");
    }

   public void MainMenu()
    {
        // 🔥 FULL RESET FOR MENU
        IsPaused = false;
        IsGameOver = false;
        Time.timeScale = 1f;

        if (pauseUI != null)
            pauseUI.SetActive(false);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        SceneManager.LoadScene("Game_Menu");
        Debug.Log("Loading Main Menu...");
    }


    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quitting Game...");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
