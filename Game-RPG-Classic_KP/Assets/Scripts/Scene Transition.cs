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
            GameManager.Instance.nextSpawnPoint = targetSpawnPoint; // Simpan spawn point
            SceneManager.LoadScene(targetScene); // Pindah ke scene tujuan
        }
    }
}
