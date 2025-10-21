using UnityEngine;

public class EntityCombat : MonoBehaviour
{
    private EntityVFX vfx;
    private EntityStats stats;

    [Header("Target Detection")]
    [SerializeField] private Transform targetCheck; // |EN| Transform to check for targets |TR| Hedefleri kontrol etmek için Transform
    [SerializeField] private float targetCheckRadius = 1f; // |EN| Radius to check for targets |TR| Hedefleri kontrol etmek için yarıçap
    [SerializeField] private LayerMask whatIsTarget; // |EN| Layer mask to identify targets |TR| Hedefleri tanımlamak için katman maskesi

    [Header("Status Effects Settings")]
    [SerializeField] private float defaultDuration = 3f; // |EN| Default duration for status effects |TR| Durum etkileri için varsayılan süre
    [SerializeField] private float chillSlowMultiplier = 0.5f; // |EN| Slow multiplier for chilled effect |TR| Soğutulmuş etki için yavaşlama çarpanı
    [SerializeField] private float shockChargePerHit = 0.4f; // |EN| Charge added per hit for shock effect |TR| Şok etkisi için her vuruşta eklenen şarj
    [Space]
    [SerializeField] private float fireDamageScaleFactor = 0.4f; // |EN| Scale factor for fire damage when applying burning effect |TR| Yanma etkisi uygularken ateş hasarı için ölçek faktörü
    [SerializeField] private float lightningDamageScaleFactor = 2.5f; // |EN| Scale factor for lightning damage when applying shock effect |TR| Şok etkisi uygularken yıldırım hasarı için ölçek faktörü

    private void Awake()
    {
        vfx = GetComponent<EntityVFX>();
        stats = GetComponent<EntityStats>();
    }

    public void PerformAttack()
    {
        // |EN| Implement attack logic here |TR| Saldırı mantığını buraya uygulayın
        foreach (var target in GetDetectedColliders())
        {
            IDamageable damageable = target.GetComponent<IDamageable>(); // |EN| Get the IDamageable component of the target |TR| Hedefin IDamageable bileşenini al

            if (damageable == null)
                continue; // |EN| Skip if target is not damageable |TR| Hedef hasar verilebilir değilse atla

            float elementalDamage = stats.GetElementalDamage(out ElementType elementType, .4f); // |EN| Get elemental damage value (when attacking with basic attack it scaled down by 40%) |TR| Elemental hasar değerini al (temel saldırıyla saldırırken %40 oranında azaltıldı)

            float attackDamage = stats.GetPhysicalDamage(out bool isCriticalHit, 1f); // |EN| Get attack damage and critical hit status |TR| Saldırı hasarını ve kritik vuruş durumunu al

            bool targetGotHit = damageable.TakeDamage(attackDamage, elementalDamage, elementType, transform); // |EN| Apply damage to the target |TR| Hedefe hasar uygula

            if (targetGotHit)
            {
                vfx.SetElementalVfxColor(elementType); // |EN| Set VFX color based on elemental type |TR| Element türüne göre VFX rengini ayarla
                vfx.CreateOnHitVFX(target.transform, isCriticalHit); // |EN| Create hit VFX at target position |TR| Hedef pozisyonunda vurma VFX'si oluştur

                if (elementType != ElementType.None)
                {
                    ApplyStatusEffect(target.transform, elementType, 1f); // |EN| Apply status effect based on elemental type |TR| Element türüne göre durum etkisi uygula
                }
            }
        }
    }

    public void ApplyStatusEffect(Transform target, ElementType elementType, float scaleFactor = 1f)
    {
        EntityStatusHandler statusHandler = target.GetComponent<EntityStatusHandler>();
        if (statusHandler == null)
            return; // |EN| Target does not have a status handler |TR| Hedefin bir durum işleyicisi yok;

        if (elementType == ElementType.Ice && statusHandler.CanApplyStatusEffect(elementType))
        {
            statusHandler.ApplyChillStatus(defaultDuration, chillSlowMultiplier); // |EN| Apply chilled status effect |TR| Soğutulmuş durum etkisini uygula
        }

        if (elementType == ElementType.Fire && statusHandler.CanApplyStatusEffect(elementType))
        {
            scaleFactor = fireDamageScaleFactor;
            float totalBurnDamage = stats.offensiveStats.fireDamage.GetValue() * scaleFactor; // |EN| Get total fire damage for burning effect |TR| Yanma etkisi için toplam ateş hasarını al
            statusHandler.ApplyBurningStatus(totalBurnDamage, defaultDuration);
        }

        if (elementType == ElementType.Lightning && statusHandler.CanApplyStatusEffect(elementType))
        {
            scaleFactor = lightningDamageScaleFactor;
            float shockDamage = stats.offensiveStats.lightningDamage.GetValue() * scaleFactor; // |EN| Get lightning damage for shock effect |TR| Şok etkisi için yıldırım hasarını al
            statusHandler.ApplyShockStatus(defaultDuration, shockDamage, shockChargePerHit); // |EN| Apply shock status effect |TR| Şok durum etkisini uygula
        }
    }

    protected Collider2D[] GetDetectedColliders()
    {
        // |EN| Detect targets within the specified radius |TR| Belirtilen yarıçap içinde hedefleri tespit et
        return Physics2D.OverlapCircleAll(targetCheck.position, targetCheckRadius, whatIsTarget);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(targetCheck.position, targetCheckRadius);
    }
}
