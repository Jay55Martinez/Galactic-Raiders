using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// this class is used for loading & losing levels
public class LevelManager : MonoBehaviour
{
    public static bool gameOver; // game state bool
    public string nextLevel; // next level

    // Start is called before the first frame update
    void Start()
    {
        gameOver = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    // Game over
    public void GameOver()
    {
        gameOver = true;
        Debug.Log("HP 0, game lost.");
        Invoke("LoadCurrentLevel", 2);
    }

    // Level beat
    public void LevelBeat()
    {
        gameOver = true;
        if (!string.IsNullOrEmpty(nextLevel))
        {
            Invoke("LoadNextLevel", 2);
        }
    }

    // Loads the current level again
    private void LoadCurrentLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Loads the next level
    private void LoadNextLevel()
    {
        SceneManager.LoadScene(nextLevel);
    }
}
