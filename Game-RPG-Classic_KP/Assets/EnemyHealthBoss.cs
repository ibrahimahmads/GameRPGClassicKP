using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthBoss : MonoBehaviour
{
    public bool isInDungeon = false;
    public ItemData[] itemDrops;
    public int health = 300;
    public int exp;
    public float knockback_thrust = 10f;
    public GameObject portal;
    public HealthBar expBar;
    public GameObject deathVFX;
    public Animator animator1;
    private Knockback knockback;
    private Flash flash;
    public Timer timer;

    void Awake()
    {
        flash = GetComponent<Flash>();
        knockback = GetComponent<Knockback>();
    }

    void Start()
    {
        portal.SetActive(false);
        Time.timeScale = 1f;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        knockback?.GetKnockedBack(PlayerController.Instance.transform, knockback_thrust);
        StartCoroutine(flash.FlashBossRoutine());
    }

    public void Die()
    {
        if(health <= 0)
        {
            if (isInDungeon)
            {
                DungeonManager.instance.EnemyDefeated();
            }
            StartCoroutine(tunggu());
        }
    }

    private IEnumerator tunggu() 
    {
        animator1.SetBool("IsDead",true);
        GetComponent<MinotaurAI>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        timer.StopTimer();
        yield return new WaitForSeconds(3.5f);
        DropItems(transform.position);
        PlayerStat.Instance.GainExp(exp);
        Instantiate(deathVFX,transform.position,Quaternion.identity);
        portal.SetActive(true);
        Destroy(gameObject);
    }
    

    void DropItems(Vector2 posisi)
    {
        foreach (ItemData item in itemDrops)
        {
            if (Random.value <= item.dropChance)
            {
                GameObject droppedItem = Instantiate(item.itemPrefab, posisi, Quaternion.identity);

                // Mendapatkan Rigidbody2D dari item yang dijatuhkan
                Rigidbody2D rb = droppedItem.GetComponent<Rigidbody2D>();

                if (rb != null)
                {
                    // Tambahkan gaya ke item agar terlempar ke arah acak di sumbu X dan Y
                    Vector2 throwDirection =
                        new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized; // Arah acak
                    rb.AddForce(throwDirection * 10f, ForceMode2D.Impulse); // 5f adalah kekuatan lemparan, bisa diubah

                    rb.drag = 2f;
                }

                // Jika item memiliki animasi, tambahkan ke objek dropped
                if (item.itemAnimation != null)
                {
                    Animator animator = droppedItem.GetComponent<Animator>();
                    if (animator != null)
                    {
                        // Menambahkan animasi secara manual
                        animator.Play(item.itemAnimation.name);
                    }
                }
            }
        }
    }
}
