using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private int maxExp = 20;
    private int curExp;

    public Slider hpSlider;
    public Slider expSlider;

    void Start()
    {
        expSlider.maxValue = maxExp;
        expSlider.value = curExp;
    }

    public void SetHealth(int health)
    {
        hpSlider.value = health;
    }

    public void SetMaxHp(int health)
    {
        hpSlider.maxValue = health;
        hpSlider.value = health;
    }

    public void SetExp(int exp)
    {
        curExp += exp;
        expSlider.value = curExp;
    }
}
