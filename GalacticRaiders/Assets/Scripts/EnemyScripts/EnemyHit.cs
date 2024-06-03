using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHit : MonoBehaviour
{
    public float health = 1;
    public AudioClip deathSFX;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy() {
        if (transform.parent.CompareTag("EnemySpawner")) {
            transform.parent.GetComponent<EnemySpawner>().DecrementEnemies(); // decrease # of active 
        }

        AudioSource.PlayClipAtPoint(deathSFX, transform.position);
    }

    public void TakeDamage(int dmg) {
        // only one health, so just die here
        if (health > 0) {
            health -= dmg;
        }

        if (health <= 0) {
            Destroy(gameObject);
        }
    }
}
