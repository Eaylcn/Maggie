using System;
using JetBrains.Annotations;
using UnityEngine;

public class EntityHealth : MonoBehaviour, IDamageable
{
    private EntityVFX entityVFX; // |EN| Reference to EntityVFX script for playing VFX |TR| VFX oynatmak için EntityVFX script'ine referans
    private Entity entity; // |EN| Reference to Entity script for entity-related data |TR| Varlıkla ilgili veriler için Entity script'ine referans

    [SerializeField] protected float maxHp = 100f; // |EN| Maximum health points |TR| Maksimum sağlık puanı
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

        currentHp = maxHp; // |EN| Initialize current health to maximum health |TR| Mevcut sağlığı maksimum sağlığa başlat
    }

    public virtual void TakeDamage(float damage, Transform damageSource)
    {
        // |EN| If already dead, do nothing |TR| Zaten ölü ise, hiçbir şey yapma
        if (isDead) return;

        // |EN| Calculate and apply knockback based on damage severity |TR| Hasar şiddetine göre geri tepme hesapla ve uygula
        Vector2 knockbackDirection = CalculateKnockbackDirection(damage, damageSource);
        float knockbackDur = CalculateKnockbackDuration(damage);
        entity.ReceiveKnockback(knockbackDirection, knockbackDur);

        entityVFX?.PlayOnDamageVFX(); // |EN| Play damage VFX if Entity alive |TR| Varlık hayattaysa hasar VFX'sini oynat

        ReduceHealth(damage);
    }

    protected void ReduceHealth(float damage)
    {
        // |EN| Reduce health by damage amount |TR| Sağlığı hasar miktarı kadar azalt
        currentHp -= damage;

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

    private Vector2 CalculateKnockbackDirection(float damage, Transform damageDealer)
    {
        int direction = damageDealer.position.x >= transform.position.x ? -1 : 1;      // |EN| Determine knockback direction based on damage dealer position |TR| Hasar verenin konumuna göre geri tepme yönünü belirle
        Vector2 force = IsHeavyDamage(damage) ? heavyKnockbackForce : knockbackForce;  // |EN| Choose knockback force based on damage severity |TR| Hasar şiddetine göre geri tepme kuvvetini seç
        return new Vector2(direction * force.x, force.y);                              // |EN| Return calculated knockback vector |TR| Hesaplanan geri tepme vektörünü döndür
    }
    
    private float CalculateKnockbackDuration(float damage) => IsHeavyDamage(damage) ? heavyKnockbackDuration : knockbackDuration; // |EN| Determine knockback duration based on damage severity |TR| Hasar şiddetine göre geri tepme süresini belirle

    private bool IsHeavyDamage(float damage) => damage / maxHp >= heavyDamageThreshold; // |EN| Check if damage is considered heavy based on threshold |TR| Hasarın eşik değerine göre ağır olarak kabul edilip edilmediğini kontrol et
}
