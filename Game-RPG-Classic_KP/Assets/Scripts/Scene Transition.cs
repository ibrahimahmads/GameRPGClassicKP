using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public string targetScene; // Nama scene tujuan
    public string targetSpawnPoint; // Nama spawn point di scene tujuan

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // Pastikan objek yang masuk adalah player
        {
            // Simpan spawn point untuk scene berikutnya
            GameManager.Instance.nextSpawnPoint = targetSpawnPoint;

            // Simpan data pemain sebelum pindah scene
            SaveManager saveManager = FindObjectOfType<SaveManager>();
            if (saveManager != null)
            {
                saveManager.SavePlayerData();
            }

            // Pindah ke scene tujuan
            SceneManager.LoadScene(targetScene);
        }
    }
}
