using UnityEngine;

public class EntityStats : MonoBehaviour
{
    public StatResourceGroup resourceStats; // |EN| Resource stats group containing mana, stamina, etc. |TR| Mana, dayanıklılık vb. içeren kaynak istatistikleri grubu
    public StatMajorGroup majorStats; // |EN| Major stats group containing primary attributes |TR| Birincil özellikleri içeren ana istatistik grubu
    public StatOffensiveGroup offensiveStats; // |EN| Offensive stats group containing damage and critical hit stats |TR| Hasar ve kritik vuruş istatistiklerini içeren saldırı istatistikleri grubu
    public StatDefensiveGroup defensiveStats; // |EN| Defensive stats group containing armor and resistances |TR| Zırh ve dirençleri içeren savunma istatistikleri grubu

    public float GetElementalDamage(out ElementType elementType, float scaleFactor = 1f)
    {
        float fireDamage = offensiveStats.fireDamage.GetValue();
        float iceDamage = offensiveStats.iceDamage.GetValue();
        float lightningDamage = offensiveStats.lightningDamage.GetValue();
        float bonusElementalDamage = majorStats.intelligence.GetValue(); // |EN| Calculate bonus damage from intelligence |TR| Zekadan bonus hasarı hesapla

        float highestElementalDamage = Mathf.Max(fireDamage, iceDamage, lightningDamage); // |EN| Get the highest elemental damage type |TR| En yüksek element hasarı türünü al

        // Get the element type corresponding to the highest damage
        if (highestElementalDamage == fireDamage)
            elementType = ElementType.Fire;
        else if (highestElementalDamage == iceDamage)
            elementType = ElementType.Ice;
        else if (highestElementalDamage == lightningDamage)
            elementType = ElementType.Lightning;
        else
            elementType = ElementType.None;

        if (highestElementalDamage == 0f)
        {
            elementType = ElementType.None;
            return 0f; // |EN| No elemental damage available |TR| Mevcut element hasarı yok
        }

        float bonusFireDamage = fireDamage == highestElementalDamage ? 0f : fireDamage * 0.5f; // |EN| 50% bonus for non-highest elemental types |TR| En yüksek olmayan element türleri için %50 bonus
        float bonusIceDamage = iceDamage == highestElementalDamage ? 0f : iceDamage * 0.5f; // |EN| 50% bonus for non-highest elemental types |TR| En yüksek olmayan element türleri için %50 bonus
        float bonusLightningDamage = lightningDamage == highestElementalDamage ? 0f : lightningDamage * 0.5f; // |EN| 50% bonus for non-highest elemental types |TR| En yüksek olmayan element türleri için %50 bonus

        float weakerElementalDamage = bonusFireDamage + bonusIceDamage + bonusLightningDamage; // |EN| Sum of weaker elemental damages |TR| Daha zayıf element hasarlarının toplamı

        float finalDamage = highestElementalDamage + bonusElementalDamage + weakerElementalDamage;

        return finalDamage * scaleFactor; // |EN| Scale the final damage by the provided factor |TR| Sağlanan faktörle nihai hasarı ölçeklendir
    }

    public float GetElementalResistance(ElementType elementType)
    {
        float baseResistance = 0f;
        float bonusResistance = majorStats.intelligence.GetValue() * 0.5f; // |EN| Calculate bonus resistance from intelligence |TR| Zekadan bonus direnç hesapla

        switch (elementType)
        {
            case ElementType.Fire:
                baseResistance = defensiveStats.fireResistance.GetValue();
                break;
            case ElementType.Ice:
                baseResistance = defensiveStats.iceResistance.GetValue();
                break;
            case ElementType.Lightning:
                baseResistance = defensiveStats.lightningResistance.GetValue();
                break;
            default:
                return 0f; // |EN| No resistance for non-elemental types |TR| Elemental olmayan türler için direnç yok
        }

        float totalResistance = baseResistance + bonusResistance; // |EN| Total resistance before capping |TR| Sınırlandırılmadan önce toplam direnç
        float resistanceCap = 75f; // |EN| Cap resistance at 75% |TR| Direnci %75 ile sınırla
        float finalResistance = Mathf.Clamp(totalResistance, 0f, resistanceCap) / 100f; // |EN| Clamp resistance between 0% and cap, convert to decimal |TR| Direnci %0 ile sınır arasında sınırla, ondalığa dönüştür

        return finalResistance;
    }

    public float GetPhysicalDamage(out bool isCriticalHit, float scaleFactor = 1f)
    {
        float baseDamage = offensiveStats.physicalDamage.GetValue();
        float bonusDamage = majorStats.strength.GetValue(); // |EN| Calculate bonus damage from strength |TR| Güçten bonus hasarı hesapla
        float totalDamage = baseDamage + bonusDamage;

        float baseCriticalChance = offensiveStats.criticalChance.GetValue();
        float bonusCriticalChance = majorStats.agility.GetValue() * 0.3f; // |EN| Calculate bonus critical chance from agility |TR| Çeviklikten bonus kritik şansı hesapla
        float totalCriticalChance = baseCriticalChance + bonusCriticalChance;

        float baseCriticalDamagePower = offensiveStats.criticalDamagePower.GetValue();
        float bonusCriticalDamagePower = majorStats.strength.GetValue() * 0.5f; // |EN| Calculate bonus critical damage power from strength |TR| Güçten bonus kritik hasar gücünü hesapla
        float totalCriticalDamagePower = (baseCriticalDamagePower + bonusCriticalDamagePower) / 100f; // |EN| Convert percentage to multiplier (e.g., 150% -> 1.5) |TR| Yüzdeyi çarpana dönüştür (örneğin, %150 -> 1.5)

        // |EN| Determine if the attack is a critical hit |TR| Saldırının kritik bir vuruş olup olmadığını belirle
        isCriticalHit = Random.Range(0f, 100f) < totalCriticalChance;
        float finalDamage = isCriticalHit ? totalDamage * totalCriticalDamagePower : totalDamage;

        return finalDamage * scaleFactor; // |EN| Scale the final damage by the provided factor |TR| Sağlanan faktörle nihai hasarı ölçeklendir
    }

    public float GetArmorMitigation(float armorPenetration)
    {
        float baseArmor = defensiveStats.armor.GetValue();
        float bonusArmor = majorStats.vitality.GetValue(); // |EN| Calculate bonus armor from vitality |TR| Canlılıktan bonus zırhı hesapla
        float totalArmor = baseArmor + bonusArmor; // |EN| Total armor before mitigation calculation |TR| Azaltma hesaplamasından önce toplam zırh

        float penetrationMultiplier = Mathf.Clamp01(1f - armorPenetration); // |EN| Calculate penetration multiplier (e.g., 20% penetration -> 0.8 multiplier) Clamp01 means it will be clamped between 0 and 1 |TR| Delme çarpanını hesapla (örneğin, %20 delme -> 0.8 çarpanı) Clamp01, bunun 0 ile 1 arasında sınırlandırılacağı anlamına gelir
        float adjustedArmor = totalArmor * penetrationMultiplier; // |EN| Adjust armor based on penetration |TR| Delmeye göre zırhı ayarla

        float mitigation = adjustedArmor / (adjustedArmor + 100f); // |EN| Calculate damage mitigation percentage |TR| Hasar azaltma yüzdesini hesapla
        float mitigationCap = 0.75f; // |EN| Cap mitigation at 75% |TR| Azaltmayı %75 ile sınırla

        float finalMitigation = Mathf.Clamp(mitigation, 0f, mitigationCap); // |EN| Clamp mitigation between 0% and cap |TR| Azaltmayı %0 ile sınır arasında sınırla
        return finalMitigation;
    }

    public float GetArmorPenetration()
    {
        float armorPenetration = offensiveStats.armorPenetration.GetValue() / 100f; // |EN| Convert percentage to decimal (e.g., 20% -> 0.2) |TR| Yüzdeyi ondalığa dönüştür (örneğin, %20 -> 0.2)
        return armorPenetration;
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

    public float GetMaxHealth()
    {
        float baseHealth = resourceStats.maxHealth.GetValue();
        float bonusHealth = majorStats.vitality.GetValue() * 5f; // |EN| Calculate bonus health from vitality |TR| Canlılıktan bonus sağlığı hesapla
        float finalHealth = (baseHealth + bonusHealth);

        return finalHealth;
    }
}
