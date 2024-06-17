using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHit : MonoBehaviour
{
    [Header ("Health")]
    public int maxHealth;
    public int health;

    [Header("Drops")]
    public GameObject healDrop;
    public GameObject ammoDrop;
    private GameObject drop;

    public AudioClip deathSFX;
    public Slider slider;

    void Awake() {
        slider = GetComponentInChildren<Slider>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (Random.Range(0, 2) == 1)
            drop = healDrop;
        else
            drop = ammoDrop;

        health = maxHealth;
        slider.maxValue = maxHealth;
        slider.value = health;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Die() {
        if (transform.parent.CompareTag("EnemySpawner")) {
            transform.parent.GetComponent<EnemySpawner>().DecrementEnemies(); // decrease # of active 
        }
        Instantiate(drop, transform.position + transform.up*.5f, Quaternion.Euler(new Vector3(-90, 0, 0)));
        AudioSource.PlayClipAtPoint(deathSFX, transform.position);
    }

    public void TakeDamage(int dmg) {
        // only one health, so just die here
        if (health > 0) {
            health -= dmg;
            slider.value = Mathf.Clamp(health, 0, 100);;
        }

        if (health <= 0) {

            Die();
        }
    }
}
