using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuzzyLogicManager : MonoBehaviour
{
    [System.Serializable]
    public class FuzzySet
    {
        public string name; // Nama fuzzy set (e.g., "Sedikit", "Sedang", "Banyak")
        public float a;     // Titik awal fungsi keanggotaan
        public float b;     // Titik tengah atau akhir
        public float c;     // Titik akhir fungsi (jika triangular)
    }

    [System.Serializable]
    public class FuzzyRule
    {
        public string condition; // Kondisi (e.g., "HP Sedikit AND Waktu Cepat")
        public string output;    // Output (e.g., "HP Kecil, DEF Sedang")
        public float hpChange;   // Perubahan HP
        public float defChange;  // Perubahan DEF
        public float attChange;  // Perubahan ATT
        public float luckChange; // Perubahan Luck
    }

    public List<FuzzySet> hpSets;    // Fuzzy sets untuk HP
    public List<FuzzySet> timeSets; // Fuzzy sets untuk waktu
    public List<FuzzySet> enemySets; // Fuzzy sets untuk jumlah musuh

    public List<FuzzyRule> rules;

    // Fuzzification function (menggantikan CalculateMembership)
    private void Fuzzification(float input, FuzzySet set, out float uLeft, out float uCenter, out float uRight)
    {
        float leftC = set.b, leftD = set.c; // untuk trapezoid kiri
        float ctrA = set.a, ctrB = set.b, ctrC = set.c; // untuk segitiga tengah
        float rightA = set.a, rightB = set.b; // untuk trapezoid kanan

        // Menghitung keanggotaan untuk trapezoid kiri (Sedikit)
        if (input <= leftC)
        {
            uLeft = 1;
        }
        else if (input > leftC && input < leftD)
        {
            uLeft = (leftD - input) / (leftD - leftC);
        }
        else
        {
            uLeft = 0;
        }

        // Menghitung keanggotaan untuk segitiga tengah (Sedang)
        if (input <= ctrA || input >= ctrC)
        {
            uCenter = 0;
        }
        else if (input > ctrA && input < ctrB)
        {
            uCenter = (input - ctrA) / (ctrB - ctrA);
        }
        else if (input == ctrB)
        {
            uCenter = 1;
        }
        else // input > ctrB && input < ctrC
        {
            uCenter = (ctrC - input) / (ctrC - ctrB);
        }

        // Menghitung keanggotaan untuk trapezoid kanan (Banyak)
        if (input <= rightA)
        {
            uRight = 0;
        }
        else if (input > rightA && input < rightB)
        {
            uRight = (input - rightA) / (rightB - rightA);
        }
        else
        {
            uRight = 1;
        }
    }

    // Evaluasi aturan berdasarkan input
    public PlayerStat EvaluateRules(float hpInput, float timeInput, float enemyInput)
    {
        GameObject tempObject = new GameObject("TempPlayerStat");
        PlayerStat result = tempObject.AddComponent<PlayerStat>();
        float totalWeightHp1 = 0f;
        float totalWeightDef1 = 0f;
        float totalWeightAtt2 = 0f;
        float totalWeightLuck2 = 0f;

        float totalMembership1 = 0f, totalMembership2 = 0f;

        for (int i = 0; i < rules.Count; i++)
        {
            var rule = rules[i];
            string[] conditions = rule.condition.Split(new string[] { " AND " }, System.StringSplitOptions.None);

            float minMembership = 1f; // Nilai keanggotaan minimum untuk aturan ini

            foreach (var condition in conditions)
            {
                string[] parts = condition.Split(' ');
                string attribute = parts[0]; // e.g., "HP", "Waktu"
                string setName = parts[1];   // e.g., "Sedikit", "Cepat"

                float membership = 0f;
                if (attribute == "HP")
                    membership = GetMembership(hpInput, hpSets, setName);
                else if (attribute == "Waktu")
                    membership = GetMembership(timeInput, timeSets, setName);
                else if (attribute == "JumlahMusuh")
                    membership = GetMembership(enemyInput, enemySets, setName);

                minMembership = Mathf.Min(minMembership, membership);
            }

            if (i < 9) // Rules pertama (0-8)
            {
                totalWeightHp1 += rule.hpChange * minMembership;
                totalWeightDef1 += rule.defChange * minMembership;
                totalMembership1 += minMembership;
            }
            else // Rules kedua (9-17)
            {
                totalWeightAtt2 += rule.attChange * minMembership;
                totalWeightLuck2 += rule.luckChange * minMembership;
                totalMembership2 += minMembership;
            }
        }

        // Menghitung hasil akhir untuk masing-masing membership
        if (totalMembership1 > 0 && totalMembership2 > 0)
        {
            result.hp += Mathf.RoundToInt(totalWeightHp1 / totalMembership1);
            result.defend += Mathf.RoundToInt(totalWeightDef1 / totalMembership1);
            result.damage += Mathf.RoundToInt(totalWeightAtt2 / totalMembership2);
            result.luck += Mathf.RoundToInt(totalWeightLuck2 / totalMembership2);
        }

        return result;
    }

    // Dapatkan keanggotaan untuk fuzzy set tertentu
    private float GetMembership(float value, List<FuzzySet> sets, string setName)
    {
        foreach (var set in sets)
        {
            if (set.name == setName)
            {
                Fuzzification(value, set, out float uLeft, out float uCenter, out float uRight);

                if (setName == "Sedikit" || setName == "Cepat") {
                    //Debug.Log($"GetMembership - Set: {setName}, uLeft: {uLeft}");
                    return uLeft;
                }
                if (setName == "Sedang") 
                {
                    //Debug.Log($"GetMembership - Set: {setName}, uCenter: {uCenter}");
                    return uCenter;
                }
                if (setName == "Banyak" || setName == "Lama")
                {
                    //Debug.Log($"GetMembership - Set: {setName}, uRight: {uRight}");
                    return uRight;
                }         
            }
        }
        Debug.Log($"Set {setName} tidak ditemukan!");
        return 0f;
    }
}
