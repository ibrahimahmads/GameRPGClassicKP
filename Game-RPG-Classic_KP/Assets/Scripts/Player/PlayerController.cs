using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 1f;
    private PlayerControls playerControls;
    private SpriteRenderer spriteRenderer;
    private Vector2 movement;
    private Rigidbody2D rb;
    private Animator animator;
    public float attackRange;
    public LayerMask enemyLayers;
    private PlayerStat playerStat;
    public static PlayerController Instance;

    public GameObject attackPointRight; 
    public GameObject attackPointLeft; 
    public GameObject attackPointUp; 
    public GameObject attackPointDown;
    
    public float attackCooldown = 0.1f; // Durasi cooldown
    private float lastAttackTime = 0f;
    private AudioManager audioManager;
    
    private bool isPaused = false;
    private bool isPlayingMoveSFX = false;
    private Vector2 lastDirection = Vector2.zero;
    // knockback
    private bool isKnockbacked = false;
    private Vector2 knockbackDirection;
    //private float knockbackDuration = 0.2f; // Lama waktu knockback
    private float knockbackTimer = 2f;
    private void Awake()
    {
        Instance = this;
        GameObject audioObject = GameObject.FindGameObjectWithTag("Audio");
        if (audioObject != null)
        {
            audioManager = audioObject.GetComponent<AudioManager>();
        }
        else
        {
            Debug.LogError("Object dengan tag 'Audio' tidak ditemukan!");
        }
        playerStat = GetComponent<PlayerStat>();
        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        attackPointRight.SetActive(false);
        attackPointLeft.SetActive(false);
        attackPointUp.SetActive(false);
        attackPointDown.SetActive(false);
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        // Nonaktifkan input saat objek dihancurkan atau menjadi tidak aktif
        playerControls.Movement.Disable();
    }

    private void Update()
    {
        if (!isPaused) // Cek jika permainan tidak sedang pause
        {
            PlayerInput();
            Animation();
            Attack();
        }
    }

    private void FixedUpdate()
    {
        if (!isPaused) // Cek jika permainan tidak sedang pause
        {
            move();
        }
        if (spriteRenderer != null)
        {
            spriteRenderer.sortingOrder = Mathf.RoundToInt(-transform.position.y * 100);
        }
    }

    private void PlayerInput()
    {
        movement = playerControls.Movement.Move.ReadValue<Vector2>();
    }

    private void move()
    {
        if (isKnockbacked)
        {
            // Gerakan saat knockback
            rb.MovePosition(rb.position + knockbackDirection * (moveSpeed * Time.fixedDeltaTime));
            knockbackTimer -= Time.fixedDeltaTime;
            if (knockbackTimer <= 0)
            {
                isKnockbacked = false;
            }
        }
        else
        {
            // Gerakan normal
            rb.MovePosition(rb.position + movement * (moveSpeed * Time.fixedDeltaTime));
        }
    }
    
    public void SetPauseState(bool paused)
    {
        isPaused = paused; // Set status pause
    }

    private void Animation()
    {
        animator.SetFloat("MoveX", movement.x);
        animator.SetFloat("MoveY", movement.y);

        if (movement != Vector2.zero)
        {
            animator.SetBool("IsMoving", true);
            if(!isPlayingMoveSFX)
            {
                isPlayingMoveSFX = true;     
                audioManager.PlayLoop(0);
            }
        }
        else
        {
            animator.SetBool("IsMoving", false);
            if(isPlayingMoveSFX)
            {
                isPlayingMoveSFX = false;
                audioManager.StopSFX();
            }
           
        }
    }

    private void Attack()
    {
        if (Input.GetButtonDown("Fire1") && Time.time >= lastAttackTime + attackCooldown) // Tombol serangan, misalnya "Fire1"
        {
            lastAttackTime = Time.time;
            
            
            
            

            // Tentukan arah serangan
            if (movement.x > 0) // Kanan
            {
                attackPointRight.SetActive(true);
                animator.SetFloat("AttDirection", 2);
            }
            else if (movement.x < 0) // Kiri
            {
                attackPointLeft.SetActive(true);
                animator.SetFloat("AttDirection", 3);
            }
            else if (movement.y > 0) // Atas
            {
                attackPointUp.SetActive(true);
                animator.SetFloat("AttDirection", 1);
            }
            else if (movement.y < 0) // Bawah
            {
                attackPointDown.SetActive(true);
                animator.SetFloat("AttDirection", 0);
            }
            // Trigger animasi serangan
            animator.SetTrigger("Attack");
            audioManager.PlaySFX(1);
        }
    }

    private void upAttack()
    {
        attackPointUp.SetActive(true);
    }
    private void leftAttack()
    {
        attackPointLeft.SetActive(true);
    }
    private void rightAttack()
    {
       attackPointRight.SetActive(true);
    }
    private void downAttack()
    {
        attackPointDown.SetActive(true);
    }

    // Fungsi untuk mereset semua attack point
    private void ResetAttackPoints()
    {
        attackPointRight.SetActive(false);
        attackPointLeft.SetActive(false);
        attackPointUp.SetActive(false);
        attackPointDown.SetActive(false);
    }

    public void GiveDamage()
    {
        GameObject activeAttackPoint = null;

        if (attackPointRight.activeSelf)
            activeAttackPoint = attackPointRight;
        else if (attackPointLeft.activeSelf)
            activeAttackPoint = attackPointLeft;
        else if (attackPointUp.activeSelf)
            activeAttackPoint = attackPointUp;
        else if (attackPointDown.activeSelf)
            activeAttackPoint = attackPointDown;

        if (activeAttackPoint != null)
        {
            // Detect enemies in range of attack
            Collider2D[] hitObjects = Physics2D.OverlapCircleAll(activeAttackPoint.transform.position, attackRange, enemyLayers);
            foreach (Collider2D obj in hitObjects)
            {
                // Damage enemies
                EnemyHealth enemy = obj.GetComponent<EnemyHealth>();
                if (enemy != null)
                {
                    enemy.TakeDamage(playerStat.damage);
                    continue;
                }

                EnemyHealthBoss boss = obj.GetComponent<EnemyHealthBoss>();
                if(boss != null)
                {
                    boss.TakeDamage(playerStat.damage);
                    continue;
                }

                // Damage destructible items
                Destructible destructible = obj.GetComponent<Destructible>();
                if (destructible != null)
                {
                    destructible.TakeDamage(playerStat.damage);
                }
            }
        }
    }

    public void ApplyKnockback(Vector2 direction, float force, float duration)
    {
        isKnockbacked = true;
        knockbackDirection = direction.normalized * force;
        knockbackTimer = duration;
    }


}
