using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
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

    
}
