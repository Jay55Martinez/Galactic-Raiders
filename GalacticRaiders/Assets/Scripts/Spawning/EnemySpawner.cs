using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Wave[] waves;
    
    public float waveTransition;
    public float waveDelay;

    public GameObject progress; // enter door that unlocks / progression item
    public GameObject spawnFXPrefab;

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
        // Debug.Log(activeEnemies);
        if (waveCounter < waves.Length) { // have all the waves been spawned?
            if ((waveTimer > waveTransition) && (activeEnemies == 0) && isSpawning) {
                SpawnWave();
            }
            waveTimer += Time.deltaTime;
        } else {
            if (activeEnemies == 0) {
                if (progress != null) {
                    Progress();
                }
            }
        }
    }

    void SpawnWave() { // spawns one wave, according to the waveCounter
        var wave = waves[waveCounter];
        for(int i = 0; i < wave.enemyPrefabs.Length; i++) {
            GameObject enemy = Instantiate(wave.enemyPrefabs[i], wave.enemyInfo[i].spawnPoint, transform.rotation);
            enemy.SetActive(false);
            if (wave.enemyInfo[i].smartEnemy) { // if NPC, give it wander points
                enemy.GetComponent<EliteAI>().patrolPoints = wave.enemyInfo[i].patrolPoints;
            }
            enemy.transform.parent = gameObject.transform;

            activeEnemies ++;

            Instantiate(spawnFXPrefab, wave.enemyInfo[i].spawnPoint, transform.rotation);
        }
        waveCounter++;
        Debug.Log(waveCounter);

        Invoke("ActivateEnemies", 1f);
    }

    public void DecrementEnemies() { // called in OnDestroy() for enemies
        Debug.Log(activeEnemies);
        if (activeEnemies > 0) {
            activeEnemies--; 
        } 
        
        if (activeEnemies == 0) {
            waveTimer = 0;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            if (progress != null) {
                if (progress.CompareTag("Door")) {
                    progress.GetComponent<DoorBehaviour>().Close();
                }
            }
            isSpawning = true;
            Destroy(GetComponent<BoxCollider>());
        }
    }

    private void Progress() {
        if (progress.CompareTag("Door")) {
            progress.GetComponent<DoorBehaviour>().Open();
        }
        if (progress.CompareTag("ForceField"))
        {
            progress.SetActive(false);
        }
    }

    void ActivateEnemies() {
        foreach (Transform enemy in transform) {
            enemy.gameObject.SetActive(true);
        }
    }
}
