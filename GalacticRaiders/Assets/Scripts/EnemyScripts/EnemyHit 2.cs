using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHit : MonoBehaviour
{
    public GameObject healDrop;
    public GameObject ammoDrop;
    private GameObject drop;
    public float health = 1;
    public AudioClip deathSFX;
    // Start is called before the first frame update
    void Start()
    {
        if (Random.Range(0, 2) == 1)
        {
            drop = healDrop;
        }
        else
        {
            drop = ammoDrop;
        }
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
        Destroy(gameObject);
    }

    public void TakeDamage(int dmg) {
        // only one health, so just die here
        if (health > 0) {
            health -= dmg;
        }

        if (health <= 0) {
            Die();
        }
    }
}
