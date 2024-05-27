using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public float totalCurrency; // tracks the player's currency throughout levels
    public float levelCurrency; // track the currency collected this level
    public Text currencyText;

    // Start is called before the first frame update
    void Start()
    {
        levelCurrency = 0;
        DontDestroyOnLoad(gameObject);
        UpdateCurrencyText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void IncreaseCurrency(int amt) {
        levelCurrency += amt;
        UpdateCurrencyText();
    }

    void UpdateCurrencyText() {
        currencyText.text = "Currency: " + levelCurrency;
    }
}
