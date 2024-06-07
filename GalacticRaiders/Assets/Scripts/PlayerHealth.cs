using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100; // maximum health of player
    public int currentHealth; // current health of player
    public Image healthFill; // health UI element
    public Text healthText; // health text element

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateHealthUI()
    {
        healthFill.fillAmount = (float)currentHealth / maxHealth;
        healthText.text = currentHealth.ToString();
    }

    // Taking damage
    public void Damage(int amt)
    {
        currentHealth -= amt;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI();

        if (currentHealth == 0) {
            FindObjectOfType<LevelManager>().GameOver();
        }
    }

    // Heal HP
    public void Heal(int amt)
    {
        currentHealth += amt;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI();
    }
}
