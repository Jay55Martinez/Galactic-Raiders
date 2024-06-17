using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EliteAI : MonoBehaviour
{
    public enum FSMStates {
        Idle, 
        Patrol, 
        Chase, 
        Melee, 
        Fire, 
        Death
    }

    public FSMStates currentState;
    bool isDead;

    [Header("Navigation")]
    public float walkSpeed;
    public float runSpeed;
    public GameObject[] patrolPoints;
    public Transform nextDestination;
    public float meleeDistance;
    public float fireDistance;
    public float meleeChaseDistance;
    private float distToPlayer;
    public float idleTime;
    private float idleTimer;
    private int currentDestinationIndex;

    [Header("Vision")]
    public Transform eyes;
    public float FOV;
    public float viewDistance;
    
    [Header("Attack")]
    public GameObject projectilePrefab;
    public Transform firePoint;
    public int meleeDamage;
    public float meleeTime;
    private float meleeTimer;
    public float fireTime;
    private float fireTimer;
    public AudioClip fireSFX;

    [Header("Health")]
    EnemyHit healthScript;
    int health;

    GameObject player;
    NavMeshAgent agent;
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        healthScript = GetComponent<EnemyHit>();

        currentState = FSMStates.Patrol;

        FindNextPoint();
    }

    // Update is called once per frame
    void Update()
    {
        distToPlayer = Vector3.Distance(transform.position, player.transform.position);

        switch (currentState) {
            case FSMStates.Patrol: 
                UpdatePatrolState();
                break;

            case FSMStates.Chase: 
                UpdateChaseState();
                break;

            case FSMStates.Melee:
                UpdateMeleeState();
                break;

            case FSMStates.Fire: 
                UpdateFireState();
                break;

            case FSMStates.Idle:
                UpdateIdleState();
                break;
            
            case FSMStates.Death:
                UpdateDeadState();
                break;
        }

        fireTimer += Time.deltaTime;
        meleeTimer += Time.deltaTime;
        idleTimer += Time.deltaTime;

        health = healthScript.health;
        if (health <= 0) {
            currentState = FSMStates.Death;
        }
    }

    void UpdatePatrolState() {
        anim.SetInteger("animState", 1);
        agent.SetDestination(nextDestination.position);
        agent.stoppingDistance = 0;
        agent.speed = walkSpeed;

        // wait at each wander point
        if (Vector3.Distance(transform.position, nextDestination.position) < 1) {
            currentState = FSMStates.Idle;
            idleTimer = 0;
        }
        
        // if player detected, switch to chase state
        if (CanSeePlayer()) {
            currentState = FSMStates.Chase;
        }

        FaceTarget(nextDestination.position);
        agent.SetDestination(nextDestination.position);
    }

    void UpdateChaseState() {
        anim.SetInteger("animState", 2);
        agent.speed = runSpeed;
        nextDestination = player.transform;
        agent.stoppingDistance = 0;

        // if the player is further than fire distance, chase them
        // if the player is further than melee chase distance but within fire distance, shoot
        // if the player is closer than melee chase distance, chase them until melee
        if (distToPlayer >= meleeChaseDistance && distToPlayer < fireDistance) {
            currentState = FSMStates.Fire;
        } else if (distToPlayer >= viewDistance) { // player no longer detected
            currentState = FSMStates.Patrol;
            FindNextPoint();
        } else if (distToPlayer >= meleeDistance && distToPlayer < meleeChaseDistance) {
            currentState = FSMStates.Chase;
        } else if (distToPlayer < meleeDistance) {
            currentState = FSMStates.Melee;
        }

        FaceTarget(nextDestination.position);
        agent.SetDestination(nextDestination.position);
    }

    void UpdateIdleState() {
        anim.SetInteger("animState", 0);
        agent.speed = 0;

        if (idleTimer >= idleTime) {
            currentState = FSMStates.Patrol;
            FindNextPoint();
        }

        if (CanSeePlayer()) {
            currentState = FSMStates.Chase;
        }
    }

    void UpdateMeleeState() {
        anim.SetInteger("animState", 3);
        agent.speed = 0;
        agent.stoppingDistance = meleeDistance;
        
        nextDestination = player.transform;

        if (distToPlayer <= meleeDistance) {
            currentState = FSMStates.Melee;
        } else if (distToPlayer > meleeDistance) {
            currentState = FSMStates.Chase;
        } else if (distToPlayer > meleeChaseDistance) {
            currentState = FSMStates.Fire;
        }

        FaceTarget(nextDestination.position);

        if (meleeTimer >= meleeTime) {
            if (!isDead) {
                var animDuration = anim.GetCurrentAnimatorStateInfo(0).length;
                meleeTime = animDuration;
                Invoke("Melee", animDuration);
                meleeTimer = 0;
            }
        }
    }

    void UpdateFireState() {
        anim.SetInteger("animState", 4);
        agent.stoppingDistance = fireDistance;

        nextDestination = player.transform;

        if (distToPlayer < meleeChaseDistance) {
            currentState = FSMStates.Chase;
        } else if (distToPlayer >= fireDistance) {
            currentState = FSMStates.Chase;
        }

        FaceTarget(nextDestination.position);

        if (fireTimer >= fireTime) {
            if (!isDead) {
                var animDuration = anim.GetCurrentAnimatorStateInfo(0).length;
                fireTime = animDuration;
                Fire();
                fireTimer = 0;
            }
        }
    }

    void UpdateDeadState() {
        isDead = true;
        anim.SetInteger("animState", 5);
        Destroy(gameObject, 2);
    }

    void Melee() {
        player.GetComponent<PlayerHealth>().Damage(meleeDamage);
    }

    void Fire() {
        Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        AudioSource.PlayClipAtPoint(fireSFX, firePoint.position);
    }

    void FaceTarget(Vector3 target) {
        Vector3 directionToTarget = (target - transform.position).normalized;
        directionToTarget.y = 0;
        Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 10 * Time.deltaTime);
    }

    void FindNextPoint() {
        nextDestination = patrolPoints[currentDestinationIndex].transform;
        currentDestinationIndex = (currentDestinationIndex + 1) % patrolPoints.Length;
    }

    bool CanSeePlayer() {
        RaycastHit hit;
        Vector3 directionVector = player.transform.position - eyes.position;
        if (Vector3.Angle(directionVector, eyes.forward) <= FOV) {
            if (Physics.Raycast(eyes.position, directionVector, out hit, viewDistance)) {
                if (hit.collider.CompareTag("Player")) {
                    return true;
                } else {
                    return false;
                }
            } else {
                return false;
            }
        } else {
            return false;
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, meleeDistance);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, meleeChaseDistance);

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, fireDistance);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, viewDistance);

        Vector3 frontRayPoint = eyes.position + (eyes.forward*viewDistance);
        Vector3 leftRayPoint = Quaternion.Euler(0, FOV * 0.5f, 0) * frontRayPoint;
        Vector3 rightRayPoint = Quaternion.Euler(0, -FOV * 0.5f, 0) * frontRayPoint;   

        Debug.DrawLine(eyes.position, frontRayPoint, Color.cyan);
        Debug.DrawLine(eyes.position, leftRayPoint, Color.yellow);
        Debug.DrawLine(eyes.position, rightRayPoint, Color.yellow);     
    }
}
