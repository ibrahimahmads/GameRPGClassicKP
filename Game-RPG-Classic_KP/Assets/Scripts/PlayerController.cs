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
    public float attackRange;
    public LayerMask enemyLayers;
    private PlayerStat playerStat;

    public GameObject attackPointRight; // GameObject untuk serangan kanan
    public GameObject attackPointLeft; // GameObject untuk serangan kiri
    public GameObject attackPointUp; // GameObject untuk serangan atas
    public GameObject attackPointDown;

    [SerializeField] private float knockBackThrust = 5f;

    private void Awake()
    {
        playerStat = GetComponent<PlayerStat>();
        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        attackPointRight.SetActive(false);
        attackPointLeft.SetActive(false);
        attackPointUp.SetActive(false);
        attackPointDown.SetActive(false);
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
            attackPointRight.SetActive(false);
            attackPointLeft.SetActive(false);
            attackPointUp.SetActive(false);
            attackPointDown.SetActive(false);

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
            }else
            {
                attackPointDown.SetActive(true);
                animator.SetFloat("AttDirection", 0);
            }

            // Trigger animasi serangan
            animator.SetTrigger("Attack");
            GiveDamage();
            
        }
    }

    public void AttackAnimEndEvent()
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
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(activeAttackPoint.transform.position, attackRange, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<EnemyHealth>()?.TakeDamage(playerStat.damage, transform, knockBackThrust);
        }
    }
    }

}
