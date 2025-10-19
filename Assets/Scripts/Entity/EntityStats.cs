using UnityEngine;

public class EntityStats : MonoBehaviour
{
    public Stat maxHealth; // |EN| Base maximum health points |TR| Temel maksimum sağlık puanı
    public StatMajorGroup majorStats; // |EN| Major stats group containing primary attributes |TR| Birincil özellikleri içeren ana istatistik grubu
    public StatOffensiveGroup offensiveStats; // |EN| Offensive stats group containing damage and critical hit stats |TR| Hasar ve kritik vuruş istatistiklerini içeren saldırı istatistikleri grubu
    public StatDefensiveGroup defensiveStats; // |EN| Defensive stats group containing armor and resistances |TR| Zırh ve dirençleri içeren savunma istatistikleri grubu

    public float GetMaxHealth()
    {
        float baseHealth = maxHealth.GetValue();
        float bonusHealth = majorStats.vitality.GetValue() * 5f; // |EN| Calculate bonus health from vitality |TR| Canlılıktan bonus sağlığı hesapla
        return baseHealth + bonusHealth;
    }

    public float GetEvasion()
    {
        float baseEvasion = defensiveStats.evasion.GetValue();
        float bonusEvasion = majorStats.agility.GetValue() * 0.5f; // |EN| Calculate bonus evasion from agility |TR| Çeviklikten bonus kaçınmayı hesapla

        float totalEvasion = baseEvasion + bonusEvasion; // |EN| Total evasion before capping |TR| Sınırlandırılmadan önce toplam kaçınma
        float evasionCap = 75f; // |EN| Cap evasion at 75% |TR| Kaçınmayı %75 ile sınırla

        float finalEvasion = Mathf.Clamp(totalEvasion, 0f, evasionCap); // |EN| Clamp evasion between 0% and cap |TR| Kaçınmayı %0 ile sınır arasında sınırla
        return finalEvasion;
    }
}
