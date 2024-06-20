using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuBehaviour : MonoBehaviour
{
    public void StartGame() {
        SceneManager.LoadScene(GameManager.NextLevel());
        GameManager.UpdateLevel();
    }

    public void ExitGame() {
        Application.Quit();
    }
}
