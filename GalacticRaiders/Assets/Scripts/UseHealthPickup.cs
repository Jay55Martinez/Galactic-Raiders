using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UseHealthPickup : MonoBehaviour
{
    public Text healCounterText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha4) && int.Parse(healCounterText.text) > 0)
        {
            //GameManager.AddHeal(-1);
            PlayerHealth playerHealth = FindObjectOfType<PlayerHealth>();
            if (playerHealth.currentHealth != playerHealth.maxHealth)
            {
                playerHealth.Heal(5);
                GameManager.AddHeal(-1);
            }
        }
    }
}
