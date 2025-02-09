using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class SaveManager : Singleton<SaveManager>
{
    private string filePath;
    public GameObject alert;
    private float x = -7.07f;
    private float y = 4.06f;
    private void Start()
    {
        // Tentukan path file data
        filePath = Application.persistentDataPath + "/playerData.json";
    }

    // Fungsi untuk menghapus file playerData.json
    public void DeletePlayerData()
    {
        if (File.Exists(filePath))
        {
            File.Delete(filePath); // Menghapus file jika ada
            Debug.Log("Data permainan lama dihapus");
        }
    }

    // Fungsi untuk menginisialisasi data permainan baru
    public void InitializeNewGame()
    {
        DeletePlayerData();
        PlayerStat.Instance.hp = 50;
        PlayerStat.Instance.curHp = PlayerStat.Instance.hp;
        PlayerStat.Instance.level = 1;
        PlayerStat.Instance.curExp = 0;
        PlayerStat.Instance.maxExp = 20;
        PlayerStat.Instance.damage = 10;
        PlayerStat.Instance.defend = 5;
        PlayerStat.Instance.luck = 5;
        
        PlayerStat.Instance.hpBar.SetMaxHp(PlayerStat.Instance.hp);
        PlayerStat.Instance.hpBar.SetHealth(PlayerStat.Instance.curHp);
        PlayerStat.Instance.expBar.SetMaxExp(PlayerStat.Instance.maxExp);
        PlayerStat.Instance.expBar.SetExp(PlayerStat.Instance.curExp);
        PlayerStat.Instance.lvlText.text = PlayerStat.Instance.level.ToString();

        ItemManager.Instance.ResetItems();
        ItemManager.Instance.AddCoins(0);
        ItemManager.Instance.AddPotions(3);

        SavePlayerDataBaru();

        Debug.Log("Permainan baru berhasil diinisialisasi dan disimpan.");
    }


    // Fungsi untuk menyimpan data pemain ke file
    public void SavePlayerData()
    {
        PlayerData data = new PlayerData
        {
            level = PlayerStat.Instance.level,
            hp = PlayerStat.Instance.hp,
            curHp = PlayerStat.Instance.curHp,
            damage = PlayerStat.Instance.damage,
            defend  = PlayerStat.Instance.defend,
            luck  = PlayerStat.Instance.luck,
            curExp  = PlayerStat.Instance.curExp,
            maxExp = PlayerStat.Instance.maxExp,
            coinCount = ItemManager.Instance.CoinCount,
            potionCount = ItemManager.Instance.PotionCount,
            positionX = PlayerStat.Instance.transform.position.x,
            positionY = PlayerStat.Instance.transform.position.y,
            lastScene = SceneManager.GetActiveScene().name,
            spawnTarget = GameManager.Instance.nextSpawnPoint
            
        };
        
        string json = JsonUtility.ToJson(data); // Mengubah data ke format JSON

        // Simpan file
        File.WriteAllText(filePath, json);
        Debug.Log("Data pemain disimpan");
    }

    public void SavePlayerDataDungeon()
    {
        PlayerData data = new PlayerData
        {
            level = PlayerStat.Instance.level,
            hp = PlayerStat.Instance.hp,
            curHp = PlayerStat.Instance.hp,
            damage = PlayerStat.Instance.damage,
            defend  = PlayerStat.Instance.defend,
            luck  = PlayerStat.Instance.luck,
            curExp  = PlayerStat.Instance.curExp,
            maxExp = PlayerStat.Instance.maxExp,
            coinCount = ItemManager.Instance.CoinCount,
            potionCount = ItemManager.Instance.PotionCount,
            positionX = -11.5775f,
            positionY = 9.664f,
            lastScene = GameManager.Instance.currentScene,
            spawnTarget = GameManager.Instance.nextSpawnPoint
            
        };
        
        string json = JsonUtility.ToJson(data); // Mengubah data ke format JSON

        // Simpan file
        File.WriteAllText(filePath, json);
        Debug.Log("Data pemain disimpan");
    }

    public void SavePlayerDataBaru()
    {
        PlayerData data = new PlayerData
        {
            level = PlayerStat.Instance.level,
            hp = PlayerStat.Instance.hp,
            curHp = PlayerStat.Instance.curHp,
            damage = PlayerStat.Instance.damage,
            defend  = PlayerStat.Instance.defend,
            luck  = PlayerStat.Instance.luck,
            curExp  = PlayerStat.Instance.curExp,
            maxExp = PlayerStat.Instance.maxExp,
            coinCount = ItemManager.Instance.CoinCount,
            potionCount = ItemManager.Instance.PotionCount,
            positionX = x,
            positionY = y,
            lastScene = SceneManager.GetActiveScene().name
            
        };
        
        string json = JsonUtility.ToJson(data); // Mengubah data ke format JSON

        // Simpan file
        File.WriteAllText(filePath, json);
        Debug.Log("Data pemain disimpan");
    }

    public void LoadGame()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            PlayerData data = JsonUtility.FromJson<PlayerData>(json);

            // Terapkan data yang dimuat
            PlayerStat.Instance.level = data.level;
            PlayerStat.Instance.hp = data.hp;
            PlayerStat.Instance.curHp = data.curHp;
            PlayerStat.Instance.damage = data.damage;
            PlayerStat.Instance.defend = data.defend;
            PlayerStat.Instance.luck = data.luck;
            PlayerStat.Instance.curExp = data.curExp;
            PlayerStat.Instance.maxExp = data.maxExp;
            // Perbarui UI PlayerStat
            PlayerStat.Instance.hpBar.SetMaxHp(data.hp);
            PlayerStat.Instance.hpBar.SetHealth(data.curHp);
            PlayerStat.Instance.expBar.SetMaxExp(data.maxExp);
            PlayerStat.Instance.expBar.SetExp(data.curExp);
            PlayerStat.Instance.lvlText.text = data.level.ToString();
            // Terapkan data ke ItemManager
            ItemManager.Instance.ResetItems(); // Pastikan di-reset sebelum mengisi ulang
            ItemManager.Instance.AddCoins(data.coinCount); // Terapkan jumlah koin
            ItemManager.Instance.AddPotions(data.potionCount); // Terapkan jumlah potion
            GameManager.Instance.SetPosisi(new Vector2(data.positionX, data.positionY));
            GameManager.Instance.currentScene = data.lastScene;
            GameManager.Instance.nextSpawnPoint = data.spawnTarget;

            Debug.Log("Game loaded!");   
        }
        else 
        {
            alert.SetActive(true);
        }
    }

    public string GetFilePath()
    {
        return filePath;
    }





}
