using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LosesGame : MonoBehaviour
{
    public string targetSpawn;
    public void RestartGame()
    {
        SaveManager.Instance.SavePlayerDataDungeon();
        Time.timeScale = 1f; // Kembalikan waktu normal

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reload scene
    }

    public void DungeonKeMainmenu(string sceneName)
    {
        Time.timeScale = 1f;
        GameManager.Instance.nextSpawnPoint = targetSpawn;
        SaveManager.Instance.SavePlayerDataDungeon();
        FadeTransition.Instance.FadeToBlack();
        //SaveManager.Instance.LoadGame();
        SceneManager.LoadScene(sceneName);
    }

    public void KeMainmenu(string sceneName)
    {
        Time.timeScale = 1f;
        SaveManager.Instance.SavePlayerData();
        SceneManager.LoadScene(sceneName);
    }
    

}
