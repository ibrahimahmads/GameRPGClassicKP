using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    public int health = 1; // Jumlah serangan yang dibutuhkan untuk menghancurkan item
    public GameObject destructionEffect; // Efek visual ketika item hancur

    // Fungsi untuk menerima damage
    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            DestroyItem();
        }
    }

    // Fungsi untuk menghancurkan item
    private void DestroyItem()
    {
        if (destructionEffect != null)
        {
            Instantiate(destructionEffect, transform.position, Quaternion.identity);
        }

        Destroy(gameObject); // Hapus item dari game
    }
}
