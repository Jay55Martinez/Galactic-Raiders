using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuBehaviour : MonoBehaviour
{
    public static bool isGamePaused = false;
    public GameObject pauseMenu;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (isGamePaused) {
                ResumeGame();
            } else {
                PauseGame();
            }
        }
    }

    void PauseGame() {
        isGamePaused = true;
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void ResumeGame() {
        isGamePaused = false;
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void LoadMainMenu() {
        // save
        GameManager.Save();
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1;
        isGamePaused = false;
    }

    public void ExitGame() {
        // save
        GameManager.Save();
        Application.Quit();
    }
}