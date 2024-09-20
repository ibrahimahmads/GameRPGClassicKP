using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 1f;
    private PlayerControls playerControls;
    private Vector2 movement;
    private Rigidbody2D rb;
    private Animator animator;
    public Transform attackPoint;
    public float attackRange;
    public LayerMask enemyLayers;
    public int damage = 40;
    public float attLeft;
    public float attUp;
    public float attDown;
    public float attRight;

    private void Awake()
    {
        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void Update()
    {
        PlayerInput();
        Animation();
        Attack();
    }

    private void FixedUpdate()
    {
        move();
    }

    private void PlayerInput()
    {
        movement = playerControls.Movement.Move.ReadValue<Vector2>();
    }

    private void move()
    {
        rb.MovePosition(rb.position + movement * (moveSpeed * Time.fixedDeltaTime));
    }

    private void Animation()
    {
        animator.SetFloat("MoveX", movement.x);
        animator.SetFloat("MoveY", movement.y);

        if (movement != Vector2.zero)
        {
            animator.SetBool("IsMoving", true);
        }
        else
        {
            animator.SetBool("IsMoving", false);
        }
    }

    private void Attack()
    {
        if (Input.GetButtonDown("Fire1")) // Tombol serangan, misalnya "Fire1"
        {
            // Tentukan arah serangan berdasarkan input dan update posisi attackPoint
            Vector2 attackDirection = Vector2.down;

            if (movement.x > 0) // Kanan
            {
                attackDirection = Vector2.right;
                animator.SetFloat("AttDirection", 2);
                attackPoint.localPosition = attackDirection * attRight;
            }
            else if (movement.x < 0) // Kiri
            {
                attackDirection = Vector2.left;
                animator.SetFloat("AttDirection", 3);
                attackPoint.localPosition = attackDirection * attLeft;
            }
            else if (movement.y > 0) // Atas
            {
                attackDirection = Vector2.up;
                animator.SetFloat("AttDirection", 1);
                attackPoint.localPosition = attackDirection * attUp;
            }
            else if (movement.y < 0) // Bawah
            {
                attackDirection = Vector2.down;
                animator.SetFloat("AttDirection", 0);
                attackPoint.localPosition = attackDirection * attDown;
            }

            // Trigger animasi serangan
            animator.SetTrigger("Attack");
            GiveDamage();
            
        }
    }

    public void GiveDamage()
    {
        // Detect enemies in range of attack
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        // Berikan damage ke musuh yang terkena
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<EnemyHealth>()?.TakeDamage(damage); // Pastikan enemy punya script untuk menerima damage
        }
    }

    private void OnDrawGizmos()
    {
        if (attackPoint == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

}
