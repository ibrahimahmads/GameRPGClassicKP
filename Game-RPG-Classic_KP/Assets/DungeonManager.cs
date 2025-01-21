using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DungeonManager : MonoBehaviour
{
    public static DungeonManager instance;
    public TextMeshProUGUI musuhText;

    private int enemiesDefeated = 0;

    private void Awake()
    {
        // Pastikan hanya ada satu instance DungeonManager
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void EnemyDefeated()
    {
        enemiesDefeated++;
        musuhText.text = enemiesDefeated.ToString();
        //Debug.Log($"Enemies defeated: {enemiesDefeated}");
    }

    public int GetEnemiesDefeated()
    {
        return enemiesDefeated;
    }
}
