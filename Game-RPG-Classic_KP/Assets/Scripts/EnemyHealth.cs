using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int health = 100;
    public HealthBar expBar;

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
        expBar.SetExp(4);
        Destroy(gameObject);
    }
}
