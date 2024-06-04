using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Wave[] waves;
    
    public float waveTransition;
    public float waveDelay;

    public GameObject progress; // enter door that unlocks / progression item

    private int waveCounter = 0;
    private int activeEnemies = 0;
    private float waveTimer;
    private bool isSpawning;

    // Start is called before the first frame update
    void Start()
    {
        isSpawning = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (waveCounter < waves.Length) { // have all the waves been spawned?
            if ((waveTimer > waveTransition) && (activeEnemies == 0) && isSpawning) {
                SpawnWave();
            }
            waveTimer += Time.deltaTime;
        } else {
            if (activeEnemies == 0) {
                if (progress.CompareTag("Door")) {
                    progress.GetComponent<DoorBehaviour>().Open();
                }
            }
        }
    }

    void SpawnWave() { // spawns one wave, according to the waveCounter
        var wave = waves[waveCounter];
        for(int i = 0; i < wave.enemyPrefabs.Length; i++) {
            GameObject enemy = Instantiate(wave.enemyPrefabs[i], wave.locations[i], transform.rotation);
            enemy.transform.parent = gameObject.transform;
            
            activeEnemies ++;
        }
        waveCounter++;
    }

    public void DecrementEnemies() { // called in OnDestroy() for enemies
        if (activeEnemies > 0) {
            activeEnemies--; 
        } 
        
        if (activeEnemies == 0) {
            waveTimer = 0;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            if (progress.CompareTag("Door")) {
                progress.GetComponent<DoorBehaviour>().Close();
            }
            isSpawning = true;
        }
    }
}
