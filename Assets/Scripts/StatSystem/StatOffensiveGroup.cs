using UnityEngine;

[System.Serializable]
public class StatOffensiveGroup
{
    // |EN| Physical attack stats |TR| Fiziksel saldırı istatistikleri
    public Stat physicalDamage;
    public Stat criticalChance;
    public Stat criticalDamagePower;
    public Stat armorPenetration;
    public Stat attackSpeed;

    // |EN| Elemental damage stats |TR| Elemental hasar istatistikleri
    public Stat fireDamage;
    public Stat iceDamage;
    public Stat lightningDamage;
}
