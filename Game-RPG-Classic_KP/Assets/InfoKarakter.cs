using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InfoKarakter : MonoBehaviour
{
    public Slider expSlider;
    public TextMeshProUGUI textExp;
    public TextMeshProUGUI textlvl;
    public TextMeshProUGUI textHP; 
    public TextMeshProUGUI textATT; 
    public TextMeshProUGUI textDEF; 
    public TextMeshProUGUI textLUCK; 
    public TextMeshProUGUI textGOLD; 

    public void showInfo()
    {
        int maxExp = PlayerStat.Instance.maxExp;
        int curExp = PlayerStat.Instance.curExp;
        int lvl = PlayerStat.Instance.level;
        int hp = PlayerStat.Instance.hp;
        int att = PlayerStat.Instance.damage;
        int def = PlayerStat.Instance.defend;
        int luck = PlayerStat.Instance.luck;
        int koin = ItemManager.Instance.CoinCount;

        textATT.text = "Att		: "+att;
        textHP.text = "HP		: "+hp;
        textDEF.text = "DEF		: "+def;
        textLUCK.text = "Luck	: "+luck;
        textlvl.text = "Level : "+lvl;
        textGOLD.text = koin.ToString();
        textExp.text = curExp+" / "+maxExp;
        expSlider.maxValue = maxExp;
        expSlider.value = curExp;
        
    }
    
}
