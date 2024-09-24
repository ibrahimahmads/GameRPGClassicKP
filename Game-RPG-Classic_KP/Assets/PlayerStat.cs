using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    public int hp;
    public int curHp;
    public int damage;

    public HealthBar hpBar;

    // Start is called before the first frame update
    void Start()
    {
        curHp = hp;
        hpBar.SetMaxHp(hp);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(20);
        }
    }

    public void TakeDamage(int damage)
    {
        curHp -= damage;

        hpBar.SetHealth(curHp);
    }
}
