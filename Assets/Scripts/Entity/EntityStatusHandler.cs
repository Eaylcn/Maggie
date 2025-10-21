using System;
using System.Collections;
using UnityEngine;

public class EntityStatusHandler : MonoBehaviour
{
    private Entity entity; // |EN| Reference to the Entity script |TR| Entity script'ine referans
    private EntityHealth entityHealth; // |EN| Reference to the EntityHealth script |TR| EntityHealth script'ine referans
    private EntityVFX entityVFX; // |EN| Reference to the EntityVFX script |TR| EntityVFX script'ine referans
    private EntityStats stats; // |EN| Reference to the EntityStats script |TR| EntityStats script'ine referans
    private ElementType currentElementalStatus = ElementType.None; // |EN| Current elemental status effect on the entity |TR| Varlık üzerindeki mevcut elemental durum etkisi

    [Header("Shock Status Settings")]
    [SerializeField] private GameObject lightningStrikeVfxPrefab; // |EN| Prefab for lightning strike VFX |TR| Yıldırım çarpması VFX'si için prefab
    [SerializeField] private float currentChargeLevel = 0f; // |EN| Current charge level for shock status |TR| Şok durumu için mevcut şarj seviyesi
    [SerializeField] private float maxChargeLevel = 1f; // |EN| Maximum charge level for shock status |TR| Şok durumu için maksimum şarj seviyesi
    private Coroutine shockStatusCoroutine; // |EN| Coroutine reference for shock status effect |TR| Şok durumu etkisi için Coroutine referansı


    private void Awake()
    {
        entity = GetComponent<Entity>(); // |EN| Get reference to the Entity script |TR| Entity script'ine referans al
        entityVFX = GetComponent<EntityVFX>(); // |EN| Get reference to the EntityVFX script |TR| EntityVFX script'ine referans al
        entityHealth = GetComponent<EntityHealth>(); // |EN| Get reference to the EntityHealth script |TR| EntityHealth script'ine referans al
        stats = GetComponent<EntityStats>(); // |EN| Get reference to the EntityStats script |TR| EntityStats script'ine referans al
    }

    public void ApplyShockStatus(float duration, float damage, float charge)
    {
        float lightningResistance = stats.GetElementalResistance(ElementType.Lightning); // |EN| Get lightning resistance from stats |TR| İstatistiklerden yıldırım direncini al
        float adjustedCharge = charge * (1f - lightningResistance); // |EN| Adjust charge based on resistance |TR| Dirence göre şarjı ayarla

        currentChargeLevel += adjustedCharge; // |EN| Increase current charge level |TR| Mevcut şarj seviyesini artır

        if (currentChargeLevel >= maxChargeLevel)
        {
            DoLightningStrike(damage); // |EN| Trigger lightning strike when max charge is reached |TR| Maksimum şarj seviyesine ulaşıldığında yıldırım çarpmasını tetikle
            StopShockStatus(); // |EN| Reset shock status after strike |TR| Çarpma sonrası şok durumunu sıfırla
            return;
        }

        if (shockStatusCoroutine != null)
            StopCoroutine(shockStatusCoroutine); // |EN| Stop any existing shock status coroutine |TR| Mevcut şok durumu coroutine'ini durdur
            
        shockStatusCoroutine = StartCoroutine(ShockStatusCo(duration)); // |EN| Start coroutine to handle shock status duration |TR| Şok durumu süresini yönetmek için coroutine başlat
    }

    private void StopShockStatus()
    {
        currentElementalStatus = ElementType.None; // |EN| Reset elemental status to None |TR| Elemental durumu Hiçbiri olarak sıfırla
        currentChargeLevel = 0f; // |EN| Reset charge level |TR| Şarj seviyesini sıfırla
        entityVFX.StopAllStatusVfx(); // |EN| Stop all status VFX |TR| Tüm durum VFX'lerini durdur
    }

    private void DoLightningStrike(float damage)
    {
        Instantiate(lightningStrikeVfxPrefab, transform.position, Quaternion.identity); // |EN| Instantiate lightning strike VFX at entity's position |TR| Varlığın pozisyonunda yıldırım çarpması VFX'si oluştur
        entityHealth.ReduceHealth(damage); // |EN| Apply shock damage to the entity |TR| Varlığa şok hasarı uygula
        entity.StunEntity(1f); // |EN| Stun the entity briefly upon lightning strike |TR| Yıldırım çarpması üzerine varlığı kısa süreliğine sersemlet
    }

    private IEnumerator ShockStatusCo(float duration)
    {
        currentElementalStatus = ElementType.Lightning; // |EN| Set current elemental status to Lightning |TR| Mevcut elemental durumu Yıldırım olarak ayarla
        entityVFX.PlayStatusVfx(duration, currentElementalStatus); // |EN| Play the status VFX |TR| Durum VFX'sini oynat

        yield return new WaitForSeconds(duration);

        StopShockStatus(); // |EN| Reset shock status after duration ends |TR| Süre sona erdikten sonra şok durumunu sıfırla
    }

    public void ApplyBurningStatus(float totalDamage, float duration)
    {
        float fireResistance = stats.GetElementalResistance(ElementType.Fire); // |EN| Get fire resistance from stats |TR| İstatistiklerden ateş direncini al
        float adjustedDamage = totalDamage * (1f - fireResistance); // |EN| Adjust damage based on resistance |TR| Dirence göre hasarı ayarla

        StartCoroutine(BurningStatusCo(adjustedDamage, duration)); // |EN| Start coroutine to handle burning status damage over time |TR| Zamanla yanma durumu hasarını yönetmek için coroutine başlat
    }

    private IEnumerator BurningStatusCo(float totalDamage, float duration)
    {
        currentElementalStatus = ElementType.Fire; // |EN| Set current elemental status to Fire |TR| Mevcut elemental durumu Ateş olarak ayarla
        entityVFX.PlayStatusVfx(duration, currentElementalStatus); // |EN| Play the status VFX |TR| Durum VFX'sini oynat

        int ticksPerSecond = 2; // |EN| Number of damage ticks per second |TR| Saniye başına hasar vuruşu sayısı
        int tickCount = Mathf.RoundToInt(ticksPerSecond * duration); // |EN| Total number of ticks over the duration |TR| Süre boyunca toplam vuruş sayısı

        float damagePerTick = totalDamage / tickCount; // |EN| Damage to apply each tick |TR| Her vuruşta uygulanacak hasar
        float tickInterval = 1f / ticksPerSecond; // |EN| Interval between each tick |TR| Her vuruş arasındaki aralık

        for (int i = 0; i < tickCount; i++) // |EN| Loop through each tick |TR| Her vuruşta döngü
        {
            if (entityHealth.isDead) break;

            entityHealth.ReduceHealth(damagePerTick); // |EN| Apply damage for this tick |TR| Bu vuruş için hasar uygula

            yield return new WaitForSeconds(tickInterval); // |EN| Wait for the next tick interval |TR| Bir sonraki vuruş aralığını bekle
        }

        currentElementalStatus = ElementType.None; // |EN| Reset elemental status after duration ends |TR| Süre sona erdikten sonra elemental durumu sıfırla
    }

    public void ApplyChillStatus(float duration, float slowMultiplier)
    {
        float iceResistance = stats.GetElementalResistance(ElementType.Ice); // |EN| Get ice resistance from stats |TR| İstatistiklerden buz direncini al
        float adjustedDuration = duration * (1f - iceResistance); // |EN| Adjust duration based on resistance |TR| Dirence göre süreyi ayarla

        StartCoroutine(ChillStatusCo(slowMultiplier, adjustedDuration)); // |EN| Start coroutine to handle chilled status duration |TR| Soğutulmuş durum süresini yönetmek için coroutine başlat
    }

    private IEnumerator ChillStatusCo(float slowMultiplier, float duration)
    {
        if (entityHealth.isDead || entityHealth.isLastAttackEvaded) yield break;

        currentElementalStatus = ElementType.Ice; // |EN| Set current elemental status to Ice |TR| Mevcut elemental durumu Buz olarak ayarla
        entity.SlowdownEntity(slowMultiplier, duration); // |EN| Apply slowdown effect to the entity |TR| Varlığa yavaşlatma etkisi uygula
        entityVFX.PlayStatusVfx(duration, currentElementalStatus); // |EN| Play the status VFX |TR| Durum VFX'sini oynat

        yield return new WaitForSeconds(duration);

        currentElementalStatus = ElementType.None; // |EN| Reset elemental status after duration ends |TR| Süre sona erdikten sonra elemental durumu sıfırla
    }

    public bool CanApplyStatusEffect(ElementType newElementType)
    {
        if (newElementType == ElementType.Lightning && currentElementalStatus == ElementType.Lightning)
            return true; // |EN| Allow applying Lightning status over Lightning status |TR| Yıldırım durumu üzerine Yıldırım durumunun uygulanmasına izin ver

        // |EN| Prevent applying the same status effect if already present |TR| Zaten mevcutsa aynı durum etkisini uygulamayı önle
        return currentElementalStatus == ElementType.None;
    }
}
