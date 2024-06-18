using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileBehaviour : MonoBehaviour
{
    
    PlayerHealth playerHealth;
    public int attackDamage = 10;
    public float speed;
    GameObject player;
    Rigidbody rb;
    bool collided = false;

    void Start() {
        player = GameObject.FindWithTag("Player");
        playerHealth = player.GetComponent<PlayerHealth>();
        rb = GetComponent<Rigidbody>();
        
        transform.LookAt(player.transform);
        rb.AddForce(transform.forward * speed, ForceMode.VelocityChange);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player") && !collided) {
            collided = true;
            playerHealth.Damage(attackDamage);
            Destroy(gameObject);
        }
    }
}
