using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alert : MonoBehaviour
{
    public GameObject panelAlert;
    public float shiftAmountx = 1f;
    public float shiftAmounty = 1f;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // Pastikan objek yang masuk adalah player
        {
            panelAlert.SetActive(true);
            // Dapatkan posisi saat ini dari player
            Vector2 currentPosition = collision.transform.position;
            
            // Menggeser posisi berdasarkan nilai positif atau negatif untuk x dan y
            float newX = currentPosition.x + shiftAmountx;  // Geser ke kanan
            float newY = currentPosition.y + shiftAmounty;  // Geser ke atas
            
            // Terapkan perubahan posisi ke player
            collision.transform.position = new Vector2(newX, newY);
            Time.timeScale = 0f;
        }
    }
}
