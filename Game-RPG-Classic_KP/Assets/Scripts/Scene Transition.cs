using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public string targetScene; // Nama scene tujuan
    public string targetSpawnPoint; // Nama spawn point di scene tujuan

    private float waitToLoad = 1f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // Pastikan objek yang masuk adalah player
        {
            // Simpan spawn point untuk scene berikutnya
            GameManager.Instance.nextSpawnPoint = targetSpawnPoint;
            SaveManager.Instance.SavePlayerData();
            FadeTransition.Instance.FadeToBlack();
            StartCoroutine(LoadSceneRoutine());
        }
    }

    private IEnumerator LoadSceneRoutine()
    {
        while(waitToLoad>=0)
        {
            waitToLoad -= Time.deltaTime;
            yield return null;
        }

        SceneManager.LoadScene(targetScene);
    }
}
