using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int totalCurrency; // tracks the player's currency throughout levels
    public int levelCurrency; // track the currency collected this level
    public Text currencyText; // currency text element
    public int maxHealth = 100; // maximum health of player
    public int currentHealth; // current health of player
    public Image healthFill; // health UI element
    public Text healthText; // health text element
    public GameObject[] inventory; // Weapon, ammo, and health inventory

    // Start is called before the first frame update
    void Start()
    {
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

    // Updtes health UI
    void UpdateHealthUI()
    {
        healthFill.fillAmount = (float)currentHealth / maxHealth;
        healthText.text = currentHealth.ToString();
    }

    // Game over
    public void GameOver()
    {
        Debug.Log("HP 0, game lost.");
    }

    // Level beat
    public void LevelBeat()
    {

    }
}
