using UnityEngine;

[System.Serializable]
public class StatOffensiveGroup
{
    // |EN| Physical attack stats |TR| Fiziksel saldırı istatistikleri
    public Stat damage;
    public Stat criticalChance;
    public Stat criticalDamagePower;

    // |EN| Elemental damage stats |TR| Elemental hasar istatistikleri
    public Stat fireDamage;
    public Stat iceDamage;
    public Stat lightningDamage;
}
