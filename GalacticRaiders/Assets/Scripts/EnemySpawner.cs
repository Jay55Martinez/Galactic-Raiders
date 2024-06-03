using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Wave[] waves;
    
    public float waveTransition;
    public float waveDelay;
    private int waveCounter = 0;
    private int activeEnemies = 0;
    private float waveTimer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (waveCounter < waves.Length) { // have all the waves been spawned?
            if ((waveTimer > waveTransition) && (activeEnemies == 0)) {
                SpawnWave();
            }
            waveTimer += Time.deltaTime;
        } else {
            // the room has been completed, unlock door / progress level
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
}
