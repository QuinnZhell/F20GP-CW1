using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CrabNavigator : MonoBehaviour 
{
    public NavMeshAgent agent;
    public Transform player = default;
    public Transform centerPoint = default;
    public float minPlayerDistance = 10.0f;
    public float range; 

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    
    void Update()
    {
        Vector3 playerPosition = player.position;
        Vector3 agentPosition = agent.transform.position;

        Vector3 playerDirection = (playerPosition - agentPosition).normalized;
        float playerDistance = Vector3.Distance(playerPosition, agentPosition);

        if(playerDistance <= minPlayerDistance){
            agent.SetDestination(-playerDirection * range); // RUN AWAY
        }
        else if(agent.remainingDistance <= agent.stoppingDistance) 
        {
            Vector3 point;
            if (RandomPoint(centerPoint, range, out point)) 
            {
                agent.SetDestination(point); // GO IN RANDOM DIRECTION
            }
        }

    }
    bool RandomPoint(Transform center, float range, out Vector3 result)
    {

        Vector3 randomPoint = center.position + Random.insideUnitSphere * range; 
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
        { 
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }

    
}