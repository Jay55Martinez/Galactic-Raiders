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

    public string[] voicelines;
    public Text textBox;
    public int textIndex;

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
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(Vector3.Distance(transform.position, nextDestination.position));
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

        if (Vector3.Distance(transform.position, nextDestination.position) < agent.stoppingDistance) {
            FindNextPoint();
        }

        if (distToPlayer < chaseDistance) {
            currentState = FSMStates.Chase;
        }

        textBox.gameObject.SetActive(false);
    }

    void UpdateChaseState() {
        anim.SetInteger("animState", 2);
        agent.SetDestination(player.transform.position);
        agent.stoppingDistance = talkDistance - 0.01f;

        if (distToPlayer < talkDistance) {
            currentState = FSMStates.Talk;
            InvokeRepeating("NextVoiceLine", 2f, 2f);
        } else if (distToPlayer > chaseDistance) {
            currentState = FSMStates.Patrol;
            FindNextPoint();
        }

        textBox.gameObject.SetActive(false);
    }

    void UpdateTalkState() {
        anim.SetInteger("animState", 1);
        textBox.gameObject.SetActive(true);
        textBox.text = voicelines[textIndex];

        if (distToPlayer > talkDistance) {
            currentState = FSMStates.Chase;
            textIndex = 0;
        }
    }

    void FindNextPoint() {
        nextDestination = wanderPoints[currentDestinationIndex].transform;
        currentDestinationIndex = (currentDestinationIndex + 1) % wanderPoints.Length;
    }

    void NextVoiceLine() {
        if (textIndex < voicelines.Length - 1) {
            textIndex++;
        } 
    }
}
