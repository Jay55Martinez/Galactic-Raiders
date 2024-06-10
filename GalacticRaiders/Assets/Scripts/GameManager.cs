using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

// this class is used to manage global variables about the player and progress
public class GameManager : MonoBehaviour
{
    public int totalCurrency; // tracks the player's currency throughout levels

    // game progress
    public static int gameProgress; // which levels have the player beaten?
    public static int levelAmt; // how many levels are there?
    public static bool isBase; // is the current level an intermediary level?
    private static bool hasPistol;
    private static bool hasRifle;
    private static bool hasRailgun;

    void Awake() {
        isBase = false;
        gameProgress = 1;  
        levelAmt = 2;
        // set all player guns to false
    }

    // Start is called before the first frame update
    void Start()
    {
        // levelCurrency = 0;
        // UpdateCurrencyText();
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

    // // Increases level currency
    // public void IncreaseCurrency(int amt) {
    //     levelCurrency += amt;
    //     UpdateCurrencyText();
    // }

    // // Updates currency text
    // void UpdateCurrencyText() {
    //     currencyText.text = "Currency: " + levelCurrency;
    // }

    /*
    // Adds item to inventory
    public void AddToInventory(GameObject collectable)
    {
        if (collectable.CompareTag("Revolver") && inventory[0] == null)
        {
            inventory[0] = collectable;
            inventoryUI[0].SetActive(true);
        }
        else if (collectable.CompareTag("Rifle") && inventory[1] == null)
        {
            inventory[1] = collectable;
            inventoryUI[1].SetActive(true);
        }
        else if (collectable.CompareTag("Railgun") && inventory[2] == null)
        {
            inventory[2] = collectable;
            inventoryUI[2].SetActive(true);
        }
        else if (collectable.CompareTag("Heal") && inventory[3] == null)
        {
            inventory[3] = collectable;
            inventoryUI[3].SetActive(true);
            Text healCounterText = inventory[3].GetComponent<Text>();
            healCounterText.text = (Convert.ToInt32(healCounterText.text) + 1).ToString();
        } 
        else if (collectable.CompareTag("Ammo"))
        {
            Text ammoCounterText = inventory[1].GetComponent<Text>();
            ammoCounterText.text = (Convert.ToInt32(ammoCounterText.text) + 1).ToString();
        }
    }
    */

    // // Updates ammo counter
    // public void UpdateAmmoCounter(int amt)
    // {
    //     Text ammoCounterText = inventoryUI[1].GetComponentInChildren<Text>();
    //     //int ammoCount = int.Parse(ammoCounterText.text) + amt;
    //     ammoCounterText.text = (int.Parse(ammoCounterText.text) + amt).ToString();
    // }

    // // Updates heal counter
    // public void UpdateHealCounter(int amt)
    // {
    //     Text healCounterText = inventoryUI[3].GetComponentInChildren<Text>();
    //     //int healCount = int.Parse(healCounterText.text) + amt;
    //     healCounterText.text = (int.Parse(healCounterText.text) + amt).ToString();
    // }



}
