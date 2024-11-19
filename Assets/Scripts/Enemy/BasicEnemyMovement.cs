using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BasicEnemyMovement : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform target;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        target = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (agent && agent.enabled) agent.SetDestination(target.position);
    }
}
