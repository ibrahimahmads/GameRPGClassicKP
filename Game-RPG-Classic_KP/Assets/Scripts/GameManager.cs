using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public string currentScene; // Nama scene aktif

    public static GameManager Instance;

    public string nextSpawnPoint; // Nama spawn point untuk scene berikutnya
    public Vector2 posisi;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Jangan hancurkan saat pindah scene
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void SetPosisi(Vector2 newPosition)
    {
        Debug.Log($"[GameManager] Posisi sebelum update: {posisi}");
        posisi = newPosition;
        Debug.Log($"[GameManager] Posisi setelah update: {posisi}");
    }

}
