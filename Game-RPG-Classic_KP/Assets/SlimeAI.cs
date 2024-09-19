using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeAI : MonoBehaviour
{
    public float detectionRange = 5f;
    public float attackRange = 1.5f;
    private Transform player;
    private Animator animator;
    private Vector2 startPosition;
    private bool returningToSpawn;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        startPosition = transform.position;
        returningToSpawn = false;
    }

    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            // Player within attack range
            animator.SetTrigger("Attack");  // Misalnya, menggunakan trigger untuk menyerang
        }
        else if (distanceToPlayer <= detectionRange)
        {
            // Player detected, slime should chase
            animator.SetBool("IsChasing", true);
            animator.SetBool("IsReturning", false);
            animator.SetBool("IsPatrolling", false);
            returningToSpawn = false;
        }
        else if (returningToSpawn)
        {
            // Player lost, slime should return to spawn
            animator.SetBool("IsChasing", false);
            animator.SetBool("IsReturning", true);
            animator.SetBool("IsPatrolling", false);
        }
        else
        {
            // No player detected, slime should patrol
            animator.SetBool("IsChasing", false);
            animator.SetBool("IsReturning", false);
            animator.SetBool("IsPatrolling", true);
        }
        
        // If player was detected but now out of range, return to spawn
        if (!returningToSpawn && distanceToPlayer > detectionRange && animator.GetBool("IsChasing"))
        {
            returningToSpawn = true;
        }

        // Check if slime has reached spawn point when returning
        if (returningToSpawn && Vector2.Distance(transform.position, startPosition) < 0.5f)
        {
            returningToSpawn = false;  // Stop returning when reaching spawn
        }
    }

    
}
