using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeDungeonRoaming : MonoBehaviour
{
    public float speed = 2f;
    public float roamDelay = 2f;
    public float idleTime = 1.5f;
    public Collider2D roamArea; // Area terbatas untuk roaming
    private Vector2 targetPosition;
    private float roamTimer;
    private bool isIdle = false;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        SetNewTarget();
    }

    void Update()
    {
        if (isIdle) return; // Jika sedang idle, tidak bergerak

        roamTimer -= Time.deltaTime;
        if (roamTimer <= 0)
        {
            StartIdle();
        }
        else
        {
            Move();
        }
    }

    void Move()
    {
        animator.SetBool("IsMoving", true);
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // Mengubah arah sprite berdasarkan arah pergerakan
        if (transform.position.x < targetPosition.x)
            spriteRenderer.flipX = false; // Menghadap kanan
        else if (transform.position.x > targetPosition.x)
            spriteRenderer.flipX = true;  // Menghadap kiri

         if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
        {
            StartIdle();
        }
    }

    void StartIdle()
    {
        isIdle = true;
        animator.SetBool("IsMoving", false); // Set animasi idle
        Invoke(nameof(SetNewTarget), idleTime);
    }

    void SetNewTarget()
    { 
        if (roamArea != null)
        {
            Bounds bounds = roamArea.bounds;
            targetPosition = new Vector2(
                Random.Range(bounds.min.x, bounds.max.x),
                Random.Range(bounds.min.y, bounds.max.y)
            );
        }
        roamTimer = roamDelay;
        isIdle = false;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Vector2 knockbackDirection = (collision.transform.position - transform.position).normalized;
            collision.GetComponent<PlayerStat>().TakeDamage(6);
            collision.GetComponent<PlayerController>().ApplyKnockback(knockbackDirection,6f,0.03f);
        }  
    }
}
