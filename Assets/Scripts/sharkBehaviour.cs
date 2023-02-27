using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class sharkBehaviour : MonoBehaviour
{

    public NavMeshAgent agent;
    public Transform player;

    public LayerMask playerMask;

    private float distanceToPlayer;

    private float defaultBaseOffset;
    private float defaultSpeed;

    // patrol
    public float patrolRange = 20.0f;

    // chase
    public float minSpottedDistance = 15.0f;
    public bool playerSpotted = false;
    public float chaseSpeed = 6.0f;

    // attack
    public float minAttackDistance = 5.0f;
    public bool attackTriggered = false;

    // evade
    public bool evadeTriggered = false;
    public float evadeDistanceMultiplier = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        defaultBaseOffset = agent.baseOffset;
        defaultSpeed = agent.speed;

        

        distanceToPlayer = Vector3.Distance(player.position, agent.transform.position);
    }

    // Update is called once per frame
    void Update()
    {

        distanceToPlayer = Vector3.Distance(player.position, agent.transform.position);

        if(distanceToPlayer > minSpottedDistance * evadeDistanceMultiplier) {
            evadeTriggered = false;
            attackTriggered = false;
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
        agent.SetDestination(player.position);
    }

    void Attack()
    {
        Debug.Log("ATTACK");

        attackTriggered = true;
        Evade();
    }

    void Evade()
    {
        Debug.Log("EVADE");

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
