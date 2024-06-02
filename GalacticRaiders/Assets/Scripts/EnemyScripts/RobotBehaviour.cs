using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotBehaviour : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 10;
    public int damageAmount = 10;
    public float minDistance = 1;
    
    // Start is called before the first frame update
    void Start()
    {
        // find player if missing
        if (player == null) {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // move towards the player. only do this if the game isn't over!
        float step = moveSpeed * Time.deltaTime;
        Vector3 targetPos = new Vector3(player.position.x, transform.position.y, player.position.z);

        transform.LookAt(targetPos);
        float dist = (targetPos-transform.position).magnitude; 
        if (dist > minDistance) {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, step);
        }
    }


    // dealing damage to player
    // private void OnCollisionEnter(Collision other) {
    //     if (other.gameObject.CompareTag("Player")) { 
    //         var playerHealth = other.gameObject.GetComponent<PlayerHealth>();
    //         playerHealth.TakeDamage(damageAmount);  
    //     }
    // }
}
