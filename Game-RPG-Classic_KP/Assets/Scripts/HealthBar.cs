using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider hpSlider;
    public Slider expSlider;

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
        expSlider.value = exp;
    }

    public void SetMaxExp(int maxExp)
    {
        expSlider.value = 0;
        expSlider.maxValue = maxExp;
    }
}
