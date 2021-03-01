using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AgentWalkRandomizer : MonoBehaviour
{
    public float Range = 25.0f;
    NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (agent.pathPending || agent.remainingDistance > 0.1f)
            return;

        agent.destination = Range * Random.insideUnitCircle;
    }
}
