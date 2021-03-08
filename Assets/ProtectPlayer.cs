using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtectPlayer : MonoBehaviour
{
    Transform player;
    float followDist = 4f;
    float guardDetectRadius = 10f;
    bool isProtecting = false;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(player.position, this.transform.position) > followDist && !isProtecting)
            GetComponent<UnityEngine.AI.NavMeshAgent>().SetDestination(player.position);

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, guardDetectRadius);
        for (int i = 0; i < hitColliders.Length; i++)
        {
            //if a guard is in the area
            if (hitColliders[i].tag == "Guard")
            {
                //If guard is chasing, Attempt to get between the player and guard
                if (hitColliders[i].GetComponent<GuardController>().currentState == GuardController.GuardState.Chase)
                {
                    //find point between the player and guard that's chasing
                    //TODO what about when there's multiple guards chasing?
                    Vector3 guardingPoint = FindGuardPoint(player.position,
                                                            hitColliders[i].transform.position, 
                                                            10f);
                    GetComponent<UnityEngine.AI.NavMeshAgent>().SetDestination(guardingPoint);
                    isProtecting = true;
                }
                else
                {
                    isProtecting = false;
                }
            }
        }
    }

    Vector3 FindGuardPoint(Vector3 A, Vector3 B, float x)
    {
        Vector3 P = x * Vector3.Normalize(B - A) + A;
        return P;
    }
}
