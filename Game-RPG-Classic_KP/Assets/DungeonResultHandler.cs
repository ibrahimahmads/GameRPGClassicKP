using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DungeonResultHandler : MonoBehaviour
{
    // Referensi ke timer dan score manager (drag & drop via Inspector)
    public Timer dungeonTimer;
    public DungeonManager dungeonScoreManager;

    public TextMeshProUGUI musuhText;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI attText;
    public TextMeshProUGUI defText;
    public TextMeshProUGUI luckText;

    // public float currentHP = 68;
    // public float timeTaken = 74;
    // public float enemiesDefeated = 2;

    private void Start()
    {
        // Pastikan script ini hanya aktif setelah dungeon selesai
        gameObject.SetActive(false);
    }

    // Fungsi ini dipanggil saat dungeon selesai
    public void OnDungeonComplete()
    {
        // Ambil referensi FuzzyLogicManager
        FuzzyLogicManager fuzzyManager = FindObjectOfType<FuzzyLogicManager>();
        if (fuzzyManager == null)
        {
            Debug.LogError("FuzzyLogicManager tidak ditemukan!");
            return;
        }

        float curHP = PlayerStat.Instance.curHp;
        float maxHP = PlayerStat.Instance.hp;

        // Input dari hasil dungeon
        float currentHP = curHP/ maxHP* 100; // Persentase HP saat ini
        float timeTaken = dungeonTimer.GetElapsedTime(); // Ambil waktu yang tercatat
        float enemiesDefeated = dungeonScoreManager.GetEnemiesDefeated(); // Jumlah musuh dikalahkan

        
        Debug.Log("Input HP % : "+currentHP+"%. waktu : "+ timeTaken+" detik. jmlMusuh : "+enemiesDefeated);
        // Hitung hasil fuzzy
        PlayerStat result = fuzzyManager.EvaluateRules(currentHP, timeTaken, enemiesDefeated);

        // Terapkan hasil ke player
        PlayerStat.Instance.hp += Mathf.RoundToInt(result.hp);
        PlayerStat.Instance.defend += Mathf.RoundToInt(result.defend);
        PlayerStat.Instance.damage += Mathf.RoundToInt(result.damage);
        PlayerStat.Instance.luck += Mathf.RoundToInt(result.luck);

        // Debug untuk memeriksa hasil
        //Debug.Log($"Fuzzy Result Applied: HP={result.hp}, DEF={result.defend}, ATK={result.damage}, LUCK={result.luck}");

        // Tampilkan perubahan pada UI atau notifikasi
        ShowStatChanges(result);
    }

    // Menampilkan perubahan atribut di UI
    private void ShowStatChanges(PlayerStat result)
    {
        // Implementasikan logika untuk menampilkan hasil pada UI
        Debug.Log($"HP Bertambah: {result.hp}");
        Debug.Log($"DEF Bertambah: {result.defend}");
        Debug.Log($"ATK Bertambah: {result.damage}");
        Debug.Log($"LUCK Bertambah: {result.luck}");
        hpText.text = (PlayerStat.Instance.hp - result.hp)+" + " + result.hp;
        attText.text = (PlayerStat.Instance.damage - result.damage)+" + " + result.damage;
        defText.text = (PlayerStat.Instance.defend - result.defend)+" + " + result.defend;
        luckText.text = (PlayerStat.Instance.luck - result.luck)+" + " + result.luck;

    }

    public void showResult()
    {
        float waktu = dungeonTimer.GetElapsedTime();
        int jmlMusuh = dungeonScoreManager.GetEnemiesDefeated();

        int minutes = Mathf.FloorToInt(waktu / 60f); // Hitung menit
        int seconds = Mathf.FloorToInt(waktu % 60f); // Hitung detik

        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        musuhText.text = jmlMusuh.ToString();

    }
}
