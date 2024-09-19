using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimePatrol : StateMachineBehaviour
{
    public float patrolSpeed = 5f;
    public float chaseSpeed = 7f;
    public float returnSpeed = 4f;
    
    private Rigidbody2D rb;
    private Transform player;
    private Vector2 spawnPosition;
    private Vector2 patrolTarget;
    private SlimeController slimeController;
    
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rb = animator.GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        slimeController = animator.GetComponent<SlimeController>();
        spawnPosition = slimeController.startPosition;
        patrolTarget = GetNewPatrolPoint();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.GetBool("IsChasing"))
        {
            MoveTowards(player.position, chaseSpeed); // Chase player
        }
        else if (animator.GetBool("IsReturning"))
        {
            MoveTowards(spawnPosition, returnSpeed); // Return to spawn
        }
        else if (animator.GetBool("IsPatrolling"))
        {
            if (Vector2.Distance(rb.position, patrolTarget) < 0.5f)
            {
                patrolTarget = GetNewPatrolPoint();  // Get new patrol target
            }
            MoveTowards(patrolTarget, patrolSpeed); // Patrol
        }
    }

    private void MoveTowards(Vector2 target, float speed)
    {
        Vector2 newPosition = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
        rb.MovePosition(newPosition);
    }

    private Vector2 GetNewPatrolPoint()
    {
        // Generate a random point within patrol radius
        return spawnPosition + Random.insideUnitCircle.normalized * 5f;
    }

}
