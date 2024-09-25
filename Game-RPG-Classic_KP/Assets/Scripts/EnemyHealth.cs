using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public ItemData[] itemDrops;
    public int health = 100;
    public HealthBar expBar;

    void Start()
    {
        Time.timeScale = 1f;
    }
    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("Enemy Take damage");
        if (health <= 0)
        {
            Die();
        }
        
    }

    void Die()
    {
        DropItems();
        expBar.SetExp(4);
        Destroy(gameObject);
    }

    void DropItems()
    {
        foreach (ItemData item in itemDrops)
        {
            if (Random.value <= item.dropChance) // Kalikan dengan 100 untuk bandingkan dengan dropChance
            {
                GameObject droppedItem = Instantiate(item.itemPrefab, transform.position, Quaternion.identity);

                // Tambahkan gaya untuk melempar item ke atas
                Rigidbody2D rb = droppedItem.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    // Terapkan gaya ke arah atas dengan sedikit variasi
                    float forceX = Random.Range(-5f, 5f); // Variasi gaya ke arah X
                    float forceY = Random.Range(8f, 12f); // Gaya ke arah Y (vertikal)
                    rb.AddForce(new Vector2(forceX, forceY), ForceMode2D.Impulse);
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
