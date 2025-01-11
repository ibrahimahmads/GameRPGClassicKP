using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spikeTraps : MonoBehaviour
{
   private bool canDamage = false;
   private float nextDamageTime = 0f; // Waktu berikutnya damage dapat diberikan
   private float damageCooldown = 1.0f;
   public float knockbackForce = 1f; 
   public bool hasDamagedPlayer = false; 

   private Collider2D spikeCollider; // Menyimpan referensi ke collider

    private void Awake()
    {
        spikeCollider = GetComponent<Collider2D>(); // Ambil collider dari objek spike
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (canDamage && other.CompareTag("Player") && !hasDamagedPlayer)
        {
            dealDamage(other);
            hasDamagedPlayer = true; 
        }
    }

    // Dipanggil dari Animation Event
    public void EnableDamage()
    {
        canDamage = true;
        if (spikeCollider != null)
        {
            spikeCollider.enabled = true; // Aktifkan collider saat damage dapat diberikan
        }
    }

    // Dipanggil dari Animation Event
    public void DisableDamage()
    {
        canDamage = false;
        if (spikeCollider != null)
        {
            spikeCollider.enabled = false; // Aktifkan collider saat damage dapat diberikan
        }
        ResetDamageFlag();
    }

    private void dealDamage(Collider2D player)
    {
        if (Time.time >= nextDamageTime)
        {
            player.GetComponent<PlayerStat>().TakeDamage(4);
            nextDamageTime = Time.time + damageCooldown;

            ApplyKnockback(player);
        }
    }

    private void ApplyKnockback(Collider2D player)
    {
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Vector2 knockbackDirection = (player.transform.position - transform.position).normalized;
            player.GetComponent<PlayerController>().ApplyKnockback(knockbackDirection, knockbackForce, 0.2f);
        }
    }

    public void ResetDamageFlag()
    {
        hasDamagedPlayer = false;
    }
}
