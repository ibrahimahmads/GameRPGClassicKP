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
    }

    // Update is called once per frame
    void Update()
    {
        // if(Input.GetKeyDown(KeyCode.Space))
        // {
        //     TakeDamage(20);
        // }
    }

    public void TakeDamage(int damage)
    {
        curHp -= damage;

        hpBar.SetHealth(curHp);
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
        hp += 20;
        damage += 5;
        defend += 3;
        
        curHp = hp;  // Reset HP ke max saat level up
        hpBar.SetMaxHp(hp);  // Update bar HP
        hpBar.SetHealth(curHp);
        lvlText.text = level.ToString();
    }
}
