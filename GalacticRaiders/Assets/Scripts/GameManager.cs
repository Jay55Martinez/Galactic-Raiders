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

    // game progress
    public static int gameProgress; // which levels have the player beaten?
    public static int levelAmt; // how many levels are there?
    public static bool isBase; // is the current level an intermediary level?
    public static bool[] weapons = {false, false, false};
    public bool devMode;

    void Awake() {
        isBase = false;
        gameProgress = 1;  
        levelAmt = 3;
        if (devMode) {
            weapons[0] = true;
            weapons[1] = true;
            weapons[2] = true;
        }
        // set all player guns to false
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

    public static string NextLevel() { // what is the name of the next level?
        if (!isBase) {
            isBase = true;
            return "Base";
        } else if (gameProgress <= levelAmt) {
            gameProgress++;
            isBase = false;
            return "Level" + gameProgress.ToString();
        } else {
            return null;
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
}
