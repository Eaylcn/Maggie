using UnityEngine;
using UnityEngine.UI;

public class EntityHealth : MonoBehaviour, IDamageable
{
    private Slider healthBar;
    private EntityVFX entityVFX; // |EN| Reference to EntityVFX script for playing VFX |TR| VFX oynatmak için EntityVFX script'ine referans
    private Entity entity; // |EN| Reference to Entity script for entity-related data |TR| Varlıkla ilgili veriler için Entity script'ine referans
    private EntityStats stats; // |EN| Reference to EntityStats script for stats data |TR| İstatistik verileri için EntityStats script'ine referans

    [SerializeField] protected float currentHp; // |EN| Current health points |TR| Mevcut sağlık puanı
    [SerializeField] protected bool isDead = false; // |EN| Is the entity dead? |TR| Varlık ölü mü?

    [Header("On Damage Knockback")]
    [SerializeField] private Vector2 knockbackForce = new Vector2(1.5f, 2.5f); // |EN| Force applied for knockback on taking damage |TR| Hasar alındığında uygulanan geri tepme kuvveti
    [SerializeField] private Vector2 heavyKnockbackForce = new Vector2(7f, 7f); // |EN| Force applied for heavy knockback on taking heavy damage |TR| Ağır hasar alındığında uygulanan ağır geri tepme kuvveti
    [SerializeField] private float knockbackDuration = 0.2f; // |EN| Duration of the knockback effect |TR| Sarsma etkisinin süresi
    [SerializeField] private float heavyKnockbackDuration = 0.5f; // |EN| Duration of the heavy knockback effect |TR| Ağır geri tepme etkisinin süresi

    [Header("On Heavy Damage")]
    [SerializeField] private float heavyDamageThreshold = 0.3f; // |EN| Percentage of max HP that defines heavy damage |TR| Ağır hasarı tanımlayan maksimum sağlık yüzdesi

    private void Awake()
    {
        entityVFX = GetComponent<EntityVFX>(); // |EN| Get reference to EntityVFX script |TR| EntityVFX script'ine referans al
        entity = GetComponent<Entity>(); // |EN| Get reference to Entity script |TR| Entity script'ine referans al
        stats = GetComponent<EntityStats>(); // |EN| Get reference to EntityStats script |TR| EntityStats script'ine referans al
        healthBar = GetComponentInChildren<Slider>(); // |EN| Get reference to health bar slider if exists |TR| Varsa sağlık çubuğu kaydırıcısına referans al

        currentHp = stats.GetMaxHealth(); // |EN| Initialize current health to maximum health |TR| Mevcut sağlığı maksimum sağlığa başlat
        UpdateHealthBar(); // |EN| Update health bar to reflect initial health |TR| Başlangıç sağlığını yansıtmak için sağlık çubuğunu güncelle
    }

    public virtual bool TakeDamage(float damage, Transform damageSource)
    {
        // |EN| If already dead, do nothing |TR| Zaten ölü ise, hiçbir şey yapma
        if (isDead) return false;

        // |EN| Check for evasion chance |TR| Kaçınma şansını kontrol et
        if (IsAttackEvaded())
        {
            return false;
        }

        // |EN| Calculate and apply knockback based on damage severity |TR| Hasar şiddetine göre geri tepme hesapla ve uygula
        Vector2 knockbackDirection = CalculateKnockbackDirection(damage, damageSource);
        float knockbackDur = CalculateKnockbackDuration(damage);
        entity.ReceiveKnockback(knockbackDirection, knockbackDur);

        entityVFX?.PlayOnDamageVFX(); // |EN| Play damage VFX if Entity alive |TR| Varlık hayattaysa hasar VFX'sini oynat

        ReduceHealth(damage);

        return true; // |EN| Damage was successfully applied |TR| Hasar başarıyla uygulandı
    }

    private bool IsAttackEvaded()
    {
        float evasionChance = stats.GetEvasion(); // |EN| Get evasion stat value |TR| Kaçınma istatistik değerini al
        float roll = Random.Range(0f, 100f);      // |EN| Roll a random number between 0 and 100 |TR| 0 ile 100 arasında rastgele bir sayı atla

        return roll < evasionChance; // |EN| Attack is avoided if roll is less than evasion chance |TR| Atış, atış kaçınma şansından azsa kaçınılır
    }

    protected void ReduceHealth(float damage)
    {
        currentHp -= damage; // |EN| Subtract damage from current health |TR| Mevcut sağlıktan hasarı çıkar
        UpdateHealthBar(); // |EN| Update health bar after taking damage |TR| Hasar aldıktan sonra sağlık çubuğunu güncelle

        // |EN| Check for death |TR| Ölümü kontrol et
        if (currentHp <= 0)
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

        healthBar.value = currentHp / stats.GetMaxHealth(); // |EN| Update health bar slider value |TR| Sağlık çubuğu kaydırıcı değerini güncelle
    }

    private Vector2 CalculateKnockbackDirection(float damage, Transform damageDealer)
    {
        int direction = damageDealer.position.x >= transform.position.x ? -1 : 1;      // |EN| Determine knockback direction based on damage dealer position |TR| Hasar verenin konumuna göre geri tepme yönünü belirle
        Vector2 force = IsHeavyDamage(damage) ? heavyKnockbackForce : knockbackForce;  // |EN| Choose knockback force based on damage severity |TR| Hasar şiddetine göre geri tepme kuvvetini seç
        return new Vector2(direction * force.x, force.y);                              // |EN| Return calculated knockback vector |TR| Hesaplanan geri tepme vektörünü döndür
    }
    
    private float CalculateKnockbackDuration(float damage) => IsHeavyDamage(damage) ? heavyKnockbackDuration : knockbackDuration; // |EN| Determine knockback duration based on damage severity |TR| Hasar şiddetine göre geri tepme süresini belirle

    private bool IsHeavyDamage(float damage) => damage / stats.GetMaxHealth() >= heavyDamageThreshold; // |EN| Check if damage is considered heavy based on threshold |TR| Hasarın eşik değerine göre ağır olarak kabul edilip edilmediğini kontrol et
}
