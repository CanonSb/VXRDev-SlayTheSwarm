using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform target;
    private Animator animator;

    [SerializeField]
    private float attackRange = 2f;
    [SerializeField]
    private float idleTimeBeforeAttack = 1f;

    private bool isAttackCycleActive = false;
    private Coroutine attackCycleCoroutine = null;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        target = Camera.main.transform; // Sets the target to the main camera's position
    }

    void Update()
    {
        if (agent && agent.enabled)
        {
            // Move the agent toward the target
            agent.SetDestination(target.position);

            // Update animation parameters
            UpdateAnimator();
        }
    }

    void UpdateAnimator()
    {
        if (!animator) return;
        // Check if the agent is moving
        bool isRunning = agent.velocity.magnitude > 0.8f && agent.remainingDistance > agent.radius;
        animator.SetBool("isRunning", isRunning);

        // Check if within attack range
        float distanceToTarget = Vector3.Distance(transform.position, target.position);
        bool isInAttackRange = distanceToTarget <= attackRange;

        // Handle attack cycle
        if (isInAttackRange && !isRunning)
        {
            // Start attack cycle if not already active
            if (!isAttackCycleActive)
            {
                if (attackCycleCoroutine != null)
                {
                    StopCoroutine(attackCycleCoroutine);
                }
                attackCycleCoroutine = StartCoroutine(AttackCycle());
            }
        }
        else
        {
            // Stop attack cycle if out of range or moving
            if (isAttackCycleActive)
            {
                if (attackCycleCoroutine != null)
                {
                    StopCoroutine(attackCycleCoroutine);
                    attackCycleCoroutine = null;
                }
                isAttackCycleActive = false;
                animator.SetBool("isAttacking", false);
            }
        }
    }

    IEnumerator AttackCycle()
    {
        
        isAttackCycleActive = true;

        while (true)
        {
            // Wait in idle state
            yield return new WaitForSeconds(idleTimeBeforeAttack);

            // Perform attack
            animator.SetBool("isAttacking", true);

            // Immediately reset attack state
            yield return new WaitForEndOfFrame();
            animator.SetBool("isAttacking", false);
        }
    }
}