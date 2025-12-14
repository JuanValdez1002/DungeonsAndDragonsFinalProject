using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverUI;
    public GameObject player;

    public void Start()
    {
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
            gameOverUI.SetActive(true);
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

