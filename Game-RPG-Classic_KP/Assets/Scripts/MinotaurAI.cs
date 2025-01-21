using UnityEngine;

public class MinotaurAI : MonoBehaviour
{
    [Header("Movement Settings")]
    public float roamSpeed = 2f;
    public float chaseSpeed = 4f;
    public float detectionRange = 10f;
    public float attackRange = 2f;

    [Header("Combat Settings")]
    public float knockbackForce = 5f;
    private int maxHealth;
    public float attackCooldownTime = 2f;
    public int attackDamage = 20;

    [Header("References")]
    public Animator animator;
    public Rigidbody2D rb;
    public Collider2D bossArea;
    public LayerMask attackMask;
    public Vector3 attackOffset;
    private Knockback knockback;

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
        InitializeVariables();
    }

    void Update()
    {
        HandleState();
    }

    // =========================
    // Initialization Functions
    // =========================

    private void InitializeVariables()
    {
        maxHealth = GetComponent<EnemyHealthBoss>().health;
        currentHealth = maxHealth;
        roamTimer = Random.Range(2f, 5f);
        PickNewRoamDirection();
        attackPatternIndex = 0;
        isPaused = false;
        pauseTimer = 0f;
        isFacingRight = true;
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        knockback = GetComponent<Knockback>();
    }

    // =========================
    // State Management
    // =========================

    private void HandleState()
    {
        if (!IsPlayerInBossArea())
        {
            ResetToRoamState();
            return;
        }

        float distanceToPlayer = GetDistanceToPlayer();

        if (distanceToPlayer <= attackRange)
        {
            rb.velocity = Vector2.zero; // Pastikan Minotaur berhenti
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
                PauseBeforeRoaming();
            else
                Roam();
        }

        if (attackCooldown > 0f)
            attackCooldown -= Time.deltaTime;
    }

    private void ResetToRoamState()
    {
        rb.velocity = Vector2.zero;
        animator.ResetTrigger("attack");
        animator.ResetTrigger("attack_sweep");
        if (isPaused)
            PauseBeforeRoaming();
        else
            Roam();
    }

    // =========================
    // Movement Handling
    // =========================

    private void Roam()
    {
        animator.SetBool("isWalking", true);
        Vector2 newPosition = rb.position + roamDirection * roamSpeed * Time.fixedDeltaTime;

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
            pauseTimer = Random.Range(1f, 3f);
        }
    }

    private void PauseBeforeRoaming()
    {
        pauseTimer -= Time.deltaTime;
        if (pauseTimer <= 0f)
        {
            isPaused = false;
            roamTimer = Random.Range(2f, 5f);
            PickNewRoamDirection();
        }
    }

    private void PickNewRoamDirection()
    {
        roamDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }

    private void ChasePlayer()
    {
        if (!player || !IsPlayerInBossArea()) return;

        Vector2 direction = new Vector2(player.position.x,player.position.y);
        Vector2 direction2 = (player.position - transform.position).normalized;
        animator.SetBool("isWalking", true);
        Vector2 newPos = Vector2.MoveTowards(rb.position, direction, chaseSpeed * Time.fixedDeltaTime);
		rb.MovePosition(newPos);
        FaceDirection(direction2.x);
    }

    private void FaceDirection(float directionX)
    {
        if ((directionX > 0 && !isFacingRight) || (directionX < 0 && isFacingRight))
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1;
            transform.localScale = localScale;
        }
    }

    // =========================
    // Combat Handling
    // =========================

    private void Attack()
    {
        animator.SetBool("isWalking", false);
        rb.velocity = Vector2.zero;

        if (!player || !IsPlayerInBossArea() || GetDistanceToPlayer() > attackRange)
        {
            animator.ResetTrigger("attack");
            animator.ResetTrigger("attack_sweep");
            return;
        }

        if (currentHealth < maxHealth / 2)
        {
            HandleLowHealthAttackPattern();
        }
        else
        {
            animator.SetTrigger("attack");
        }

        attackCooldown = attackCooldownTime;
    }

    public void hitPlayer()
    {
        if (player == null) return;

        // Cek apakah player berada di dalam jangkauan serang
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (distanceToPlayer <= attackRange)
        {
            // Ambil komponen PlayerHealth dari player
            PlayerStat playerStat = player.GetComponent<PlayerStat>();
            Collider2D pemain = player.GetComponent<Collider2D>();

            if (playerStat != null)
            {
                playerStat.TakeDamage(attackDamage);
                ApplyKnockback(pemain);
            }
        }
    }

    private void HandleLowHealthAttackPattern()
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

    private void ApplyKnockback(Collider2D player)
    {
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Vector2 knockbackDirection = (player.transform.position - transform.position).normalized;
            player.GetComponent<PlayerController>().ApplyKnockback(knockbackDirection, knockbackForce, 0.05f);
        }
    }

    // =========================
    // Utility Functions
    // =========================

    private bool IsPlayerInBossArea()
    {
        return player && bossArea.OverlapPoint(player.position);
    }

    private float GetDistanceToPlayer()
    {
        return player ? Vector2.Distance(transform.position, player.position) : Mathf.Infinity;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        // Gizmos.color = Color.blue;
        // Gizmos.DrawWireSphere(transform.position, attackOffset);

        if (bossArea)
        {
            Bounds bounds = bossArea.bounds;
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(bounds.center, bounds.size);
        }
    }
}
