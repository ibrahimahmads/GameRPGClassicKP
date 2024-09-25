using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public ItemData[] itemDrops;
    public int health = 100;
    public HealthBar expBar;

    public float launchForce = 300f; // Kekuatan lemparan
    public float launchAngle = 45f; 
    public float fallBackSpeed = 2f;

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
        DropItems(transform.position);
        expBar.SetExp(4);
        Destroy(gameObject);
    }

    void DropItems(Vector2 posisi)
    {
        foreach (ItemData item in itemDrops)
        {
            if (Random.value <= item.dropChance) // Kalikan dengan 100 untuk bandingkan dengan dropChance
            {
                GameObject droppedItem = Instantiate(item.itemPrefab, posisi, Quaternion.identity);

                Vector2 direction = new Vector2(transform.position.x + Random.Range(-1f,1f), Random.Range(2f,4f));
                droppedItem.GetComponent<Rigidbody2D>().AddForce(direction*3f,ForceMode2D.Impulse);
                
                
                
                //StartCoroutine(LaunchItem(droppedItem));

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

    IEnumerator ReturnToStartY(GameObject item, float startY)
    {
        while (Mathf.Abs(item.transform.position.y - startY) > 0.1f) // Ketika item belum mencapai posisi awal
        {
            Vector3 currentPosition = item.transform.position;
            currentPosition.y = Mathf.MoveTowards(currentPosition.y, startY, fallBackSpeed * Time.deltaTime);
            item.transform.position = currentPosition;
            yield return null;
        }
    }
}
