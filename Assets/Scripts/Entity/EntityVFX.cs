using System.Collections;
using UnityEngine;

public class EntityVFX : MonoBehaviour
{
    private SpriteRenderer sr;

    [Header("On Taking Damage VFX")]
    [SerializeField] private Material onTakingDamageMaterial; // |EN| Material to use when entity takes damage |TR| Varlık hasar aldığında kullanılacak malzeme
    [SerializeField] private float onTakingDamageVFXDuration = 0.2f; // |EN| Duration to show the damage VFX |TR| Hasar VFX'sini gösterme süresi
    private Material originalMaterial; // |EN| Original material of the entity |TR| Varlığın orijinal malzemesi
    private Coroutine onTakingDamageVfxCoroutine; // |EN| Coroutine reference for damage VFX |TR| Hasar VFX'si için Coroutine referansı

    [Header("On Doing Damage VFX")]
    [SerializeField] private Color hitVfxColor = Color.white; // |EN| Color tint for hit VFX when entity deals damage |TR| Varlık hasar verdiğinde vurma VFX'si için renk tonu
    [SerializeField] private GameObject hitVfxPrefab; // |EN| Prefab for hit VFX when entity deals damage |TR| Varlık hasar verdiğinde vurma VFX'si için prefab

    private void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>(); // |EN| Get reference to SpriteRenderer in children |TR| Çocuklardaki SpriteRenderer'a referans al
        originalMaterial = sr.material; // |EN| Get the original material of the entity |TR| Varlığın orijinal malzemesini al
    }

    public void CreateOnHitVFX(Transform target)
    {
        if (hitVfxPrefab != null)
        {
            GameObject hitVfx = Instantiate(hitVfxPrefab, target.position, Quaternion.identity); // |EN| Instantiate hit VFX at specified position |TR| Belirtilen pozisyonda vurma VFX'si oluştur
            hitVfx.GetComponentInChildren<SpriteRenderer>().color = hitVfxColor; // |EN| Apply color tint to hit VFX |TR| Vurulan VFX'ye renk tonu uygula
        }
    }

    public void PlayOnDamageVFX()
    {
        if (onTakingDamageVfxCoroutine != null) // |EN| If a damage VFX is already playing, stop it |TR| Zaten bir hasar VFX'si oynuyorsa, durdur
            StopCoroutine(onTakingDamageVfxCoroutine); 

        onTakingDamageVfxCoroutine = StartCoroutine(OnDamageVfxCo()); // |EN| Start the damage VFX coroutine |TR| Hasar VFX'si coroutine'ini başlat
    }

    private IEnumerator OnDamageVfxCo()
    {
        sr.material = onTakingDamageMaterial; // |EN| Change to damage material |TR| Hasar malzemesine değiştir
        yield return new WaitForSeconds(onTakingDamageVFXDuration); // |EN| Wait for the specified duration |TR| Belirtilen süreyi bekle
        sr.material = originalMaterial; // |EN| Revert to original material |TR| Orijinal malzemeye geri dön
    }
}
