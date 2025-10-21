using System.Collections;
using UnityEngine;

public class Dummy : Entity, IDamageable
{
    private EntityVFX fx;
    private EnemyHealth health;

    protected override void Awake()
    {
        base.Awake();

        fx = GetComponentInChildren<EntityVFX>();
        health = GetComponent<EnemyHealth>();
    }

    protected override void Update()
    {
        // |EN| Dummy-specific update logic can go here |TR| Dummy'ye özgü güncelleme mantığı buraya gelebilir
    }

    public bool TakeDamage(float damage, float elementalDamage, ElementType elementType, Transform damageSource)
    {

        fx?.PlayOnDamageVFX(); // |EN| Play damage VFX when dummy takes damage |TR| Dummy hasar aldığında hasar VFX'sini oynat
        anim.SetTrigger("Hit"); // |EN| Trigger hit animation upon taking damage |TR| Hasar alındığında vurma animasyonunu tetikle

        // |EN| Check for evasion chance |TR| Kaçınma şansını kontrol et
        if (health.IsAttackEvaded())
        {
            Debug.Log("Dummy evaded the attack!");
            return false;
        }

        EntityStats attackerStats = damageSource.GetComponent<EntityStats>(); // |EN| Get attacker's stats for armor penetration calculation |TR| Zırh delme hesaplaması için saldırganın istatistiklerini al
        float armorPenetration = attackerStats != null ? attackerStats.GetArmorPenetration() : 0f; // |EN| Get armor penetration value or default to 0 |TR| Zırh delme değerini al veya varsayılan olarak 0'a ayarla

        // |EN| Calculate damage mitigation |TR| Hasar azaltma hesapla
        float mitigation = stats.GetArmorMitigation(armorPenetration);
        float finalPhysicalDamage = damage * (1f - mitigation); // |EN| Apply mitigation to incoming damage (e.g., 20% mitigation means 80% damage taken) |TR| Gelen hasara azaltma uygula (örneğin, %20 azaltma %80 hasar alınması anlamına gelir)

        float elementalResistance = stats.GetElementalResistance(elementType); // |EN| Get elemental resistance for the specific element type |TR| Belirli element türü için elemental direnci al
        float finalElementalDamage = elementalDamage * (1f - elementalResistance); // |EN| Apply resistance to elemental damage |TR| Elemental hasara direnci uygula

        Debug.Log($"Dummy took {finalPhysicalDamage} physical damage and {finalElementalDamage} {elementType} damage.");

        return true; // |EN| Damage was successfully applied |TR| Hasar başarıyla uygulandı
    }

    protected override IEnumerator SlowdownEntityCo(float slowMultiplier, float duration)
    {
        float originalAnimSpeed = anim.speed;

        float speedMultiplier = 1 - slowMultiplier; // |EN| Calculate speed multiplier based on slow effect |TR| Yavaşlatma etkisine göre hız çarpanını hesapla

        anim.speed *= speedMultiplier;

        yield return new WaitForSeconds(duration);

        anim.speed = originalAnimSpeed;
    }

    protected override void OnDrawGizmos()
    {
        // |EN| Draw gizmos for debugging purposes |TR| Hata ayıklama amaçlı gizmos çiz
    }
}
