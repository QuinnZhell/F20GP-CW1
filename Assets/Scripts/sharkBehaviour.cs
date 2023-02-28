using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class sharkBehaviour : MonoBehaviour
{

    public NavMeshAgent agent;
    public Transform player;

    public LayerMask playerMask;
    public GameManager gameManager;

    private float distanceToPlayer;

    private float defaultBaseOffset;
    private float defaultSpeed;

    // patrol
    public float patrolRange = 20.0f;

    // chase
    public float minSpottedDistance = 15.0f;
    public bool playerSpotted = false;
    public float chaseSpeed = 7.0f;

    // attack
    public float minAttackDistance = 8.0f;
    public bool attackTriggered = false;

    // evade
    public bool evadeTriggered = false;
    public float evadeDistanceMultiplier = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        defaultBaseOffset = agent.baseOffset;
        defaultSpeed = agent.speed;

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        distanceToPlayer = Vector3.Distance(player.position, agent.transform.position);
    }

    // Update is called once per frame
    void Update()
    {

        distanceToPlayer = Vector3.Distance(player.position, agent.transform.position);

        // remove triggers when far away
        if(distanceToPlayer > minSpottedDistance * evadeDistanceMultiplier) {
            evadeTriggered = false;
            attackTriggered = false;
        }
        // this allows the shark to continue evading if for some reason the player is chasing
        else if(evadeTriggered) {
            Evade();
        }

        playerSpotted = Physics.CheckSphere(transform.position, minSpottedDistance, playerMask);
        attackTriggered = Physics.CheckSphere(transform.position, minAttackDistance, playerMask);

        if(!playerSpotted && !attackTriggered && !evadeTriggered) {
            Patrol();
        }
        else if(playerSpotted && !attackTriggered && !evadeTriggered) {
            Chase();
        }
        else {
            if(!evadeTriggered) Attack();
        }
    }

    void Patrol()
    {
        Debug.Log("PATROL");

        if(agent.transform.position.y > defaultBaseOffset + 1) {
            agent.baseOffset -= 0.001f;
        }
        else if (agent.transform.position.y < defaultBaseOffset + 1) {
            agent.baseOffset += 0.001f;
        }

        agent.speed = defaultSpeed;
        Vector3 agentPosition = agent.transform.position;

        Vector3 playerDirection = (player.position - agentPosition).normalized;
        float playerDistance = Vector3.Distance(player.position, agentPosition);

        if(agent.remainingDistance <= agent.stoppingDistance) 
        {
            Vector3 point;
            if (RandomPoint(patrolRange, out point)) 
            {
                agent.SetDestination(point); 
            }
        }
    }

    void Chase()
    {
        Debug.Log("CHASE");

        agent.speed = chaseSpeed;

        if(Mathf.Abs(player.position.y - agent.transform.position.y) > 0.5f){
            if(player.position.y > agent.transform.position.y) {
                agent.baseOffset += 0.001f;
            }
            else if (player.position.y < agent.transform.position.y) {
                agent.baseOffset -= 0.001f;
            }
        }

        agent.SetDestination(player.position);
    }

    void Attack()
    {
        Debug.Log("ATTACK");

        agent.speed = 0;
        attackTriggered = true;
        gameManager.applyDamage(34.0f);

        Evade();
    }

    void Evade()
    {
        Debug.Log("EVADE");

        agent.speed = chaseSpeed;

        attackTriggered = false;
        evadeTriggered = true;

        Vector3 agentPosition = agent.transform.position;
        Vector3 playerDirection = (player.position - agentPosition).normalized;
        
        agent.SetDestination(-playerDirection * minSpottedDistance * evadeDistanceMultiplier);
    }

    bool RandomPoint(float range, out Vector3 result)
    {

        Vector3 randomPoint = Random.insideUnitSphere * range; 
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
        { 
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }

    private void OnDrawGizmos()
    {

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, minSpottedDistance);
        Gizmos.DrawSphere(agent.destination, 0.5f);
    }
}
