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
        UpdateAmmoCounter();
        UpdateHealCounter();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCurrencyText();
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
        GameManager.UpdateLevel();
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
        if (GameManager.isBase)
        {
            currencyText.text = "Total Currency: " + GameManager.totalCurrency;
        }
        else
        {
            currencyText.text = "Level Currency: " + levelCurrency;
        }
    }

    // Updates ammo counter
    public void UpdateAmmoCounter()
    {
        if (inventoryUI[1].GetComponentInChildren<Text>() != null)
        {
            inventoryUI[1].GetComponentInChildren<Text>().text = GameManager.reserveAmmo.ToString();
        }
    }

    // Updates heal counter
    public void UpdateHealCounter()
    {
        inventoryUI[3].GetComponentInChildren<Text>().text = GameManager.heals.ToString();
    }

    public void UpdateWeaponUI(int weaponIndex)
    {
        for (int i = 0; i < GameManager.weapons.Length; i++)
        {
            if (GameManager.weapons[i])
            {
                if (i == 0)
                {
                    inventoryUI[i].transform.Find("RevolverImage").GetComponent<Image>().gameObject.SetActive(true);
                }
                else if (i == 1)
                {
                    inventoryUI[i].transform.Find("RifleImage").GetComponent<Image>().gameObject.SetActive(true);
                    UpdateAmmoCounter();
                }
                else if (i ==2)
                {
                    inventoryUI[i].transform.Find("ShotgunImage").GetComponent<Image>().gameObject.SetActive(true);
                }
            }
        }

        if (weaponIndex == 0 || weaponIndex == 1 || weaponIndex == 2)
        {
            inventoryUI[weaponIndex].color = Color.red;
            inventoryUI[(weaponIndex + 1) % 3].color = Color.gray;
            inventoryUI[(weaponIndex + 2) % 3].color = Color.gray;
        }
    }
}
