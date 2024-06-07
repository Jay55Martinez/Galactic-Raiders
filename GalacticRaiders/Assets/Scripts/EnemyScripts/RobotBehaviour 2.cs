using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotBehaviour : MonoBehaviour
{
    public GameObject player;
    public float moveSpeed = 10;
    public int damageAmount = 10;
    public float minDistance = 1;
    public float cooldown = 5;
    private float attackTimer;
    
    // Start is called before the first frame update
    void Start()
    {
        // find player if missing
        if (player == null) {
            player = GameObject.FindGameObjectWithTag("Player");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // move towards the player. only do this if the game isn't over!
        float step = moveSpeed * Time.deltaTime;
        Vector3 targetPos = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);

        transform.LookAt(targetPos);
        float dist = (targetPos-transform.position).magnitude; 
        if (dist > minDistance) {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, step);
        } else {
            if (attackTimer > cooldown) {
                Attack();
            }
        }
        attackTimer += Time.deltaTime;
    }

    // attack the player
    void Attack() {
        attackTimer = 0f; // reset the timer
        // insert player health script stuff here
    }

    // dealing damage to player
    // private void OnCollisionEnter(Collision other) {
    //     if (other.gameObject.CompareTag("Player")) { 
    //         var playerHealth = other.gameObject.GetComponent<PlayerHealth>();
    //         playerHealth.TakeDamage(damageAmount);  
    //     }
    // }
}
