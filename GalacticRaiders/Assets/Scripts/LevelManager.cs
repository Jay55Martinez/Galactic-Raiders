using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// this class is used for loading & losing levels
public class LevelManager : MonoBehaviour
{
    public static bool gameOver; // game state bool
    private string nextLevel; // next level

    // inventory
    public int levelCurrency; // track the currency collected this level
    public Text currencyText; // currency text element
    public GameObject[] inventory = new GameObject[4]; // Weapon, ammo, and health inventory
    public Image[] inventoryUI = new Image[4]; // inventory UI element

    public AudioClip winSFX;
    private Transform player;

    // Start is called before the first frame update
    void Start()
    {
        gameOver = false;
        levelCurrency = 0;
        UpdateCurrencyText();
        player = GameObject.FindGameObjectWithTag("Player").transform;
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
        AudioSource.PlayClipAtPoint(winSFX, player.position);
        GameManager.AddCurrency(levelCurrency); // add to the player's total currency
        nextLevel = GameManager.NextLevel();
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

    // Increases level currency
    public void IncreaseCurrency(int amt) {
        levelCurrency += amt;
        UpdateCurrencyText();
    }

    // Updates currency text
    void UpdateCurrencyText() {
        currencyText.text = "Currency: " + levelCurrency;
    }

    // Updates ammo counter
    public void UpdateAmmoCounter(int amt)
    {
        Text ammoCounterText = inventoryUI[1].GetComponentInChildren<Text>();
        //int ammoCount = int.Parse(ammoCounterText.text) + amt;
        ammoCounterText.text = (int.Parse(ammoCounterText.text) + amt).ToString();
    }

    // Updates heal counter
    public void UpdateHealCounter(int amt)
    {
        Text healCounterText = inventoryUI[3].GetComponentInChildren<Text>();
        //int healCount = int.Parse(healCounterText.text) + amt;
        healCounterText.text = (int.Parse(healCounterText.text) + amt).ToString();
    }
}
