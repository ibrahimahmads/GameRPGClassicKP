using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WizardAttack : MonoBehaviour
{
    public GameObject fireballPrefab;
    public Transform firePoint;
    public float attackCooldown = 2f;
    public float detectionRange = 5f;
    public Collider2D roamArea;
    
    private Transform player;
    private float attackTimer;
    private WizardRoaming wizardRoaming;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Vector3 defaultFirePointOffset;
    private bool isAttacking = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        wizardRoaming = GetComponent<WizardRoaming>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultFirePointOffset = firePoint.localPosition;
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (isAttacking) return; // Jika sedang menyerang, hentikan update

        if (distance <= detectionRange && !isAttacking)
        {
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0)
            {
                StartCoroutine(StartAttack());
                attackTimer = attackCooldown;
            }
        }
    }

    IEnumerator StartAttack()
    {
        isAttacking = true;
        wizardRoaming.enabled = false; // Matikan roaming saat menyerang
        UpdateFirePoint();
        // Menghadap ke player
        spriteRenderer.flipX = transform.position.x > player.position.x;

        // Gunakan trigger attack
        animator.SetTrigger("Attack");

        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        
        isAttacking = false;
    }

    public void Fireball()
    {
        if (fireballPrefab != null && firePoint != null)
        {
            GameObject fireball = Instantiate(fireballPrefab, firePoint.position, Quaternion.identity);
            fireball.GetComponent<Fireball>().SetDirection((player.position - firePoint.position).normalized);
        }

        Invoke(nameof(Retreat), 0.5f); // Setelah menembak, mulai retreat
    }

    void Retreat()
    {
        Vector2 retreatPosition = GetRetreatPosition();
        wizardRoaming.SetCustomTarget(retreatPosition, 3f);
        isAttacking = false;
        wizardRoaming.enabled = true; // Aktifkan roaming kembali
    }

    Vector2 GetRetreatPosition()
    {
        if (roamArea == null) return transform.position;

        Bounds bounds = roamArea.bounds;
        Vector2 directionAway = (transform.position - player.position).normalized;
        Vector2 retreatPos = (Vector2)transform.position + directionAway * detectionRange;

        // Jika retreat keluar dari roam area, sesuaikan posisinya
        if (!bounds.Contains(retreatPos))
        {
            // Coba geser ke dalam roam area
            retreatPos.x = Mathf.Clamp(retreatPos.x, bounds.min.x, bounds.max.x);
            retreatPos.y = Mathf.Clamp(retreatPos.y, bounds.min.y, bounds.max.y);
            
            // Jika masih terlalu dekat ke player, pilih titik acak dalam roam area
            if (Vector2.Distance(retreatPos, player.position) < detectionRange / 2)
            {
                for (int i = 0; i < 10; i++) // Batasi loop agar tidak infinite
                {
                    Vector2 randomPos = new Vector2(
                        Random.Range(bounds.min.x, bounds.max.x),
                        Random.Range(bounds.min.y, bounds.max.y)
                    );

                    if (Vector2.Distance(randomPos, player.position) >= detectionRange / 2)
                    {
                        retreatPos = randomPos;
                        break;
                    }
                }
            }
        }
        return retreatPos;
    }



    void UpdateFirePoint()
    {
        if (player != null)
        {
            float direction = player.position.x - transform.position.x;
            if (direction < 0)
                firePoint.localPosition = new Vector3(-Mathf.Abs(defaultFirePointOffset.x), defaultFirePointOffset.y, 0);
            else
                firePoint.localPosition = new Vector3(Mathf.Abs(defaultFirePointOffset.x), defaultFirePointOffset.y, 0);
        }
    }

}
