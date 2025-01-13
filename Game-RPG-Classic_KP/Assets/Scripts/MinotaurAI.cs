using UnityEngine;

public class MinotaurAI : MonoBehaviour
{
    public float roamSpeed = 2f;
    public float chaseSpeed = 4f;
    public float detectionRange = 10f;
    public float attackRange = 2f;
    public float knockbackForce = 5f;
    public int maxHealth = 100;

    public Animator animator;
    public Rigidbody2D rb;
    public Collider2D bossArea; // Reference to the collider defining the boss area

    private Vector2 roamDirection;
    private Transform player;
    private int currentHealth;
    private float roamTimer;
    private float pauseTimer;
    private float attackCooldown;
    private int attackPatternIndex;

    private bool isPaused;
    private bool isKnockedBack;
    private bool isFacingRight;

    void Start()
    {
        currentHealth = maxHealth;
        roamTimer = Random.Range(2f, 5f);
        PickNewRoamDirection();
        attackPatternIndex = 0;
        isPaused = false;
        pauseTimer = 0f;

        // Assume initial facing direction is right
        isFacingRight = true;

        // Find the player
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        if (currentHealth <= 0)
        {
            Die();
            return;
        }

        if (!IsPlayerInBossArea())
        {
            if (isPaused)
            {
                PauseBeforeRoaming();
            }
            else
            {
                Roam();
            }
            return;
        }

        float distanceToPlayer = player ? Vector2.Distance(transform.position, player.position) : Mathf.Infinity;

        if (distanceToPlayer <= attackRange)
        {
            if (attackCooldown <= 0f)
            {
                Attack();
            }
        }
        else if (distanceToPlayer <= detectionRange)
        {
            ChasePlayer();
        }
        else
        {
            if (isPaused)
            {
                PauseBeforeRoaming();
            }
            else
            {
                Roam();
            }
        }

        if (attackCooldown > 0f)
        {
            attackCooldown -= Time.deltaTime;
        }
    }

    void Roam()
    {
        animator.SetBool("isWalking", true);
        Vector2 newPosition = rb.position + roamDirection * roamSpeed * Time.deltaTime;

        // Check if the new position is inside the boss area
        if (bossArea.OverlapPoint(newPosition))
        {
            rb.MovePosition(newPosition);
            FaceDirection(roamDirection.x);
        }
        else
        {
            PickNewRoamDirection();
        }

        roamTimer -= Time.deltaTime;
        if (roamTimer <= 0f)
        {
            rb.velocity = Vector2.zero;
            animator.SetBool("isWalking", false);
            isPaused = true;
            pauseTimer = Random.Range(1f, 3f); // Durasi berhenti sebelum roaming lagi
        }
    }

    void PauseBeforeRoaming()
    {
        pauseTimer -= Time.deltaTime;
        if (pauseTimer <= 0f)
        {
            isPaused = false;
            roamTimer = Random.Range(2f, 5f);
            PickNewRoamDirection();
        }
    }

    void PickNewRoamDirection()
    {
        roamDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }

    void ChasePlayer()
    {
        if (!player) return;

        Vector2 direction = (player.position - transform.position).normalized;
        animator.SetBool("isWalking", true);
        rb.velocity = direction * chaseSpeed;
        FaceDirection(direction.x);
    }

    void Attack()
    {
        animator.SetBool("isWalking", false);
        rb.velocity = Vector2.zero;

        if (currentHealth < maxHealth / 2)
        {
            if (isKnockedBack)
            {
                isKnockedBack = false;
                animator.SetTrigger("attack_sweep");
            }
            else
            {
                if (attackPatternIndex == 0 || attackPatternIndex == 3)
                {
                    animator.SetTrigger("attack_sweep");
                }
                else
                {
                    animator.SetTrigger("attack");
                }
                attackPatternIndex = (attackPatternIndex + 1) % 4;
            }
        }
        else
        {
            animator.SetTrigger("attack");
        }

        attackCooldown = 2f; // Adjust cooldown as needed
    }

    void FaceDirection(float directionX)
    {
        if ((directionX > 0 && !isFacingRight) || (directionX < 0 && isFacingRight))
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1;
            transform.localScale = localScale;
        }
    }

    bool IsPlayerInBossArea()
    {
        return player && bossArea.OverlapPoint(player.position);
    }

    public void TakeDamage(int damage, Vector2 knockbackDirection)
    {
        currentHealth -= damage;

        if (currentHealth <= maxHealth / 2 && !isKnockedBack)
        {
            isKnockedBack = true;
            rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
        }
    }

    void Die()
    {
        animator.SetTrigger("die");
        rb.velocity = Vector2.zero;
        enabled = false; // Disable the script
    }

    void OnDrawGizmosSelected()
    {
        // Draw detection range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        // Draw attack range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        // Draw boss area bounds
        if (bossArea)
        {
            Bounds bounds = bossArea.bounds;
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(bounds.center, bounds.size);
        }
    }

}
