using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverUI;
    public GameObject player;
    public static bool IsGameOver = false;

    public void Start()
    {
        IsGameOver = false;          // 🔥 RESET GAME STATE
        Time.timeScale = 1f;         // 🔥 SAFETY RESET
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

  void Update()
    {
        if (gameOverUI.activeInHierarchy)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }


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

    public void RestartLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            Debug.Log("Restarting Level...");
        }

    public void MainMenu()
        {
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

