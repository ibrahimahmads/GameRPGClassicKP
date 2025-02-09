using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{ 
    private string curScene;

    private void Start()
   {
        curScene = PlayerPrefs.GetString("LastScene", "Lorong");
        // Muat data pemain saat scene dimulai
        SaveManager.Instance.LoadGame();
        // Jika ada data posisi terakhir pemain, gunakan itu
        if (GameManager.Instance != null && curScene == "MainMenu" && GameManager.Instance.posisi != null)
        {
            Debug.Log($"curScene saat load: {curScene}");

            Debug.Log("game manager instance posisi = "+ GameManager.Instance.posisi);
            transform.position = GameManager.Instance.posisi; // Gunakan posisi terakhir pemain
            Debug.Log("Posisi pemain dimuat dari data terakhir: " + GameManager.Instance.posisi);
        }
        else if (GameManager.Instance != null && !string.IsNullOrEmpty(GameManager.Instance.nextSpawnPoint))
        {
            //Debug.Log($"curScene saat load: {curScene}");

            // Jika tidak ada data posisi terakhir, gunakan spawn point
            GameObject spawnPoint = GameObject.Find(GameManager.Instance.nextSpawnPoint);
            if (spawnPoint != null)
            {
                // Ambil offset dari komponen SpawnPoint
                SpawnPoint spawnPointComponent = spawnPoint.GetComponent<SpawnPoint>();
                Vector3 adjustedPosition = spawnPoint.transform.position;

                if (spawnPointComponent != null)
                {
                    adjustedPosition += (Vector3)spawnPointComponent.spawnOffset;
                }
                transform.position = adjustedPosition;
                Debug.Log("Posisi pemain diatur berdasarkan spawn point: " + GameManager.Instance.nextSpawnPoint);
            }
        }

        // Transisi fade selesai
        FadeTransition.Instance.FadeToClear();
    }
}


