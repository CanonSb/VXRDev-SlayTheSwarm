using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class EnemyMovement : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform target;
    private Animator animator;
    private PlayerHealth hpController;

    [SerializeField]
    private float attackRange = 2f;
    [SerializeField]
    private float idleTimeBeforeAttack = 1f;

    private bool isAttackCycleActive = false;
    private Coroutine attackCycleCoroutine = null;
    private bool isInAttackRange = false;

    private AudioSource goblinHitPlayerAudioSource;
    public GameObject goblinHitPlayerController;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        target = Camera.main.transform; // Sets the target to the main camera's position
        hpController = GameObject.FindWithTag("GameController")?.GetComponent<PlayerHealth>();

        goblinHitPlayerAudioSource = goblinHitPlayerController.GetComponent<AudioSource>();
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
        isInAttackRange = distanceToTarget <= attackRange;

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

            // About how much time it takes for the sword to come down
            yield return new WaitForSeconds(0.8f);
            if (isInAttackRange) 
            {
                hpController.takeDamage();
                goblinHitPlayerAudioSource.Play();
            }
        }
    }

    // Stop attack if this script is disabled
    void OnDisable()
    {
        if (attackCycleCoroutine != null)
        {
            StopCoroutine(attackCycleCoroutine);  // Stop the coroutine
        }
    }
}