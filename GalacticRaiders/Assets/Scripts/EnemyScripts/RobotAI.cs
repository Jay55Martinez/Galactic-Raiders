using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RobotBehaviour : MonoBehaviour
{
    public enum FSMStates {
        Chase, 
        Attack
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

    public GameObject player;
    Animator anim;
    NavMeshAgent agent;
    
    // Start is called before the first frame update
    void Start()
    {
        // find player if missing
        if (player == null) {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
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
        }

        attackTimer += Time.deltaTime;
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
            anim.SetInteger("animState", 2);
            float animDuration = anim.GetCurrentAnimatorStateInfo(0).length;
            Invoke("Attack", animDuration);
            
            attackTimer = 0;
        }

        FaceTarget(player.transform.position);
    }

    // attack the player
    void Attack() {
        attackTimer = 0f; // reset the timer
        player.GetComponent<PlayerHealth>().Damage(damageAmount);
    }

    void FaceTarget(Vector3 target) {
        Vector3 directionToTarget = (target - transform.position).normalized;
        directionToTarget.y = 0;
        Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 10 * Time.deltaTime);
    }
}
