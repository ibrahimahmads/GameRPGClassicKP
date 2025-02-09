using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerStat : MonoBehaviour
{
    public static PlayerStat Instance { get; private set; }
    public int hp;
    public int curHp;
    public int damage;
    public int defend;
    public int luck;
    public int level = 1;  // Level awal
    public int curExp = 0; // EXP saat ini
    public int maxExp = 20;

    public HealthBar hpBar;
    public HealthBar expBar;
    public TextMeshProUGUI lvlText;
    public GameObject loseScreen;
    private Flash flash;

    void Awake()
    {
        // Singleton setup
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        curHp = hp;
        hpBar.SetMaxHp(hp);
        expBar.SetMaxExp(maxExp);  // Set batas max EXP pada level awal
        expBar.SetExp(curExp); 
        lvlText.text = ""+level;
        loseScreen.SetActive(false);
        flash = GetComponent<Flash>(); 
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            UsePotion();
        }
    }

    void UsePotion()
    {
        if(ItemManager.Instance.PotionCount > 0)
        {
            if(curHp >= hp)
            {
                return;
            }else{
                curHp += 10;
                if (curHp > hp) // Pastikan HP tidak melebihi maksimum
                {
                    curHp = hp;
                }

                hpBar.SetHealth(curHp);
                ItemManager.Instance.UsePotions(1);
            }     
        }else if (ItemManager.Instance.PotionCount == 0)
        {
            return;
        }
    }

    public void TakeDamage(int damage)
    {
        curHp -= damage;
        StartCoroutine(flash.FlashPlayerRoutine());
        hpBar.SetHealth(curHp);
        if (curHp <= 0) 
        {
            curHp = 0;
            hpBar.SetHealth(curHp);
            LoseGame();
        }
    }

    public void LoseGame()
    {
        loseScreen.SetActive(true); // Aktifkan panel lose screen
        Time.timeScale = 0f;
    }

    public void GainExp(int exp)
    {
        curExp += exp;
        expBar.SetExp(curExp);

        while (curExp >= maxExp)
        {
            int overflowExp = curExp - maxExp; // Hitung sisa EXP yang lebih dari maxExp
            LevelUp(); // Naik level, curExp akan di-reset di dalam LevelUp()
            curExp = overflowExp; // Masukkan kelebihan EXP ke curExp
            expBar.SetExp(curExp); // Update bar EXP dengan kelebihan EXP
        }
    }

    void LevelUp()
    {
        level++;
        maxExp += (maxExp+(maxExp*10)/100);  // Tambahkan batas EXP untuk level berikutnya (sesuaikan kebutuhan)
        expBar.SetMaxExp(maxExp);  // Update bar EXP untuk level berikutnya

        // Tingkatkan stats pemain
        hp += 5;
        damage += 3;
        defend += 1;
        
        curHp = hp;  // Reset HP ke max saat level up
        hpBar.SetMaxHp(hp);  // Update bar HP
        hpBar.SetHealth(curHp);
        lvlText.text = level.ToString();
    }
}
