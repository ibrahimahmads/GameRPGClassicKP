using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public GameObject ledakanPrefabs;
    public float speed = 5f;
    public float lifetime = 2f;
    public float knockbackForce = 3f;
    private Vector2 direction;

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
        transform.right = direction;
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.position += (Vector3)direction * speed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerStat playerStat = collision.GetComponent<PlayerStat>();
            if(playerStat != null)
            {
                Vector2 knockbackDirection = (playerStat.transform.position - transform.position).normalized;
                playerStat.TakeDamage(15);
                playerStat.GetComponent<PlayerController>().ApplyKnockback(knockbackDirection,knockbackForce,0.03f);
            }
            Destroy(gameObject);
            GameObject ledakan = Instantiate(ledakanPrefabs, transform.position, Quaternion.identity);
            Destroy(ledakan,0.2f);
        }

        if(collision.CompareTag("Batas"))
        {
            Destroy(gameObject);
            GameObject ledakan = Instantiate(ledakanPrefabs, transform.position, Quaternion.identity);
            Destroy(ledakan,0.2f);
        }

        if(collision.CompareTag("Sword"))
        {
            Destroy(gameObject);
            GameObject ledakan = Instantiate(ledakanPrefabs, transform.position, Quaternion.identity);
            Destroy(ledakan,0.2f);
        }


    }
}
