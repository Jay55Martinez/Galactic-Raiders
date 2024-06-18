using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class CaptainAI : MonoBehaviour
{
    public enum FSMStates {
        Patrol, // going through wander points
        Chase, // following the player
        Talk // talking to the player
    }

    public FSMStates currentState;

    [Header("Voicelines")]
    public Dialogue[] dialogues;
    // public Text textBox;
    // public int textIndex;
    NPCDialogue dia;

    [Header("Navigation")]
    public GameObject[] wanderPoints;
    public Transform nextDestination;
    public float chaseDistance = 5;
    public float talkDistance = 3;
    private int currentDestinationIndex = 0;
    private float distToPlayer;

    public GameObject player;

    NavMeshAgent agent;
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        if (player == null) {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        FindNextPoint();

        currentState = FSMStates.Patrol;

        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        dia = GetComponent<NPCDialogue>();

        dia.setDialogue(dialogues[0]);
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

            case FSMStates.Talk: 
                UpdateTalkState();
                break;
        }
    }

    void UpdatePatrolState() {
        anim.SetInteger("animState", 2);
        agent.SetDestination(nextDestination.position);
        agent.stoppingDistance = 0;

        if (Vector3.Distance(transform.position, nextDestination.position) < 1) {
            FindNextPoint();
        }

        if (distToPlayer < chaseDistance) {
            currentState = FSMStates.Chase;
        }
    }

    void UpdateChaseState() {
        anim.SetInteger("animState", 2);
        agent.SetDestination(player.transform.position);
        agent.stoppingDistance = 0;  

        if (distToPlayer < talkDistance) {
            currentState = FSMStates.Talk;
            dia.ResetDialogue();
            dia.StartTalking();
        } else if (distToPlayer > chaseDistance) {
            currentState = FSMStates.Patrol;
            FindNextPoint();
        }

        FaceTarget(player.transform.position);
    }

    void UpdateTalkState() {
        anim.SetInteger("animState", 1);
        agent.stoppingDistance = talkDistance;

        if (distToPlayer > talkDistance) {
            currentState = FSMStates.Chase;
            dia.StopTalking();
        }

        FaceTarget(player.transform.position);
    }

    void FindNextPoint() {
        nextDestination = wanderPoints[currentDestinationIndex].transform;
        currentDestinationIndex = (currentDestinationIndex + 1) % wanderPoints.Length;
    }

    void FaceTarget(Vector3 target) {
        Vector3 directionToTarget = (target - transform.position).normalized;
        directionToTarget.y = 0;
        Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 10 * Time.deltaTime);
    }
}
