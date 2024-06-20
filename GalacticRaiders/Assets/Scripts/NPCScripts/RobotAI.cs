using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RobotBehaviour : MonoBehaviour
{
    public enum FSMStates {
        Chase, 
        Attack,
        Death
    }

    public FSMStates currentState;

    [Header("Navigation")]
    public float moveSpeed = 10;
    public float attackDist = 1;
    private float distToPlayer;

    [Header("Attack")]
    public int damageAmount = 10;
    public float cooldown = 5;
    private float attackTimer;
    private bool isDead;

    [Header("Health")]
    EnemyHit healthScript;
    int health;

    public GameObject player;
    Animator anim;
    NavMeshAgent agent;
    
    // Start is called before the first frame update
    void Start()
    {
        // find player if missing
        player = GameObject.FindGameObjectWithTag("Player");

        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        healthScript = GetComponent<EnemyHit>();
        health = healthScript.health;

        currentState = FSMStates.Chase;
        isDead = false;
    }

    // Update is called once per frame
    void Update()
    {
        distToPlayer = Vector3.Distance(transform.position, player.transform.position);

        switch(currentState) {
            case FSMStates.Chase:
                UpdateChaseState();
                break;

            case FSMStates.Attack: 
                UpdateAttackState();
                break;

            case FSMStates.Death:
                UpdateDeadState();
                break;
        }

        attackTimer += Time.deltaTime;

        health = healthScript.health;

        if (health <= 0) {
            currentState = FSMStates.Death;
        }
    }

    void UpdateChaseState() {
        anim.SetInteger("animState", 1); // running animation
        agent.SetDestination(player.transform.position);
        agent.stoppingDistance = 0;

        if (distToPlayer <= attackDist) {
            currentState = FSMStates.Attack;
        }

        FaceTarget(player.transform.position);
    }

    void UpdateAttackState() {
        agent.stoppingDistance = attackDist;
        if (distToPlayer > attackDist) {
            currentState = FSMStates.Chase;
        }

        if (attackTimer > cooldown) {
            if (!isDead) {
                anim.SetInteger("animState", 2);
                float animDuration = anim.GetCurrentAnimatorStateInfo(0).length;
                Invoke("Attack", animDuration);
                
                attackTimer = 0;
            }
        }

        FaceTarget(player.transform.position);
    }

    void UpdateDeadState() {
        isDead = true;
        anim.SetInteger("animState", 3);
        Destroy(gameObject, 1);
    }

    // attack the player
    void Attack() {
        attackTimer = 0f; // reset the timer
        if (distToPlayer < attackDist) 
            player.GetComponent<PlayerHealth>().Damage(damageAmount);
    }

    void FaceTarget(Vector3 target) {
        Vector3 directionToTarget = (target - transform.position).normalized;
        directionToTarget.y = 0;
        Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 10 * Time.deltaTime);
    }
}
