using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

// this class is used to manage global variables about the player and progress
public class GameManager : MonoBehaviour
{
    public static int totalCurrency; // tracks the player's currency throughout levels
    public static int reserveAmmo; // Tracks rifle ammo thoughout levels

    // game progress
    public static int gameProgress; // which levels have the player beaten?
    /* Level Progression scheme: 
    level 0 base (main menu)
    level 1 notbase (level 1)
    level 1 base (base after level 1)
    level 2 notbase (level 2)
    ...
    */
    public static int levelAmt; // how many levels are there?
    public static bool isBase; // is the current level an intermediary level?
    public static bool[] weapons = {false, false, false};
    public bool devMode = true;

    // sensitivity
    public static float sensitivity;

    void Awake() {
        // isBase = true;
        // gameProgress = 0;  
        levelAmt = 3;
        if (devMode) {
            weapons[0] = true;
            weapons[1] = true;
            weapons[2] = true;
            totalCurrency = 150;
            reserveAmmo = 150;
        }
        // set all player guns to false
        
        Load();
    }

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public static string CurrentLevel()
    {
        if (isBase)
        {
            return "Base";
        }
        else if (!isBase)
        {
            return "Level" + gameProgress.ToString();
        }
        else
        {
            return null;
        }
    }

    public static string NextLevel() { // what is the name of the next level?
        if (!isBase) {
            // isBase = true;
            return "Base";
        } else if (gameProgress <= levelAmt) {
            // gameProgress++;
            // isBase = false;
            return "Level" + (gameProgress+1).ToString();
        } else {
            return null;
        }
    }

    public static void UpdateLevel() {
        if (!isBase) {
            isBase = true;
        } else if (gameProgress <= levelAmt) {
            gameProgress++;
            isBase = false;
        }
    }

    public static void AddCurrency(int amt) {
        totalCurrency += amt;
    }

    public static void SubtractCurrenct(int amt) {
        if (totalCurrency >= amt) {
            totalCurrency -= amt;
        }
    }

    public static void UpdateSens(float val) {
        sensitivity = val;
        PlayerPrefs.SetFloat("sensitivity", val);
    }

    public static void ResetGame() {
        PlayerPrefs.SetInt("gameProgress", 0);

        PlayerPrefs.SetInt("isBase", 1);

        // gun progress
        PlayerPrefs.SetInt("hasPistol", 0);
        PlayerPrefs.SetInt("hasRifle", 0);
        PlayerPrefs.SetInt("hasShotgun", 0);

        PlayerPrefs.SetInt("totalCurrency", 0);
    }

    private static void RevertLevel() { // sets gameProgress and isBase level to last completed level state
        if (isBase) {
            isBase = false;
        } else {
            isBase = true;
            gameProgress--;
        }
    }

    public static void Load() {
        gameProgress = PlayerPrefs.GetInt("gameProgress", 0);
        isBase = PlayerPrefs.GetInt("isBase", 1) == 1;
        sensitivity = PlayerPrefs.GetFloat("sensitivity", 1);

        weapons[0] = PlayerPrefs.GetInt("hasPistol", 0) == 1;
        weapons[1] = PlayerPrefs.GetInt("hasRifle", 0) == 1;
        weapons[2] = PlayerPrefs.GetInt("haseShotgun", 0) == 1;

        totalCurrency = PlayerPrefs.GetInt("totalCurrency", 0);
    }

    public static void Save() {
        RevertLevel(); // get the gameProgress and isBase of most recently completed level

        // level progress
        PlayerPrefs.SetInt("gameProgress", gameProgress);

        PlayerPrefs.SetInt("isBase", isBase ? 1 : 0);

        // gun progress
        PlayerPrefs.SetInt("hasPistol", weapons[0] ? 1 : 0);
        PlayerPrefs.SetInt("hasRifle", weapons[1] ? 1 : 0);
        PlayerPrefs.SetInt("hasShotgun", weapons[2] ? 1 : 0);

        PlayerPrefs.SetInt("totalCurrency", totalCurrency);
    }
}
