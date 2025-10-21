using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EntityHealth : MonoBehaviour, IDamageable
{
    private Slider healthBar;
    private Entity entity; // |EN| Reference to Entity script for entity-related data |TR| Varlıkla ilgili veriler için Entity script'ine referans
    private EntityStats entityStats; // |EN| Reference to EntityStats script for stats data |TR| İstatistik verileri için EntityStats script'ine referans
    private EntityVFX entityVFX; // |EN| Reference to EntityVFX script for playing VFX |TR| VFX oynatmak için EntityVFX script'ine referans

    [SerializeField] protected float currentHealth; // |EN| Current health points |TR| Mevcut sağlık puanı
    public bool isDead = false; // |EN| Is the entity dead? |TR| Varlık ölü mü?
    public bool isLastAttackEvaded { get; private set; } = false; // |EN| Was the last attack evaded? |TR| Son saldırı savuşturuldu mu?

    [Header("Health Settings")]
    [SerializeField] private float healthRegenerationInterval = 1f; // |EN| Interval in seconds for health regeneration ticks |TR| Sağlık yenileme tikleri için saniye cinsinden aralık
    [SerializeField] private bool canRegenerateHealth = true; // |EN| Can the entity regenerate health over time? |TR| Varlık zamanla sağlık yenileyebilir mi?

    [Header("On Damage Knockback")]
    [SerializeField] private Vector2 knockbackForce = new Vector2(1.5f, 2.5f); // |EN| Force applied for knockback on taking damage |TR| Hasar alındığında uygulanan geri tepme kuvveti
    [SerializeField] private Vector2 heavyKnockbackForce = new Vector2(7f, 7f); // |EN| Force applied for heavy knockback on taking heavy damage |TR| Ağır hasar alındığında uygulanan ağır geri tepme kuvveti
    [SerializeField] private float knockbackDuration = 0.2f; // |EN| Duration of the knockback effect |TR| Sarsma etkisinin süresi
    [SerializeField] private float heavyKnockbackDuration = 0.5f; // |EN| Duration of the heavy knockback effect |TR| Ağır geri tepme etkisinin süresi

    [Header("On Heavy Damage")]
    [SerializeField] private float heavyDamageThreshold = 0.3f; // |EN| Percentage of max HP that defines heavy damage |TR| Ağır hasarı tanımlayan maksimum sağlık yüzdesi

    private void Awake()
    {
        entity = GetComponent<Entity>(); // |EN| Get reference to Entity script |TR| Entity script'ine referans al
        entityStats = GetComponent<EntityStats>(); // |EN| Get reference to EntityStats script |TR| EntityStats script'ine referans al
        entityVFX = GetComponent<EntityVFX>(); // |EN| Get reference to EntityVFX script |TR| EntityVFX script'ine referans al
        healthBar = GetComponentInChildren<Slider>(); // |EN| Get reference to health bar slider if exists |TR| Varsa sağlık çubuğu kaydırıcısına referans al

        currentHealth = entityStats.GetMaxHealth(); // |EN| Initialize current health to maximum health |TR| Mevcut sağlığı maksimum sağlığa başlat
        UpdateHealthBar(); // |EN| Update health bar to reflect initial health |TR| Başlangıç sağlığını yansıtmak için sağlık çubuğunu güncelle

        InvokeRepeating(nameof(RegenerateHealth), 0, healthRegenerationInterval); // |EN| Start health regeneration ticks |TR| Sağlık yenileme tiklerine başla
    }

    public virtual bool TakeDamage(float damage, float elementalDamage, ElementType elementType, Transform damageSource)
    {
        // |EN| If already dead, do nothing |TR| Zaten ölü ise, hiçbir şey yapma
        if (isDead) return false;

        // |EN| Check for evasion chance |TR| Kaçınma şansını kontrol et
        if (IsAttackEvaded())
        {
            return false;
        }

        EntityStats attackerStats = damageSource.GetComponent<EntityStats>(); // |EN| Get attacker's stats for armor penetration calculation |TR| Zırh delme hesaplaması için saldırganın istatistiklerini al
        float armorPenetration = attackerStats != null ? attackerStats.GetArmorPenetration() : 0f; // |EN| Get armor penetration value or default to 0 |TR| Zırh delme değerini al veya varsayılan olarak 0'a ayarla

        // |EN| Calculate damage mitigation |TR| Hasar azaltma hesapla
        float mitigation = entityStats.GetArmorMitigation(armorPenetration);
        float finalPhysicalDamage = damage * (1f - mitigation); // |EN| Apply mitigation to incoming damage (e.g., 20% mitigation means 80% damage taken) |TR| Gelen hasara azaltma uygula (örneğin, %20 azaltma %80 hasar alınması anlamına gelir)

        float elementalResistance = entityStats.GetElementalResistance(elementType); // |EN| Get elemental resistance for the specific element type |TR| Belirli element türü için elemental direnci al
        float finalElementalDamage = elementalDamage * (1f - elementalResistance); // |EN| Apply resistance to elemental damage |TR| Elemental hasara direnci uygula

        ApplyKnockback(damageSource, finalPhysicalDamage); // |EN| Apply knockback effect based on damage severity |TR| Hasar şiddetine göre geri tepme etkisi uygula

        ReduceHealth(finalPhysicalDamage + finalElementalDamage); // |EN| Reduce health by final damage amount |TR| Sağlığı nihai hasar miktarı kadar azalt

        return true; // |EN| Damage was successfully applied |TR| Hasar başarıyla uygulandı
    }
    public bool IsAttackEvaded()
    {
        float evasionChance = entityStats.GetEvasion(); // |EN| Get evasion stat value |TR| Kaçınma istatistik değerini al
        float roll = Random.Range(0f, 100f);      // |EN| Roll a random number between 0 and 100 |TR| 0 ile 100 arasında rastgele bir sayı atla

        isLastAttackEvaded = roll < evasionChance; // |EN| Update last attack evasion status |TR| Son saldırı kaçınma durumunu güncelle

        return isLastAttackEvaded; // |EN| Attack is avoided if roll is less than evasion chance |TR| Atış, atış kaçınma şansından azsa kaçınılır
    }

    private void RegenerateHealth()
    {
        if (isDead || !canRegenerateHealth) return; // |EN| If dead or cannot regenerate, exit |TR| Öldüyse veya yenileyemiyorsa çık

        float regenAmount = entityStats.resourceStats.healthRegen.GetValue();
        IncreaseHealth(regenAmount); // |EN| Increase current health by regeneration amount |TR| Mevcut sağlığı yenileme miktarı kadar artır
    }

    public void IncreaseHealth(float healAmount)
    {
        if (isDead) return; // |EN| If dead, cannot increase health |TR| Öldüyse sağlığı artıramazsın

        float newHealth = currentHealth + healAmount; // |EN| Calculate new health after increase |TR| Artıştan sonra yeni sağlığı hesapla
        float maxHealth = entityStats.GetMaxHealth(); // |EN| Get maximum health from stats |TR| İstatistiklerden maksimum sağlığı al

        currentHealth = Mathf.Min(newHealth, maxHealth); // |EN| Clamp health to not exceed maximum |TR| Sağlığın maksimumu aşmaması için sınırla
        UpdateHealthBar(); // |EN| Update health bar after healing |TR| İyileştirmeden sonra sağlık çubuğunu güncelle
    }

    public void ReduceHealth(float damage)
    {
        entityVFX?.PlayOnDamageVFX(); // |EN| Play damage VFX if EntityVFX reference exists |TR| EntityVFX referansı varsa hasar VFX'sini oynat
        currentHealth -= damage; // |EN| Subtract damage from current health |TR| Mevcut sağlıktan hasarı çıkar
        UpdateHealthBar(); // |EN| Update health bar after taking damage |TR| Hasar aldıktan sonra sağlık çubuğunu güncelle

        // |EN| Check for death |TR| Ölümü kontrol et
        if (currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        // |EN| Handle death logic here |TR| Ölüm mantığını burada ele alın
        isDead = true;
        entity.EntityDeath();
    }

    private void UpdateHealthBar()
    {
        if (healthBar == null) return; // |EN| If no health bar exists, skip update |TR| Sağlık çubuğu yoksa güncellemeyi atla

        healthBar.value = currentHealth / entityStats.GetMaxHealth(); // |EN| Update health bar slider value |TR| Sağlık çubuğu kaydırıcı değerini güncelle
    }

    private void ApplyKnockback(Transform damageSource, float finalDamage)
    {

        // |EN| Calculate and apply knockback based on damage severity |TR| Hasar şiddetine göre geri tepme hesapla ve uygula
        Vector2 knockbackDirection = CalculateKnockbackDirection(finalDamage, damageSource);
        float knockbackDur = CalculateKnockbackDuration(finalDamage);

        entity.ReceiveKnockback(knockbackDirection, knockbackDur);
    }

    private Vector2 CalculateKnockbackDirection(float damage, Transform damageDealer)
    {
        int direction = damageDealer.position.x >= transform.position.x ? -1 : 1;      // |EN| Determine knockback direction based on damage dealer position |TR| Hasar verenin konumuna göre geri tepme yönünü belirle
        Vector2 force = IsHeavyDamage(damage) ? heavyKnockbackForce : knockbackForce;  // |EN| Choose knockback force based on damage severity |TR| Hasar şiddetine göre geri tepme kuvvetini seç
        return new Vector2(direction * force.x, force.y);                              // |EN| Return calculated knockback vector |TR| Hesaplanan geri tepme vektörünü döndür
    }
    
    private float CalculateKnockbackDuration(float damage) => IsHeavyDamage(damage) ? heavyKnockbackDuration : knockbackDuration; // |EN| Determine knockback duration based on damage severity |TR| Hasar şiddetine göre geri tepme süresini belirle

    private bool IsHeavyDamage(float damage) => damage / entityStats.GetMaxHealth() >= heavyDamageThreshold; // |EN| Check if damage is considered heavy based on threshold |TR| Hasarın eşik değerine göre ağır olarak kabul edilip edilmediğini kontrol et
}
