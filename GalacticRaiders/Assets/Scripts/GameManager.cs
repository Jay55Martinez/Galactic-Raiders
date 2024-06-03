using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
    public static bool gameOver; // game state bool
    public string nextLevel; // next level
    public int totalCurrency; // tracks the player's currency throughout levels
    public int levelCurrency; // track the currency collected this level
    public Text currencyText; // currency text element
    public int maxHealth = 100; // maximum health of player
    public int currentHealth; // current health of player
    public Image healthFill; // health UI element
    public Text healthText; // health text element
    public GameObject[] inventory = new GameObject[4]; // Weapon, ammo, and health inventory
    public GameObject[] inventoryUI = new GameObject[4]; // inventory UI element

    // Start is called before the first frame update
    void Start()
    {
        gameOver = false;
        levelCurrency = 0;
        UpdateCurrencyText();
        currentHealth = maxHealth;
        UpdateHealthUI();
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth == 0)
        {
            GameOver();
        }
    }

    // Increases level currency
    public void IncreaseCurrency(int amt) {
        levelCurrency += amt;
        UpdateCurrencyText();
    }

    // Updates currency text
    void UpdateCurrencyText() {
        currencyText.text = "Currency: " + levelCurrency;
    }

    // Taking damage
    public void Damage(int amt)
    {
        currentHealth -= amt;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI();

    }

    // Heal HP
    public void Heal(int amt)
    {
        currentHealth += amt;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI();
    }

    // Updates health UI
    void UpdateHealthUI()
    {
        healthFill.fillAmount = (float)currentHealth / maxHealth;
        healthText.text = currentHealth.ToString();
    }

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

    // Updates inventory UI
    void UpdateInventoryUI()
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
